// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Obsolete;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;

// Differences w/ Yemoda:
// - DateFields does not force y, m, d to be in a specific range; we still
//   require m and d to be >= 1.
// - DateFields does not implement IComparable<>.

/// <summary>
/// Represents a triple of a year, a month and a day.
/// <para><see cref="DateFields"/> is an immutable struct.</para>
/// </summary>
/// <remarks>
/// <para>A <see cref="DateFields"/> does NOT represent a date.</para>
/// </remarks>
public readonly partial struct DateFields : IEqualityOperators<DateFields, DateFields>
{
    // We make sure that default(DateFields) is such that Month = Day = 1,
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
    /// Initializes a new instance of the <see cref="DateFields"/> struct from the specified
    /// date parts.
    /// </summary>
    public DateFields(Yemoda parts)
    {
        parts.Unpack(out int y, out int m, out int d);
        Year = y;
        _month0 = m - 1;
        _day0 = d - 1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateFields"/> struct from the specified year,
    /// month and day values.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="month"/> is a negative integer.
    /// </exception>
    /// <exception cref="AoorException"><paramref name="day"/> is a negative integer.</exception>
    public DateFields(int year, int month, int day)
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
}

public partial struct DateFields // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="DateFields"/> are equal.
    /// </summary>
    public static bool operator ==(DateFields left, DateFields right) =>
        left.Year == right.Year && left._month0 == right._month0 && left._day0 == right._day0;

    /// <summary>
    /// Determines whether two specified instances of <see cref="DateFields"/> are not equal.
    /// </summary>
    public static bool operator !=(DateFields left, DateFields right) => !(left == right);

    /// <inheritdoc />
    public bool Equals(DateFields other) => this == other;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DateFields parts && this == parts;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Year, Month, Day);
}
