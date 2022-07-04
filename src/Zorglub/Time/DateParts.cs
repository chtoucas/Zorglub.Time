﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    // FIXME(code): use a record struct? tests with negative valuse for m and d.
    // ToYemoda(ICalendarScope scope) it's not enough to ensure that Year and
    // Month are valid for Yemo. Idem w/ the other fields types.

    // Main difference w/ Yemoda: DateParts does not force y, m, d to be in a
    // specific range; we don't even require m and d to be >= 1.

    /// <summary>
    /// Represents a triple of a year, a month and a day.
    /// <para><see cref="DateParts"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>This type uses the lexicographic order on triples (Year, Month, Day).</para>
    /// <para>A <see cref="DateParts"/> does NOT represent a date.</para>
    /// </remarks>
    public readonly partial struct DateParts : IEqualityOperators<DateParts, DateParts>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateParts"/> struct from the specified
        /// date parts.
        /// </summary>
        public DateParts(Yemoda parts)
        {
            parts.Unpack(out int y, out int m, out int d);
            Year = y;
            Month = m;
            Day = d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateParts"/> struct from the specified year,
        /// month and day values.
        /// </summary>
        public DateParts(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
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
        /// Gets the day of the month.
        /// </summary>
        public int Day { get; }

        /// <summary>
        /// Gets the month parts.
        /// </summary>
        public MonthParts MonthParts => new(Year, Month);

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"{Day:D2}/{Month:D2}/{Year:D4}");

        /// <summary>
        /// Deconstructs the current instance into its components.
        /// </summary>
        public void Deconstruct(out int year, out int month, out int day) =>
            (year, month, day) = (Year, Month, Day);

        /// <summary>
        /// Converts the current instance to a <see cref="Yemoda"/> value.
        /// </summary>
        /// <exception cref="AoorException">The current instance is not representable by a
        /// <see cref="Yemoda"/>; one of the value is too large to be handled by the system.
        /// </exception>
        [Pure]
        public Yemoda ToYemoda() => Yemoda.Create(Year, Month, Day);

        /// <summary>
        /// Validates the current instance with the specified scope then converts it to a
        /// <see cref="Yemoda"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="scope"/> is null.</exception>
        /// <exception cref="AoorException">The current instance is not valid according to the
        /// specified scope.</exception>
        [Pure]
        public Yemoda ToYemoda(ICalendarScope scope)
        {
            Requires.NotNull(scope);

            scope.ValidateYearMonthDay(Year, Month, Day);
            return new Yemoda(Year, Month, Day);
        }
    }

    public partial struct DateParts // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="DateParts"/> are equal.
        /// </summary>
        public static bool operator ==(DateParts left, DateParts right) =>
            left.Year == right.Year && left.Month == right.Month && left.Day == right.Day;

        /// <summary>
        /// Determines whether two specified instances of <see cref="DateParts"/> are not equal.
        /// </summary>
        public static bool operator !=(DateParts left, DateParts right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(DateParts other) => this == other;

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is DateParts parts && this == parts;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Year, Month, Day);
    }

    //public partial struct DateParts // IComparable
    //{
    //    /// <summary>
    //    /// Compares the two specified instances to see if the left one is strictly earlier than the
    //    /// right one.
    //    /// </summary>
    //    /// <remarks>
    //    /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
    //    /// </para>
    //    /// </remarks>
    //    public static bool operator <(DateParts left, DateParts right) => left.CompareTo(right) < 0;

    //    /// <summary>
    //    /// Compares the two specified instances to see if the left one is earlier than or equal to
    //    /// the right one.
    //    /// </summary>
    //    /// <remarks>
    //    /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
    //    /// </para>
    //    /// </remarks>
    //    public static bool operator <=(DateParts left, DateParts right) => left.CompareTo(right) <= 0;

    //    /// <summary>
    //    /// Compares the two specified instances to see if the left one is strictly later than the
    //    /// right one.
    //    /// </summary>
    //    /// <remarks>
    //    /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
    //    /// </para>
    //    /// </remarks>
    //    public static bool operator >(DateParts left, DateParts right) => left.CompareTo(right) > 0;

    //    /// <summary>
    //    /// Compares the two specified instances to see if the left one is later than or equal to
    //    /// the right one.
    //    /// </summary>
    //    /// <remarks>
    //    /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
    //    /// </para>
    //    /// </remarks>
    //    public static bool operator >=(DateParts left, DateParts right) => left.CompareTo(right) >= 0;

    //    /// <inheritdoc />
    //    /// <remarks>
    //    /// <para>The comparison is done using the lexicographic order on triples (Year, Month, Day).
    //    /// </para>
    //    /// </remarks>
    //    [Pure]
    //    public int CompareTo(DateParts other) =>
    //        Equals(other) ? 0
    //        : Year.CompareTo(other.Year) < 0 ? -1
    //        : Month.CompareTo(other.Month) < 0 ? -1
    //        : Day.CompareTo(other.Day) < 0 ? - 1
    //        : 1;

    //    /// <inheritdoc />
    //    [Pure]
    //    public int CompareTo(object? obj) =>
    //        obj is null ? 1
    //        : obj is DateParts ymd ? CompareTo(ymd)
    //        : Throw.NonComparable(typeof(Yemoda), obj);
    //}
}
