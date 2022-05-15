// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Hemerology;

    // Differences w/ Yemoda:
    // - DateParts does not force y, m, d to be in a specific range; we still
    //   require m and d to be >= 1.
    // - DateParts does not implement IComparable<>.

    /// <summary>
    /// Represents a triple of a year, a month and a day.
    /// <para><see cref="DateParts"/> is an immutable struct.</para>
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="DateParts"/> value is NOT a true date.</para>
    /// </remarks>
    public readonly partial struct DateParts : IEqualityOperators<DateParts, DateParts>
    {
        // We make sure that default(DateParts) is such that Month = Day = 1,
        // not 0 which surely is invalid.

        /// <summary>
        /// Represents the algebraic month of the year (start at zero).
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _month0;

        /// <summary>
        /// Represents the algebraic day of the month (start at zero).
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _day0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateParts"/> struct from the specified
        /// date parts.
        /// </summary>
        public DateParts(Yemoda parts)
        {
            parts.Unpack(out int y, out int m, out int d);
            Year = y;
            _month0 = m - 1;
            _day0 = d - 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateParts"/> struct from the specified year,
        /// month and day.
        /// </summary>
        /// <exception cref="AoorException"><paramref name="month"/> is a negative integer.
        /// </exception>
        /// <exception cref="AoorException"><paramref name="day"/> is a negative integer.</exception>
        public DateParts(int year, int month, int day)
        {
            if (month < 1) Throw.MonthOutOfRange(month);
            if (day < 1) Throw.DayOutOfRange(day);

            Year = year;
            _month0 = month - 1;
            _day0 = day - 1;
        }

        /// <summary>
        /// Gets the century of the era.
        /// </summary>
        public Ord CenturyOfEra => Ord.FromInt32(Century);

        /// <summary>
        /// Gets the century number.
        /// </summary>
        public int Century => YearNumbering.GetCentury(Year);

        /// <summary>
        /// Gets the year of the era.
        /// </summary>
        /// <exception cref="AoorException"><see cref="Year"/> is lower than
        /// <see cref="Ord.MinAlgebraicValue"/>.</exception>
        public Ord YearOfEra => Ord.FromInt32(Year);

        /// <summary>
        /// Gets the year of the century.
        /// <para>The result is in the range from 1 to 100.</para>
        /// </summary>
        public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

        /// <summary>
        /// Gets the (algebraic) year number.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// Gets the month of the year.
        /// </summary>
        public int Month => _month0 + 1;

        /// <summary>
        /// Gets the day of the month.
        /// </summary>
        public int Day => _day0 + 1;

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
            left.Year == right.Year && left._month0 == right._month0 && left._day0 == right._day0;

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
}
