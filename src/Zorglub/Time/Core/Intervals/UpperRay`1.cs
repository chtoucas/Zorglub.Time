// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    /// <summary>
    /// Represents a left-bounded closed ray; it has an minimum and no upper bound.
    /// <para>This type has been designed with discrete intervals in mind, nevertheless everything
    /// should work fine for continuous intervals, except that formatting and documentation should
    /// feel a bit awkward.</para>
    /// <para>Represents also the (weak) upper closure of a value.</para>
    /// <para><see cref="UpperRay{T}"/> is an immutable struct.</para>
    /// </summary>
    /// <typeparam name="T">The type of the ray's elements.</typeparam>
    public readonly partial struct UpperRay<T> :
        IRay<T>,
        ISetComparable<UpperRay<T>>,
        IEqualityOperators<UpperRay<T>, UpperRay<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpperRay{T}"/> struct representing the ray
        /// [<paramref name="min"/>..], the set of values greater than or equal to
        /// <paramref name="min"/>.
        /// </summary>
        public UpperRay(T min) { Min = min; }

        /// <summary>
        /// Gets the minimum.
        /// <para>The minimum is a minimal element also called the least element.</para>
        /// </summary>
        public T Min { get; }

        T IRay<T>.Endpoint => Min;

        /// <inheritdoc />
        public bool IsLeftOpen => false;

        /// <inheritdoc />
        public bool IsRightOpen => true;

        /// <inheritdoc />
        public bool IsLeftBounded => true;

        /// <inheritdoc />
        public bool IsRightBounded => false;

        /// <summary>
        /// Returns a culture-independent string representation of this ray.
        /// </summary>
        [Pure]
        public override string ToString() => FormattableString.Invariant($"[{Min}..]");
    }

    public partial struct UpperRay<T> // ISet...
    {
        #region Membership

        /// <inheritdoc />
        [Pure]
        public bool Contains(T value) => value.CompareTo(Min) >= 0;

        #endregion
        #region Inclusion

        /// <inheritdoc />
        [Pure]
        public bool IsSubsetOf(UpperRay<T> other) => other.Min.CompareTo(Min) <= 0;

        /// <inheritdoc />
        [Pure]
        public bool IsProperSubsetOf(UpperRay<T> other) => other.Min.CompareTo(Min) < 0;

        /// <inheritdoc />
        [Pure]
        public bool IsSupersetOf(UpperRay<T> other) => Min.CompareTo(other.Min) <= 0;

        /// <inheritdoc />
        [Pure]
        public bool IsProperSupersetOf(UpperRay<T> other) => Min.CompareTo(other.Min) < 0;

        #endregion
        #region Equality

        /// <inheritdoc />
        [Pure]
        public bool SetEquals(UpperRay<T> other) => Min.Equals(other.Min);

        #endregion
    }

    public partial struct UpperRay<T> // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="UpperRay{T}"/> are equal.
        /// </summary>
        public static bool operator ==(UpperRay<T> left, UpperRay<T> right) =>
            left.Min.Equals(right.Min);

        /// <summary>
        /// Determines whether two specified instances of <see cref="UpperRay{T}"/> are not
        /// equal.
        /// </summary>
        public static bool operator !=(UpperRay<T> left, UpperRay<T> right) =>
            !left.Min.Equals(right.Min);

        /// <inheritdoc />
        [Pure]
        public bool Equals(UpperRay<T> other) => Min.Equals(other.Min);

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is UpperRay<T> ray && Equals(ray);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Min.GetHashCode();
    }
}
