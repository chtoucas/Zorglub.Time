// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    internal static class ThrowHelpers2
    {
        [Pure]
        [DebuggerStepThrough]
        public static void BadTimescale(
            string paramName, Timescale expected, Timescale actual)
        {
            throw new ArgumentException(
                $"The object should be in the timescale \"{expected}\" but it is in \"{actual}\".",
                paramName);
        }
    }
}
