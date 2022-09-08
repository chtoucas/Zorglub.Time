// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using static Zorglub.Time.Core.TemporalConstants;

// Adapted from
// https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/SntpClient.java
// See also
// https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/util/NtpTrustedTime.java
// https://android.googlesource.com/platform/frameworks/base/+/master/core/tests/coretests/src/android/net/sntp/

/// <summary>
/// Provides a stateless client for the Simple Network Time Protocol (SNTP).
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class SntpClient
{
    /// <summary>
    /// Represents the default SNTP version.
    /// <para>This field is a constant equal to 4.</para>
    /// </summary>
    public const int DefaultVersion = 4;

    /// <summary>
    /// Represents the default SNTP port.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int DefaultPort = 123;

    /// <summary>
    /// Represents the default amount of time in milliseconds after which a synchronous call Send
    /// will time out.
    /// <para>This field is a constant equal to 500 milliseconds.</para>
    /// </summary>
    public const int DefaultSendTimeout = 500;

    /// <summary>
    /// Represents the default amount of time in milliseconds after which a synchronous call Receive
    /// will time out.
    /// <para>This field is a constant equal to 500 milliseconds.</para>
    /// </summary>
    public const int DefaultReceiveTimeout = 500;

    /// <summary>
    /// Represents the random number generator.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly IRandomGenerator _randomGenerator = new DefaultRandomGenerator();

    /// <summary>
    /// Initializes a new instance of the <see cref="SntpClient"/> class.
    /// </summary>
    public SntpClient(string host)
    {
        EndPoint = new DnsEndPoint(host, DefaultPort);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SntpClient"/> class.
    /// </summary>
    public SntpClient(EndPoint endpoint)
    {
        EndPoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
    }

    private int _version = DefaultVersion;
    /// <summary>
    /// Gets or sets the NTP version.
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

    /// <summary>
    /// Gets or sets a value that specifies the amount of time in milliseconds after which a
    /// synchronous send call to the undelying socket will time out.
    /// </summary>
    public int SendTimeout { get; set; } = DefaultSendTimeout;

    /// <summary>
    /// Gets or sets a value that specifies the amount of time in milliseconds after which a
    /// synchronous receive call to the undelying socket will time out.
    /// </summary>
    public int ReceiveTimeout { get; set; } = DefaultReceiveTimeout;

    /// <summary>
    /// Gets or initializes the random number generator.
    /// </summary>
    public IRandomGenerator RandomGenerator
    {
        get => _randomGenerator;
        init => _randomGenerator = value ?? throw new ArgumentNullException(nameof(value));
    }

    // Real ops are done elsewhere, like keeping state, polling, etc.
    public bool EnableValidation { get; init; }

    // An NTP server may always return 3, e.g. "time.windows.com"
    // or "time.nist.gov".
    public bool DisableVersionCheck { get; init; }

    /// <summary>
    /// Gets the network address for the SNTP server.
    /// </summary>
    private EndPoint EndPoint { get; }

    /// <summary>
    /// Gets the first byte of the NTP packet.
    /// </summary>
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

    /// <summary>
    /// Queries the SNTP server.
    /// </summary>
    [Pure]
    public SntpResponse QueryTime()
    {
        var buf = new byte[NtpPacket.BinarySize].AsSpan();
        buf[0] = FirstByte;

        // A system clock is not monotonic (unattended synchronization).
        // Do NOT write
        // > var responseTime = DateTime.UtcNow;
        // otherwise we could end up with responseTime < requestTime!
        var stopwatch = Stopwatch.StartNew();

        using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp)
        {
            SendTimeout = SendTimeout,
            ReceiveTimeout = ReceiveTimeout,
        };

        sock.Connect(EndPoint);

        // Start time on this side of the network.
        var requestTime = DateTime.UtcNow;
        long startTicks = stopwatch.ElapsedTicks;

        // Randomize the timestamp, then write the result into the buffer.
        var requestTimestamp =
            Timestamp64.FromDateTime(requestTime)
                .RandomizeSubMilliseconds(RandomGenerator);
        requestTimestamp.WriteTo(buf, NtpPacket.TransmitTimestampOffset);

        sock.Send(buf);
        int len = sock.Receive(buf);

        long endTicks = stopwatch.ElapsedTicks;

        // Ensure that the response has enough bytes before proceeding further.
        if (len < NtpPacket.BinarySize) NtpException.Throw();

        // Elapsed ticks during the query on this side of the network.
        long elapsedTicks = endTicks - startTicks;
        // End time on this side of the network.
        var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
        var responseTimestamp = Timestamp64.FromDateTime(responseTime);

        return ReadResponse(buf, requestTimestamp, responseTimestamp);
    }

    /// <summary>
    /// Queries the SNTP server asynchronously.
    /// </summary>
    [Pure]
    public async Task<SntpResponse> QueryTimeAsync()
    {
        var bytes = new byte[NtpPacket.BinarySize];
        bytes[0] = FirstByte;

        var stopwatch = Stopwatch.StartNew();

        using var sock = new Socket(SocketType.Dgram, ProtocolType.Udp);

        await sock.ConnectAsync(EndPoint).ConfigureAwait(false);

        var requestTime = DateTime.UtcNow;
        long startTicks = stopwatch.ElapsedTicks;

        var requestTimestamp =
            Timestamp64.FromDateTime(requestTime)
                .RandomizeSubMilliseconds(RandomGenerator);
        requestTimestamp.WriteTo(bytes, NtpPacket.TransmitTimestampOffset);

        var buf = new ArraySegment<byte>(bytes);
        await sock.SendAsync(buf, SocketFlags.None).ConfigureAwait(false);
        int len = await sock.ReceiveAsync(buf, SocketFlags.None).ConfigureAwait(false);

        long endTicks = stopwatch.ElapsedTicks;

        if (len < NtpPacket.BinarySize) NtpException.Throw();

        long elapsedTicks = endTicks - startTicks;
        var responseTime = requestTime.AddMilliseconds(elapsedTicks / TicksPerMillisecond);
        var responseTimestamp = Timestamp64.FromDateTime(responseTime);

        return ReadResponse(buf, requestTimestamp, responseTimestamp);
    }
}

