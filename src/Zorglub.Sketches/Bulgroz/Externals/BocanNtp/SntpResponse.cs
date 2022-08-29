#pragma warning disable IDE0073 // Require file header (Style)

#pragma warning disable CA1305 // Specify IFormatProvider

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    using System.Text;

    public sealed class SntpResponse
    {
        public int Version { get; private init; }
        public SntpLeapIndicator LeapIndicator { get; private init; }
        public SntpMode Mode { get; private init; }
        public SntpStratum Stratum { get; private init; }
        public int PollInterval { get; private init; }

        public double Precision { get; private init; }
        public double RootDelay { get; private init; }
        public double RootDispersion { get; private init; }
        public double RoundTripDelay { get; private init; }
        public double LocalClockOffset { get; private init; }

        public string? ReferenceId { get; private init; }

        // Time when the client sent its request.
        public DateTime OriginateTime { get; private init; }
        // Time when the request was received by the server.
        public DateTime ReceiveTime { get; private init; }
        // Time when the response was sent.
        public DateTime TransmitTime { get; private init; }
        // Time when the response was received.
        public DateTime DestinationTime { get; private init; }

        public static SntpResponse Create(
            byte[] data,
            DateTime transmitTime,
            DateTime destinationTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        public override string ToString()
        {
            var str = new StringBuilder();

            str.Append("Leap indicator     : ");
            switch (LeapIndicator)
            {
                case SntpLeapIndicator.NoWarning:
                    str.AppendLine("No warning");
                    break;
                case SntpLeapIndicator.LastMinute61:
                    str.AppendLine("Last minute has 61 seconds");
                    break;
                case SntpLeapIndicator.LastMinute59:
                    str.AppendLine("Last minute has 59 seconds");
                    break;
                case SntpLeapIndicator.Alarm:
                    str.AppendLine("Alarm Condition (clock not synchronized)");
                    break;
            }
            str.AppendLine($"Version number     : {Version}");
            str.Append("Mode               : ");
            switch (Mode)
            {
                case SntpMode.Unknown:
                    str.AppendLine("Unknown");
                    break;
                case SntpMode.SymmetricActive:
                    str.AppendLine("Symmetric Active");
                    break;
                case SntpMode.SymmetricPassive:
                    str.AppendLine("Symmetric Pasive");
                    break;
                case SntpMode.Client:
                    str.AppendLine("Client");
                    break;
                case SntpMode.Server:
                    str.AppendLine("Server");
                    break;
                case SntpMode.Broadcast:
                    str.AppendLine("Broadcast");
                    break;
            }
            str.Append("Stratum            : ");
            switch (Stratum)
            {
                case SntpStratum.Unspecified:
                case SntpStratum.Reserved:
                    str.AppendLine("Unspecified");
                    break;
                case SntpStratum.PrimaryReference:
                    str.AppendLine("Primary reference");
                    break;
                case SntpStratum.SecondaryReference:
                    str.AppendLine("Secondary reference");
                    break;
            }

            str.AppendLine($"Precision          : {Precision} s.");
            str.AppendLine($"Poll interval      : {PollInterval} s.");
            str.AppendLine($"Reference ID       : {ReferenceId}");
            str.AppendLine($"Root delay         : {RootDelay} ms.");
            str.AppendLine($"Root dispersion    : {RootDispersion} ms.");
            str.AppendLine($"Round trip delay   : {RoundTripDelay} ms.");
            str.AppendLine($"Local clock offset : {LocalClockOffset} ms.");
            str.AppendLine($"Local time         : {DateTime.Now.AddMilliseconds(LocalClockOffset)}");
            str.AppendLine();

            return str.ToString();
        }
    }
}
