// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    internal static class TrySelectorExtensions
    {
        [Pure]
        public static Func<TIn, TOut?> ToSelector<TIn, TOut>(this TryFunc<TIn, TOut> @this!!)
            where TOut : notnull
        {
            return x => @this.Invoke(x, out TOut? result) ? result : default;
        }
    }
}
