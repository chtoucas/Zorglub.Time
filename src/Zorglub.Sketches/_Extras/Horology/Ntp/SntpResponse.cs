// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    // Fields removed from SntpServerInfo:
    // - Mode (always = Server)
    // - ReferenceIdentifier

    public sealed class SntpResponse
    {
        public SntpResponse(SntpServerInfo serverInfo, SntpTimeInfo timeInfo)
        {
            ServerInfo = serverInfo ?? throw new ArgumentNullException(nameof(serverInfo));
            TimeInfo = timeInfo ?? throw new ArgumentNullException(nameof(timeInfo));
        }

        public SntpServerInfo ServerInfo { get; }

        public SntpTimeInfo TimeInfo { get; }
    }

    public sealed record SntpServerInfo
    {
        /// <summary>
        /// Gets the warning of an impending leap second to be inserted/deleted in the last minute
        /// of the current day.
        /// </summary>
        public LeapIndicator LeapIndicator { get; init; }

        /// <summary>
        /// Gets the NTP version.
        /// </summary>
        public int Version { get; init; }

        /// <summary>
        /// Gets the NTP stratum.
        /// </summary>
        public NtpStratum Stratum { get; init; }

        /// <summary>
        /// Gets the maximum interval between successive messages in seconds.
        /// </summary>
        public int PollInterval { get; init; }

        /// <summary>
        /// Gets the log base 2 of the precision of the system clock of the server in seconds.
        /// </summary>
        public int Precision { get; init; }

        /// <summary>
        /// Gets the round-trip time (RTT) to the primary reference source.
        /// </summary>
        public Duration32 Rtt { get; init; }

        /// <summary>
        /// Gets the maximum error due to the clock frequency tolerance
        /// </summary>
        public Duration32 Dispersion { get; init; }

        public Timestamp64 ReferenceTimestamp { get; init; }
    }

    public sealed record SntpTimeInfo
    {
        /// <summary>
        /// Gets the time at which the client transmitted the request, according to its clock.
        /// </summary>
        public Timestamp64 RequestTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the server received the request, according to its clock.
        /// </summary>
        public Timestamp64 ReceiveTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the server transmitted the response, according to its clock.
        /// </summary>
        public Timestamp64 TransmitTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the client received the response, according to its clock.
        /// </summary>
        public Timestamp64 ResponseTimestamp { get; set; }

        // OriginateTimestamp (T1):     request sent by the client
        // ReceiveTimestamp (T2):       request received by the server
        // TransmitTimestamp (T3):      reply sent by the server
        // DestinationTimestamp (T4):   reply received by the client
        //
        // Round-trip delay = (T4 - T1) - (T3 - T2)
        // Clock offset = ((T2 - T1) + (T3 - T4)) / 2

        public Duration64 Rtt =>
            ResponseTimestamp - RequestTimestamp - (TransmitTimestamp - ReceiveTimestamp);

        /// <summary>
        /// Gets the offset for the system clock relative to the primary synchonization source.
        /// </summary>
        public Duration64 ClockOffset =>
            (ReceiveTimestamp - RequestTimestamp + (TransmitTimestamp - ResponseTimestamp)) / 2;
    }
}
