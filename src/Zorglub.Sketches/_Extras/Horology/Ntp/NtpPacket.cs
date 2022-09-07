// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Text;

    #region Developer Notes

    // Poll Interval
    // =============
    //
    // Signed 8-bit integer = log_2(poll)
    // Range = [4..17], 16 (2^4) seconds <= poll <= 131_072 (2^17) seconds.
    //
    //
    // Precision
    // =========
    //
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
    //
    //
    // Reference Identifier
    // ====================
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

    #endregion

    // See https://www.rfc-editor.org/rfc/rfc5905#section-7.2

    /// <summary>
    /// Represents an NTP packet.
    /// <para><see cref="NtpPacket"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>Beware, <see cref="NtpPacket"/> is a HUGE struct.</para>
    /// </remarks>
    internal readonly struct NtpPacket
    {
        public const int BinarySize = 48;
        public const int TransmitTimestampOffset = 40;

        public LeapIndicator LeapIndicator { get; private init; }
        public byte Version { get; private init; }
        public NtpMode Mode { get; private init; }
        public NtpStratum Stratum { get; private init; }

        public sbyte PollInterval { get; private init; }
        public sbyte Precision { get; private init; }
        public Duration32 RootDelay { get; private init; }
        public Duration32 RootDispersion { get; private init; }

        public uint ReferenceIdentifier { get; private init; }
        public string ReferenceCode { get; private init; }
        public Timestamp64 ReferenceTimestamp { get; private init; }

        public Timestamp64 OriginateTimestamp { get; private init; }
        public Timestamp64 ReceiveTimestamp { get; private init; }
        public Timestamp64 TransmitTimestamp { get; private init; }

        [Pure]
        public static NtpPacket ReadFrom(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= BinarySize);

            // First byte.
            int li = (buf[0] >> 6) & 3; // 0 <= li < 4
            int vn = (buf[0] >> 3) & 7; // 0 <= vn < 8
            int mode = buf[0] & 7;      // 0 <= mode < 8

            // TODO(code): delay, dispersion and refid values.
            // RFC 4330 (SNTP) says that delay and dispersion are 32-bit signed
            // fixed-point numbers and that they can be negative. RFC 5905 (NTP)
            // says that they are in NTP short format (unsigned). In fact, it
            // seems that ntpd ensures that delay stays positive; see
            // https://www.rfc-editor.org/rfc/rfc5905#appendix-A.5.1.1
            // Refid: 32-bit bitstring identifying the particular reference source.
            // See https://support.ntp.org/bin/view/Support/NTPRelatedDefinitions
            return new NtpPacket
            {
                LeapIndicator = (LeapIndicator)(li + 1),
                Version = (byte)vn,
                Mode = (NtpMode)(mode + 1),

                Stratum = ReadStratum(buf[1]),
                PollInterval = ReadSByte(buf[2]),
                Precision = ReadSByte(buf[3]),

                RootDelay = Duration32.ReadFrom(buf[4..]),
                RootDispersion = Duration32.ReadFrom(buf[8..]),

                ReferenceIdentifier = BitConverter.ToUInt32(buf[12..]),
                ReferenceCode = Encoding.ASCII.GetString(buf[12..16]),
                ReferenceTimestamp = Timestamp64.ReadFrom(buf[16..]),

                OriginateTimestamp = Timestamp64.ReadFrom(buf[24..]),
                ReceiveTimestamp = Timestamp64.ReadFrom(buf[32..]),
                TransmitTimestamp = Timestamp64.ReadFrom(buf[40..])
            };

            [Pure]
            static NtpStratum ReadStratum(byte x) =>
                x switch
                {
                    0 => NtpStratum.Unspecified,
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
