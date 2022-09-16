// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents a pair of a year and a day of the year.
    /// <para>This type uses the lexicographic order on pairs (Year, DayOfYear).</para>
    /// <para><see cref="OrdinalParts"/> does NOT represent an ordinal date, its default value is
    /// not even a valid ordinal date.</para>
    /// <para><see cref="OrdinalParts"/> is an immutable struct.</para>
    /// </summary>
    /// <param name="Year">Algebraic year number.</param>
    /// <param name="DayOfYear">Day of the year.</param>
    public readonly record struct OrdinalParts(int Year, int DayOfYear) :
        IComparisonOperators<OrdinalParts, OrdinalParts>
    {
        /// <summary>
        /// Creates a new instance of <see cref="OrdinalParts"/> representing the first day of the
        /// specified year.
        /// </summary>
        [Pure]
        public static OrdinalParts AtStartOfYear(int y) => new(y, 1);

        //
        // IComparable
        //

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        public static bool operator <(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        public static bool operator <=(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        public static bool operator >(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        public static bool operator >=(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) >= 0;

        /// <inheritdoc />
        [Pure]
        public int CompareTo(OrdinalParts other)
        {
            int c = Year.CompareTo(other.Year);
            return c == 0 ? DayOfYear.CompareTo(other.DayOfYear) : c;
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is OrdinalParts parts ? CompareTo(parts)
            : Throw.NonComparable(typeof(OrdinalParts), obj);
    }
}
