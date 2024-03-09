// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

/// <summary>
/// Represents a response from an NTP server.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class NtpResponse
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
    public required LeapIndicator LeapIndicator { get; init; }

    /// <summary>
    /// Gets the NTP version.
    /// <para>The result is in the range from 0 to 7.</para>
    /// </summary>
    public required int Version { get; init; }

    private readonly byte _stratumLevel;
    /// <summary>
    /// Gets the NTP stratum level.
    /// </summary>
    public required byte StratumLevel
    {
        get => _stratumLevel;
        init
        {
            _stratumLevel = value;

            StratumFamily = value switch
            {
                0 => StratumFamily.Unspecified,
                1 => StratumFamily.PrimaryReference,
                <= 15 => StratumFamily.SecondaryReference,
                16 => StratumFamily.Unsynchronized,
                _ => StratumFamily.Reserved
            };
        }
    }

    /// <summary>
    /// Gets the NTP stratum family.
    /// </summary>
    public StratumFamily StratumFamily { get; private init; }

    /// <summary>
    /// Gets the log base 2 of the maximum interval between successive messages in seconds.
    /// <para>The result is in the range from -128 to 127.</para>
    /// </summary>
    public required int PollExponent { get; init; }

    /// <summary>
    /// Gets the maximum interval between successive messages in seconds.
    /// </summary>
    public double PollInterval => Math.Pow(2, PollExponent);

    /// <summary>
    /// Gets the log base 2 of the precision of the server clock in seconds.
    /// <para>The result is in the range from -128 to 127.</para>
    /// </summary>
    public required int PrecisionExponent { get; init; }

    /// <summary>
    /// Gets the precision of the server clock in seconds.
    /// </summary>
    public double Precision => Math.Pow(2, PrecisionExponent);

    /// <summary>
    /// Gets the round-trip delay (RTT) to the primary reference clock.
    /// </summary>
    public required Duration32 RoundTripTime { get; init; }

    /// <summary>
    /// Gets the maximum error due to the clock frequency tolerance.
    /// </summary>
    public required Duration32 Dispersion { get; init; }

    /// <summary>
    /// Gets the NTP identifier of the particular reference clock.
    /// <para>See also <seealso cref="NtpCode"/>.</para>
    /// </summary>
    public required ReferenceId ReferenceId { get; init; }

    /// <summary>
    /// Gets the NTP code identifying the particular server or reference clock -or- a "kiss code".
    /// </summary>
    public NtpCode NtpCode => ReferenceId.ToNtpCodeFor(StratumFamily);

    /// <summary>
    /// Gets the time the system clock was last set or corrected.
    /// </summary>
    public required Timestamp64 ReferenceTimestamp { get; init; }
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
    public required Timestamp64 RequestTimestamp { get; init; }

    /// <summary>
    /// Gets the time at which the <i>server</i> received the request, according to the server
    /// clock.
    /// </summary>
    public required Timestamp64 ReceiveTimestamp { get; init; }

    /// <summary>
    /// Gets the time at which the <i>server</i> sent the response, according to the server
    /// clock.
    /// </summary>
    public required Timestamp64 TransmitTimestamp { get; init; }

    /// <summary>
    /// Gets the time at which the <i>client</i> received the response, according to the client
    /// clock.
    /// </summary>
    public required Timestamp64 ResponseTimestamp { get; init; }

    // OriginateTimestamp (T1):     request sent by the client
    // ReceiveTimestamp (T2):       request received by the server
    // TransmitTimestamp (T3):      reply sent by the server
    // DestinationTimestamp (T4):   reply received by the client
    //
    // Round-trip delay = (T4 - T1) - (T3 - T2)
    // Client clock offset = ((T2 - T1) + (T3 - T4)) / 2
    //
    // The RTT formula is trivial. To obtain the clock offset, assuming that the
    // network is "symmetrical", we just need to resolve the equations:
    //   T1 + offset + RTT/2 = T2
    //   T4 + offset - RTT/2 = T3
    //
    // The smaller is RTT, the better is the precision of the clock offset.

    /// <summary>
    /// Gets the round-trip delay (RTT) to the NTP server.
    /// </summary>
    public Duration64 RoundTripTime =>
        ResponseTimestamp - RequestTimestamp - (TransmitTimestamp - ReceiveTimestamp);

    /// <summary>
    /// Gets the offset for the system clock relative to the primary synchonization source.
    /// </summary>
    public Duration64 ClockOffset =>
        (ReceiveTimestamp - RequestTimestamp + (TransmitTimestamp - ResponseTimestamp)) / 2;
}
