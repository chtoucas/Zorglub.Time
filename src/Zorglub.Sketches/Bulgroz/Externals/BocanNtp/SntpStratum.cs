#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    /// <summary>
    /// Stratum field values
    /// </summary>
    public enum SntpStratum
    {
        Unspecified,			// 0 - unspecified or unavailable
        PrimaryReference,		// 1 - primary reference (e.g. radio-clock)
        SecondaryReference,     // 2-15 - secondary reference (via NTP or SNTP)
#pragma warning disable CA1700 // Do not name enum values 'Reserved'
        Reserved                // 16-255 - reserved
#pragma warning restore CA1700 // Do not name enum values 'Reserved'
    }
}