public partial class SntpClient // Helpers
{
    /// <summary>
    /// Represents a duration of exactly one second.
    /// <para>This field is read-only.</para>
    /// </summary>
    private static readonly Duration32 s_OneSecond = new(1, 0);

    /// <summary>
    /// Reads an <see cref="SntpResponse"/> value from the beginning of a read-only span of bytes.
    /// </summary>
    [Pure]
    private SntpResponse ReadResponse(
        ReadOnlySpan<byte> buf,
        Timestamp64 requestTimestamp,
        Timestamp64 responseTimestamp)
    {
        var pkt = NtpPacket.ReadFrom(buf);
        // Full validation or not?
        if (EnableValidation) { ValidatePacket(in pkt); } else { CheckPacketRfc4330(in pkt); }
        // The only fields not yet validated are those that the server is
        // expected to copy verbatim from the request.
        if (DisableVersionCheck == false && pkt.Version != Version)
            NtpException.Throw(FormattableString.Invariant(
                $"Invalid packet. Version missmatch: expected {Version}, received {pkt.Version}."));
        if (pkt.OriginateTimestamp != requestTimestamp)
            NtpException.Throw(
                "Invalid packet. Originate Timestamp does not match the Request Timestamp.");

        var si = new SntpServerInfo
        {
            LeapIndicator = pkt.LeapIndicator,
            Version = pkt.Version,
            Stratum = pkt.Stratum,
            PollInterval = pkt.PollInterval,
            Precision = pkt.Precision,
            Rtt = pkt.RootDelay,
            Dispersion = pkt.RootDispersion,
            ReferenceIdentifier = pkt.ReferenceIdentifier,
            ReferenceCode = pkt.ReferenceCode,
            ReferenceTimestamp = pkt.ReferenceTimestamp,
        };

        var ti = new SntpTimeInfo
        {
            RequestTimestamp = requestTimestamp,
            ReceiveTimestamp = pkt.ReceiveTimestamp,
            TransmitTimestamp = pkt.TransmitTimestamp,
            ResponseTimestamp = responseTimestamp
        };

        return new SntpResponse(si, ti);
    }

