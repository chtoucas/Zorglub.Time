// Part of GuerrillaNtp: https://guerrillantp.machinezoo.com

namespace Zorglub.Bulgroz.Externals.GuerrillaNtp
{
    /// <summary>
    /// Describes SNTP packet mode, i.e. client or server.
    /// </summary>
    /// <seealso cref="NtpPacket.Mode" />
    public static class NtpMode
    {
        /// <summary>
        /// Identifies client-to-server SNTP packet.
        /// </summary>
        public const int Client = 3;

        /// <summary>
        /// Identifies server-to-client SNTP packet.
        /// </summary>
        public const int Server = 4;

        public static bool IsKnown(int value) => value == 3 || value == 4;
    }
}
