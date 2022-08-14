// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#define USE_ADJUSTERS

namespace Samples;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

// Exploring the idea of a date type without a companion calendar type.
// Pros:
// - Faster, no Calendar lookup, we are also free to add any local
//   optimisation we like.
// - We can add custom methods only meaningful to a specific date type.
// Cons:
// - Puts more burden on the developer.

/// <summary>
/// Provides a Gregorian date based on <see cref="Yemoda"/>.
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial struct MyDate :
    IDate<MyDate>,
    IMinMaxValue<MyDate>
{
    // NB: the order in which the static fields are written is important.
    // Being based on Yemoda, the schema should derived from SystemSchema.

    private static readonly SystemSchema s_Schema = CivilSchema.GetInstance().Unbox();
    private static readonly CalendarScope s_Scope = new StandardScope(s_Schema, DayZero.NewStyle);

    private static readonly DayNumber s_Epoch = s_Scope.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Scope.Domain;
    private static readonly int EpochDayOfWeek = (int)s_Epoch.DayOfWeek;

    private static readonly SystemSegment s_Segment = SystemSegment.FromCalendricalSegment(s_Scope.Segment);

    private static readonly SystemPartsFactory s_PartsFactory = SystemPartsFactory.Create(s_Segment);
    private static readonly SystemArithmetic s_Arithmetic = SystemArithmetic.CreateDefault(s_Segment);

    private static readonly MyDate s_MinValue = new(s_Segment.MinMaxDateParts.LowerValue);
    private static readonly MyDate s_MaxValue = new(s_Segment.MinMaxDateParts.UpperValue);

    private readonly Yemoda _bin;

    public MyDate(int year, int month, int day)
    {
        _bin = s_PartsFactory.CreateYemoda(year, month, day);
    }

    private MyDate(Yemoda bin)
    {
        _bin = bin;
    }

    public static MyDate MinValue => s_MinValue;
    public static MyDate MaxValue => s_MaxValue;

    public DayNumber DayNumber => s_Epoch + DaysSinceEpoch;

    public int DaysSinceEpoch
    {
        get
        {
            var (y, m, d) = _bin;
            return s_Schema.CountDaysSinceEpoch(y, m, d);
        }
    }

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
            return s_Schema.GetDayOfYear(y, m, d);
        }
    }

    public int Day => _bin.Day;

    public DayOfWeek DayOfWeek =>
        (DayOfWeek)Modulo(
            checked(EpochDayOfWeek + DaysSinceEpoch),
            CalendricalConstants.DaysInWeek);

    public bool IsIntercalary
    {
        get
        {
            var (y, m, d) = _bin;
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    public bool IsSupplementary
    {
        get
        {
            var (y, m, d) = _bin;
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    [Pure]
    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = _bin;

    public void Deconstruct(out int year, out int dayOfYear)
    {
        (year, var m, var d) = _bin;
        dayOfYear = s_Schema.GetDayOfYear(year, m, d);
    }

    //
    // Helpers
    //

    private static int Modulo(int m, int n)
    {
        Debug.Assert(n > 0);

        int r = m % n;
        return r >= 0 ? r : (r + n);
    }

#if !USE_ADJUSTERS
    private static bool IsInvalid(DayOfWeek dayOfWeek) =>
        dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday;
#endif
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
        s_Domain.Validate(dayNumber);
        var ymd = s_Schema.GetDateParts(dayNumber - s_Epoch);
        return new MyDate(ymd);
    }

    #endregion
    #region Counting

    [Pure]
    public int CountElapsedDaysInYear()
    {
        var (y, m, d) = _bin;
        return s_Schema.CountDaysInYearBefore(y, m, d);
    }

    [Pure]
    public int CountRemainingDaysInYear()
    {
        var (y, m, d) = _bin;
        return s_Schema.CountDaysInYearAfter(y, m, d);
    }

    [Pure]
    public int CountElapsedDaysInMonth() => Day - 1;

    [Pure]
    public int CountRemainingDaysInMonth()
    {
        var (y, m, d) = _bin;
        return s_Schema.CountDaysInMonthAfter(y, m, d);
    }

    #endregion
    #region Adjusters

    [Pure]
    public static MyDate GetStartOfYear(MyDate date) => new(date._bin.StartOfYear);

    [Pure]
    public static MyDate GetEndOfYear(MyDate date)
    {
        var ymd = s_Schema.GetDatePartsAtEndOfYear(date.Year);
        return new MyDate(ymd);
    }

    [Pure]
    public static MyDate GetStartOfMonth(MyDate date) => new(date._bin.StartOfMonth);

    [Pure]
    public static MyDate GetEndOfMonth(MyDate date)
    {
        var (y, m, _) = date._bin;
        var ymd = s_Schema.GetDatePartsAtEndOfMonth(y, m);
        return new MyDate(ymd);
    }

    #endregion
    #region Adjust the day of the week

    [Pure]
    public MyDate Previous(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTERS
        return DayOfWeekAdjusters.Previous(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) throw new ArgumentOutOfRangeException(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return PlusDays(δ >= 0 ? δ - 7 : δ);
#endif
    }

    [Pure]
    public MyDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTERS
        return DayOfWeekAdjusters.PreviousOrSame(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) throw new ArgumentOutOfRangeException(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : PlusDays(δ > 0 ? δ - 7 : δ);
#endif
    }

    [Pure]
    public MyDate Nearest(DayOfWeek dayOfWeek)
    {
        DayNumber nearest = DayNumber.Nearest(dayOfWeek);
        return FromDayNumber(nearest);
    }

    [Pure]
    public MyDate NextOrSame(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTERS
        return DayOfWeekAdjusters.NextOrSame(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) throw new ArgumentOutOfRangeException(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : PlusDays(δ < 0 ? δ + 7 : δ);
#endif
    }

    [Pure]
    public MyDate Next(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTERS
        return DayOfWeekAdjusters.Next(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) throw new ArgumentOutOfRangeException(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return PlusDays(δ <= 0 ? δ + 7 : δ);
#endif
    }

    #endregion
}

public partial struct MyDate // IEquatable
{
    public static bool operator ==(MyDate left, MyDate right) => left._bin == right._bin;
    public static bool operator !=(MyDate left, MyDate right) => left._bin != right._bin;

    [Pure] public bool Equals(MyDate other) => _bin == other._bin;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MyDate date && Equals(date);

    [Pure] public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct MyDate // IComparable
{
    public static bool operator <(MyDate left, MyDate right) => left._bin < right._bin;
    public static bool operator <=(MyDate left, MyDate right) => left._bin <= right._bin;
    public static bool operator >(MyDate left, MyDate right) => left._bin > right._bin;
    public static bool operator >=(MyDate left, MyDate right) => left._bin >= right._bin;

    [Pure]
    public static MyDate Min(MyDate x, MyDate y) => x < y ? x : y;

    [Pure]
    public static MyDate Max(MyDate x, MyDate y) => x > y ? x : y;

    [Pure]
    public int CompareTo(MyDate other) => _bin.CompareTo(other._bin);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MyDate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(MyDate).GetType()}.",
            nameof(obj));
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
    public int CountDaysSince(MyDate other) => s_Arithmetic.CountDaysBetween(other._bin, _bin);

    [Pure]
    public MyDate PlusDays(int days) => new(s_Arithmetic.AddDays(_bin, days));

    [Pure]
    public MyDate NextDay() => new(s_Arithmetic.NextDay(_bin));

    [Pure]
    public MyDate PreviousDay() => new(s_Arithmetic.PreviousDay(_bin));
}
