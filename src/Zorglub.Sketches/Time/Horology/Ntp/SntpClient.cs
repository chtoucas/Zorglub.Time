// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using static Zorglub.Time.Core.TemporalConstants;

    public sealed partial class SntpClient
    {
        private const int DataLength = 48;

        private const int TransmitTimestampOffset = 40;

        public const int DefaultVersion = 3;

        public const string DefaultHost = "pool.ntp.org";

        public const int DefaultPort = 123;

        public const int DefaultReceiveTimeout = 1000;

        private readonly IRandomProvider _randomProvider = new DefaultRandomProvider();

        public SntpClient()
        {
            Endpoint = new DnsEndPoint(DefaultHost, DefaultPort);
        }

        // Host name or IP address of the NTP server.
        public SntpClient(string host)
        {
            Endpoint = new DnsEndPoint(host, DefaultPort);
        }

        public SntpClient(EndPoint endpoint)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
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

        public IRandomProvider RandomProvider
        {
            get => _randomProvider;
            init => _randomProvider = value ?? throw new ArgumentNullException(nameof(value));
        }

        private EndPoint Endpoint { get; }

        private byte FirstByte
        {
            // CIL code size = XXX bytes <= 32 bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                const int ClientMode = 3;

                // Initialize the first byte to: LI = 0, VN = 3 or 4, Mode = 3.
                // Version 3: 00 011 011 (0x1b)
                // Version 4: 00 100 011 (0x23)
                return (byte)((Version << 3) | ClientMode);
            }
        }

        [Pure]
        public SntpResponse Query()
        {
            var buf = new byte[DataLength].AsSpan();
            buf[0] = FirstByte;

            // REVIEW(code): use UpdClient?

            // A system clock is not monotonic. We don't write
            // > var responseTime = DateTime.UtcNow;
            // because the system clock may be synchronized in the mean time.
            var stopwatch = Stopwatch.StartNew();

            using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = ReceiveTimeout,
            };

            sock.Connect(Endpoint);

            // Start time on this side of the wire.
            var requestTime = DateTime.UtcNow;
            long requestTicks = stopwatch.ElapsedTicks;

            // Randomize the timestamp, then write the result into the buffer.
            var requestTimestamp =
                Timestamp64.FromDateTime(requestTime)
                    .RandomizeSubMilliseconds(RandomProvider.NextInt32());
            requestTimestamp.WriteTo(buf, TransmitTimestampOffset);

            sock.Send(buf);
            int len = sock.Receive(buf);

            long responseTicks = stopwatch.ElapsedTicks;

            //sock.Close();

            // Ensure that the response is complete.
            if (len < DataLength) Throw();

            var rsp = ReadReply(buf);
            CheckReply(rsp, requestTimestamp);

            // Elapsed ticks during the query on this side of the wire.
            var elapsedTicks = responseTicks - requestTicks;
            // End time on this side of the wire.
            var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
            rsp.DestinationTimestamp = Timestamp64.FromDateTime(responseTime);

            return rsp;
        }

        [Pure]
        public async Task<SntpResponse> QueryAsync(CancellationToken token = default)
        {
            var bytes = new byte[DataLength];
            bytes[0] = FirstByte;

            var stopwatch = Stopwatch.StartNew();

            using var sock = new Socket(Endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = ReceiveTimeout,
            };

            await sock.ConnectAsync(Endpoint).ConfigureAwait(false);

            var requestTime = DateTime.UtcNow;
            long requestTicks = stopwatch.ElapsedTicks;

            var requestTimestamp =
                Timestamp64.FromDateTime(requestTime)
                    .RandomizeSubMilliseconds(RandomProvider.NextInt32());
            requestTimestamp.WriteTo(bytes, TransmitTimestampOffset);

            var buf = new ArraySegment<byte>(bytes);
            await sock.SendAsync(buf, SocketFlags.None, token).ConfigureAwait(false);
            int len = await sock.ReceiveAsync(buf, SocketFlags.None, token).ConfigureAwait(false);

            long responseTicks = stopwatch.ElapsedTicks;

            //sock.Close();

            if (len < DataLength) Throw();

            var rsp = ReadReply(bytes);
            CheckReply(rsp, requestTimestamp);

            var elapsedTicks = responseTicks - requestTicks;
            var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
            rsp.DestinationTimestamp = Timestamp64.FromDateTime(responseTime);

            return rsp;
        }

        [Pure]
        private static SntpResponse ReadReply(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= DataLength);

            var rsp = new SntpResponse
            {
                // First byte.
                LeapIndicator = ReadLeapIndicator((buf[0] >> 6) & 3),
                Version = (buf[0] >> 3) & 7,
                Mode = ReadMode(buf[0] & 7),
                Stratum = ReadStratum(buf[1]),
                // Signed 8-bit integer = log_2(poll)
                // Range = [4..17], 16 (2^4) seconds <= poll <= 131_072 (2^17) seconds.
                PollInterval = ReadSByte(buf[2]),
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
                Precision = ReadSByte(buf[3]),
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

            // REVIEW(code): PollInterval and Precision.
            // https://en.wikipedia.org/wiki/Two%27s_complement
            static int ReadSByte(byte v) => v > 127 ? v - 256 : v;
        }

        // TODO(code): move this to ThrowHelpers.
        private static void Throw(string message = "Bad reply from the NTP server") =>
            throw new NtpException(message);

        // TODO(code): Error checks in client-server model, see RFC 4330, section 5.
        private void CheckReply(SntpResponse rsp, Timestamp64 requestTimestamp)
        {
            if (rsp.LeapIndicator == LeapIndicator.Invalid) Throw();
            if (rsp.Version != Version) Throw();
            if (rsp.Mode != NtpMode.Server) Throw();
            if (rsp.Stratum == NtpStratum.Reserved
                || rsp.Stratum == NtpStratum.Unsynchronized
                || rsp.Stratum == NtpStratum.Invalid) Throw();

            if (rsp.OriginateTimestamp != requestTimestamp) Throw();
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
                case NtpStratum.Unavailable:
                case NtpStratum.PrimaryReference:
                    // Four-character ASCII string, left justified and zero
                    // padded to 32 bits.
                    return Encoding.ASCII.GetString(buf);

                case NtpStratum.SecondaryReference:
                    return rsp.Version == 3
                        // NTP Version 3 only supports IPv4.
                        ? FormattableString.Invariant($"{buf[0]}.{buf[1]}.{buf[2]}.{buf[3]}")
                        // It's complicated...
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
