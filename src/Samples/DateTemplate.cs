// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#define USE_ADJUSTER

namespace Samples;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;

using static Zorglub.Time.Extensions.Unboxing;

// Exploring the idea of a date type without a companion calendar type.
// Pros:
// - Faster, no Calendar lookup, we are also free to add any local
//   optimisation we like.
// - We can add custom methods only meaningful to a specific date type.
// Cons:
// - Puts more burden on the developer.

// Using the template in the Gregorian case.
public partial struct DateTemplate
{
    [Pure]
    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }

    private static partial SystemSchema InitSchema() => GregorianSchema.GetInstance().Unbox();

    private static partial DayNumber InitEpoch() => DayZero.NewStyle;
}

/// <summary>
/// Provides a Gregorian date.
/// </summary>
public readonly partial struct DateTemplate :
    IDate<DateTemplate>,
    IYearEndpointsProvider<DateTemplate>,
    IMonthEndpointsProvider<DateTemplate>,
    IMinMaxValue<DateTemplate>,
    ISubtractionOperators<DateTemplate, int, DateTemplate>
{ }

public partial struct DateTemplate // Type init, partial methods
{
    // WARNING: proper initialization of the static fields depends on the
    // order in which they are written.

    private static readonly SystemSchema s_Schema = InitSchema();

    private static readonly DayNumber s_Epoch = InitEpoch();
    private static readonly int s_EpochDayOfWeek = (int)s_Epoch.DayOfWeek;

    private static readonly ICalendarScope s_Scope = MinMaxYearScope.WithMinYear(s_Schema, s_Epoch, 1);
    private static readonly PartsFactory s_PartsFactory = new(s_Scope);
    private static readonly ICalendricalArithmetic s_Arithmetic = ICalendricalArithmeticPlus.CreateDefault(s_Schema, s_Scope.SupportedYears);

    [Pure] private static partial SystemSchema InitSchema();
    [Pure] private static partial DayNumber InitEpoch();
}

public partial struct DateTemplate
{
    private readonly Yemoda _bin;

    public DateTemplate(int year, int month, int day)
    {
        _bin = s_PartsFactory.CreateYemoda(year, month, day);
    }

    private DateTemplate(Yemoda bin)
    {
        _bin = bin;
    }

    private static Range<DayNumber> Domain { get; } = s_Scope.Domain;

    public static DayNumber Epoch => s_Epoch;
    public static Range<int> SupportedYears => s_Scope.SupportedYears;
    public static DateTemplate MinValue { get; } = new(s_Schema.GetStartOfYearParts(s_Scope.SupportedYears.Min));
    public static DateTemplate MaxValue { get; } = new(s_Schema.GetEndOfYearParts(s_Scope.SupportedYears.Max));

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

    public DayOfWeek DayOfWeek
    {
        get
        {
            var (y, m, d) = _bin;
            int daysSinceEpoch = s_Schema.CountDaysSinceEpoch(y, m, d);
            return (DayOfWeek)Modulo(
                checked(s_EpochDayOfWeek + daysSinceEpoch),
                CalendricalConstants.DaysInWeek);
        }
    }

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

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = _bin;

    //
    // Helpers
    //

    private static int Modulo(int m, int n)
    {
        Debug.Assert(n > 0);

        int r = m % n;
        return r >= 0 ? r : (r + n);
    }

#if !USE_ADJUSTER
    private static bool IsInvalid(DayOfWeek @this) =>
        @this < DayOfWeek.Sunday || @this > DayOfWeek.Saturday;
#endif
}

public partial struct DateTemplate // Conversions, adjustments...
{
    #region Factories

    [Pure]
    public static DateTemplate Today() => FromDayNumber(DayNumber.Today());

    #endregion
    #region Conversions

    [Pure]
    public static DateTemplate FromDayNumber(DayNumber dayNumber)
    {
        Domain.Validate(dayNumber);
        var ymd = s_Schema.GetDateParts(dayNumber - Epoch);
        return new DateTemplate(ymd);
    }

