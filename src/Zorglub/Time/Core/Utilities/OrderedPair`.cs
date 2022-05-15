// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    #region Developer Notes

    // An ordered pair is just a 2-tuple whose elements are pre-ordered.
    //
    // Type constraints:
    // - (readonly) struct -> order of values does not change afterward
    //   It's also useful if we want to ensure that default(OrderedPair<>) works
    //   properly.
    // - IComparable -> ctor
    // - IEquatable -> avoid boxing when comparing two pairs for equality
    //
    // We use an OrderedPair<T> rather than an interval for date parts (Yemoda
    // & co). An interval of date parts make little sense. Indeed, a date part
    // is always used in the context of a schema, but most elements of an
    // interval of Yemoda's is simply not valid for this schema.
    //
    // -- no longer true --
    // For dates, we shall provide a custom interval type for each date type.
    // For instance, DayNumber -> DayRange, CalendarDate -> DateRange.
    // Nevertheless, in Calendar APIs, we shall use DayRange and
    // OrderedPair<CalendarDate>, not DateRange:
    // - DayRange is global, it has no dependence on calendars.
    //   Not all DayRange's are valid for a given calendar, and
    //   MinMaxDayNumber is the maximal valid interval of days for it.
    // - By construction, a DateRange is always valid for a given calendar.
    //   As such, using a DateRange for MinMaxDate doesn't give us more than
    //   the min/max values, so better get straight to the point and use an
    //   OrderedPair<CalendarDate>. Furthermore, it is easy to construct the
    //   maximal valid interval from this pair.

    #endregion

    /// <summary>
    /// Provides static helpers for <see cref="OrderedPair{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class OrderedPair
    {
        /// <summary>
        /// Creates a new <see cref="OrderedPair{T}"/> struct from the specified values.
        /// </summary>
        [Pure]
        public static OrderedPair<T> Create<T>(T x, T y)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(x, y);
        }

        /// <summary>
        /// Creates a new <see cref="OrderedPair{T}"/> struct from the <i>already ordered</i> values.
        /// <para>This factory method does NOT validate its parameters.</para>
        /// </summary>
        [Pure]
        internal static OrderedPair<T> FromOrderedValues<T>(T lowerValue, T upperValue)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            return new(lowerValue, upperValue, true);
        }
    }

    /// <summary>
    /// Defines an ordered pair of <i>comparable</i> values.
    /// <para><typeparamref name="T"/> SHOULD be an <i>immutable</i> value type.</para>
    /// <para><see cref="OrderedPair{T}"/> is an immutable struct.</para>
    /// </summary>
    /// <typeparam name="T">The type of the pair's elements.</typeparam>
    public readonly partial struct OrderedPair<T> :
        IEqualityOperators<OrderedPair<T>, OrderedPair<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedPair{T}"/> struct.
        /// </summary>
        public OrderedPair(T x, T y)
        {
            if (y.CompareTo(x) < 0)
            {
                LowerValue = y;
                UpperValue = x;
            }
            else
            {
                LowerValue = x;
                UpperValue = y;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedPair{T}"/> struct.
        /// <para>This constructor method does NOT validate its parameters.</para>
        /// </summary>
        internal OrderedPair(T lowerValue, T upperValue, bool _)
        {
            Debug.Assert(lowerValue.CompareTo(upperValue) <= 0);

            LowerValue = lowerValue;
            UpperValue = upperValue;
        }

        /// <summary>
        /// Gets the lower value.
        /// </summary>
        public T LowerValue { get; }

        /// <summary>
        /// Gets the upper value.
        /// </summary>
        public T UpperValue { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"({LowerValue}, {UpperValue})");

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out T lowerValue, out T upperValue) =>
            (lowerValue, upperValue) = (LowerValue, UpperValue);
    }

    public partial struct OrderedPair<T> // QEP
    {
        /// <summary>
        /// Maps both enclosed elements with the specified selector.
        /// <para>The order of the pair's elements may not be preserved.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        [Pure]
        public OrderedPair<TResult> Select<TResult>(Func<T, TResult> selector)
            where TResult : struct, IEquatable<TResult>, IComparable<TResult>
        {
            Requires.NotNull(selector);

            return new OrderedPair<TResult>(selector(LowerValue), selector(UpperValue));
        }

        /// <summary>
        /// Maps the enclosed elements with the specified selectors.
        /// <para>The order of the pair's elements may not be preserved.</para>
        /// </summary>
        /// <remarks>
        /// <para>Despite its name, this method cannot appear within a Query Expression Pattern.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="lowerValueSelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="upperValueSelector"/> is null.</exception>
        [Pure]
        public OrderedPair<TResult> Select<TResult>(
            Func<T, TResult> lowerValueSelector, Func<T, TResult> upperValueSelector)
            where TResult : struct, IEquatable<TResult>, IComparable<TResult>
        {
            Requires.NotNull(lowerValueSelector);
            Requires.NotNull(upperValueSelector);

            return new OrderedPair<TResult>(lowerValueSelector(LowerValue), upperValueSelector(UpperValue));
        }
    }

    public partial struct OrderedPair<T> // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="OrderedPair{T}"/> are equal.
        /// </summary>
        public static bool operator ==(OrderedPair<T> left, OrderedPair<T> right) =>
            left.LowerValue.Equals(right.LowerValue) && left.UpperValue.Equals(right.UpperValue);

        /// <summary>
        /// Determines whether two specified instances of <see cref="OrderedPair{T}"/> are not equal.
        /// </summary>
        public static bool operator !=(OrderedPair<T> left, OrderedPair<T> right) =>
            !left.LowerValue.Equals(right.LowerValue) || !left.UpperValue.Equals(right.UpperValue);

        /// <inheritdoc/>
        [Pure]
        public bool Equals(OrderedPair<T> other) =>
            LowerValue.Equals(other.LowerValue) && UpperValue.Equals(other.UpperValue);

        /// <inheritdoc/>
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is OrderedPair<T> pair && Equals(pair);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => HashCode.Combine(LowerValue, UpperValue);
    }
}
