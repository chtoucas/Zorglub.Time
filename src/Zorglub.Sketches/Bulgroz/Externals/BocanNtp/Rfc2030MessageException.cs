#pragma warning disable IDE0073 // Require file header (Style)

namespace Zorglub.Bulgroz.Externals.BocanNtp
{
    using System;

    public sealed class Rfc2030MessageException : Exception
    {
        public Rfc2030MessageException() { }

        public Rfc2030MessageException(string message)
            : base(message) { }

        public Rfc2030MessageException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
