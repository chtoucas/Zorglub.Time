// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    /// <summary>
    /// Represents a closed bounded interval.
    /// <para>This type has been designed with discrete intervals in mind, nevertheless everything
    /// should work fine for continuous intervals, except that formatting and documentation should
    /// feel a bit awkward.</para>
    /// <para>An instance may be reduced to a single value.</para>
    /// <para><typeparamref name="T"/> SHOULD be an <i>immutable</i> value type.</para>
    /// <para><see cref="Range{T}"/> is an immutable struct.</para>
    /// </summary>
    /// <typeparam name="T">The type of the interval's elements.</typeparam>
    public readonly partial struct Range<T> :
        ISegment<T>,
        ISetComparable<Range<T>>,
        IEqualityOperators<Range<T>, Range<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Range{T}"/> struct representing the
        /// range [<paramref name="min"/>..<paramref name="max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        public Range(T min, T max)
        {
            if (max.CompareTo(min) < 0) Throw.ArgumentOutOfRange(nameof(max));

            Endpoints = OrderedPair.FromOrderedValues(min, max);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range{T}"/> struct from the specified
        /// endpoints.
        /// </summary>
        // Public version: see Range.FromEndpoints().
        internal Range(OrderedPair<T> endpoints) { Endpoints = endpoints; }

        /// <inheritdoc />
        public OrderedPair<T> Endpoints { get; }

        /// <summary>
        /// Gets the minimum.
        /// <para>The minimum is a minimal element also called the least element.</para>
        /// </summary>
        public T Min => Endpoints.LowerValue;

        T ISegment<T>.LowerEnd => Min;

        /// <summary>
        /// Gets the maximum.
        /// <para>The maximum is a maximal element also called the greatest element.</para>
        /// </summary>
        public T Max => Endpoints.UpperValue;

        T ISegment<T>.UpperEnd => Max;

        /// <inheritdoc />
        public bool IsLeftOpen => false;

        /// <inheritdoc />
        public bool IsRightOpen => false;

        /// <inheritdoc />
        public bool IsLeftBounded => true;

        /// <inheritdoc />
        public bool IsRightBounded => true;

        /// <summary>
        /// Returns true if this interval consists of a single value; otherwise returns false.
        /// <para>A singleton interval is also said to be <i>degenerate</i>.</para>
        /// </summary>
        public bool IsSingleton => Max.Equals(Min);

        /// <summary>
        /// Returns a culture-independent string representation of this range.
        /// </summary>
        [Pure]
        public override string ToString() =>
            IsSingleton ? FormattableString.Invariant($"[{Min}]")
            : FormattableString.Invariant($"[{Min}..{Max}]");
    }

    public partial struct Range<T> // Adjustments
    {
        #region Adjustments

        /// <summary>
        /// Adjusts the minimum to the specified value, yielding a new <see cref="Range{T}"/>
        /// instance representing the interval [<paramref name="min"/>, <see cref="Max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="min"/> is greater than <see cref="Max"/>.
        /// </exception>
        [Pure]
        public Range<T> WithMin(T min)
        {
            // We don't write
            // > new(min, Max)
            // to obtain the correct parameter name in the exception.
            if (min.CompareTo(Max) > 0) Throw.ArgumentOutOfRange(nameof(min));

            var endpoints = OrderedPair.FromOrderedValues(min, Max);
            return new(endpoints);
        }

        /// <summary>
        /// Adjusts the maximum to the specified value, yielding a new <see cref="Range{T}"/>
        /// instance representing the interval [<see cref="Min"/>, <paramref name="max"/>].
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than <see cref="Min"/>.
        /// </exception>
        [Pure]
        public Range<T> WithMax(T max) => new(Min, max);

        #endregion
    }

    public partial struct Range<T> // ISet...
    {
        #region Membership

        /// <inheritdoc />
        [Pure]
        public bool Contains(T value) => Min.CompareTo(value) <= 0 && value.CompareTo(Max) <= 0;

        #endregion
        #region Inclusion

        /// <inheritdoc />
        [Pure]
        public bool IsSubsetOf(Range<T> other) =>
            other.Min.CompareTo(Min) <= 0 && Max.CompareTo(other.Max) <= 0;

        /// <inheritdoc />
        [Pure]
        public bool IsProperSubsetOf(Range<T> other) => this != other && IsSubsetOf(other);

        /// <inheritdoc />
        [Pure]
        public bool IsSupersetOf(Range<T> other) =>
            Min.CompareTo(other.Min) <= 0 && other.Max.CompareTo(Max) <= 0;

        /// <inheritdoc />
        [Pure]
        public bool IsProperSupersetOf(Range<T> other) => this != other && IsSupersetOf(other);

        #endregion
        #region Equality

        /// <inheritdoc />
        [Pure]
        public bool SetEquals(Range<T> other) => Endpoints == other.Endpoints;

        #endregion
    }

    public partial struct Range<T> // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Range{T}"/> are equal.
        /// </summary>
        public static bool operator ==(Range<T> left, Range<T> right) =>
            left.Endpoints == right.Endpoints;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Range{T}"/> are not equal.
        /// </summary>
        public static bool operator !=(Range<T> left, Range<T> right) =>
            left.Endpoints != right.Endpoints;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Range<T> other) => Endpoints == other.Endpoints;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Range<T> range && Equals(range);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Endpoints.GetHashCode();
    }

    public partial struct Range<T> // Strict partial order
    {
        // REVIEW(api): strict partial order. For other types too. External comparer?
#if false
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

        /// <summary>
        /// Compares the two specified instances to see if the left one is before the right one.
        /// </summary>
        public static bool operator <(Range<T> left, Range<T> right) =>
            left.Max.CompareTo(right.Min) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is after the right one.
        /// </summary>
        public static bool operator >(Range<T> left, Range<T> right) =>
            right.Max.CompareTo(left.Min) < 0;

        /// <summary>
        /// Compares the two specified values to see if the left one is before the right one.
        /// </summary>
        public static bool operator <(Range<T> left, T right) => left.Max.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified values to see if the left one is before the right one.
        /// </summary>
        public static bool operator >(Range<T> left, T right) => left.Min.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified values to see if the left one is before the right one.
        /// </summary>
        public static bool operator <(T left, Range<T> right) => left.CompareTo(right.Min) < 0;

        /// <summary>
        /// Compares the two specified values to see if the left one is before the right one.
        /// </summary>
        public static bool operator >(T left, Range<T> right) => left.CompareTo(right.Max) > 0;

#pragma warning restore CA2225
#endif
    }
}
