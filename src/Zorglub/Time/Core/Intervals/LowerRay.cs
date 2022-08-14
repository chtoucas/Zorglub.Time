// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    // Complement(): another option for the whole interval is to return the
    // empty set, but it does not match the behaviour of the set operation.

    /// <summary>
    /// Provides static helpers and extension methods for <see cref="LowerRay{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class LowerRay { }

    public partial class LowerRay // Factories
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LowerRay{T}"/> struct representing the ray
        /// [..<paramref name="max"/>], the set of values less than or equal to
        /// <paramref name="max"/>.
        /// </summary>
        [Pure]
        public static LowerRay<T> EndingAt<T>(T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(max);
        }
    }

    public partial class LowerRay // LowerRay<int>
    {
        /// <summary>
        /// Obtains the set complement of this ray.
        /// </summary>
        /// <exception cref="InvalidOperationException">The ray represents the whole
        /// <see cref="Int32"/> range.</exception>
        [Pure]
        public static UpperRay<int> Complement(this LowerRay<int> ray)
        {
            int max = ray.Max;
            return max == Int32.MaxValue ? throw new InvalidOperationException() : new(max + 1);
        }
    }

    public partial class LowerRay // LowerRay<DayNumber>
    {
        /// <summary>
        /// Obtains the set complement of this ray.
        /// </summary>
        /// <exception cref="InvalidOperationException">The ray represents the whole
        /// <see cref="DayNumber"/> range.</exception>
        [Pure]
        public static UpperRay<DayNumber> Complement(this LowerRay<DayNumber> ray)
        {
            DayNumber max = ray.Max;
            return max == DayNumber.MaxValue ? throw new InvalidOperationException() : new(max + 1);
        }
    }
}
