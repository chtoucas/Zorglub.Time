#pragma warning disable IDE0073 // Require file header (Style)

/*
*The C# SNTP client used by Microsoft in .NET Micro Framework
 *
 * Copyright (C)2001-2019 Valer BOCAN, PhD <valer@bocan.ro>
 * Last modified: August 3rd, 2019
 * Historically, this has been the very first piece of C# code I've written.
 *
 * Comments, bugs and suggestions are welcome.
 *
 * Update history:
 *
 * August 3rd, 2019
 * - Removed SNTP_WindowsMobile compilation directive
 * - Fixed a few "obsolete" warnings by using TimeZoneInfo
 * - Removed Windows-specific ability to set the time of the local computer
 * - Various code enhancements, mostly cosmetic
 *
 * November 20, 2011
 * - Added the SNTP_WindowsMobile compilation directive for discrimination between Windows Desktop and Windows Mobile
 *
 * - Altered Connect() method to provide a socket timeout (Jason Garrett - jason.garrett@hotmail.com)
 *    - Credit goes to Kyle Jones who posted this improved Connect() method on
 *      http://objectmix.com/dotnet/98919-socket-receive-timeout-compact-framework.html
 *      on 10 Mar 2009.
 * - Added <summary> tags to class methods and attributes
 *
 * May 2, 2011
 * - RoundTripDelay and LocalClockOffset now return a double instead of an integer to avoid overflows
 *   when the computer clock is way off.
 * - Added the DllImport directive for Windows Mobile 6.0
 *   Thanks to Andre Rippstein <andre@rippstein.net>
 *
 * September 20, 2003
 * - Renamed the class from NTPClient to SNTPClient.
 * - Fixed the RoundTripDelay and LocalClockOffset properties.
 *   Thanks go to DNH <dnharris@csrlink.net>.
 * - Fixed the PollInterval property.
 *   Thanks go to Jim Hollenhorst <hollenho@attbi.com>.
 * - Changed the ReceptionTimestamp variable to DestinationTimestamp to follow the standard
 *   more closely.
 * - Precision property is now shown is seconds rather than milliseconds in the
 *   ToString method.
 *
 * May 28, 2002
 * - Fixed a bug in the Precision property and the SetTime function.
 *   Thanks go to Jim Hollenhorst <hollenho@attbi.com>.
 *
 * March 14, 2001
 * - First public release.
 */

