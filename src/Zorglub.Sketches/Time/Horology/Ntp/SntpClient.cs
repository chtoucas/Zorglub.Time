// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Net;
    using System.Net.Sockets;

    using Zorglub.Bulgroz.Externals.BocanNtp;

    public sealed class SntpClient
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

        public SntpResponse Query()
        {
            using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = ReceiveTimeout,
            };

            sock.Connect(_endpoint);

            var req = Rfc2030Message.Request(transmitTime: DateTime.UtcNow);
            sock.Send(req.ToBinary());

            var bin = new byte[160];
            int len = sock.Receive(bin);
            var destinationTime = DateTime.UtcNow;

            sock.Close();

            var rsp = Rfc2030Message.Response(bin);

            return SntpResponse.Create(rsp, destinationTime);
        }
    }
}
