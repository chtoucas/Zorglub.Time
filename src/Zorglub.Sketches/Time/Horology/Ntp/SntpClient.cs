// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public sealed partial class SntpClient
    {
        private const int DataLength = 48;

        public const int DefaultVersion = 3;

        public const string DefaultHost = "pool.ntp.org";

        public const int DefaultPort = 123;

        public const int DefaultReceiveTimeout = 1000;

        // To be improved...
        private readonly Random _random = new();

        private readonly EndPoint _endpoint;

        public SntpClient()
        {
            _endpoint = new DnsEndPoint(DefaultHost, DefaultPort);
        }

        // Host name or IP address of the NTP server.
        public SntpClient(string host)
        {
            _endpoint = new DnsEndPoint(host, DefaultPort);
        }

        public SntpClient(EndPoint endpoint)
        {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        private int _version = DefaultVersion;
        /// <summary>
        /// Gets or initializes the NTP version.
        /// </summary>
        public int Version
        {
            get => _version;
            init
            {
                if (value != 3 && value != 4) ThrowHelpers.ArgumentOutOfRange(nameof(value));
                _version = value;
            }
        }

        // Receive timeout in milliseconds; see Socket.ReceiveTimeout.
        public int ReceiveTimeout { get; init; } = DefaultReceiveTimeout;

        [Pure]
        public SntpResponse Query()
        {
            const int
                ClientMode = 3,
                TransmitTimestampOffset = 40;

            var buf = new byte[DataLength].AsSpan();
            // Initialize the first byte to: LI = 0, VN = 3 or 4, Mode = 3.
            buf[0] = (byte)(ClientMode | (Version << 3));

            using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = ReceiveTimeout,
            };

            sock.Connect(_endpoint);

            // Initialize TransmitTimestamp.
            var clientTransmitTimestamp =
                Timestamp64
                    .FromDateTime(DateTime.UtcNow)
                    .RandomizeSubmilliseconds(_random);
            clientTransmitTimestamp.WriteTo(buf[TransmitTimestampOffset..]);

            sock.Send(buf);
            int len = sock.Receive(buf);
            var destinationTime = DateTime.UtcNow;

            sock.Close();

            var rsp = ReadReply(buf);
            rsp.DestinationTimestamp = Timestamp64.FromDateTime(destinationTime);
            CheckReply(rsp, clientTransmitTimestamp);
            return rsp;
        }

        [Pure]
        private static SntpResponse ReadReply(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= DataLength);

            // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
            // |LI | VN  |Mode |    Stratum    |     Poll      |   Precision   |
            // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
            // |                          Root Delay                           |
            // |                       Root Dispersion                         |
            // |                     Reference Identifier                      |
            // |                   Reference Timestamp (64)                    |
            // |                   Originate Timestamp (64)                    |
            // |                    Receive Timestamp (64)                     |
            // |                    Transmit Timestamp (64)                    |
            // |                 Key Identifier (optional) (32)                |
            // |                 Message Digest (optional) (128)               |
            // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
            var rsp = new SntpResponse
            {
                // First byte.
                LeapIndicator = ReadLeapIndicator((buf[0] >> 6) & 3),
                Version = (buf[0] >> 3) & 7,
                Mode = ReadMode(buf[0] & 7),
                Stratum = ReadStratum(buf[1]),
                PollInterval = buf[2],
                Precision = buf[3],
                RootDelay = Duration64.ReadFrom(buf[4..]),
                RootDispersion = Duration64.ReadFrom(buf[8..]),
                // Bytes 12 to 15; see Reference below.
                ReferenceTimestamp = Timestamp64.ReadFrom(buf[16..]),
                OriginateTimestamp = Timestamp64.ReadFrom(buf[24..]),
                ReceiveTimestamp = Timestamp64.ReadFrom(buf[32..]),
                TransmitTimestamp = Timestamp64.ReadFrom(buf[40..])
            };

            rsp.Reference = ReadReference(buf[12..16], rsp);

            return rsp;
        }

        // TODO(code): move this to ThrowHelpers.
        private static void Throw(string message = "Bad reply from the NTP server") =>
            throw new NtpException(message);

        // TODO(code): Error checks in client-server model, see RFC 4330, section 5.
        private void CheckReply(SntpResponse rsp, Timestamp64 clientTransmitTimestamp)
        {
            if (rsp.LeapIndicator == LeapIndicator.Invalid) Throw();
            if (rsp.Version != Version) Throw();
            if (rsp.Mode != NtpMode.Server) Throw();
            if (rsp.Stratum == NtpStratum.Reserved
                || rsp.Stratum == NtpStratum.Unsynchronized
                || rsp.Stratum == NtpStratum.Invalid) Throw();

            if (rsp.OriginateTimestamp != clientTransmitTimestamp) Throw();
            if (rsp.TransmitTimestamp == Timestamp64.Zero) Throw();
        }
    }

    public partial class SntpClient // Helpers
    {
        [Pure]
        private static LeapIndicator ReadLeapIndicator(int x) =>
            // 2-bit number.
            x switch
            {
                0 => LeapIndicator.NoWarning,
                1 => LeapIndicator.PositiveLeapSecond,
                2 => LeapIndicator.NegativeLeapSecond,
                3 => LeapIndicator.Unknown,

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
        private static string? ReadReference(ReadOnlySpan<byte> buf, SntpResponse rsp)
        {
            Debug.Assert(rsp != null);
            Debug.Assert(buf.Length == 4);

            // This is a 32-bit bitstring identifying the particular reference
            // source.
            switch (rsp.Stratum)
            {
                // Four-character ASCII string, left justified and zero padded
                // to 32 bits.
                case NtpStratum.Unavailable:
                case NtpStratum.PrimaryReference:
                    return Encoding.ASCII.GetString(buf);

                // In NTP Version 4, it's complicated...
                // RFC 2030
                // > In NTP Version 3 secondary servers, this is the 32-bit IPv4
                // > address of the reference source. In NTP Version 4 secondary
                // > servers, this is the low order 32 bits of the latest
                // > transmit timestamp of the reference source.
                // RFC 4330
                // > For IPv4 secondary servers, the value is the 32-bit
                // > IPv4 address of the synchronization source.
                // > For IPv6 and OSI secondary servers, the value is
                // > the first 32 bits of the MD5 hash of the IPv6 or
                // > NSAP address of the synchronization source.
                // RFC 5905
                // > Above stratum 1 (secondary servers and clients): this is the
                // > reference identifier of the server and can be used to detect
                // > timing loops. If using the IPv4 address family, the
                // > identifier is the four-octet IPv4 address. If using the IPv6
                // > address family, it is the first four octets of the MD5 hash
                // > of the IPv6 address. Note that, when using the IPv6 address
                // > family on an NTPv4 server with a NTPv3 client, the Reference
                // > Identifier field appears to be a random value and a timing
                // > loop might not be detected.
                case NtpStratum.SecondaryReference:
                    return rsp.Version == 3
                        // Easy case: NTP Version 3 only supports IPv4.
                        ? FormattableString.Invariant($"{buf[0]}.{buf[1]}.{buf[2]}.{buf[3]}")
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
