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
        /// Gets the time at which the request departed the client for the server.
        /// <para>"Originate timestamp" in NTP parlance.</para>
        /// </summary>
        public Timestamp64 ClientTransmitTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the request arrived at the server.
        /// <para>"Receive timestamp" in NTP parlance.</para>
        /// </summary>
        public Timestamp64 ServerReceiveTimestamp { get; init; }

        /// <summary>
        /// Gets the time at which the reply departed the server.
        /// <para>"Transmit timestamp" in NTP parlance.</para>
        /// </summary>
        public Timestamp64 ServerTransmitTimestamp { get; init; }

        /// <summary>
        /// Gets the time of arrival of the reply according to the client clock.
        /// <para>"Destination timestamp" in NTP parlance.</para>
        /// </summary>
        public Timestamp64 ClientReceiveTimestamp { get; internal set; }

        /// <summary>
        /// Gets the offset for the client clock.
        /// </summary>
        public double ClientClockOffset { get; init; }

        // OriginateTimestamp (T1):     Time request sent by client
        // ReceiveTimestamp (T2):       Time request received by server
        // TransmitTimestamp (T3):      Time reply sent by server
        // DestinationTimestamp (T4):   Time reply received by client
        //
        // > RoundtripDelay = (T4 - T1) - (T2 - T3)
        // > LocalClockOffset = ((T2 - T1) + (T3 - T4)) / 2

        public Duration64 RoundtripDelay =>
            ClientReceiveTimestamp - ClientTransmitTimestamp
            - (ServerReceiveTimestamp - ServerTransmitTimestamp);

        // The offset of the local clock relative to the primary reference source.
        public double LocalClockOffset
        {
            get
            {
                var span =
                    ServerReceiveTimestamp - ClientTransmitTimestamp
                    + (ServerTransmitTimestamp - ClientReceiveTimestamp);

                return span.CountMilliseconds() / 2;
            }
        }
    }

    public sealed record SntpResponse
    {
        public LeapIndicator LeapIndicator { get; init; }

        public int Version { get; init; }

        public NtpMode Mode { get; init; }

        public NtpStratum Stratum { get; init; }

        // 2^PollInterval
        public int PollInterval { get; init; }

        // 2^(-Precision)
        public int Precision { get; init; }

        public Duration64 RootDelay { get; init; }

        public Duration64 RootDispersion { get; init; }

        public string? Reference { get; internal set; }

        public Timestamp64 ReferenceTimestamp { get; init; }

        public Timestamp64 OriginateTimestamp { get; init; }

        public Timestamp64 ReceiveTimestamp { get; init; }

        public Timestamp64 TransmitTimestamp { get; init; }

        public Timestamp64 DestinationTimestamp { get; internal set; }

        // OriginateTimestamp (T1):     Time request sent by client
        // ReceiveTimestamp (T2):       Time request received by server
        // TransmitTimestamp (T3):      Time reply sent by server
        // DestinationTimestamp (T4):   Time reply received by client
        //
        // > RoundtripDelay = (T4 - T1) - (T2 - T3)
        // > LocalClockOffset = ((T2 - T1) + (T3 - T4)) / 2

        public Duration64 RoundtripDelay =>
            DestinationTimestamp - OriginateTimestamp
            - (ReceiveTimestamp - TransmitTimestamp);

        // The offset of the local clock relative to the primary reference source.
        public double LocalClockOffset
        {
            get
            {
                var span =
                    ReceiveTimestamp - OriginateTimestamp
                    + (TransmitTimestamp - DestinationTimestamp);

                return span.CountMilliseconds() / 2;
            }
        }
    }
}
