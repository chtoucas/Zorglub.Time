#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    using System.Net;

#pragma warning disable CA1305 // Specify IFormatProvider

    internal sealed partial class Rfc2030Message
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

        private readonly byte[] _rawdata;

        public Rfc2030Message()
        {
            // Set version number to 4 and Mode to 3 (client).
            var data = new byte[DataLength];
            data[0] = 0x1b;
            _rawdata = data;
        }

        public Rfc2030Message(byte[] rawdata)
        {
            Requires.NotNull(rawdata);
            if (rawdata.Length < DataLength) throw new ArgumentException("", nameof(rawdata));

            _rawdata = rawdata;

            if (Mode != SntpMode.Server) throw new ArgumentException("", nameof(rawdata));
        }

        internal static Rfc2030Message Empty => new();

        internal byte[] RawData => _rawdata;
    }

    internal partial class Rfc2030Message // First line
    {
        public SntpLeapIndicator LeapIndicator
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
            get
            {
                int x = _rawdata[0] >> 6;

                return x switch
                {
                    0 => SntpLeapIndicator.NoWarning,
                    1 => SntpLeapIndicator.PositiveLeapSecond,
                    2 => SntpLeapIndicator.NegativeLeapSecond,
                    3 => SntpLeapIndicator.Alarm,

                    _ => Throw.Unreachable<SntpLeapIndicator>(),
                };
            }
        }

        public int Version
        {
            // Version Number(VN): This is a three-bit integer indicating the
            // NTP/SNTP version number. The version number is 3 for Version 3 (IPv4
            // only) and 4 for Version 4 (IPv4, IPv6 and OSI). If necessary to
            // distinguish between IPv4, IPv6 and OSI, the encapsulating context
            // must be inspected.
            get => (_rawdata[0] & 0x38) >> 3;
        }

        public SntpMode Mode
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
            get
            {
                int x = _rawdata[0] & 0x7;

                return x switch
                {
                    0 => SntpMode.Special0,
                    1 => SntpMode.SymmetricActive,
                    2 => SntpMode.SymmetricPassive,
                    3 => SntpMode.Client,
                    4 => SntpMode.Server,
                    5 => SntpMode.Broadcast,
                    6 => SntpMode.Special6,
                    7 => SntpMode.Special7,

                    _ => Throw.Unreachable<SntpMode>(),
                };
            }
        }

        public SntpStratum Stratum
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
            get
            {
                byte x = _rawdata[1];

                return x switch
                {
                    0 => SntpStratum.Unspecified,
                    1 => SntpStratum.PrimaryReference,
                    <= 15 => SntpStratum.SecondaryReference,
                    _ => SntpStratum.Special
                };
            }
        }

        public int PollInterval
        {
            // Poll Interval: This is an eight-bit signed integer indicating the
            // maximum interval between successive messages, in seconds to the
            // nearest power of two. The values that can appear in this field
            // presently range from 4 (16 s) to 14 (16284 s); however, most
            // applications use only the sub-range 6 (64 s) to 10 (1024 s).
            get => (int)Math.Pow(2, _rawdata[2]);
        }

        public double Precision
        {
            // Precision: This is an eight-bit signed integer indicating the
            // precision of the local clock, in seconds to the nearest power of two.
            // The values that normally appear in this field range from -6 for
            // mains-frequency clocks to -20 for microsecond clocks found in some
            // workstations.
            get => Math.Pow(2, _rawdata[3]);
        }
    }

    internal partial class Rfc2030Message
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
                int x = 256 * (256 * (256 * _rawdata[4] + _rawdata[5]) + _rawdata[6]) + _rawdata[7];
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
                int x = 256 * (256 * (256 * _rawdata[8] + _rawdata[9]) + _rawdata[10]) + _rawdata[11];
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
                    case SntpStratum.Unspecified:
                    case SntpStratum.PrimaryReference:
                        r += (char)_rawdata[ReferenceIdOffset + 0];
                        r += (char)_rawdata[ReferenceIdOffset + 1];
                        r += (char)_rawdata[ReferenceIdOffset + 2];
                        r += (char)_rawdata[ReferenceIdOffset + 3];
                        break;
                    case SntpStratum.SecondaryReference:
                        switch (Version)
                        {
                            case 3:	// Version 3, Reference ID is an IPv4 address.
                                string addr = _rawdata[ReferenceIdOffset + 0].ToString() + "." +
                                                 _rawdata[ReferenceIdOffset + 1].ToString() + "." +
                                                 _rawdata[ReferenceIdOffset + 2].ToString() + "." +
                                                 _rawdata[ReferenceIdOffset + 3].ToString();
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

    internal partial class Rfc2030Message // NTP Timestamp Format
    {
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
                intPart = 256 * intPart + _rawdata[offset + i];
            }

            ulong fracPart = 0;
            for (int i = 4; i <= 7; i++)
            {
                fracPart = 256 * fracPart + _rawdata[offset + i];
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
