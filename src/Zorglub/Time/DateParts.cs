// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using System.Globalization;
using System.Text;

using Zorglub.Time.Core;

// Main difference w/ Yemoda: y, m, d are open values, we don't even require
// m and d to be >= 1.

/// <summary>Represents a triple of a year, a month and a day.</summary>
/// <remarks>
/// <para>This type uses the lexicographic order on triples (Year, Month, Day).</para>
/// <para><see cref="DateParts"/> does NOT represent a date, its default value is not even a valid
/// date.</para>
/// <para><see cref="DateParts"/> is an immutable struct.</para>
/// </remarks>
/// <param name="Year">Algebraic year number.</param>
/// <param name="Month">Month of the year.</param>
/// <param name="Day">Day of the month.</param>
public readonly record struct DateParts(int Year, int Month, int Day) :
    IComparisonOperators<DateParts, DateParts>
{
    /// <summary>Gets the month parts of the current instance.</summary>
    public MonthParts MonthParts => new(Year, Month);

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(CultureInfo.InvariantCulture, $"Year = {Year}, Month = {Month}, Day = {Day}");
        return true;
    }

    /// <summary>Creates a new instance of <see cref="DateParts"/> representing the first day of the
    /// specified year.</summary>
    [Pure]
    public static DateParts AtStartOfYear(int y) => new(y, 1, 1);

    /// <summary>Creates a new instance of <see cref="DateParts"/> representing the first day of the
    /// specified month.</summary>
    [Pure]
    public static DateParts AtStartOfMonth(int y, int m) => new(y, m, 1);

    /// <inheritdoc />
    public static bool operator <(DateParts left, DateParts right) => left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(DateParts left, DateParts right) => left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(DateParts left, DateParts right) => left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(DateParts left, DateParts right) => left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(DateParts other)
    {
        int c = Year.CompareTo(other.Year);
        if (c == 0)
        {
            c = Month.CompareTo(other.Month);
            if (c == 0)
            {
                c = Day.CompareTo(other.Day);
            }
        }
        return c;
    }

    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DateParts parts ? CompareTo(parts)
        : Throw.NonComparable(typeof(DateParts), obj);
}
