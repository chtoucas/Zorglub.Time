// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Net;
    using System.Net.Sockets;

    public sealed partial class SntpClient
    {
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

            var req = new byte[Rfc4330.DataLength];
            // Initialize the first byte to: LI = 0, VN = 3, Mode = 3.
            req[0] = 0x1B;
            // TODO(code): Initialize TransmitTimestamp to DateTime.UtcNow.
            sock.Send(req);

            var rsp = new byte[160];
            int len = sock.Receive(rsp);
            var destinationTime = DateTime.UtcNow;

            sock.Close();

            return Rfc4330.ReadResponse(rsp.AsSpan(), destinationTime);
        }
    }
}
