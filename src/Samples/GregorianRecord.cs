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
    private static readonly SchemaContext __ = SchemaContext.Create<GregorianSchema>();
    private static readonly CalendricalSchema Schema = __.Schema;
    private static readonly CalendricalSegment Segment = __.Segment;
}

public readonly partial record struct GregorianRecord
{
    private static PartsAdapter PartsAdapter { get; } = new(Schema);
    //private static ICalendricalArithmetic Arithmetic { get; } = new BasicArithmetic(Segment);
    private static ICalendricalArithmetic Arithmetic { get; } = new RegularArithmetic(Schema, Segment.SupportedYears);
    private static ICalendricalPreValidator PreValidator => Schema.PreValidator;
    private static Range<int> Domain => Segment.Domain;

    public GregorianRecord(int year, int month, int day)
    {
        if (SupportedYears.Contains(year) == false) throw new ArgumentOutOfRangeException(nameof(year));
        PreValidator.ValidateMonthDay(year, month, day);

        Year = year;
        Month = month;
        Day = day;
    }

    private GregorianRecord(DateParts parts)
    {
        (Year, Month, Day) = parts;
    }

    public static Range<int> SupportedYears => Segment.SupportedYears;
    public static GregorianRecord MinValue { get; } = new(DateParts.AtStartOfYear(SupportedYears.Min));
    public static GregorianRecord MaxValue { get; } = new(PartsAdapter.GetDatePartsAtEndOfYear(SupportedYears.Max));

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

public partial record struct GregorianRecord // Conversions, adjustments...
{
    [Pure]
    public static GregorianRecord FromDaysSinceEpoch(int daysSinceEpoch)
    {
        if (Domain.Contains(daysSinceEpoch) == false) throw new ArgumentOutOfRangeException(nameof(daysSinceEpoch));

        var parts = PartsAdapter.GetDateParts(daysSinceEpoch);
        return new GregorianRecord(parts);
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
    #region Year and month boundaries

    [Pure]
    public static GregorianRecord GetStartOfYear(GregorianRecord day)
    {
        var parts = PartsAdapter.GetDatePartsAtEndOfYear(day.Year);
        return new GregorianRecord(parts);
    }

    [Pure]
    public static GregorianRecord GetEndOfYear(GregorianRecord day)
    {
        var parts = PartsAdapter.GetDatePartsAtEndOfYear(day.Year);
        return new GregorianRecord(parts);
    }

    [Pure]
    public static GregorianRecord GetStartOfMonth(GregorianRecord day)
    {
        var (y, m, _) = day;
        var parts = DateParts.AtStartOfMonth(y, m);
        return new GregorianRecord(parts);
    }

    [Pure]
    public static GregorianRecord GetEndOfMonth(GregorianRecord day)
    {
        var (y, m, _) = day;
        var parts = PartsAdapter.GetDatePartsAtEndOfMonth(y, m);
        return new GregorianRecord(parts);
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
    public int CompareTo(GregorianRecord other) => DateParts.CompareTo(other.DateParts);

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
        Arithmetic.CountDaysBetween(other.DateParts, DateParts);

    [Pure]
    public GregorianRecord PlusDays(int days) =>
        new(Arithmetic.AddDays(DateParts, days));

    [Pure]
    public GregorianRecord NextDay() =>
        new(Arithmetic.NextDay(DateParts));

    [Pure]
    public GregorianRecord PreviousDay() =>
        new(Arithmetic.PreviousDay(DateParts));
}
