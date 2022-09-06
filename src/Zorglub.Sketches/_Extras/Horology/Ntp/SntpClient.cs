// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using static Zorglub.Time.Core.TemporalConstants;

    // REVIEW(code): UpdClient? CancellationToken? ConfigureAwait(...)

    // Adapted from
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/SntpClient.java
    // See also
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/util/NtpTrustedTime.java
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/tests/coretests/src/android/net/sntp/

    public sealed partial class SntpClient
    {
        private const int DataLength = 48;
        private const int TransmitTimestampOffset = 40;

        /// <summary>
        /// Represents the default SNTP version.
        /// <para>This field is a constant equal to 4.</para>
        /// </summary>
        public const int DefaultVersion = 4;

        /// <summary>
        /// Represents the default SNTP host.
        /// <para>This field is a constant equal to "pool.ntp.org".</para>
        /// </summary>
        public const string DefaultHost = "pool.ntp.org";

        /// <summary>
        /// Represents the default SNTP port.
        /// <para>This field is a constant equal to 13.</para>
        /// </summary>
        public const int DefaultPort = 123;

        /// <summary>
        /// Represents the default amount of time in milliseconds after which a synchronous call
        /// Send will time out.
        /// <para>This field is a constant equal to 100 milliseconds.</para>
        /// </summary>
        public const int DefaultSendTimeout = 100;

        /// <summary>
        /// Represents the default amount of time in milliseconds after which a synchronous call
        /// Receive will time out.
        /// <para>This field is a constant equal to 100 milliseconds.</para>
        /// </summary>
        public const int DefaultReceiveTimeout = 100;

        private readonly IRandomGenerator _randomGenerator = new DefaultRandomGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpClient"/> class.
        /// </summary>
        public SntpClient()
        {
            Endpoint = new DnsEndPoint(DefaultHost, DefaultPort);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpClient"/> class.
        /// </summary>
        public SntpClient(string host)
        {
            Endpoint = new DnsEndPoint(host, DefaultPort);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpClient"/> class.
        /// </summary>
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
                if (value != 3 && value != 4) Throw.ArgumentOutOfRange(nameof(value));
                _version = value;
            }
        }

        // Send timeout in milliseconds; see Socket.SendTimeout.
        public int SendTimeout { get; init; } = DefaultSendTimeout;

        // Receive timeout in milliseconds; see Socket.ReceiveTimeout.
        public int ReceiveTimeout { get; init; } = DefaultReceiveTimeout;

        public IRandomGenerator RandomGenerator
        {
            get => _randomGenerator;
            init => _randomGenerator = value ?? throw new ArgumentNullException(nameof(value));
        }

        private EndPoint Endpoint { get; }

        private byte FirstByte
        {
            // CIL code size = 12 bytes <= 32 bytes.
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

            // A system clock is not monotonic, it may be synchronized backward
            // in time, therefore do NOT write
            // > var responseTime = DateTime.UtcNow;
            // otherwise we could end up with responseTime < requestTime!
            var stopwatch = Stopwatch.StartNew();

            using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp)
            {
                SendTimeout = SendTimeout,
                ReceiveTimeout = ReceiveTimeout,
            };

            sock.Connect(Endpoint);

            // Start time on this side of the network.
            var requestTime = DateTime.UtcNow;
            long startTicks = stopwatch.ElapsedTicks;

            // Randomize the timestamp, then write the result into the buffer.
            var requestTimestamp =
                Timestamp64.FromDateTime(requestTime)
                    .RandomizeSubMilliseconds(RandomGenerator);
            requestTimestamp.WriteTo(buf, TransmitTimestampOffset);

            sock.Send(buf);
            int len = sock.Receive(buf);

            long endTicks = stopwatch.ElapsedTicks;

            // Ensure that the response has enough bytes before proceeding
            // further.
            if (len < DataLength) NtpException.Throw();

            // Elapsed ticks during the query on this side of the network.
            long elapsedTicks = endTicks - startTicks;
            // End time on this side of the network.
            var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
            var responseTimestamp = Timestamp64.FromDateTime(responseTime);

            return ReadResponse(buf, requestTimestamp, responseTimestamp);
        }

        [Pure]
        public async Task<SntpResponse> QueryAsync()
        {
            var bytes = new byte[DataLength];
            bytes[0] = FirstByte;

            var stopwatch = Stopwatch.StartNew();

            using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp);

            await sock.ConnectAsync(Endpoint).ConfigureAwait(false);

            var requestTime = DateTime.UtcNow;
            long startTicks = stopwatch.ElapsedTicks;

            var requestTimestamp =
                Timestamp64.FromDateTime(requestTime)
                    .RandomizeSubMilliseconds(RandomGenerator);
            requestTimestamp.WriteTo(bytes, TransmitTimestampOffset);

            var buf = new ArraySegment<byte>(bytes);
            await sock.SendAsync(buf, SocketFlags.None).ConfigureAwait(false);
            int len = await sock.ReceiveAsync(buf, SocketFlags.None).ConfigureAwait(false);

            long endTicks = stopwatch.ElapsedTicks;

            if (len < DataLength) NtpException.Throw();

            long elapsedTicks = endTicks - startTicks;
            var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
            var responseTimestamp = Timestamp64.FromDateTime(responseTime);

            return ReadResponse(buf, requestTimestamp, responseTimestamp);
        }
    }

    public partial class SntpClient // Helpers
    {
        [Pure]
        private SntpResponse ReadResponse(
            ReadOnlySpan<byte> buf,
            Timestamp64 requestTimestamp,
            Timestamp64 responseTimestamp)
        {
            // The only fields not yet validated are those that the server is
            // expected to copy verbatim from the request.

            var si = ReadServerInfo(buf);
            if (si.Version != Version) NtpException.Throw();

            var ti = ReadTimeInfo(buf);
            ti.ResponseTimestamp = responseTimestamp;
            if (ti.RequestTimestamp != requestTimestamp) NtpException.Throw();

            return new SntpResponse(si, ti);
        }

        [Pure]
        private static SntpServerInfo ReadServerInfo(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= DataLength);

            // mode = 4 (server).
            if ((buf[0] & 7) != 4) NtpException.Throw();

            var info = new SntpServerInfo
            {
                LeapIndicator = ReadLeapIndicator((buf[0] >> 6) & 3),
                Version = (buf[0] >> 3) & 7,
                Stratum = ReadStratum(buf[1]),
                PollInterval = 1 << ReadSByte(buf[2]),
                Precision = ReadSByte(buf[3]),
                Rtt = Duration32.ReadFrom(buf[4..]),
                Dispersion = Duration32.ReadFrom(buf[8..]),
                ReferenceTimestamp = Timestamp64.ReadFrom(buf[16..]),
            };

            return info;

            // We validate the data according to RFC 4330, section 5.
            // Since we decided not to process the Reference Identifier we cannot
            // validate it here.

            [Pure]
            static LeapIndicator ReadLeapIndicator(int x) =>
                x switch
                {
                    0 => LeapIndicator.NoWarning,
                    1 => LeapIndicator.PositiveLeapSecond,
                    2 => LeapIndicator.NegativeLeapSecond,
                    3 => NtpException.Throw<LeapIndicator>("The server clock is not synchronised."),
                    // 0 <= "x" <= 3
                    _ => Throw.Unreachable<LeapIndicator>()
                };

            [Pure]
            static NtpStratum ReadStratum(byte x) =>
                x switch
                {
                    0 => NtpException.Throw<NtpStratum>("The server is unavailable."),
                    1 => NtpStratum.PrimaryReference,
                    <= 15 => NtpStratum.SecondaryReference,
                    16 => NtpException.Throw<NtpStratum>("The server clock is not synchronised."),
                    _ => NtpException.Throw<NtpStratum>("Reserved.")
                };

            [Pure]
            static int ReadSByte(byte v) => v > 127 ? v - 256 : v;
        }

        [Pure]
        private static SntpTimeInfo ReadTimeInfo(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf != null);
            Debug.Assert(buf.Length >= DataLength);

            var receiveTimestamp = Timestamp64.ReadFrom(buf[32..]);
            var transmitTimestamp = Timestamp64.ReadFrom(buf[TransmitTimestampOffset..]);

            // Sanity checks.
            if (receiveTimestamp == Timestamp64.Zero) NtpException.Throw();
            if (transmitTimestamp == Timestamp64.Zero) NtpException.Throw();
            // The server clock should be monotonically increasing.
            if (transmitTimestamp < receiveTimestamp) NtpException.Throw();

            return new SntpTimeInfo
            {
                RequestTimestamp = Timestamp64.ReadFrom(buf[24..]),
                ReceiveTimestamp = receiveTimestamp,
                TransmitTimestamp = transmitTimestamp
            };
        }
    }
}
