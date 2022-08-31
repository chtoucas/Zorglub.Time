// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Buffers.Binary;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public sealed partial class SntpClient
    {
        private const int DataLength = 48;

        public const string DefaultHost = "pool.ntp.org";

        public const int DefaultPort = 123;

        public const int DefaultReceiveTimeout = 1000;

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

        // Receive timeout in milliseconds; see Socket.ReceiveTimeout.
        public int ReceiveTimeout { get; init; } = DefaultReceiveTimeout;

        [Pure]
        public SntpResponse Query()
        {
            using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = ReceiveTimeout,
            };

            sock.Connect(_endpoint);

            var req = InitRequest();
            sock.Send(req);

            var data = new byte[160];
            int len = sock.Receive(data);
            var destinationTime = DateTime.UtcNow;

            sock.Close();

            var rsp = ReadReply(data.AsSpan(), destinationTime);
            if (rsp.Check() == false) throw new Exception();
            return rsp;

            static Span<byte> InitRequest()
            {
                var data = new byte[DataLength].AsSpan();
                // Initialize the first byte to: LI = 0, VN = 3, Mode = 3.
                data[0] = 3 | (3 << 3);
                // FIXME(code): Initialize TransmitTimestamp. Overflow, real UTC.
                //BinaryPrimitives.WriteInt64BigEndian(data[40..], ...);
                return data;
            }
        }

        [Pure]
        private static SntpResponse ReadReply(ReadOnlySpan<byte> buf, DateTime destinationTime)
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
                RootDelay = ReadDuration(buf[4..]),
                RootDispersion = ReadDuration(buf[8..]),
                // Bytes 12 to 15; see Reference below.
                ReferenceTimestamp = ReadTimestamp(buf[16..]),
                OriginateTimestamp = ReadTimestamp(buf[24..]),
                ReceiveTimestamp = ReadTimestamp(buf[32..]),
                TransmitTimestamp = ReadTimestamp(buf[40..]),
                DestinationTime = destinationTime,
            };

            rsp.Reference = ReadReference(buf[12..16], rsp);

            return rsp;
        }
    }

    public partial class SntpClient
    {
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
                6 => NtpMode.ReservedForNtpControlMessage,
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
                _ => NtpStratum.Reserved
            };

        [Pure]
        private static NtpTimestamp ReadTimestamp(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 8);

            uint secondOfEra = BinaryPrimitives.ReadUInt32BigEndian(buf);
            uint fractionalSecond = BinaryPrimitives.ReadUInt32BigEndian(buf[4..]);

            return new NtpTimestamp(secondOfEra, fractionalSecond);
        }

        [Pure]
        private static double ReadDuration(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 4);

            return (double)BinaryPrimitives.ReadInt32BigEndian(buf) / 0x10000;
        }

        [Pure]
        private static string ReadReference(ReadOnlySpan<byte> buf, SntpResponse rsp)
        {
            Debug.Assert(rsp != null);
            Debug.Assert(buf.Length == 4);

            return rsp.Stratum switch
            {
                NtpStratum.Unavailable
                or NtpStratum.PrimaryReference =>
                    // Four-character ASCII string, left justified and zero
                    // padded to 32 bits.
                    Encoding.ASCII.GetString(buf),

                NtpStratum.SecondaryReference =>
                    rsp.Version switch
                    {
                        // 32-bit IPv4 address of the reference source.
                        3 => FormattableString.Invariant($"{buf[0]}.{buf[1]}.{buf[2]}.{buf[3]}"),

                        // Low order 32 bits of the latest transmit timestamp of
                        // the reference source.
                        4 => throw new NotImplementedException(),
                        // TODO(code): low order 32 bits only!
                        // NtpTimestamp.FromBytes(buf).ToDateTime().ToString(CultureInfo.CurrentCulture),

                        _ => String.Empty
                    },

                _ => String.Empty,
            };
            ;
        }
    }
}
