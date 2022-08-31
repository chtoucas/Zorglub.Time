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

        public double RootDelay { get; init; }

        public double RootDispersion { get; init; }

        public string? Reference { get; internal set; }

        public NtpTimestamp ReferenceTimestamp { get; init; }
        public DateTime ReferenceTime => ReferenceTimestamp.ToDateTime();

        // Time request sent by client (ID = T1).
        public NtpTimestamp OriginateTimestamp { get; init; }
        public DateTime OriginateTime => OriginateTimestamp.ToDateTime();

        // Time request received by server (ID = T2).
        public NtpTimestamp ReceiveTimestamp { get; init; }
        public DateTime ReceiveTime => ReceiveTimestamp.ToDateTime();

        // Time reply sent by server (ID = T3).
        public NtpTimestamp TransmitTimestamp { get; init; }
        public DateTime TransmitTime => TransmitTimestamp.ToDateTime();

        // Time reply received by client (ID = T3).
        public DateTime DestinationTime { get; init; }

        public double RoundtripDelay
        {
            // d = (T4 - T1) - (T2 - T3)
            get
            {
                TimeSpan span = DestinationTime - OriginateTime - (ReceiveTime - TransmitTime);
                return span.TotalMilliseconds;
            }
        }

        // The offset of the local clock relative to the primary reference source.
        public double LocalClockOffset
        {
            // t = ((T2 - T1) + (T3 - T4)) / 2
            get
            {
                TimeSpan span = ReceiveTime - OriginateTime + (TransmitTime - DestinationTime);
                return span.TotalMilliseconds / 2;
            }
        }

        [Pure]
        internal bool Check()
        {
            if (LeapIndicator == LeapIndicator.Invalid
                || LeapIndicator == LeapIndicator.Alarm)
                return false;

            if (Mode != NtpMode.Server) return false;

            if (Stratum != NtpStratum.PrimaryReference
                && Stratum != NtpStratum.SecondaryReference)
                return false;

            return true;
        }
    }
}
