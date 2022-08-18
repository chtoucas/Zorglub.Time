﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    using System.Linq;

    // REVIEW(code): extension methods "in param"? Public Maximal32?

    // Name conflict with BCL System.Range: use an alias, either
    // > using BclRange = System.Range;
    // or
    // > using ZRange = Zorglub.Time.Core.Intervals.Range;
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

        // TODO(api): name?

        /// <summary>
        /// Creates a new instance of the <see cref="Range{T}"/> struct from the specified minimum
        /// and length.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="length"/> is less than 1.</exception>
        [Pure]
        public static Range<T> CreateWithLength<T>(T min, int length)
            where T : struct, IEquatable<T>, IComparable<T>, IAdditionOperators<T, int, T>
        {
            return new(min, min + (length - 1));
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
        /// [<see cref="IMinMaxValue{T}.MinValue"/>..<see cref="IMinMaxValue{T}.MaxValue"/>].
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

    public partial class Range // Conversions, transformations
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
        /// Gets the range [<see cref="Int32.MinValue"/>..<see cref="Int32.MaxValue"/>].
        /// <para>This is the largest range of 32-bit signed integers representable by the system.
        /// </para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        internal static Range<int> Maximal32 { get; } = Create(Int32.MinValue, Int32.MaxValue);

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static int Count(this Range<int> range) => checked(range.Max - range.Min + 1);

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        [Pure]
        public static long LongCount(this Range<int> range) => ((long)range.Max) - range.Min + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        /// <exception cref="OverflowException">The operation would overflow the capacity of
        /// <see cref="Int32"/>.</exception>
        [Pure]
        public static IEnumerable<int> ToEnumerable(this Range<int> range) =>
            // TODO(code): overflow... don't bother? This differs from the
            // behaviour of Range<DayNumber>.ToEnumerable()
            Enumerable.Range(range.Min, range.Count());
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
        public static int Count(this Range<DayNumber> range) => range.Max - range.Min + 1;

        /// <summary>
        /// Obtains the number of elements in the specified range.
        /// </summary>
        [Pure]
        public static long LongCount(this Range<DayNumber> range) =>
            ((long)range.Max.DaysSinceZero) - range.Min.DaysSinceZero + 1;

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
        /// </summary>
        [Pure]
        public static IEnumerable<DayNumber> ToEnumerable(this Range<DayNumber> range)
        {
            var min = range.Min;
            var max = range.Max;

            for (var i = min; i <= max; i++)
            {
                yield return i;
            }
        }
    }
}
