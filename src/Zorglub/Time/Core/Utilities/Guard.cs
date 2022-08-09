// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerStepThrough]
    internal static class Guard
    {
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotNull<T>(
            T? value,
            [CallerArgumentExpression("value")] string paramName = "")
            where T : class
        {
            return value ?? throw new ArgumentNullException(paramName);
        }

        //        [Pure]
        //        // CIL code size = XXX bytes <= 32 bytes.
        //        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //        public static T DebugNotNull<T>(T? value) where T : class
        //        {
        //#if DEBUG
        //            Debug.Assert(value != null);
        //            return value;
        //#else
        //            return value!;
        //#endif
        //        }
    }
}
