// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    public sealed record SntpInfo
    {
        public LeapIndicator LeapIndicator { get; init; }

        public int Version { get; init; }

        public NtpMode Mode { get; init; }

        public NtpStratum Stratum { get; init; }

        public int PollInterval { get; init; }

        public int Precision { get; init; }

        public Duration64 RootDelay { get; init; }

        public Duration64 RootDispersion { get; init; }

        public string? Reference { get; internal set; }

        public Timestamp64 ReferenceTimestamp { get; init; }
    }

    // Time info in client-server mode.
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
        // > RoundtripDelay = (T4 - T1) - (T3 - T2)
        // > ClockOffset = ((T2 - T1) + (T3 - T4)) / 2

        public Duration64 RoundTripDelay =>
            DestinationTimestamp - OriginateTimestamp - (TransmitTimestamp - ReceiveTimestamp);

        /// <summary>
        /// Gets the offset for the client clock relative to the primary reference source.
        /// </summary>
        public Duration64 ClockOffset =>
            (ReceiveTimestamp - OriginateTimestamp + (TransmitTimestamp - DestinationTimestamp) ) / 2;
    }

    public sealed record SntpResponse
    {
        public LeapIndicator LeapIndicator { get; init; }

        public int Version { get; init; }

        public NtpMode Mode { get; init; }

        public NtpStratum Stratum { get; init; }

        public int PollInterval { get; init; }

        public int Precision { get; init; }

        public Duration64 RootDelay { get; init; }

        public Duration64 RootDispersion { get; init; }

        public string? Reference { get; internal set; }

        public Timestamp64 ReferenceTimestamp { get; init; }

        public Timestamp64 OriginateTimestamp { get; init; }

        public Timestamp64 ReceiveTimestamp { get; init; }

        public Timestamp64 TransmitTimestamp { get; init; }

        public Timestamp64 DestinationTimestamp { get; internal set; }

        public Duration64 RoundTripDelay =>
            DestinationTimestamp - OriginateTimestamp - (TransmitTimestamp - ReceiveTimestamp);

        public Duration64 ClockOffset =>
            (ReceiveTimestamp - OriginateTimestamp + (TransmitTimestamp - DestinationTimestamp)) / 2;
    }
}
