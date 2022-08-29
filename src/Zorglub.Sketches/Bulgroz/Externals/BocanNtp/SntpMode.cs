#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    /// <summary>
    /// Mode field values
    /// </summary>
    public enum SntpMode
    {
        SymmetricActive,	// 1 - Symmetric active
        SymmetricPassive,	// 2 - Symmetric pasive
        Client,				// 3 - Client
        Server,				// 4 - Server
        Broadcast,			// 5 - Broadcast
        Unknown				// 0, 6, 7 - Reserved
    }
}
