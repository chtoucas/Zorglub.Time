// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    public sealed class Rfc2030MessageException : Exception
    {
        public Rfc2030MessageException() { }

        public Rfc2030MessageException(string message)
            : base(message) { }

        public Rfc2030MessageException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
