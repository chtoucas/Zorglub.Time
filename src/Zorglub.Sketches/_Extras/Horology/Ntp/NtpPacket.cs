// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Buffers.Binary;
    using System.Text;

    // Reference Identifier
    //
    // Unavailable
    // -----------
    // Four-character ASCII string representing the Kiss Code.
    //
    // PrimaryReference
    // ----------------
    // Four-character ASCII string identifying of the reference source.
    //
    // SecondaryReference
    // ------------------
    // When Version = 4, it's a mess... the various RFCs seem to be
    // contradictory.
    //
    // Depending on the ntpd version, we get an IPv4 or the first four
    // bytes of the (binary) md5 digest of the IPv6 address.
    //
    //   Currently, ntpq has no way to know which type of Refid the
    //   server is sending and always displays the Refid value in
    //   dotted-quad format -- which means that any IPv6 Refids will be
    //   listed as if they were IPv4 addresses, even though they are not.
    //   See
    //   https://support.ntp.org/bin/view/Support/RefidFormat
    //   See also
    //   https://support.ntp.org/bin/view/Dev/UpdatingTheRefidFormat
    //
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

    // See https://www.rfc-editor.org/rfc/rfc5905#section-7.2
    internal readonly partial struct NtpPacket
    {
        public const int DataLength = 48;
        public const int TransmitTimestampOffset = 40;

        public LeapIndicator LeapIndicator { get; private init; }

        /// <summary>The result is in the range from 0 to 7.</summary>
        public byte Version { get; private init; }

        public NtpMode Mode { get; private init; }

        public NtpStratum Stratum { get; private init; }

        // Signed 8-bit integer = log_2(poll)
        // Range = [4..17], 16 (2^4) seconds <= poll <= 131_072 (2^17) seconds.
        public byte PollInterval { get; private init; }

        // Signed 8-bit integer = log_2(precision)
        // Clock resolution =
        //   2^-p where p is the number of significant bits in the
        //   fraction part, e.g. Timestamp64.RandomizeSubMilliseconds()
        //   randomize the 22 lower bits, therefore the resolution is
        //   equal to 10 (~ millisecond).
        // Clock precision =
        //   Running time to read the system clock, in seconds.
        // Precision = Max(clock resolution, clock precision).
        // Range = [-20..-6], 2^-20 seconds <= precision <= 2^-6 seconds.
        public sbyte Precision { get; private init; }

        // FIXME(code): delay and dispersion values.
        // RFC 4330 (SNTP) says that it's a 32-bit signed fixed-point
        // number and that it can be negative.
        // RFC 5905 (NTP) says that it's in NTP short format (unsigned).
        // See also
        // https://support.ntp.org/bin/view/Support/NTPRelatedDefinitions
        public Duration32 RootDelay { get; private init; }

        public Duration32 RootDispersion { get; private init; }

        public uint ReferenceIdentifier { get; private init; }
        public string ReferenceCode { get; private init; }

        // 32-bit bitstring identifying the particular reference source.
        public Timestamp64 ReferenceTimestamp { get; private init; }

        public Timestamp64 OriginateTimestamp { get; private init; }
        public Timestamp64 ReceiveTimestamp { get; private init; }
        public Timestamp64 TransmitTimestamp { get; private init; }

        [Pure]
        public static NtpPacket ReadFrom(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= DataLength);

            return new NtpPacket
            {
                LeapIndicator = ReadLeapIndicator((buf[0] >> 6) & 3),
                Version = (byte)((buf[0] >> 3) & 7),
                Mode = ReadMode(buf[0] & 7),
                Stratum = ReadStratum(buf[1]),
                PollInterval = buf[2],
                Precision = ReadSByte(buf[3]),
                RootDelay = Duration32.ReadFrom(buf[4..]),
                RootDispersion = Duration32.ReadFrom(buf[8..]),
                ReferenceIdentifier = BinaryPrimitives.ReadUInt32LittleEndian(buf[12..]),
                ReferenceCode = Encoding.ASCII.GetString(buf[12..16]),
                ReferenceTimestamp = Timestamp64.ReadFrom(buf[16..]),
                OriginateTimestamp = Timestamp64.ReadFrom(buf[24..]),
                ReceiveTimestamp = Timestamp64.ReadFrom(buf[32..]),
                TransmitTimestamp = Timestamp64.ReadFrom(buf[40..])
            };

            [Pure]
            static LeapIndicator ReadLeapIndicator(int x) =>
                // "x" is a 2-bit unsigned integer.
                x switch
                {
                    0 => LeapIndicator.NoWarning,
                    1 => LeapIndicator.PositiveLeapSecond,
                    2 => LeapIndicator.NegativeLeapSecond,
                    3 => LeapIndicator.Unsynchronized,

                    _ => Throw.Unreachable<LeapIndicator>()
                };

            [Pure]
            static NtpMode ReadMode(int x) =>
                // "x" is a 3-bit unsigned integer.
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

                    _ => Throw.Unreachable<NtpMode>()
                };

            [Pure]
            static NtpStratum ReadStratum(byte x) =>
                x switch
                {
                    0 => NtpStratum.Unavailable,
                    1 => NtpStratum.PrimaryReference,
                    <= 15 => NtpStratum.SecondaryReference,
                    16 => NtpStratum.Unsynchronized,
                    _ => NtpStratum.Reserved
                };

            // Two's-complement representation of a signed byte.
            // https://en.wikipedia.org/wiki/Two%27s_complement
            [Pure]
            static sbyte ReadSByte(byte v) => (sbyte)(v > 127 ? v - 256 : v);
        }
    }
}