#pragma warning disable CA1305 // Specify IFormatProvider

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// SNTPClient is a C# class designed to connect to time servers on the Internet and
    /// fetch the current date and time. The implementation of the protocol is based on the RFC 2030.
    ///
    /// Public class members:
    ///
    /// LeapIndicator - Warns of an impending leap second to be inserted/deleted in the last
    /// minute of the current day. (See the _LeapIndicator enum)
    ///
    /// VersionNumber - Version number of the protocol (3 or 4).
    ///
    /// Mode - Returns mode. (See the _Mode enum)
    ///
    /// Stratum - Stratum of the clock. (See the _Stratum enum)
    ///
    /// PollInterval - Maximum interval between successive messages
    ///
    /// Precision - Precision of the clock
    ///
    /// RootDelay - Round trip time to the primary reference source.
    ///
    /// RootDispersion - Nominal error relative to the primary reference source.
    ///
    /// ReferenceID - Reference identifier (either a 4 character string or an IP address).
    ///
    /// ReferenceTimestamp - The time at which the clock was last set or corrected.
    ///
    /// OriginateTimestamp - The time at which the request departed the client for the server.
    ///
    /// ReceiveTimestamp - The time at which the request arrived at the server.
    ///
    /// Transmit Timestamp - The time at which the reply departed the server for client.
    ///
    /// RoundTripDelay - The time between the departure of request and arrival of reply.
    ///
    /// LocalClockOffset - The offset of the local clock relative to the primary reference
    /// source.
    ///
    /// Initialize - Sets up data structure and prepares for connection.
    ///
    /// Connect - Connects to the time server and populates the data structure.
    ///	It can also update the system time.
    ///
    /// ToString - Returns a string representation of the object.
    ///
    /// -----------------------------------------------------------------------------
    /// Structure of the standard NTP header (as described in RFC 2030)
    ///                       1                   2                   3
    ///   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |LI | VN  |Mode |    Stratum    |     Poll      |   Precision   |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                          Root Delay                           |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                       Root Dispersion                         |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                     Reference Identifier                      |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                   Reference Timestamp (64)                    |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                   Originate Timestamp (64)                    |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                    Receive Timestamp (64)                     |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                    Transmit Timestamp (64)                    |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                 Key Identifier (optional) (32)                |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                                                               |
    ///  |                 Message Digest (optional) (128)               |
    ///  |                                                               |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///
    /// -----------------------------------------------------------------------------
    ///
    /// SNTP Timestamp Format (as described in RFC 2030)
    ///                         1                   2                   3
    ///     0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |                           Seconds                             |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |                  Seconds Fraction (0-padded)                  |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///
    /// </summary>

    public sealed partial class SntpClient
    {
        // Data Structure Length
        private const int DataLength = 48;

        // Offset constants for timestamps in the data structure
        private const int ReferenceIdOffset = 12;
        private const int ReferenceOffset = 16;
        private const int OriginateOffset = 24;
        private const int ReceiveOffset = 32;
        private const int TransmitOffset = 40;

        // Data Structure (as described in RFC 2030)
        private readonly byte[] _data = new byte[DataLength];

        /// <summary>
        /// Warns of an impending leap second to be inserted/deleted in the last
        /// minute of the current day.
        /// </summary>
        // Two most significant bits
        public SntpLeapIndicator LeapIndicator
        {
            get
            {
                int val = _data[0] >> 6;
                return val switch
                {
                    0 => SntpLeapIndicator.NoWarning,
                    1 => SntpLeapIndicator.LastMinute61,
                    2 => SntpLeapIndicator.LastMinute59,
                    // 3
                    _ => SntpLeapIndicator.Alarm,
                };
            }
        }

        /// <summary>
        /// Version number of the protocol (3 or 4).
        /// </summary>
        // Bits 3 - 5
        public int Version => (_data[0] & 0x38) >> 3;

        /// <summary>
        /// Returns mode. (See the _Mode enum)
        /// </summary>
        // Bits 0 - 3
        public SntpMode Mode
        {
            get
            {
                int val = _data[0] & 0x7;
                return val switch
                {
                    1 => SntpMode.SymmetricActive,
                    2 => SntpMode.SymmetricPassive,
                    3 => SntpMode.Client,
                    4 => SntpMode.Server,
                    5 => SntpMode.Broadcast,
                    // 0, 6, 7
                    _ => SntpMode.Unknown,
                };
            }
        }

        /// <summary>
        /// Stratum of the clock. (See the _Stratum enum)
        /// </summary>
        public SntpStratum Stratum
        {
            get
            {
                int val = _data[1];
                if (val == 0) return SntpStratum.Unspecified;
                else
                    if (val == 1) return SntpStratum.PrimaryReference;
                else
                        if (val <= 15) return SntpStratum.SecondaryReference;
                else
                    return SntpStratum.Reserved;
            }
        }

        /// <summary>
        /// Maximum interval (seconds) between successive messages
        /// </summary>
        // Thanks to Jim Hollenhorst <hollenho@attbi.com>
        public int PollInterval => (int)Math.Pow(2, _data[2]);

        /// <summary>
        /// Precision (in seconds) of the clock
        /// </summary>
        // Thanks to Jim Hollenhorst <hollenho@attbi.com>
        public double Precision => Math.Pow(2, _data[3]);

        /// <summary>
        /// Round trip time (in milliseconds) to the primary reference source.
        /// </summary>
        public double RootDelay
        {
            get
            {
                int temp = 256 * (256 * (256 * _data[4] + _data[5]) + _data[6]) + _data[7];
                return 1000 * ((double)temp / 0x10000);
            }
        }

        /// <summary>
        /// Nominal error (in milliseconds) relative to the primary reference source.
        /// </summary>
        public double RootDispersion
        {
            get
            {
                int temp = 256 * (256 * (256 * _data[8] + _data[9]) + _data[10]) + _data[11];
                return 1000 * ((double)temp / 0x10000);
            }
        }

        /// <summary>
        /// Reference identifier (either a 4 character string or an IP address)
        /// </summary>
        public string ReferenceId
        {
            get
            {
                string val = "";
                switch (Stratum)
                {
                    case SntpStratum.Unspecified:
                        goto case SntpStratum.PrimaryReference;
                    case SntpStratum.PrimaryReference:
                        val += (char)_data[ReferenceIdOffset + 0];
                        val += (char)_data[ReferenceIdOffset + 1];
                        val += (char)_data[ReferenceIdOffset + 2];
                        val += (char)_data[ReferenceIdOffset + 3];
                        break;
                    case SntpStratum.SecondaryReference:
                        switch (Version)
                        {
                            case 3:	// Version 3, Reference ID is an IPv4 address
                                string Address = _data[ReferenceIdOffset + 0].ToString() + "." +
                                                 _data[ReferenceIdOffset + 1].ToString() + "." +
                                                 _data[ReferenceIdOffset + 2].ToString() + "." +
                                                 _data[ReferenceIdOffset + 3].ToString();
                                IPHostEntry Host = Dns.GetHostEntry(Address);
                                val = Host.HostName + " (" + Address + ")";
                                break;
                            case 4: // Version 4, Reference ID is the timestamp of last update
                                DateTime time = ComputeDate(GetMilliSeconds(ReferenceIdOffset));
                                // Take care of the time zone
                                TimeSpan offspan = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                                val = (time + offspan).ToString();
                                break;
                            default:
                                val = "N/A";
                                break;
                        }
                        break;
                }

                return val;
            }
        }

        /// <summary>
        /// The time at which the clock was last set or corrected
        /// </summary>
        public DateTime ReferenceTime
        {
            get
            {
                DateTime time = ComputeDate(GetMilliSeconds(ReferenceOffset));
                // Take care of the time zone
                TimeSpan offspan = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                return time + offspan;
            }
        }

        /// <summary>
        /// The time (T1) at which the request departed the client for the server
        /// </summary>
        public DateTime OriginateTime => ComputeDate(GetMilliSeconds(OriginateOffset));

        /// <summary>
        /// The time (T2) at which the request arrived at the server
        /// </summary>
        public DateTime ReceiveTime
        {
            get
            {
                DateTime time = ComputeDate(GetMilliSeconds(ReceiveOffset));
                // Take care of the time zone
                TimeSpan offspan = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                return time + offspan;
            }
        }

        /// <summary>
        /// The time (T3) at which the reply departed the server for client
        /// </summary>
        public DateTime TransmitTime
        {
            get
            {
                DateTime time = ComputeDate(GetMilliSeconds(TransmitOffset));
                // Take care of the time zone
                TimeSpan offspan = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                return time + offspan;
            }
            set
            {
                SetDate(TransmitOffset, value);
            }
        }

        /// <summary>
        /// Destination Timestamp (T4)
        /// </summary>
        private DateTime DestinationTime;

        /// <summary>
        /// The time (in milliseconds) between the departure of request and arrival of reply
        /// </summary>
        public double RoundTripDelay
        {
            get
            {
                // Thanks to DNH <dnharris@csrlink.net>
                TimeSpan span = DestinationTime - OriginateTime - (ReceiveTime - TransmitTime);
                return span.TotalMilliseconds;
            }
        }

        /// <summary>
        /// The offset (in milliseconds) of the local clock relative to the primary reference source
        /// </summary>
        public double LocalClockOffset
        {
            get
            {
                // Thanks to DNH <dnharris@csrlink.net>
                TimeSpan span = ReceiveTime - OriginateTime + (TransmitTime - DestinationTime);
                return span.TotalMilliseconds / 2;
            }
        }
    }

    public sealed partial class SntpClient
    {
        /// <summary>
        /// Connects to the time server and populates the data structure.
        ///	It can also update the system time.
        /// </summary>
        /// <param name="host">Address of the NTP server.</param>
        /// <param name="timeOut">Time in milliseconds after which the method returns.</param>
        public SntpResponse Query(string host, int timeOut)
        {
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                sock.Bind(new IPEndPoint(IPAddress.Any, 123));

                var hostEntry = Dns.GetHostEntry(host);
                EndPoint endpoint = new IPEndPoint(hostEntry.AddressList[0], 123);

                var data = InitializeData();

                // Timeout code
                bool received = false;
                int elapsedTime = 0;

                var transmitTime = DateTime.Now;

                while (!received && elapsedTime < timeOut)
                {
                    sock.SendTo(data, data.Length, SocketFlags.None, endpoint);

                    // Check if data has been received by the listening socket and is available to be read
                    if (sock.Available > 0)
                    {
                        int len = sock.ReceiveFrom(data, ref endpoint);
                        if (!IsResponseValid()) throw new SocketException();
                        received = true;
                        break;
                    }

                    // Wait a bit
                    Thread.Sleep(500);
                    elapsedTime += 500;
                }

                if (!received) throw new TimeoutException("Host did not respond.");

                return SntpResponse.Create(data, transmitTime, DateTime.Now);
            }
            finally
            {
                sock.Close();
            }

            static byte[] InitializeData()
            {
                var data = new byte[DataLength];
                // Set version number to 4 and Mode to 3 (client).
                data[0] = 0x1b;
                for (int i = 1; i < DataLength; i++) { data[i] = 0; }

                return data;
            }
        }
    }

    public sealed partial class SntpClient
    {
        /// <summary>
        /// Compute date, given the number of milliseconds since January 1, 1900
        /// </summary>
        private static DateTime ComputeDate(ulong milliseconds) =>
            new DateTime(1900, 1, 1) + TimeSpan.FromMilliseconds(milliseconds);

        /// <summary>
        /// Compute the number of milliseconds, given the offset of a 8-byte array
        /// </summary>
        private ulong GetMilliSeconds(byte offset)
        {
            ulong intpart = 0, fractpart = 0;

            for (int i = 0; i <= 3; i++)
            {
                intpart = 256 * intpart + _data[offset + i];
            }
            for (int i = 4; i <= 7; i++)
            {
                fractpart = 256 * fractpart + _data[offset + i];
            }
            ulong milliseconds = intpart * 1000 + fractpart * 1000 / 0x100000000L;
            return milliseconds;
        }

        /// <summary>
        /// Set the date part of the SNTP data
        /// </summary>
        /// <param name="offset">Offset at which the date part of the SNTP data is</param>
        /// <param name="date">The date</param>
        private void SetDate(byte offset, DateTime date)
        {
            var StartOfCentury = new DateTime(1900, 1, 1, 0, 0, 0);	// January 1, 1900 12:00 AM

            ulong milliseconds = (ulong)(date - StartOfCentury).TotalMilliseconds;
            ulong intpart = milliseconds / 1000;
            ulong fractpart = ((milliseconds % 1000) * 0x100000000L) / 1000;

            ulong temp = intpart;
            for (int i = 3; i >= 0; i--)
            {
                _data[offset + i] = (byte)(temp % 256);
                temp /= 256;
            }

            temp = fractpart;
            for (int i = 7; i >= 4; i--)
            {
                _data[offset + i] = (byte)(temp % 256);
                temp /= 256;
            }
        }

        /// <summary>
        /// Returns true if received data is valid and if comes from a NTP-compliant time server.
        /// </summary>
        private bool IsResponseValid()
        {
            if (_data.Length < DataLength || Mode != SntpMode.Server)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
