// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;
using System.Text;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Arithmetic;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;

using static Zorglub.Time.Extensions.Unboxing;

using ZRange = Zorglub.Time.Core.Intervals.Range;

// Using a record struct is not a great choice. Main drawbacks:
// - invalid default value: 0/0/0, year, month and day should be > 0 (but of
//   course we could change that)
// - slower and bigger runtime size than the other types

/// <summary>
/// Provides an <i>affine</i> Gregorian date as a record struct (Year, Month, Day).
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial record struct CivilParts :
    IAffineDate<CivilParts>,
    IMinMaxValue<CivilParts>
{
    // NB: the order in which the static fields are written is important.

    private static readonly CalendricalSchema s_Schema = CivilSchema.GetInstance().Unbox();
    private static readonly CalendricalSegment s_Segment = CalendricalSegment.Create(s_Schema, ZRange.Create(1, 9999));

    private static readonly ICalendricalPreValidator s_PreValidator = s_Schema.PreValidator;
    private static readonly DaysValidator s_DaysValidator = new(s_Segment.SupportedDays);
    private static readonly YearsValidator s_YearsValidator = new(s_Segment.SupportedYears);

    private static readonly PartsAdapter s_PartsAdapter = new(s_Schema);
    private static readonly PartsArithmetic s_PartsArithmetic = PartsArithmetic.CreateDefault(s_Schema, s_Segment.SupportedYears);

    private static readonly CivilParts s_MinValue = new(s_Segment.MinMaxDateParts.LowerValue);
    private static readonly CivilParts s_MaxValue = new(s_Segment.MinMaxDateParts.UpperValue);

    public CivilParts(int year, int month, int day)
    {
        s_YearsValidator.Validate(year);
        s_PreValidator.ValidateMonthDay(year, month, day);

        Year = year;
        Month = month;
        Day = day;
    }

    private CivilParts(DateParts parts)
    {
        (Year, Month, Day) = parts;
    }

    public static CivilParts MinValue => s_MinValue;
    public static CivilParts MaxValue => s_MaxValue;

    public int DaysSinceEpoch => s_Schema.CountDaysSinceEpoch(Year, Month, Day);

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

    private DateParts DateParts => new(Year, Month, Day);

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(CultureInfo.InvariantCulture, $"Year = {Year}, Month = {Month}, Day = {Day}");
        return true;
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = (Year, Month, Day);

    public void Deconstruct(out int year, out int dayOfYear)
    {
        year = Year;
        dayOfYear = s_Schema.GetDayOfYear(year, Month, Day);
    }
}

public partial record struct CivilParts // Conversions, adjustments...
{
    [Pure]
    public static CivilParts FromDaysSinceEpoch(int daysSinceEpoch)
    {
        s_DaysValidator.Validate(daysSinceEpoch);
        var parts = s_PartsAdapter.GetDateParts(daysSinceEpoch);
        return new CivilParts(parts);
    }

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
        s_PartsArithmetic.CountDaysBetween(other.DateParts, DateParts);

    [Pure]
    public CivilParts PlusDays(int days) =>
        new(s_PartsArithmetic.AddDays(DateParts, days));

    [Pure]
    public CivilParts NextDay() =>
        new(s_PartsArithmetic.NextDay(DateParts));

    [Pure]
    public CivilParts PreviousDay() =>
        new(s_PartsArithmetic.PreviousDay(DateParts));
}
