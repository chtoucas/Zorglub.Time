// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete
{
    using System.Text;

    using Zorglub.Time.Horology.Ntp;

    public sealed record class SntpMessage
    {
        private const int DataLength = 48;

        public LeapIndicator LeapIndicator { get; init; }
        public int Version { get; init; }
        public NtpMode Mode { get; init; }
        public NtpStratum Stratum { get; init; }
        public int PollInterval { get; init; }
        public int Precision { get; init; }
        public Duration64 RootDelay { get; init; }
        public Duration64 RootDispersion { get; init; }
        public string? ReferenceIdentifier { get; internal set; }
        public Timestamp64 ReferenceTimestamp { get; init; }

        public Timestamp64 OriginateTimestamp { get; init; }
        public Timestamp64 ReceiveTimestamp { get; init; }
        public Timestamp64 TransmitTimestamp { get; init; }
        public Timestamp64 DestinationTimestamp { get; internal set; }

        public Duration64 RoundTripDelay =>
            DestinationTimestamp - OriginateTimestamp - (TransmitTimestamp - ReceiveTimestamp);

        public Duration64 ClockOffset =>
            (ReceiveTimestamp - OriginateTimestamp + (TransmitTimestamp - DestinationTimestamp)) / 2;

        [Pure]
        internal static SntpMessage ReadFrom(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= DataLength);

            var rsp = new SntpMessage
            {
                LeapIndicator = ReadLeapIndicator((buf[0] >> 6) & 3),
                Version = (buf[0] >> 3) & 7,
                Mode = ReadMode(buf[0] & 7),
                Stratum = ReadStratum(buf[1]),
                PollInterval = ReadSByte(buf[2]),
                Precision = ReadSByte(buf[3]),
                RootDelay = Duration64.ReadFourBytesFrom(buf[4..]),
                RootDispersion = Duration64.ReadFourBytesFrom(buf[8..]),
                // Bytes 12 to 15; see ReferenceIdentifier below.
                ReferenceTimestamp = Timestamp64.ReadFrom(buf[16..]),
                OriginateTimestamp = Timestamp64.ReadFrom(buf[24..]),
                ReceiveTimestamp = Timestamp64.ReadFrom(buf[32..]),
                TransmitTimestamp = Timestamp64.ReadFrom(buf[40..])
            };

            rsp.ReferenceIdentifier = ReadReferenceIdentifier(buf[12..16], rsp);

            return rsp;

            // https://en.wikipedia.org/wiki/Two%27s_complement
            static int ReadSByte(byte v) => v > 127 ? v - 256 : v;
        }

        public bool Check(int version, Timestamp64 requestTimestamp)
        {
            // Legit binary values: 0, 1, 2.
            if (LeapIndicator != LeapIndicator.NoWarning
                && LeapIndicator != LeapIndicator.PositiveLeapSecond
                && LeapIndicator != LeapIndicator.NegativeLeapSecond)
                return false;

            if (Version != version) return false;
            if (Mode != NtpMode.Server) return false;

            // Legit binary values: 1 to 15.
            if (Stratum != NtpStratum.PrimaryReference
                && Stratum != NtpStratum.SecondaryReference)
                return false;

            // NTP client-server model: RootDelay and RootDispersion >= 0 and < 1s
            if (RootDelay < Duration64.Zero
                || RootDelay >= Duration64.OneSecond)
                return false;
            if (RootDispersion < Duration64.Zero
                || RootDispersion >= Duration64.OneSecond)
                return false;

            if (OriginateTimestamp != requestTimestamp) return false;
            if (TransmitTimestamp == Timestamp64.Zero) return false;

            return true;
        }

        [Pure]
        private static LeapIndicator ReadLeapIndicator(int x) =>
            // 2-bit number.
            x switch
            {
                0 => LeapIndicator.NoWarning,
                1 => LeapIndicator.PositiveLeapSecond,
                2 => LeapIndicator.NegativeLeapSecond,
                3 => LeapIndicator.Alarm,

                _ => LeapIndicator.Invalid
            };

        [Pure]
        private static NtpMode ReadMode(int x) =>
            // 3-bit number.
            x switch
            {
                0 => NtpMode.Reserved,
                1 => NtpMode.SymmetricActive,
                2 => NtpMode.SymmetricPassive,
                3 => NtpMode.Client,
                4 => NtpMode.Server,
                5 => NtpMode.Broadcast,
                6 => NtpMode.NtpControlMessage,
                7 => NtpMode.ReservedForPrivateUse,

                _ => NtpMode.Invalid
            };

        [Pure]
        private static NtpStratum ReadStratum(byte x) =>
            // 8-bit unsigned integer, a byte.
            x switch
            {
                0 => NtpStratum.Unavailable,
                1 => NtpStratum.PrimaryReference,
                <= 15 => NtpStratum.SecondaryReference,
                16 => NtpStratum.Unsynchronized,
                _ => NtpStratum.Reserved
            };

        [Pure]
        private static string? ReadReferenceIdentifier(ReadOnlySpan<byte> buf, SntpMessage msg)
        {
            Debug.Assert(msg != null);
            Debug.Assert(buf.Length == 4);

            // This is a 32-bit bitstring identifying the particular reference
            // source.
            switch (msg.Stratum)
            {
                case NtpStratum.Unavailable:
                case NtpStratum.PrimaryReference:
                    // Four-character ASCII string, left justified and zero
                    // padded to 32 bits.
                    return Encoding.ASCII.GetString(buf);

                case NtpStratum.SecondaryReference:
                    return msg.Version == 3
                        // NTP Version 3 only supports IPv4.
                        ? FormattableString.Invariant($"{buf[0]}.{buf[1]}.{buf[2]}.{buf[3]}")
                        // It's complicated...
                        // RFC 2030
                        //   In NTP Version 4 secondary servers, this is the low
                        //   order 32 bits of the latest transmit timestamp of
                        //   the reference source.
                        // RFC 4330
                        //   For IPv4 secondary servers, the value is the 32-bit
                        //   IPv4 address of the synchronization source.
                        //   For IPv6 and OSI secondary servers, the value is
                        //   the first 32 bits of the MD5 hash of the IPv6 or
                        //   NSAP address of the synchronization source.
                        // RFC 5905
                        //   Above stratum 1 (secondary servers and clients): this is the
                        //   reference identifier of the server and can be used to detect
                        //   timing loops. If using the IPv4 address family, the
                        //   identifier is the four-octet IPv4 address. If using the IPv6
                        //   address family, it is the first four octets of the MD5 hash
                        //   of the IPv6 address. Note that, when using the IPv6 address
                        //   family on an NTPv4 server with a NTPv3 client, the Reference
                        //   Identifier field appears to be a random value and a timing
                        //   loop might not be detected.
                        : String.Empty;

                case NtpStratum.Reserved:
                    return String.Empty;

                case NtpStratum.Invalid:
                default:
                    return null;
            }
        }
    }
}
