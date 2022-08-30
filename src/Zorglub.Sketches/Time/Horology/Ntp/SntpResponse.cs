// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Text;

    using Zorglub.Time.Horology.Ntp;

    public sealed record SntpResponse
    {
        public LeapIndicator LeapIndicator { get; private init; }
        public int Version { get; private init; }
        public NtpMode Mode { get; private init; }
        public NtpStratum Stratum { get; private init; }
        public int PollInterval { get; private init; }
        public double Precision { get; private init; }

        public double RootDelay { get; private init; }
        public double RootDispersion { get; private init; }
        public string? Reference { get; private init; }

        public DateTime ReferenceTime { get; private init; }
        // Time request sent by client (ID = T1).
        public DateTime OriginateTime { get; private init; }
        // Time request received by server (ID = T2).
        public DateTime ReceiveTime { get; private init; }
        // Time reply sent by server (ID = T3).
        public DateTime TransmitTime { get; private init; }
        // Time reply received by client (ID = T3).
        public DateTime DestinationTime { get; private init; }

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

        internal static SntpResponse Create(Rfc2030Message msg, DateTime destinationTime)
        {
            return new()
            {
                LeapIndicator = msg.LeapIndicator,
                Version = msg.Version,
                Mode = msg.Mode,
                Stratum = msg.Stratum,
                PollInterval = msg.PollInterval,
                Precision = msg.Precision,

                RootDelay = msg.RootDelay,
                RootDispersion = msg.RootDispersion,
                Reference = msg.Reference,

                ReferenceTime = msg.ReferenceTime,
                OriginateTime = msg.OriginateTime,
                ReceiveTime = msg.ReceiveTime,
                TransmitTime = msg.TransmitTime,

                DestinationTime = destinationTime,
            };
        }
    }
}
