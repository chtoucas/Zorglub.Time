// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;

using static Zorglub.Time.Extensions.Unboxing;

// Using a record struct is not a great choice. Main drawbacks:
// - bad default value: 0/0/0, month and day should be > 0 (but of course we
//   could change that)
// - ToString() outputs all props (but of course we could change that)
// - slower and bigger runtime size than the other types

/// <summary>
/// Provides an affine Gregorian date as a record struct.
/// </summary>
public readonly partial record struct GregorianRecord :
    IAffineDate<GregorianRecord>,
    IYearEndpointsProvider<GregorianRecord>,
    IMonthEndpointsProvider<GregorianRecord>,
    IMinMaxValue<GregorianRecord>,
    ISubtractionOperators<GregorianRecord, int, GregorianRecord>
{
    private static readonly GregorianSchema s_Schema = GregorianSchema.GetInstance().Unbox();

    public GregorianRecord(int year, int month, int day)
    {
        if (s_Schema.SupportedYears.Contains(year) == false) throw new ArgumentOutOfRangeException(nameof(year));
        s_Schema.PreValidator.ValidateMonthDay(year, month, day);

        Year = year;
        Month = month;
        Day = day;
    }

    private GregorianRecord(Yemoda ymd)
    {
        (Year, Month, Day) = ymd;
    }

    public static Range<int> SupportedYears => s_Schema.SupportedYears;
    public static GregorianRecord MinValue { get; } = new(s_Schema.Segment.MinMaxDateParts.LowerValue);
    public static GregorianRecord MaxValue { get; } = new(s_Schema.Segment.MinMaxDateParts.UpperValue);

    private static Range<int> Domain => s_Schema.Domain;
    private static ICalendricalArithmetic Arithmetic => s_Schema.Arithmetic;

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year { get; }
    public int Month { get; }
    public int DayOfYear => s_Schema.GetDayOfYear(Year, Month, Day);
    public int Day { get; }
    public bool IsIntercalary => s_Schema.IsIntercalaryDay(Year, Month, Day);
    public bool IsSupplementary => s_Schema.IsSupplementaryDay(Year, Month, Day);

    // FIXME(code): initialization of Yemoda.
    private Yemoda Yemoda => Yemoda.Create(Year, Month, Day);

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = (Year, Month, Day);
}

public partial record struct GregorianRecord // Conversions, adjustments...
{
    [Pure]
    public static GregorianRecord FromDaysSinceEpoch(int daysSinceEpoch)
    {
        if (Domain.Contains(daysSinceEpoch) == false) throw new ArgumentOutOfRangeException(nameof(daysSinceEpoch));

        var (y, m, d) = s_Schema.GetDateParts(daysSinceEpoch);
        return new GregorianRecord(y, m, d);
    }

    [Pure]
    public int CountDaysSinceEpoch() => s_Schema.CountDaysSinceEpoch(Year, Month, Day);

    #region Counting

    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(Year, Month, Day);

    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(Year, Month, Day);

    [Pure]
    public int CountElapsedDaysInMonth() => Day - 1;

    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(Year, Month, Day);

    #endregion
    #region Year and month boundaries

    [Pure]
    public static GregorianRecord GetStartOfYear(GregorianRecord day) => new(day.Year, 1, 1);

    [Pure]
    public static GregorianRecord GetEndOfYear(GregorianRecord day)
    {
        int y = day.Year;
        s_Schema.GetDatePartsAtEndOfYear(y, out int m, out int d);
        return new GregorianRecord(y, m, d);
    }

    [Pure]
    public static GregorianRecord GetStartOfMonth(GregorianRecord day) => new(day.Year, day.Month, 1);

    [Pure]
    public static GregorianRecord GetEndOfMonth(GregorianRecord day)
    {
        int y = day.Year;
        int m = day.Month;
        int d = s_Schema.CountDaysInMonth(y, m);
        return new GregorianRecord(y, m, d);
    }

    #endregion
}

public partial record struct GregorianRecord // IComparable
{
    public static bool operator <(GregorianRecord left, GregorianRecord right) => left.CompareTo(right) < 0;
    public static bool operator <=(GregorianRecord left, GregorianRecord right) => left.CompareTo(right) <= 0;
    public static bool operator >(GregorianRecord left, GregorianRecord right) => left.CompareTo(right) > 0;
    public static bool operator >=(GregorianRecord left, GregorianRecord right) => left.CompareTo(right) >= 0;

    [Pure]
    public static GregorianRecord Min(GregorianRecord x, GregorianRecord y) => x.CompareTo(y) < 0 ? x : y;

    [Pure]
    public static GregorianRecord Max(GregorianRecord x, GregorianRecord y) => x.CompareTo(y) > 0 ? x : y;

    [Pure]
    public int CompareTo(GregorianRecord other) =>
        // FIXME(code): I feel lazy right now.
        throw new NotImplementedException();

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianRecord date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(GregorianRecord).GetType()}.",
            nameof(obj));
}

public partial record struct GregorianRecord // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(GregorianRecord left, GregorianRecord right) => left.CountDaysSince(right);
    public static GregorianRecord operator +(GregorianRecord value, int days) => value.PlusDays(days);
    public static GregorianRecord operator -(GregorianRecord value, int days) => value.PlusDays(-days);
    public static GregorianRecord operator ++(GregorianRecord value) => value.NextDay();
    public static GregorianRecord operator --(GregorianRecord value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(GregorianRecord other) =>
        Arithmetic.CountDaysBetween(other.Yemoda, Yemoda);

    [Pure]
    public GregorianRecord PlusDays(int days) =>
        new(Arithmetic.AddDays(Yemoda, days));

    [Pure]
    public GregorianRecord NextDay() =>
        new(Arithmetic.NextDay(Yemoda));

    [Pure]
    public GregorianRecord PreviousDay() =>
        new(Arithmetic.PreviousDay(Yemoda));
}
