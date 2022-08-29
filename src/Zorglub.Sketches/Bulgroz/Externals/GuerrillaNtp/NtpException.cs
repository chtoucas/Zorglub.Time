#pragma warning disable IDE0073 // Require file header (Style)

// Part of GuerrillaNtp: https://guerrillantp.machinezoo.com

namespace Zorglub.Bulgroz.Externals.GuerrillaNtp
{
    using System;

    /// <summary>
    /// Represents errors that occur in SNTP packets or during SNTP operation.
    /// </summary>
    public sealed class NtpException : Exception
    {
        public NtpException() { }

        public NtpException(string message) : base(message) { }

        public NtpException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