    /// <summary>
    /// Validation according to RFC 4330, section 5 (client operations).
    /// </summary>
    private static void CheckPacketRfc4330(in NtpPacket pkt)
    {
        if (pkt.Mode != NtpMode.Server)
            NtpException.Throw(FormattableString.Invariant(
                $"Invalid NTP mode: received \"{pkt.Mode}\" but it should be \"Server\"."));

        // Invalid is not supposed to be possible here.
        Debug.Assert(pkt.Stratum != NtpStratum.Invalid);
        if (pkt.Stratum == NtpStratum.Reserved)
            NtpException.Throw("Invalid stratum: \"Reserved\".");

        if (pkt.TransmitTimestamp == Timestamp64.Zero)
            NtpException.Throw("Transmit Timestamp = 0.");
    }

    // Other things we could check:
    // - ReferenceCode
    // - ReferenceTimestamp is not too far in the past
    // - RootDelay & co
    // - Peer sync distance: RootDelay / 2 + RootDispersion < 16s
    // - IP addresses
    private static void ValidatePacket(in NtpPacket pkt)
    {
        if (pkt.LeapIndicator != LeapIndicator.NoWarning
            && pkt.LeapIndicator != LeapIndicator.PositiveLeapSecond
            && pkt.LeapIndicator != LeapIndicator.NegativeLeapSecond)
            NtpException.Throw(FormattableString.Invariant(
                $"The server clock is not synchronised -or- the leap indicator is not valid: {pkt.LeapIndicator}."));

        if (pkt.Mode != NtpMode.Server)
            NtpException.Throw(FormattableString.Invariant(
                $"Invalid NTP mode: received \"{pkt.Mode}\" but it should be \"Server\"."));

        if (pkt.Stratum != NtpStratum.PrimaryReference
            && pkt.Stratum != NtpStratum.SecondaryReference)
            NtpException.Throw(FormattableString.Invariant(
                $"The server is unavailable, unsynchronised -or- the stratum is not valid: {pkt.Stratum}."));

        // RootDelay and RootDispersion >= 0 and < 1s.
        // Notice that positivity is always guaranteed.
        if (pkt.RootDelay >= s_OneSecond)
            NtpException.Throw(FormattableString.Invariant(
                $"Root delay >= 1s: {pkt.RootDelay}."));
        if (pkt.RootDispersion >= s_OneSecond)
            NtpException.Throw(FormattableString.Invariant(
                $"Root dispersion >= 1s: {pkt.RootDispersion}."));

        // The server clock should be monotonically increasing.
        if (pkt.ReceiveTimestamp < pkt.ReferenceTimestamp)
            NtpException.Throw(FormattableString.Invariant(
                $"The server clock should be monotonically increasing: Receive Timestamp ({pkt.ReceiveTimestamp}) < Reference Timestamp ({pkt.ReferenceTimestamp})."));
        if (pkt.TransmitTimestamp < pkt.ReceiveTimestamp)
            NtpException.Throw(FormattableString.Invariant(
                $"The server clock should be monotonically increasing: Transmit Timestamp ({pkt.TransmitTimestamp}) < Receive Timestamp ({pkt.ReceiveTimestamp})."));
        // TransmitTimestamp >= ReceiveTimestamp >= ReferenceTimestamp
        // therefore if ReferenceTimestamp != 0 i.e. > 0, then the other
        // timestamps are non-zero too.
        if (pkt.ReferenceTimestamp == Timestamp64.Zero)
            NtpException.Throw("Reference Timestamp = 0.");
    }
}
