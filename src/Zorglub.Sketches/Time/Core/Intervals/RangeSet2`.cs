// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    // RangeSet<T> is the return type for Intersect() and Gap().
    //
    // Beware, if the runtime size of RangeSet<T> and IntervalBoundary<T> is
    // equal to 12 bytes w/ Int32, it starts to be too large w/ Int64 or Double
    // (24 bytes); see Zorglub.Time.ValueTypeFacts.

    /// <summary>
    /// Provides static helpers for <see cref="RangeSet{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class RangeSet2
    {
        #region Factories

        /// <summary>
        /// Obtains the empty range.
        /// <para>The empty range is both an intersection absorber and a span identity.</para>
        /// </summary>
        [Pure]
        public static RangeSet2<T> Empty<T>()
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return RangeSet2<T>.Empty;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RangeSet2{T}"/> struct representing the
        /// interval [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        [Pure]
        public static RangeSet2<T> Create<T>(T min, T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(min, max);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RangeSet2{T}"/> struct representing the
        /// interval [<paramref name="min"/>, <paramref name="max"/>].
        /// <para>This factory method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        internal static RangeSet2<T> CreateLenient<T>(T min, T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            Debug.Assert(min.CompareTo(max) <= 0);

            return new RangeSet2<T>(new IntervalBoundary<T>(OrderedPair.FromOrderedValues(min, max)));
        }

        #endregion
        #region Conversions

        /// <summary>
        /// Creates a new instance of the <see cref="RangeSet2{T}"/> struct from the specified
        /// boundary.
        /// </summary>
        [Pure]
        public static RangeSet2<T> FromBoundary<T>(IntervalBoundary<T> boundary)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new RangeSet2<T>(boundary);
        }

        /// <summary>
        /// Converts this range to a <see cref="RangeSet2{T}"/> value.
        /// </summary>
        [Pure]
        public static RangeSet2<T> FromRange<T>(Range<T> range)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new RangeSet2<T>(new IntervalBoundary<T>(range.Endpoints));
        }

        #endregion
    }

    /// <summary>
    /// Represents a (possibly empty) closed bounded interval.
    /// <para>An instance may be empty or reduced to a single value.</para>
    /// <para><i>Unless you need to take the intersection of two ranges, you most certainly should
    /// use <see cref="Range{T}"/> instead.</i></para>
    /// <para><typeparamref name="T"/> SHOULD be an <i>immutable</i> value type.</para>
    /// <para><see cref="RangeSet2{T}"/> is an immutable struct.</para>
    /// </summary>
    /// <typeparam name="T">The type of the interval's elements.</typeparam>
    public readonly partial struct RangeSet2<T> :
        IEqualityOperators<RangeSet2<T>, RangeSet2<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Represents the empty range.
        /// <para>The empty range is both an intersection absorber and a span identity.</para>
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly RangeSet2<T> Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeSet2{T}"/> struct representing the
        /// interval [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        public RangeSet2(T min, T max)
        {
            Boundary = new IntervalBoundary<T>(min, max);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeSet2{T}"/> struct from the specified
        /// boundary.
        /// </summary>
        // Public version: see RangeSet.FromBoundary().
        internal RangeSet2(IntervalBoundary<T> boundary) { Boundary = boundary; }

        /// <summary>
        /// Gets the boundary of the range, its set of endpoints.
        /// </summary>
        /// <returns>The empty boundary if the range is empty.</returns>
        public IntervalBoundary<T> Boundary { get; }

        /// <summary>
        /// Returns true if this range is empty; otherwise returns false.
        /// </summary>
        public bool IsEmpty => Boundary.IsEmpty;

        /// <summary>
        /// Attempts to get a non-empty range.
        /// </summary>
        /// <exception cref="InvalidOperationException">The set is empty.</exception>
        public Range<T> Range => new(Boundary.Endpoints);

        /// <summary>
        /// Returns a culture-independent string representation of this range.
        /// </summary>
        [Pure]
        public override string ToString() => IsEmpty ? "[]" : Range.ToString();
    }

    public partial struct RangeSet2<T> // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="RangeSet2{T}"/> are equal.
        /// </summary>
        public static bool operator ==(RangeSet2<T> left, RangeSet2<T> right) =>
            left.Boundary == right.Boundary;

        /// <summary>
        /// Determines whether two specified instances of <see cref="RangeSet2{T}"/> are not equal.
        /// </summary>
        public static bool operator !=(RangeSet2<T> left, RangeSet2<T> right) =>
            left.Boundary != right.Boundary;

        /// <inheritdoc />
        [Pure]
        public bool Equals(RangeSet2<T> other) => Boundary == other.Boundary;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is RangeSet2<T> range && Equals(range);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Boundary.GetHashCode();
    }
}
