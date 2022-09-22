// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

using Zorglub.Time.Simple;

internal static class ExceptionFactory2
{
    [Pure, DebuggerStepThrough]
    public static ArgumentException InvalidGregorianSwitch(string paramName, CalendarDate switchover) =>
        new($"The Gregorian switchover \"{switchover}\" can not precede the 15/10/1582.",
            paramName);
}
