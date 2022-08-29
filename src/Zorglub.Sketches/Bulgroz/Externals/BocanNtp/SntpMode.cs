#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    public enum SntpMode
    {
        None = 0,

        Special0,
        SymmetricActive,
        SymmetricPassive,
        Client,
        Server,
        Broadcast,
        Special6,
        Special7
    }
}
