// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Arithmetic;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;

using ZRange = Zorglub.Time.Core.Intervals.Range;

// Using a record struct is not a great choice. Main drawbacks:
// - invalid default value: 0/0/0, year, month and day should be > 0 (but of
//   course we could change that)
// - ToString() outputs all props (but of course we could change that)
// - slower and bigger runtime size than the other types

/// <summary>
/// Provides an affine Gregorian date as a record struct (Year, Month, Day).
/// <para>Years in [1, 9999]</para>
/// </summary>
public readonly partial record struct CivilParts :
    IAffineDate<CivilParts>,
    IMinMaxValue<CivilParts>
{
    private static readonly CalendricalSchema Schema = SchemaActivator.CreateInstance<CivilSchema>();
    private static readonly CalendricalSegment Segment = CalendricalSegment.Create(Schema, ZRange.Create(1, 9999));
}

public readonly partial record struct CivilParts
{
    private static ICalendricalPreValidator PreValidator { get; } = Schema.PreValidator;
    private static DaysValidator DaysValidator { get; } = new(Segment.SupportedDays);
    private static YearsValidator YearsValidator { get; } = new(Segment.SupportedYears);

    private static PartsAdapter PartsAdapter { get; } = new(Schema);
    private static PartsArithmetic PartsArithmetic { get; } = PartsArithmetic.CreateDefault(Schema, Segment.SupportedYears);

    public CivilParts(int year, int month, int day)
    {
        YearsValidator.Validate(year);
        PreValidator.ValidateMonthDay(year, month, day);

        Year = year;
        Month = month;
        Day = day;
    }

    private CivilParts(DateParts parts)
    {
        (Year, Month, Day) = parts;
    }

    public static CivilParts MinValue { get; } = new(Segment.MinMaxDateParts.LowerValue);
    public static CivilParts MaxValue { get; } = new(Segment.MinMaxDateParts.UpperValue);

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year { get; }
    public int Month { get; }
    public int DayOfYear => Schema.GetDayOfYear(Year, Month, Day);
    public int Day { get; }
    public bool IsIntercalary => Schema.IsIntercalaryDay(Year, Month, Day);
    public bool IsSupplementary => Schema.IsSupplementaryDay(Year, Month, Day);

    private DateParts DateParts => new(Year, Month, Day);

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = (Year, Month, Day);
}

public partial record struct CivilParts // Conversions, adjustments...
{
    [Pure]
    public static CivilParts FromDaysSinceEpoch(int daysSinceEpoch)
    {
        DaysValidator.Validate(daysSinceEpoch);
        var parts = PartsAdapter.GetDateParts(daysSinceEpoch);
        return new CivilParts(parts);
    }

    [Pure]
    public int CountDaysSinceEpoch() => Schema.CountDaysSinceEpoch(Year, Month, Day);

    #region Counting

    [Pure]
    public int CountElapsedDaysInYear() => Schema.CountDaysInYearBefore(Year, Month, Day);

    [Pure]
    public int CountRemainingDaysInYear() => Schema.CountDaysInYearAfter(Year, Month, Day);

    [Pure]
    public int CountElapsedDaysInMonth() => Day - 1;

    [Pure]
    public int CountRemainingDaysInMonth() => Schema.CountDaysInMonthAfter(Year, Month, Day);

    #endregion
}

public partial record struct CivilParts // IComparable
{
    public static bool operator <(CivilParts left, CivilParts right) => left.CompareTo(right) < 0;
    public static bool operator <=(CivilParts left, CivilParts right) => left.CompareTo(right) <= 0;
    public static bool operator >(CivilParts left, CivilParts right) => left.CompareTo(right) > 0;
    public static bool operator >=(CivilParts left, CivilParts right) => left.CompareTo(right) >= 0;

    [Pure]
    public static CivilParts Min(CivilParts x, CivilParts y) => x.CompareTo(y) < 0 ? x : y;

    [Pure]
    public static CivilParts Max(CivilParts x, CivilParts y) => x.CompareTo(y) > 0 ? x : y;

    [Pure]
    public int CompareTo(CivilParts other) => DateParts.CompareTo(other.DateParts);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilParts date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(CivilParts).GetType()}.",
            nameof(obj));
}

public partial record struct CivilParts // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(CivilParts left, CivilParts right) => left.CountDaysSince(right);
    public static CivilParts operator +(CivilParts value, int days) => value.PlusDays(days);
    public static CivilParts operator -(CivilParts value, int days) => value.PlusDays(-days);
    public static CivilParts operator ++(CivilParts value) => value.NextDay();
    public static CivilParts operator --(CivilParts value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(CivilParts other) =>
        PartsArithmetic.CountDaysBetween(other.DateParts, DateParts);

    [Pure]
    public CivilParts PlusDays(int days) =>
        new(PartsArithmetic.AddDays(DateParts, days));

    [Pure]
    public CivilParts NextDay() =>
        new(PartsArithmetic.NextDay(DateParts));

    [Pure]
    public CivilParts PreviousDay() =>
        new(PartsArithmetic.PreviousDay(DateParts));
}
