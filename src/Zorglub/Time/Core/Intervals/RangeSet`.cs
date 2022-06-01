// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    // REVIEW(api): add factory Create(Range<T>), Singleton()?

    // RangeSet<T> is the return type for Intersect() and Gap().
    //
    // Beware, if the runtime size of RangeSet<T> and IntervalBoundary<T> is
    // equal to 12 bytes w/ Int32, it starts to be too large w/ Int64 or Double
    // (24 bytes); see Zorglub.Time.ValueTypeFacts.

    /// <summary>
    /// Provides static helpers for <see cref="RangeSet{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class RangeSet
    {
        #region Factories

        /// <summary>
        /// Obtains the empty range.
        /// <para>The empty range is both an intersection absorber and a span identity.</para>
        /// </summary>
        [Pure]
        public static RangeSet<T> Empty<T>()
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return RangeSet<T>.Empty;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RangeSet{T}"/> struct representing the range
        /// [<paramref name="min"/>..<paramref name="max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        [Pure]
        public static RangeSet<T> Create<T>(T min, T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(min, max);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RangeSet{T}"/> struct representing the range
        /// [<paramref name="min"/>..<paramref name="max"/>].
        /// <para>This factory method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        internal static RangeSet<T> CreateLeniently<T>(T min, T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(OrderedPair.FromOrderedValues(min, max));
        }

        #endregion
        #region Conversions

        /// <summary>
        /// Creates a new instance of the <see cref="RangeSet{T}"/> struct from the specified
        /// endpoints.
        /// </summary>
        [Pure]
        public static RangeSet<T> FromEndpoints<T>(OrderedPair<T> endpoints)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(endpoints);
        }

        #endregion
    }

    /// <summary>
    /// Represents a (possibly empty) closed bounded interval.
    /// <para>An instance may be empty or reduced to a single value.</para>
    /// <para><i>Unless you need to take the intersection of two ranges, you most certainly should
    /// use <see cref="Range{T}"/> instead.</i></para>
    /// <para><typeparamref name="T"/> SHOULD be an <i>immutable</i> value type.</para>
    /// <para><see cref="RangeSet{T}"/> is an immutable struct.</para>
    /// </summary>
    /// <typeparam name="T">The type of the interval's elements.</typeparam>
    public readonly partial struct RangeSet<T> :
        IEqualityOperators<RangeSet<T>, RangeSet<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Represents the empty range.
        /// <para>The empty range is both an intersection absorber and a span identity.</para>
        /// <para>This field is read-only.</para>
        /// </summary>
        // I would have prefered to keep this internal. This way, there would
        // have been only one way to create an empty range (not entirely true,
        // since one can always use default(RangeSet<T>). Unfortunately, the
        // compiler doesn't like it: either CS0649, or CA1805 if we initialize
        // the field to suppress the compiler warning. I could just ignore the
        // warning as this field is supposed to always have its default value.
        public static readonly RangeSet<T> Empty;

        // Default value = empty set, _isInhabited = false and _endpoints = (default(T), default(T)).

        /// <summary>
        /// Represents the pair of left and right endpoints if this range is inhabited.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly OrderedPair<T> _endpoints;

        private readonly bool _isInhabited;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeSet{T}"/> struct representing the
        /// range [<paramref name="min"/>..<paramref name="max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        public RangeSet(T min, T max)
        {
            if (max.CompareTo(min) < 0) Throw.ArgumentOutOfRange(nameof(max));

            _endpoints = OrderedPair.FromOrderedValues(min, max);
            _isInhabited = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeSet{T}"/> struct from the specified
        /// endpoints.
        /// </summary>
        // Public version: see RangeSet.FromEndpoints().
        internal RangeSet(OrderedPair<T> endpoints)
        {
            _endpoints = endpoints;
            _isInhabited = true;
        }

        /// <summary>
        /// Returns true if this range is empty; otherwise returns false.
        /// </summary>
        public bool IsEmpty => !_isInhabited;

        /// <summary>
        /// Returns a <see cref="Range{T}"/> view of this range.
        /// </summary>
        /// <exception cref="InvalidOperationException">The set is empty.</exception>
        public Range<T> Range => _isInhabited ? new Range<T>(_endpoints)
            : Throw.InvalidOperation<Range<T>>();

        /// <summary>
        /// Returns a culture-independent string representation of this range.
        /// </summary>
        [Pure]
        public override string ToString() => _isInhabited ? Range.ToString() : "[]";
    }

    public partial struct RangeSet<T> // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="RangeSet{T}"/> are equal.
        /// </summary>
        public static bool operator ==(RangeSet<T> left, RangeSet<T> right) => left.Equals(right);

        /// <summary>
        /// Determines whether two specified instances of <see cref="RangeSet{T}"/> are not equal.
        /// </summary>
        public static bool operator !=(RangeSet<T> left, RangeSet<T> right) => !left.Equals(right);

        /// <inheritdoc />
        [Pure]
        public bool Equals(RangeSet<T> other) =>
            _isInhabited == other._isInhabited
            && _endpoints.Equals(other._endpoints);

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is RangeSet<T> range && Equals(range);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(_isInhabited, _endpoints);
    }
}
