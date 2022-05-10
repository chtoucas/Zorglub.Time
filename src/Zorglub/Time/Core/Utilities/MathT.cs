// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    /// <summary>
    /// Provides static methods for generic mathematical operations.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal static class MathT
    {
        /// <summary>
        /// Returns the larger of two values.
        /// </summary>
        [Pure]
        public static T Min<T>(T x, T y) where T : IComparable<T> => x.CompareTo(y) < 0 ? x : y;

        /// <summary>
        /// Returns the smaller of two values.
        /// </summary>
        [Pure]
        public static T Max<T>(T x, T y) where T : IComparable<T> => x.CompareTo(y) > 0 ? x : y;
    }
}
