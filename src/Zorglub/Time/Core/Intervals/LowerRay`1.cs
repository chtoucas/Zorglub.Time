// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    // REVIEW(code): ISetComparable<> with other interval types.
    // Idem for the other interval types.

    /// <summary>
    /// Represents a right-bounded closed ray; it has a maximum and no lower bound.
    /// <para>This type has been designed with discrete intervals in mind, nevertheless everything
    /// should work fine for continuous intervals, except that formatting and documentation should
    /// feel a bit awkward.</para>
    /// <para>Represents also the (weak) lower closure of a value.</para>
    /// <para><see cref="LowerRay{T}"/> is an immutable struct.</para>
    /// </summary>
    /// <typeparam name="T">The type of the ray's elements.</typeparam>
    public readonly partial struct LowerRay<T> :
        IRay<T>,
        ISetComparable<LowerRay<T>>,
        IEqualityOperators<LowerRay<T>, LowerRay<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LowerRay{T}"/> struct representing the
        /// ray [..<paramref name="max"/>], the set of values less than or equal to
        /// <paramref name="max"/>.
        /// </summary>
        public LowerRay(T max) { Max = max; }

        /// <summary>
        /// Gets the maximum.
        /// <para>The maximum is a maximal element also called the greatest element.</para>
        /// </summary>
        public T Max { get; }

        T IRay<T>.Endpoint => Max;

        /// <inheritdoc />
        public bool IsLeftOpen => true;

        /// <inheritdoc />
        public bool IsRightOpen => false;

        /// <inheritdoc />
        public bool IsLeftBounded => false;

        /// <inheritdoc />
        public bool IsRightBounded => true;

        // I would have liked that ToString() returned "[..]" for the whole range.
        // Possible but it means that we have to add the constraint T : IMinMaxValue<T>.
        // However, we can also argue that returning "[..2147483647]" is the right
        // thing to do because it matches the membership condition x <= 2147483647.
        // We have a similar "problem" with the singleton lower ray x <= -2147483648.

        /// <summary>
        /// Returns a culture-independent string representation of this ray.
        /// </summary>
        [Pure]
        public override string ToString() => FormattableString.Invariant($"[..{Max}]");
    }

    public partial struct LowerRay<T> // ISet...
    {
        #region Membership

        /// <inheritdoc />
        [Pure]
        public bool Contains(T value) => value.CompareTo(Max) <= 0;

        #endregion
        #region Inclusion

        /// <inheritdoc />
        [Pure]
        public bool IsSubsetOf(LowerRay<T> other) => other.Max.CompareTo(Max) >= 0;

        /// <inheritdoc />
        [Pure]
        public bool IsProperSubsetOf(LowerRay<T> other) => other.Max.CompareTo(Max) > 0;

        /// <inheritdoc />
        [Pure]
        public bool IsSupersetOf(LowerRay<T> other) => Max.CompareTo(other.Max) >= 0;

        /// <inheritdoc />
        [Pure]
        public bool IsProperSupersetOf(LowerRay<T> other) => Max.CompareTo(other.Max) > 0;

        #endregion
        #region Equality

        /// <inheritdoc />
        [Pure]
        public bool SetEquals(LowerRay<T> other) => Max.Equals(other.Max);

        #endregion
    }

    public partial struct LowerRay<T> // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="LowerRay{T}"/> are equal.
        /// </summary>
        public static bool operator ==(LowerRay<T> left, LowerRay<T> right) =>
            left.Max.Equals(right.Max);

        /// <summary>
        /// Determines whether two specified instances of <see cref="LowerRay{T}"/> are not
        /// equal.
        /// </summary>
        public static bool operator !=(LowerRay<T> left, LowerRay<T> right) =>
            !left.Max.Equals(right.Max);

        /// <inheritdoc />
        [Pure]
        public bool Equals(LowerRay<T> other) => Max.Equals(other.Max);

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is LowerRay<T> ray && Equals(ray);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Max.GetHashCode();
    }
}
