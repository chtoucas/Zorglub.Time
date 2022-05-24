// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    using System.Linq;

    // Name conflict with BCL System.Range: use an alias, either
    // > using BclRange = System.Range;
    // or
    // > using NRange = Zorglub.Time.Core.Intervals.Range;
    //
    // Enumerable:
    // - ToEnumerable() for structs
    // - Impl IEnumerable<T> for classes

    /// <summary>
    /// Provides static helpers and extension methods for <see cref="Range{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class Range { }

    public partial class Range // Factories
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct representing the range
        /// [<paramref name="min"/>..<paramref name="max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        [Pure]
        public static Range<T> Create<T>(T min, T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(min, max);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct representing the range
        /// [<paramref name="min"/>..<paramref name="max"/>].
        /// <para>This factory method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        internal static Range<T> CreateLeniently<T>(T min, T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(OrderedPair.FromOrderedValues(min, max));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct representing the
        /// degenerate range [<paramref name="value"/>].
        /// </summary>
        [Pure]
        public static Range<T> Singleton<T>(T value)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return CreateLeniently(value, value);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct representing the range
        /// [<see name="IMinMaxValue{T}.MinValue"/>..<see cref="IMinMaxValue{T}.MaxValue"/>].
        /// </summary>
        [Pure]
        public static Range<T> Maximal<T>()
            where T : struct, IEquatable<T>, IComparable<T>, IMinMaxValue<T>
        {
            return CreateLeniently(T.MinValue, T.MaxValue);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct representing the range
        /// [<paramref name="min"/>..<see cref="IMinMaxValue{T}.MaxValue"/>].
        /// </summary>
        [Pure]
        public static Range<T> StartingAt<T>(T min)
            where T : struct, IEquatable<T>, IComparable<T>, IMinMaxValue<T>
        {
            return CreateLeniently(min, T.MaxValue);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct representing the range
        /// [<see cref="IMinMaxValue{T}.MinValue"/>..<paramref name="max"/>].
        /// </summary>
        [Pure]
        public static Range<T> EndingAt<T>(T max)
            where T : struct, IEquatable<T>, IComparable<T>, IMinMaxValue<T>
        {
            return CreateLeniently(T.MinValue, max);
        }
    }

    public partial class Range // Conversions
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct from the specified endpoints.
        /// </summary>
        [Pure]
        public static Range<T> FromEndpoints<T>(OrderedPair<T> endpoints)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new Range<T>(endpoints);
        }
    }

    public partial class Range // Range<int>
    {
        // A range of int's is finite and enumerable.

        /// <summary>
        /// Represents the range {<see cref="Int32.MinValue"/>..<see cref="Int32.MaxValue"/>}.
        /// <para>This is the largest range of 32-bit signed integers representable by the system.
        /// </para>
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly Range<int> Maximal32 = CreateLeniently(Int32.MinValue, Int32.MaxValue);

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static int Count(this Range<int> @this) => checked(@this.Max - @this.Min + 1);

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        [Pure]
        public static long LongCount(this Range<int> @this) => ((long)@this.Max) - @this.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static IEnumerable<int> ToEnumerable(this Range<int> @this) =>
            // TODO(code): Overflow... don't bother? This differs from the
            // behaviour of Range<DayNumber>.ToEnumerable()
            Enumerable.Range(@this.Min, @this.Count());
    }

    public partial class Range // Range<DayNumber>
    {
        // A range of DayNumber's is finite and enumerable.

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static int Count(this Range<DayNumber> @this) => @this.Max - @this.Min + 1;

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        [Pure]
        public static long LongCount(this Range<DayNumber> @this) =>
            ((long)@this.Max.DaysSinceZero) - @this.Min.DaysSinceZero + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<DayNumber> ToEnumerable(this Range<DayNumber> @this)
        {
            var min = @this.Min;
            var max = @this.Max;

            for (var i = min; i <= max; i++)
            {
                yield return i;
            }
        }
    }
}
