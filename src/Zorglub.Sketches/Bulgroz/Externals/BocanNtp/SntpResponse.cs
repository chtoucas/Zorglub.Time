#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    using System.Text;

    public sealed record SntpResponse
    {
        public SntpLeapIndicator LeapIndicator { get; private init; }
        public int Version { get; private init; }
        public SntpMode Mode { get; private init; }
        public SntpStratum Stratum { get; private init; }
        public int PollInterval { get; private init; }
        public double Precision { get; private init; }

        public double RootDelay { get; private init; }
        public double RootDispersion { get; private init; }
        public string? ReferenceId { get; private init; }

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

        internal static SntpResponse Create(
            Rfc2030Message rfc2030Message,
            DateTime transmitTime,
            DateTime destinationTime)
        {
            return new()
            {
                LeapIndicator = rfc2030Message.LeapIndicator,
                Version = rfc2030Message.Version,
                Mode = rfc2030Message.Mode,
                Stratum = rfc2030Message.Stratum,
                PollInterval = rfc2030Message.PollInterval,
                Precision = rfc2030Message.Precision,

                RootDelay = rfc2030Message.RootDelay,
                RootDispersion = rfc2030Message.RootDispersion,
                ReferenceId = rfc2030Message.ReferenceId,

                ReferenceTime = rfc2030Message.ReferenceTime,
                OriginateTime = rfc2030Message.OriginateTime,
                ReceiveTime = rfc2030Message.ReceiveTime,
                TransmitTime = rfc2030Message.TransmitTime,
                DestinationTime = destinationTime,
            };
        }
    }
}
