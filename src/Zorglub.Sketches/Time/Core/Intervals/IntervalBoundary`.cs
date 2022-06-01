// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Intervals
{
    using System.Linq;

    /// <summary>
    /// Provides static helpers for <see cref="IntervalBoundary{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class IntervalBoundary
    {
        /// <summary>
        /// Obtains the empty boundary.
        /// </summary>
        [Pure]
        public static IntervalBoundary<T> Empty<T>()
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return IntervalBoundary<T>.Empty;
        }

        /// <summary>
        /// Creates a new <see cref="IntervalBoundary{T}"/> from the specified minimum and maximum.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        [Pure]
        public static IntervalBoundary<T> Create<T>(T min, T max)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(min, max);
        }

        /// <summary>
        /// Creates a new <see cref="IntervalBoundary{T}"/> from the specified value.
        /// </summary>
        [Pure]
        public static IntervalBoundary<T> Singleton<T>(T value)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(value, value);
        }

        /// <summary>
        /// Creates a new <see cref="IntervalBoundary{T}"/> from the specified endpoints.
        /// </summary>
        [Pure]
        public static IntervalBoundary<T> FromEndpoints<T>(OrderedPair<T> endpoints)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new IntervalBoundary<T>(endpoints);
        }
    }

    /// <summary>
    /// Defines the boundary of an interval, the set of its endpoints, that is an empty set, a
    /// singleton or a pair of <i>comparable</i> elements.
    /// <para><typeparamref name="T"/> SHOULD be an <i>immutable</i> value type.</para>
    /// <para><see cref="IntervalBoundary{T}"/> is an immutable struct.</para>
    /// </summary>
    /// <typeparam name="T">The type of the boundary's elements.</typeparam>
    public readonly partial struct IntervalBoundary<T> :
        IEqualityOperators<IntervalBoundary<T>, IntervalBoundary<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Represents the empty set.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly IntervalBoundary<T> Empty;

        // Default value = empty set, _count = 0 and _min = _max = default(T).

        /// <summary>
        /// Represents the minimum if this boundary is not empty.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly T _min;

        /// <summary>
        /// Represents the maximum if this boundary is not empty.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly T _max;

        /// <summary>
        /// Represents the number of elements.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalBoundary{T}"/> struct.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="max"/> is less than
        /// <paramref name="min"/>.</exception>
        public IntervalBoundary(T min, T max)
        {
            if (max.CompareTo(min) < 0) Throw.ArgumentOutOfRange(nameof(max));

            _min = min;
            _max = max;
            _count = min.Equals(max) ? 1 : 2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalBoundary{T}"/> struct from the
        /// specified endpoints.
        /// </summary>
        internal IntervalBoundary(OrderedPair<T> endpoints)
        {
            (_min, _max) = endpoints;
            _count = _min.Equals(_max) ? 1 : 2;
        }

        /// <summary>
        /// Returns true if this boundary is empty; otherwise returns false.
        /// </summary>
        public bool IsEmpty => _count == 0;

        /// <summary>
        /// Returns true if this boundary is a singleton; otherwise returns false.
        /// </summary>
        public bool IsSingleton => _count == 1;

        /// <summary>
        /// Returns true if this boundary is a <i>non-degenerate</i> pair; otherwise returns false.
        /// </summary>
        public bool IsProper => _count == 2;

        /// <summary>
        /// Gets the number of elements (0, 1 or 2).
        /// <para>Returns 0 if the set is empty.</para>
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Attempts to get the pair of endpoints.
        /// </summary>
        /// <exception cref="InvalidOperationException">The set is empty.</exception>
        [Pure]
        public OrderedPair<T> Endpoints => IsEmpty ? Throw.InvalidOperation<OrderedPair<T>>()
            : OrderedPair.FromOrderedValues(Min, Max);

        /// <summary>
        /// Gets the minimum element of the boundary set.
        /// <para>Before calling this property, you MUST ensure that this boundary is not empty.
        /// </para>
        /// </summary>
        internal T Min
        {
            get
            {
                Debug.Assert(!IsEmpty);
                return _min;
            }
        }

        /// <summary>
        /// Gets the maximum element of the boundary set.
        /// <para>Before calling this property, you MUST ensure that this boundary is not empty.
        /// </para>
        /// </summary>
        internal T Max
        {
            get
            {
                Debug.Assert(!IsEmpty);
                return _max;
            }
        }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() =>
            _count switch
            {
                0 => "{}",
                1 => FormattableString.Invariant($"{{{Min}}}"),
                2 => FormattableString.Invariant($"{{{Min}, {Max}}}"),
                _ => "",
            };

        /// <summary>
        /// Obtains an <see cref="IEnumerable{T}"/> view of this boundary.
        /// </summary>
        [Pure]
        public IEnumerable<T> ToEnumerable() =>
            _count switch
            {
                0 => Enumerable.Empty<T>(),
                1 => SingletonSet.Create(Min),
                2 => new T[2] { Min, Max },
                _ => Throw.Unreachable<IEnumerable<T>>(),
            };
    }

    public partial struct IntervalBoundary<T> // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="IntervalBoundary{T}"/> are
        /// equal.
        /// </summary>
        public static bool operator ==(IntervalBoundary<T> left, IntervalBoundary<T> right) =>
            left.Equals(right);

        /// <summary>
        /// Determines whether two specified instances of <see cref="IntervalBoundary{T}"/> are not
        /// equal.
        /// </summary>
        public static bool operator !=(IntervalBoundary<T> left, IntervalBoundary<T> right) =>
            !left.Equals(right);

        /// <inheritdoc/>
        [Pure]
        public bool Equals(IntervalBoundary<T> other) =>
            _count == other._count
            && _min.Equals(other._min)
            && _max.Equals(other._max);

        /// <inheritdoc/>
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is IntervalBoundary<T> boundary && Equals(boundary);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => HashCode.Combine(_count, _min, _max);
    }
}
