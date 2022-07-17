// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    /// <summary>
    /// Represents a pair of a year and a month.
    /// <para><see cref="MonthParts"/> is an immutable struct.</para>
    /// </summary>
    /// <param name="Year">Algebraic year number.</param>
    /// <param name="Month">Month of the year.</param>
    /// <remarks>
    /// <para><see cref="MonthParts"/> does NOT represent a month.</para>
    /// </remarks>
    public readonly record struct MonthParts(int Year, int Month) :
        IComparisonOperators<MonthParts, MonthParts>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MonthParts"/> representing the first month of the
        /// specified year.
        /// </summary>
        [Pure]
        public static MonthParts AtStartOfYear(int y) => new(y, 1);

        //
        // IComparable
        //

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
            return c == 0 ? Month.CompareTo(other.Month) : c;
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is MonthParts parts ? CompareTo(parts)
            : Throw.NonComparable(typeof(MonthParts), obj);
    }
}
