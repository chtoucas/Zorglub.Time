// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Core.Utilities;

internal static class TrySelectorExtensions
{
    [Pure]
    public static Func<TIn, TOut?> ToSelector<TIn, TOut>(this TryFunc<TIn, TOut> tryFun)
        where TOut : notnull
    {
        Requires.NotNull(tryFun);

        return x => tryFun.Invoke(x, out TOut? result) ? result : default;
    }
}
