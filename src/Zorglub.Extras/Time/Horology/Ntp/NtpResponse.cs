// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

/// <summary>
/// Represents a response from an NTP server.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed record NtpResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NtpResponse"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="serverInfo"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="timeInfo"/> is null.</exception>
    public NtpResponse(NtpServerInfo serverInfo, NtpTimeInfo timeInfo)
    {
        ServerInfo = serverInfo ?? throw new ArgumentNullException(nameof(serverInfo));
        TimeInfo = timeInfo ?? throw new ArgumentNullException(nameof(timeInfo));
    }

    /// <summary>
    /// Gets the NTP server info.
    /// </summary>
    public NtpServerInfo ServerInfo { get; }

    /// <summary>
    /// Gets the NTP time info.
    /// </summary>
    public NtpTimeInfo TimeInfo { get; }

    /// <summary>
    /// Deconstructs this instance into its components.
    /// </summary>
    public void Deconstruct(out NtpServerInfo si, out NtpTimeInfo ti) =>
        (si, ti) = (ServerInfo, TimeInfo);
}

/// <summary>
/// Represents the server info of a response from an NTP server.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed record NtpServerInfo
{
    /// <summary>
    /// Gets the leap indicator, warning of an impending leap second to be inserted/deleted in
    /// the last minute of the current day.
    /// </summary>
    public LeapIndicator LeapIndicator { get; init; }

    /// <summary>
    /// Gets the NTP version.
    /// <para>The result is in the range from 0 to 7.</para>
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Gets the NTP stratum.
    /// </summary>
    public NtpStratum Stratum { get; init; }

    /// <summary>
    /// Gets the log base 2 of the maximum interval between successive messages in seconds.
    /// <para>The result is in the range from -128 to 127.</para>
    /// </summary>
    public int PollInterval { get; init; }

    /// <summary>
    /// Gets the log base 2 of the precision of the server clock in seconds.
    /// <para>The result is in the range from -128 to 127.</para>
    /// </summary>
    public int Precision { get; init; }

    /// <summary>
    /// Gets the round-trip time (RTT) to the primary reference clock.
    /// </summary>
    public Duration32 Rtt { get; init; }

    /// <summary>
    /// Gets the maximum error due to the clock frequency tolerance.
    /// </summary>
    public Duration32 Dispersion { get; init; }

    /// <summary>
    /// Gets the NTP identifier of the particular reference clock.
    /// <para>See also <seealso cref="NtpCode"/>.</para>
    /// </summary>
    public ReferenceId ReferenceId { get; init; }

    /// <summary>
    /// Gets the NTP code identifying the particular server or reference clock, or a "kiss code".
    /// </summary>
    public NtpCode NtpCode => ReferenceId.ToNtpCodeFor(Stratum);

    /// <summary>
    /// Gets the time the system clock was last set or corrected.
    /// </summary>
    public Timestamp64 ReferenceTimestamp { get; init; }
}

/// <summary>
/// Represents the time info of a response from an NTP server.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed record NtpTimeInfo
{
    /// <summary>
    /// Gets the time at which the <i>client</i> sent the request, according to the client clock.
    /// </summary>
    public Timestamp64 RequestTimestamp { get; init; }

    /// <summary>
    /// Gets the time at which the <i>server</i> received the request, according to the server
    /// clock.
    /// </summary>
    public Timestamp64 ReceiveTimestamp { get; init; }

    /// <summary>
    /// Gets the time at which the <i>server</i> sent the response, according to the server
    /// clock.
    /// </summary>
    public Timestamp64 TransmitTimestamp { get; init; }

    /// <summary>
    /// Gets the time at which the <i>client</i> received the response, according to the client
    /// clock.
    /// </summary>
    public Timestamp64 ResponseTimestamp { get; init; }

    // OriginateTimestamp (T1):     request sent by the client
    // ReceiveTimestamp (T2):       request received by the server
    // TransmitTimestamp (T3):      reply sent by the server
    // DestinationTimestamp (T4):   reply received by the client
    //
    // Round-trip delay = (T4 - T1) - (T3 - T2)
    // Clock offset = ((T2 - T1) + (T3 - T4)) / 2

    /// <summary>
    /// Gets the round-trip time (RTT) to the NTP server.
    /// </summary>
    public Duration64 Rtt =>
        ResponseTimestamp - RequestTimestamp - (TransmitTimestamp - ReceiveTimestamp);

    /// <summary>
    /// Gets the offset for the system clock relative to the primary synchonization source.
    /// </summary>
    public Duration64 ClockOffset =>
        (ReceiveTimestamp - RequestTimestamp + (TransmitTimestamp - ResponseTimestamp)) / 2;
}
