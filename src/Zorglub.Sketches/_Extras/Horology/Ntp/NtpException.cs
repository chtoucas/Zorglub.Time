// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    public sealed class NtpException : Exception
    {
        public NtpException() { }

        public NtpException(string message) : base(message) { }

        public NtpException(string message, Exception innerException)
            : base(message, innerException) { }

        // TODO(code): move to ThrowHelpers.

        [DoesNotReturn]
        internal static void Throw(string message = "Bad server reply.") =>
            throw new NtpException(message);

        [DoesNotReturn, Pure]
        internal static T Throw<T>(string message = "Bad server reply.") =>
            throw new NtpException(message);
    }
}
