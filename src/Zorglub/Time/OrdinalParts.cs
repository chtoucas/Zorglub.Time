﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    /// <summary>
    /// Represents a pair of a year and a day of the year.
    /// <para><see cref="OrdinalParts"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>An <see cref="OrdinalParts"/> does NOT represent an ordinal date.</para>
    /// </remarks>
    public readonly partial struct OrdinalParts : IComparisonOperators<OrdinalParts, OrdinalParts>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalParts"/> struct from the specified
        /// year and day of the year values.
        /// </summary>
        public OrdinalParts(int year, int dayOfYear)
        {
            Year = year;
            DayOfYear = dayOfYear;
        }

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// Gets the day of the year.
        /// </summary>
        public int DayOfYear { get; }

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"{DayOfYear:D3}/{Year:D4}");

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int dayOfYear) =>
            (year, dayOfYear) = (Year, DayOfYear);

        /// <summary>
        /// Creates a new instance of <see cref="OrdinalParts"/> representing the first day of the
        /// specified year.
        /// </summary>
        [Pure]
        public static OrdinalParts AtStartOfYear(int y) => new(y, 1);
    }

    public partial struct OrdinalParts // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalParts"/> are equal.
        /// </summary>
        public static bool operator ==(OrdinalParts left, OrdinalParts right) =>
            left.Year == right.Year && left.DayOfYear == right.DayOfYear;

        /// <summary>
        /// Determines whether two specified instances of <see cref="OrdinalParts"/> are not equal.
        /// </summary>
        public static bool operator !=(OrdinalParts left, OrdinalParts right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(OrdinalParts other) => this == other;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is OrdinalParts parts && this == parts;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Year, DayOfYear);
    }

    public partial struct OrdinalParts // IComparable
    {
        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly earlier than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
        /// </para>
        /// </remarks>
        public static bool operator <(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is earlier than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
        /// </para>
        /// </remarks>
        public static bool operator <=(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is strictly later than the
        /// right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
        /// </para>
        /// </remarks>
        public static bool operator >(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Compares the two specified instances to see if the left one is later than or equal to
        /// the right one.
        /// </summary>
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
        /// </para>
        /// </remarks>
        public static bool operator >=(OrdinalParts left, OrdinalParts right) =>
            left.CompareTo(right) >= 0;

        /// <inheritdoc />
        /// <remarks>
        /// <para>The comparison is done using the lexicographic order on pairs (Year, DayOfYear).
        /// </para>
        /// </remarks>
        [Pure]
        public int CompareTo(OrdinalParts other)
        {
            int c = Year.CompareTo(other.Year);
            if (c == 0)
            {
                c = DayOfYear.CompareTo(other.DayOfYear);
            }
            return c;
        }

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is OrdinalParts parts ? CompareTo(parts)
            : Throw.NonComparable(typeof(OrdinalParts), obj);
    }
}
