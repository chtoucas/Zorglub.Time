// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;

// Using a record struct is not a great choice. Main drawbacks:
// - bad default value: 0/0/0, month and day should be > 0 (but of course we
//   could change that)
// - ToString() outputs all props (but of course we could change that)
// - slower and bigger runtime size than the other types

/// <summary>
/// Provides an affine Gregorian date as a record struct (Year, Month, Day).
/// </summary>
public readonly partial record struct GregorianRecord :
    IAffineDate<GregorianRecord>,
    IMinMaxValue<GregorianRecord>
{
    private static readonly CalendricalSchema Schema = SchemaActivator.CreateInstance<GregorianSchema>();
}

public readonly partial record struct GregorianRecord
{
    private static CalendricalSegment Segment { get; } = CalendricalSegment.CreateMaximal(Schema);
    private static SupportedDays SupportedDays { get; } = Segment.SupportedDays;
    private static SupportedYears SupportedYears { get; } = Segment.SupportedYears;
    private static ICalendricalArithmetic Arithmetic { get; } =
        ICalendricalArithmetic.CreateDefault(Schema, SupportedYears.Range);
    private static PartsAdapter PartsAdapter { get; } = new(Schema);
    private static ICalendricalPreValidator PreValidator { get; } = Schema.PreValidator;

    public GregorianRecord(int year, int month, int day)
    {
        SupportedYears.Validate(year);
        PreValidator.ValidateMonthDay(year, month, day);

        Year = year;
        Month = month;
        Day = day;
    }

    private GregorianRecord(DateParts parts)
    {
        (Year, Month, Day) = parts;
    }

    public static GregorianRecord MinValue { get; } = new(DateParts.AtStartOfYear(SupportedYears.MinYear));
    public static GregorianRecord MaxValue { get; } = new(PartsAdapter.GetDatePartsAtEndOfYear(SupportedYears.MaxYear));

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
        SupportedDays.Validate(daysSinceEpoch);
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
