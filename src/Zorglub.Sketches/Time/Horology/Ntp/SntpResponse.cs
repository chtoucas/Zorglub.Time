// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
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

        // FIXME(code): Temporary props.
        private DateTime OriginateTime => OriginateTimestamp.ToDateTime();
        private DateTime ReceiveTime => ReceiveTimestamp.ToDateTime();
        private DateTime TransmitTime => TransmitTimestamp.ToDateTime();
        private DateTime DestinationTime => DestinationTimestamp.ToDateTime();

        // OriginateTimestamp (T1):     Time request sent by client
        // ReceiveTimestamp (T2):       Time request received by server
        // TransmitTimestamp (T3):      Time reply sent by server
        // DestinationTimestamp (T4):   Time reply received by client
        //
        // > RoundtripDelay = (T4 - T1) - (T2 - T3)
        // > LocalClockOffset = ((T2 - T1) + (T3 - T4)) / 2

        public double RoundtripDelay
        {
            get
            {
                TimeSpan span = DestinationTime - OriginateTime - (ReceiveTime - TransmitTime);
                return span.TotalMilliseconds;
            }
        }

        // The offset of the local clock relative to the primary reference source.
        public double LocalClockOffset
        {
            get
            {
                TimeSpan span = ReceiveTime - OriginateTime + (TransmitTime - DestinationTime);
                return span.TotalMilliseconds / 2;
            }
        }
    }
}
