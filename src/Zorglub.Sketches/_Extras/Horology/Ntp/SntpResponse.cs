// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
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
        /// Gets the number of seconds indicating the total roundtrip delay to the primary reference
        /// source.
        /// </summary>
        public Duration64 RootDelay { get; init; }

        /// <summary>
        /// Gets the number of seconds indicating the maximum error due to the clock frequency
        /// tolerance
        /// </summary>
        public Duration64 RootDispersion { get; init; }

        public string? ReferenceIdentifier { get; set; }

        public Timestamp64 ReferenceTimestamp { get; init; }
    }

    public sealed record SntpTimeInfo
    {
        /// <summary>
        /// Gets the time at which the client transmitted the request, according to its local clock.
        /// </summary>
        public Timestamp64 OriginateTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the server received the request.
        /// </summary>
        public Timestamp64 ReceiveTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the server transmitted the response.
        /// </summary>
        public Timestamp64 TransmitTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the client received the response, according to its local clock.
        /// </summary>
        public Timestamp64 DestinationTimestamp { get; internal set; }

        // OriginateTimestamp (T1):     Time request sent by client
        // ReceiveTimestamp (T2):       Time request received by server
        // TransmitTimestamp (T3):      Time reply sent by server
        // DestinationTimestamp (T4):   Time reply received by client
        //
        // > RoundTripDelay = (T4 - T1) - (T3 - T2)
        // > ClockOffset = ((T2 - T1) + (T3 - T4)) / 2

        public Duration64 RoundTripDelay =>
            DestinationTimestamp - OriginateTimestamp - (TransmitTimestamp - ReceiveTimestamp);

        /// <summary>
        /// Gets the offset for the system clock relative to the primary synchonization source.
        /// </summary>
        public Duration64 ClockOffset =>
            (ReceiveTimestamp - OriginateTimestamp + (TransmitTimestamp - DestinationTimestamp)) / 2;
    }
}
