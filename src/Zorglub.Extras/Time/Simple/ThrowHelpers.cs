// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

/// <summary>Provides static helpers to throw exceptions.</summary>
/// <remarks>This class cannot be inherited.</remarks>
internal static class ThrowHelpers
{
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void BadCuid(string paramName, Cuid expected, Cuid actual) =>
        throw GetBadCuidExn(paramName, expected, actual);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static ArgumentException GetBadCuidExn(string paramName, Cuid expected, Cuid actual) =>
        new($"The calendar ID should be equal to \"{expected}\" but it is equal to \"{actual}\".",
            paramName);
}
