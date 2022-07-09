// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    /// <summary>
    /// Represents a pair of a year and a month.
    /// <para><see cref="MonthParts"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="MonthParts"/> does NOT represent a month.</para>
    /// </remarks>
    public readonly partial struct MonthParts : IComparisonOperators<MonthParts, MonthParts>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthParts"/> struct from the specified
        /// year and month values.
        /// </summary>
        public MonthParts(int year, int month)
        {
            Year = year;
            Month = month;
        }

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// Gets the month of the year.
        /// </summary>
        public int Month { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => FormattableString.Invariant($"{Month:D2}/{Year:D4}");

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month) => (year, month) = (Year, Month);

        /// <summary>
        /// Creates a new instance of <see cref="MonthParts"/> representing the first month of the
        /// specified year.
        /// </summary>
        [Pure]
        public static MonthParts AtStartOfYear(int y) => new(y, 1);
    }

    public partial struct MonthParts // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthParts"/> are equal.
        /// </summary>
        public static bool operator ==(MonthParts left, MonthParts right) =>
            left.Year == right.Year && left.Month == right.Month;

        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthParts"/> are not equal.
        /// </summary>
        public static bool operator !=(MonthParts left, MonthParts right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(MonthParts other) => this == other;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is MonthParts fields && this == fields;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Year, Month);
    }

    public partial struct MonthParts // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, Month).</para>
        /// </remarks>
        public static bool operator <(MonthParts left, MonthParts right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, Month).</para>
        /// </remarks>
        public static bool operator <=(MonthParts left, MonthParts right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, Month).</para>
        /// </remarks>
        public static bool operator >(MonthParts left, MonthParts right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, Month).</para>
        /// </remarks>
        public static bool operator >=(MonthParts left, MonthParts right) =>
            left.CompareTo(right) >= 0;

        /// <inheritdoc />
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, Month).</para>
        /// </remarks>
        [Pure]
        public int CompareTo(MonthParts other)
        {
            int c = Year.CompareTo(other.Year);
            if (c == 0)
            {
                c = Month.CompareTo(other.Month);
            }
            return c;
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is MonthParts parts ? CompareTo(parts)
            : Throw.NonComparable(typeof(MonthParts), obj);
    }
}
