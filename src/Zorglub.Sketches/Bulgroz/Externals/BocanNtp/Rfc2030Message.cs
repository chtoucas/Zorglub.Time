﻿#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    using System.Net;

    using Zorglub.Time.Horology.Ntp;

#pragma warning disable CA1305 // Specify IFormatProvider

    public sealed partial class Rfc2030Message
    {
        //                      1                   2                   3
        //  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |LI | VN  |Mode |    Stratum    |     Poll      |   Precision   |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                          Root Delay                           |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                       Root Dispersion                         |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                     Reference Identifier                      |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                                                               |
        // |                    Reference Timestamp(64)                    |
        // |                                                               |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                                                               |
        // |                    Originate Timestamp(64)                    |
        // |                                                               |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                                                               |
        // |                     Receive Timestamp(64)                     |
        // |                                                               |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                                                               |
        // |                     Transmit Timestamp(64)                    |
        // |                                                               |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                  Key Identifier(optional) (32)                |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                                                               |
        // |                                                               |
        // |                  Message Digest(optional) (128)               |
        // |                                                               |
        // |                                                               |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

        internal const int DataLength = 48;

        private const int ReferenceIdOffset = 12;
        private const int ReferenceOffset = 16;
        private const int OriginateOffset = 24;
        private const int ReceiveOffset = 32;
        private const int TransmitOffset = 40;

        private readonly byte[] _bin;

        // This constructor does NOT validate its parameter.
        private Rfc2030Message(byte[] bin)
        {
            Debug.Assert(bin != null);
            Debug.Assert(bin.Length < DataLength);

            _bin = bin;
        }

        public static Rfc2030Message Request(DateTime transmitTime)
        {
            var bin = new byte[DataLength];
            // Initialize the first byte to: LI = 0, VN = 3, Mode = 3.
            bin[0] = 0x1b;
            // Initialize TransmitTimestamp.

            return new(bin);
        }

        public static Rfc2030Message Response(byte[] bin)
        {
            var msg = new Rfc2030Message(bin);
            if (msg.Mode != NtpMode.Server) Throw.Argument(nameof(bin));
            return msg;
        }

        public static Rfc2030Message FromBinary(byte[] bin)
        {
            Requires.NotNull(bin);
            // The binary message may be >= DataLength (upward compatibility).
            if (bin.Length < DataLength) Throw.Argument(nameof(bin));

            return new(bin);
        }

        public byte[] ToBinary() => _bin;
    }

    public partial class Rfc2030Message // First line
    {
        public LeapIndicator LeapIndicator
        {
            // Leap Indicator (LI): This is a two-bit code warning of an impending
            // leap second to be inserted / deleted in the last minute of the current
            // day, with bit 0 and bit 1, respectively, coded as follows:
            //
            //   LI       Value     Meaning
            //   ------------------------------------------------------ -
            //   00       0         no warning
            //   01       1         last minute has 61 seconds
            //   10       2         last minute has 59 seconds)
            //   11       3         alarm condition (clock not synchronized)
            get => (_bin[0] >> 6) switch
            {
                0 => LeapIndicator.NoWarning,
                1 => LeapIndicator.PositiveLeapSecond,
                2 => LeapIndicator.NegativeLeapSecond,
                3 => LeapIndicator.Alarm,

                _ => Throw.Unreachable<LeapIndicator>(),
            };
        }

        public int Version
        {
            // Version Number(VN): This is a three-bit integer indicating the
            // NTP/SNTP version number. The version number is 3 for Version 3 (IPv4
            // only) and 4 for Version 4 (IPv4, IPv6 and OSI). If necessary to
            // distinguish between IPv4, IPv6 and OSI, the encapsulating context
            // must be inspected.
            get => (_bin[0] & 0x38) >> 3;
        }

        public NtpMode Mode
        {
            // Mode: This is a three-bit integer indicating the mode, with values
            // defined as follows:
            //
            //   Mode     Meaning
            //   ------------------------------------
            //   0        reserved
            //   1        symmetric active
            //   2        symmetric passive
            //   3        client
            //   4        server
            //   5        broadcast
            //   6        reserved for NTP control message
            //   7        reserved for private use
            //
            // In unicast and anycast modes, the client sets this field to 3
            // (client) in the request and the server sets it to 4 (server) in the
            // reply. In multicast mode, the server sets this field to 5
            // (broadcast).
            get => (_bin[0] & 0x07) switch
            {
                0 => NtpMode.Unspecified,
                1 => NtpMode.SymmetricActive,
                2 => NtpMode.SymmetricPassive,
                3 => NtpMode.Client,
                4 => NtpMode.Server,
                5 => NtpMode.Broadcast,
                6 => NtpMode.NtpControlMessage,
                7 => NtpMode.Special7,

                _ => Throw.Unreachable<NtpMode>(),
            };
        }

        public NtpStratum Stratum
        {
            // Stratum: This is a eight-bit unsigned integer indicating the stratum
            // level of the local clock, with values defined as follows:
            //
            //   Stratum  Meaning
            //   ----------------------------------------------
            //   0        unspecified or unavailable
            //   1        primary reference (e.g., radio clock)
            //   2-15     secondary reference (via NTP or SNTP)
            //   16-255   reserved
            get => _bin[1] switch
            {
                0 => NtpStratum.Unspecified,
                1 => NtpStratum.PrimaryReference,
                <= 15 => NtpStratum.SecondaryReference,
                _ => NtpStratum.Special
            };
        }

        public int PollInterval
        {
            // Poll Interval: This is an eight-bit signed integer indicating the
            // maximum interval between successive messages, in seconds to the
            // nearest power of two. The values that can appear in this field
            // presently range from 4 (16 s) to 14 (16284 s); however, most
            // applications use only the sub-range 6 (64 s) to 10 (1024 s).
            //get => (int)Math.Pow(2, bin[2]);
            get => _bin[2];
        }

        public int Precision
        {
            // Precision: This is an eight-bit signed integer indicating the
            // precision of the local clock, in seconds to the nearest power of two.
            // The values that normally appear in this field range from -6 for
            // mains-frequency clocks to -20 for microsecond clocks found in some
            // workstations.
            //get => Math.Pow(2, bin[3]);
            get => _bin[3];
        }
    }

    public partial class Rfc2030Message
    {
        public double RootDelay
        {
            // Root Delay: This is a 32-bit signed fixed-point number indicating the
            // total roundtrip delay to the primary reference source, in seconds
            // with fraction point between bits 15 and 16. Note that this variable
            // can take on both positive and negative values, depending on the
            // relative time and frequency offsets. The values that normally appear
            // in this field range from negative values of a few milliseconds to
            // positive values of several hundred milliseconds.
            get
            {
                int x = 256 * (256 * (256 * _bin[4] + _bin[5]) + _bin[6]) + _bin[7];
                return 1000 * ((double)x / 0x10000);
            }
        }

        public double RootDispersion
        {
            // Root Dispersion: This is a 32-bit unsigned fixed-point number
            // indicating the nominal error relative to the primary reference
            // source, in seconds with fraction point between bits 15 and 16. The
            // values that normally appear in this field range from 0 to several
            // hundred milliseconds.
            get
            {
                int x = 256 * (256 * (256 * _bin[8] + _bin[9]) + _bin[10]) + _bin[11];
                return 1000 * ((double)x / 0x10000);
            }
        }

        public string ReferenceId
        {
            // Reference Identifier: This is a 32-bit bitstring identifying the
            // particular reference source. In the case of NTP Version 3 or Version
            // 4 stratum-0 (unspecified) or stratum-1 (primary) servers, this is a
            // four-character ASCII string, left justified and zero padded to 32
            // bits. In NTP Version 3 secondary servers, this is the 32-bit IPv4
            // address of the reference source. In NTP Version 4 secondary servers,
            // this is the low order 32 bits of the latest transmit timestamp of the
            // reference source. NTP primary (stratum 1) servers should set this
            // field to a code identifying the external reference source according
            // to the following list. If the external reference is one of those
            // listed, the associated code should be used. Codes for sources not
            // listed can be contrived as appropriate.
            //
            //   Code     External Reference Source
            //   ----------------------------------------------------------------
            //   LOCL     uncalibrated local clock used as a primary reference for
            //            a subnet without external means of synchronization
            //   PPS      atomic clock or other pulse-per-second source
            //            individually calibrated to national standards
            //   ACTS     NIST dialup modem service
            //   USNO     USNO modem service
            //   PTB      PTB (Germany) modem service
            //   TDF      Allouis (France) Radio 164 kHz
            //   DCF      Mainflingen (Germany) Radio 77.5 kHz
            //   MSF      Rugby (UK) Radio 60 kHz
            //   WWV      Ft. Collins (US) Radio 2.5, 5, 10, 15, 20 MHz
            //   WWVB     Boulder (US) Radio 60 kHz
            //   WWVH     Kaui Hawaii (US) Radio 2.5, 5, 10, 15 MHz
            //   CHU      Ottawa (Canada) Radio 3330, 7335, 14670 kHz
            //   LORC     LORAN-C radionavigation system
            //   OMEG     OMEGA radionavigation system
            //   GPS      Global Positioning Service
            //   GOES     Geostationary Orbit Environment Satellite
            get
            {
                string r = "";
                switch (Stratum)
                {
                    case NtpStratum.Unspecified:
                    case NtpStratum.PrimaryReference:
                        r += (char)_bin[ReferenceIdOffset + 0];
                        r += (char)_bin[ReferenceIdOffset + 1];
                        r += (char)_bin[ReferenceIdOffset + 2];
                        r += (char)_bin[ReferenceIdOffset + 3];
                        break;
                    case NtpStratum.SecondaryReference:
                        switch (Version)
                        {
                            case 3:	// Version 3, Reference ID is an IPv4 address.
                                string addr = _bin[ReferenceIdOffset + 0].ToString() + "." +
                                                 _bin[ReferenceIdOffset + 1].ToString() + "." +
                                                 _bin[ReferenceIdOffset + 2].ToString() + "." +
                                                 _bin[ReferenceIdOffset + 3].ToString();
                                IPHostEntry host = Dns.GetHostEntry(addr);
                                r = host.HostName + " (" + addr + ")";
                                break;
                            case 4: // Version 4, Reference ID is the timestamp of last update.
                                r = ReadUtcTime(ReferenceIdOffset).ToString();
                                break;
                            default:
                                r = "N/A";
                                break;
                        }
                        break;
                }

                return r;
            }
        }

        public DateTime ReferenceTime
        {
            // Reference Timestamp: This is the time at which the local clock was
            // last set or corrected, in 64-bit timestamp format.
            get => ReadUtcTime(ReferenceOffset);
        }

        // REVIEW(code): UTC or not?
        public DateTime OriginateTime
        {
            // Originate Timestamp: This is the time at which the request departed
            // the client for the server, in 64-bit timestamp format.
            get => ReadUtcTime(OriginateOffset);
        }

        public DateTime ReceiveTime
        {
            // Receive Timestamp: This is the time at which the request arrived at
            // the server, in 64-bit timestamp format.
            get => ReadUtcTime(ReceiveOffset);
        }

        public DateTime TransmitTime
        {
            // Transmit Timestamp: This is the time at which the reply departed the
            // server for the client, in 64-bit timestamp format.
            get => ReadUtcTime(TransmitOffset);
        }
    }

    public partial class Rfc2030Message // NTP Timestamp Format
    {
        /* RFC 1305
        Since NTP timestamps are cherished data and, in fact, represent the main
        product of the protocol, a special timestamp format has been
        established. NTP timestamps are represented as a 64-bit unsigned fixed-
        point number, in seconds relative to 0h on 1 January 1900. The integer
        part is in the first 32 bits and the fraction part in the last 32 bits.
        This format allows convenient multiple-precision arithmetic and
        conversion to Time Protocol representation (seconds), but does
        complicate the conversion to ICMP Timestamp message representation
        (milliseconds). The precision of this representation is about 200
        picoseconds, which should be adequate for even the most exotic
        requirements.

        Timestamps are determined by copying the current value of the local
        clock to a timestamp when some significant event, such as the arrival of
        a message, occurs. In order to maintain the highest accuracy, it is
        important that this be done as close to the hardware or software driver
        associated with the event as possible. In particular, departure
        timestamps should be redetermined for each link-level retransmission. In
        some cases a particular timestamp may not be available, such as when the
        host is rebooted or the protocol first starts up. In these cases the 64-
        bit field is set to zero, indicating the value is invalid or undefined.

        Note that since some time in 1968 the most significant bit (bit 0 of the
        integer part) has been set and that the 64-bit field will overflow some
        time in 2036. Should NTP be in use in 2036, some external means will be
        necessary to qualify time relative to 1900 and time relative to 2036
        (and other multiples of 136 years). Timestamped data requiring such
        qualification will be so precious that appropriate means should be
        readily available. There will exist an 200-picosecond interval,
        henceforth ignored, every 136 years when the 64-bit field will be zero
        and thus considered invalid.
         */

        private DateTime ReadUtcTime(byte offset)
        {
            ulong milliseconds = ReadTimestamp(offset);

            return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                + TimeSpan.FromMilliseconds(milliseconds);
        }

        private ulong ReadTimestamp(byte offset)
        {
            ulong intPart = 0;
            for (int i = 0; i <= 3; i++)
            {
                intPart = 256 * intPart + _bin[offset + i];
            }

            ulong fracPart = 0;
            for (int i = 4; i <= 7; i++)
            {
                fracPart = 256 * fracPart + _bin[offset + i];
            }

            return 1000 * intPart + 1000 * fracPart / 0x100000000L;
        }

        //internal static void WriteTimestamp(byte[] rawdata, byte offset, DateTime date)
        //{
        //    var epoch = new DateTime(1900, 1, 1, 0, 0, 0);

        //    ulong milliseconds = (ulong)(date - epoch).TotalMilliseconds;
        //    ulong intpart = milliseconds / 1000;
        //    ulong fractpart = ((milliseconds % 1000) * 0x100000000L) / 1000;

        //    ulong temp = intpart;
        //    for (int i = 3; i >= 0; i--)
        //    {
        //        rawdata[offset + i] = (byte)(temp % 256);
        //        temp /= 256;
        //    }

        //    temp = fractpart;
        //    for (int i = 7; i >= 4; i--)
        //    {
        //        rawdata[offset + i] = (byte)(temp % 256);
        //        temp /= 256;
        //    }
        //}
    }
}