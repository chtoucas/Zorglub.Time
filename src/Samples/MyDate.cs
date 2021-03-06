// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Verification that one can create a date type without having access to
// the internals of the assembly Zorglub.
// The code is not meant to showcase good coding practices.

/// <summary>
/// Provides a Gregorian date based on <see cref="Yemoda"/>.
/// </summary>
public readonly partial struct MyDate :
    IDate<MyDate>,
    IMinMaxValue<MyDate>
{
    internal static readonly SystemSchema Schema = SchemaActivator.CreateInstance<GregorianSchema>();
    internal static readonly CalendarScope Scope = new StandardScope(Schema, DayZero.NewStyle);

    [Pure]
    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }
}

public partial struct MyDate
{
    private static SystemSegment Segment { get; } = SystemSegment.FromCalendricalSegment(Scope.Segment);
    private static Range<DayNumber> Domain => Scope.Domain;

    private static SystemPartsFactory PartsFactory { get; } = SystemPartsFactory.Create(Segment);
    private static SystemArithmetic Arithmetic { get; } = SystemArithmetic.CreateDefault(Segment);

    private readonly Yemoda _bin;

    public MyDate(int year, int month, int day)
    {
        _bin = PartsFactory.CreateYemoda(year, month, day);
    }

    internal MyDate(Yemoda bin)
    {
        _bin = bin;
    }

    public static DayNumber Epoch { get; } = Scope.Epoch;
    public static MyDate MinValue { get; } = new(Segment.MinMaxDateParts.LowerValue);
    public static MyDate MaxValue { get; } = new(Segment.MinMaxDateParts.UpperValue);

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => _bin.Year;
    public int Month => _bin.Month;

    public int DayOfYear
    {
        get
        {
            var (y, m, d) = _bin;
            return Schema.GetDayOfYear(y, m, d);
        }
    }

    public int Day => _bin.Day;

    public DayOfWeek DayOfWeek => ToDayNumber().DayOfWeek;

    public bool IsIntercalary
    {
        get
        {
            var (y, m, d) = _bin;
            return Schema.IsIntercalaryDay(y, m, d);
        }
    }

    public bool IsSupplementary
    {
        get
        {
            var (y, m, d) = _bin;
            return Schema.IsSupplementaryDay(y, m, d);
        }
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = _bin;
}

public partial struct MyDate // Conversions, adjustments...
{
    #region Factories

    [Pure]
    public static MyDate Today() => FromDayNumber(DayNumber.Today());

    #endregion
    #region Conversions

    [Pure]
    public static MyDate FromDayNumber(DayNumber dayNumber)
    {
        Domain.Validate(dayNumber);
        var ymd = Schema.GetDateParts(dayNumber - Epoch);
        return new MyDate(ymd);
    }

    [Pure]
    public DayNumber ToDayNumber()
    {
        var (y, m, d) = _bin;
        return Epoch + Schema.CountDaysSinceEpoch(y, m, d);
    }

    #endregion
    #region Counting

    [Pure]
    public int CountElapsedDaysInYear()
    {
        var (y, m, d) = _bin;
        return Schema.CountDaysInYearBefore(y, m, d);
    }

    [Pure]
    public int CountRemainingDaysInYear()
    {
        var (y, m, d) = _bin;
        return Schema.CountDaysInYearAfter(y, m, d);
    }

    [Pure]
    public int CountElapsedDaysInMonth() => Day - 1;

    [Pure]
    public int CountRemainingDaysInMonth()
    {
        var (y, m, d) = _bin;
        return Schema.CountDaysInMonthAfter(y, m, d);
    }

    #endregion
    #region Adjust the day of the week

    [Pure]
    public MyDate Previous(DayOfWeek dayOfWeek)
    {
        if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }

        int δ = dayOfWeek - DayOfWeek;
        return PlusDays(δ >= 0 ? δ - 7 : δ);
    }

    [Pure]
    public MyDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : PlusDays(δ > 0 ? δ - 7 : δ);
    }

    [Pure]
    public MyDate Nearest(DayOfWeek dayOfWeek)
    {
        DayNumber nearest = ToDayNumber().Nearest(dayOfWeek);
        return FromDayNumber(nearest);
    }

    [Pure]
    public MyDate NextOrSame(DayOfWeek dayOfWeek)
    {
        if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : PlusDays(δ < 0 ? δ + 7 : δ);
    }

    [Pure]
    public MyDate Next(DayOfWeek dayOfWeek)
    {
        if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }

        int δ = dayOfWeek - DayOfWeek;
        return PlusDays(δ <= 0 ? δ + 7 : δ);
    }

    #endregion
}

public partial struct MyDate // IEquatable
{
    public static bool operator ==(MyDate left, MyDate right) => left._bin == right._bin;
    public static bool operator !=(MyDate left, MyDate right) => left._bin != right._bin;

    [Pure]
    public bool Equals(MyDate other) => _bin == other._bin;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is MyDate date && Equals(date);

    [Pure]
    public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct MyDate // IComparable
{
    public static bool operator <(MyDate left, MyDate right) => left.CompareTo(right) < 0;
    public static bool operator <=(MyDate left, MyDate right) => left.CompareTo(right) <= 0;
    public static bool operator >(MyDate left, MyDate right) => left.CompareTo(right) > 0;
    public static bool operator >=(MyDate left, MyDate right) => left.CompareTo(right) >= 0;

    [Pure]
    public static MyDate Min(MyDate x, MyDate y) => x.CompareTo(y) < 0 ? x : y;

    [Pure]
    public static MyDate Max(MyDate x, MyDate y) => x.CompareTo(y) > 0 ? x : y;

    [Pure]
    public int CompareTo(MyDate other) => _bin.CompareTo(other._bin);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MyDate date ? CompareTo(date)
        : throw new ArgumentException(null, nameof(obj));
}

public partial struct MyDate // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(MyDate left, MyDate right) => left.CountDaysSince(right);
    public static MyDate operator +(MyDate value, int days) => value.PlusDays(days);
    public static MyDate operator -(MyDate value, int days) => value.PlusDays(-days);
    public static MyDate operator ++(MyDate value) => value.NextDay();
    public static MyDate operator --(MyDate value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(MyDate other) =>
        Arithmetic.CountDaysBetween(other._bin, _bin);

    [Pure]
    public MyDate PlusDays(int days) =>
        new(Arithmetic.AddDays(_bin, days));

    [Pure]
    public MyDate NextDay() =>
        new(Arithmetic.NextDay(_bin));

    [Pure]
    public MyDate PreviousDay() =>
        new(Arithmetic.PreviousDay(_bin));
}