    [Pure]
    public DayNumber ToDayNumber()
    {
        var (y, m, d) = _bin;
        return Epoch + s_Schema.CountDaysSinceEpoch(y, m, d);
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
    #region Year and month boundaries

    [Pure]
    public static DateTemplate GetStartOfYear(DateTemplate day) => new(day._bin.StartOfYear);

    [Pure]
    public static DateTemplate GetEndOfYear(DateTemplate day)
    {
        var ymd = s_Schema.GetEndOfYearParts(day.Year);
        return new DateTemplate(ymd);
    }

    [Pure]
    public static DateTemplate GetStartOfMonth(DateTemplate day) => new(day._bin.StartOfMonth);

    [Pure]
    public static DateTemplate GetEndOfMonth(DateTemplate day)
    {
        var (y, m, _) = day._bin;
        var ymd = s_Schema.GetEndOfMonthParts(y, m);
        return new DateTemplate(ymd);
    }

    #endregion
    #region Adjust the day of the week

    [Pure]
    public DateTemplate Previous(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTER
        return DayOfWeekAdjusters.Previous(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) Throw.ArgumentOutOfRange(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return PlusDays(δ >= 0 ? δ - 7 : δ);
#endif
    }

    [Pure]
    public DateTemplate PreviousOrSame(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTER
        return DayOfWeekAdjusters.PreviousOrSame(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) Throw.ArgumentOutOfRange(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : PlusDays(δ > 0 ? δ - 7 : δ);
#endif
    }

    [Pure]
    public DateTemplate Nearest(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTER
        return DayOfWeekAdjusters.Nearest(this, dayOfWeek);
#else
        DayNumber nearest = ToDayNumber().Nearest(dayOfWeek);
        return FromDayNumber(nearest);
#endif
    }

    [Pure]
    public DateTemplate NextOrSame(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTER
        return DayOfWeekAdjusters.NextOrSame(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) Throw.ArgumentOutOfRange(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return δ == 0 ? this : PlusDays(δ < 0 ? δ + 7 : δ);
#endif
    }

    [Pure]
    public DateTemplate Next(DayOfWeek dayOfWeek)
    {
#if USE_ADJUSTER
        return DayOfWeekAdjusters.Next(this, dayOfWeek);
#else
        if (IsInvalid(dayOfWeek)) Throw.ArgumentOutOfRange(nameof(dayOfWeek));

        int δ = dayOfWeek - DayOfWeek;
        return PlusDays(δ <= 0 ? δ + 7 : δ);
#endif
    }

    #endregion
}

public partial struct DateTemplate // IEquatable
{
    public static bool operator ==(DateTemplate left, DateTemplate right) => left._bin == right._bin;
    public static bool operator !=(DateTemplate left, DateTemplate right) => left._bin != right._bin;

    [Pure] public bool Equals(DateTemplate other) => _bin == other._bin;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DateTemplate date && Equals(date);

    [Pure] public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct DateTemplate // IComparable
{
    public static bool operator <(DateTemplate left, DateTemplate right) => left._bin < right._bin;
    public static bool operator <=(DateTemplate left, DateTemplate right) => left._bin <= right._bin;
    public static bool operator >(DateTemplate left, DateTemplate right) => left._bin > right._bin;
    public static bool operator >=(DateTemplate left, DateTemplate right) => left._bin >= right._bin;

    [Pure]
    public static DateTemplate Min(DateTemplate x, DateTemplate y) => x < y ? x : y;

    [Pure]
    public static DateTemplate Max(DateTemplate x, DateTemplate y) => x > y ? x : y;

    [Pure]
    public int CompareTo(DateTemplate other) => _bin.CompareTo(other._bin);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DateTemplate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(DateTemplate).GetType()}.",
            nameof(obj));
}

public partial struct DateTemplate // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(DateTemplate left, DateTemplate right) => left.CountDaysSince(right);
    public static DateTemplate operator +(DateTemplate value, int days) => value.PlusDays(days);
    public static DateTemplate operator -(DateTemplate value, int days) => value.PlusDays(-days);
    public static DateTemplate operator ++(DateTemplate value) => value.NextDay();
    public static DateTemplate operator --(DateTemplate value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(DateTemplate other) => s_Arithmetic.CountDaysBetween(other._bin, _bin);

    [Pure]
    public DateTemplate PlusDays(int days) => new(s_Arithmetic.AddDays(_bin, days));

    [Pure]
    public DateTemplate NextDay() => new(s_Arithmetic.NextDay(_bin));

    [Pure]
    public DateTemplate PreviousDay() => new(s_Arithmetic.PreviousDay(_bin));
}
