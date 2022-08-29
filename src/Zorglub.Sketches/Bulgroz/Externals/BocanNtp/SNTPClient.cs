#pragma warning disable IDE0073 // Require file header (Style)

/*
 * Copyright (C)2001-2019 Valer BOCAN, PhD <valer@bocan.ro>
 */

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public sealed partial class SntpClient
    {
        public SntpClient(string host, int timeout)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            // FIXME(code): timeout > 0.
            Timeout = timeout;
        }

        // Address of the NTP server.
        public string Host { get; }

        // Time in milliseconds after which the method returns.
        public int Timeout { get; set; }
    }

    public sealed partial class SntpClient
    {
        public SntpResponse Query()
        {
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                sock.Bind(new IPEndPoint(IPAddress.Any, 123));

                var hostEntry = Dns.GetHostEntry(Host);
                EndPoint endpoint = new IPEndPoint(hostEntry.AddressList[0], 123);

                var data = Rfc2030Message.Empty.RawData;

                // Timeout code
                bool received = false;
                int elapsedTime = 0;

                var transmitTime = DateTime.Now;

                while (!received && elapsedTime < Timeout)
                {
                    sock.SendTo(data, data.Length, SocketFlags.None, endpoint);

                    // Check if data has been received by the listening socket
                    // and is available to be read.
                    if (sock.Available > 0)
                    {
                        int len = sock.ReceiveFrom(data, ref endpoint);
                        received = true;
                        break;
                    }

                    Thread.Sleep(500);
                    elapsedTime += 500;
                }

                if (!received) throw new TimeoutException();

                var destinationTime = DateTime.Now;

                return SntpResponse.Create(new Rfc2030Message(data), transmitTime, destinationTime);
            }
            finally
            {
                sock.Close();
            }
        }
    }
}
