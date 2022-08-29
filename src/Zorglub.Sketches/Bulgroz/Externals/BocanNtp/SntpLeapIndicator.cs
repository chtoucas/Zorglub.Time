#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    public enum SntpLeapIndicator
    {
        None = 0,

        NoWarning,
        PositiveLeapSecond,
        NegativeLeapSecond,
        Alarm
    }
}
