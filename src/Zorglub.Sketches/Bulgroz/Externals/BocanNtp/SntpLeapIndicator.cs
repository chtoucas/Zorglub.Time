#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    /// <summary>
    /// Leap indicator field values
    /// </summary>
    public enum SntpLeapIndicator
    {
        NoWarning,		// 0 - No warning
        LastMinute61,	// 1 - Last minute has 61 seconds
        LastMinute59,	// 2 - Last minute has 59 seconds
        Alarm			// 3 - Alarm condition (clock not synchronized)
    }
}
