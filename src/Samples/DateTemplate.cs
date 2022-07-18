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
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Exploring the idea of a date type without a companion calendar type.
// Pros:
// - Faster, no Calendar lookup, we are also free to add any local
//   optimisation we like.
// - We can add custom methods only meaningful to a specific date type.
// Cons:
// - Puts more burden on the developer.

/// <summary>
/// Provides a Gregorian date based on <see cref="Yemoda"/>.
/// </summary>
public readonly partial struct DateTemplate :
    IDate<DateTemplate>,
    IYearEndpointsProvider<DateTemplate>,
    IMonthEndpointsProvider<DateTemplate>,
    IMinMaxValue<DateTemplate>
{
    private static readonly CalendarScope<GregorianSchema> __ =
        ScopeActivator.CreateStandard<GregorianSchema>(DayZero.NewStyle);
    private static readonly SystemSchema Schema = __.Schema;
    private static readonly CalendarScope Scope = __.Scope;

    [Pure]
    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }
}

public partial struct DateTemplate
{
    private static SupportedYears SupportedYears { get; } = Scope.SupportedYears;
    private static SystemArithmetic Arithmetic { get; } = SystemArithmetic.CreateDefault(Schema, SupportedYears.Range);
    private static PartsFactory PartsFactory { get; } = PartsFactory.Create(Schema, SupportedYears.Range);

    private static Range<DayNumber> Domain => Scope.Domain;

    private readonly Yemoda _bin;

    public DateTemplate(int year, int month, int day)
    {
        _bin = PartsFactory.CreateYemoda(year, month, day);
    }

    private DateTemplate(Yemoda bin)
    {
        _bin = bin;
    }

    public static DayNumber Epoch { get; } = Scope.Epoch;
    public static DateTemplate MinValue { get; } = new(Schema.GetDatePartsAtStartOfYear(SupportedYears.MinYear));
    public static DateTemplate MaxValue { get; } = new(Schema.GetDatePartsAtEndOfYear(SupportedYears.MaxYear));

    private static int EpochDayOfWeek { get; } = (int)Epoch.DayOfWeek;

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

    public DayOfWeek DayOfWeek
    {
        get
        {
            var (y, m, d) = _bin;
            int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, d);
            return (DayOfWeek)Modulo(
                checked(EpochDayOfWeek + daysSinceEpoch),
                CalendricalConstants.DaysInWeek);
        }
    }

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
        var ymd = Schema.GetDateParts(dayNumber - Epoch);
        return new DateTemplate(ymd);
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
    #region Year and month boundaries

    [Pure]
    public static DateTemplate GetStartOfYear(DateTemplate day) => new(day._bin.StartOfYear);

    [Pure]
    public static DateTemplate GetEndOfYear(DateTemplate day)
    {
        var ymd = Schema.GetDatePartsAtEndOfYear(day.Year);
        return new DateTemplate(ymd);
    }

    [Pure]
    public static DateTemplate GetStartOfMonth(DateTemplate day) => new(day._bin.StartOfMonth);

    [Pure]
    public static DateTemplate GetEndOfMonth(DateTemplate day)
    {
        var (y, m, _) = day._bin;
        var ymd = Schema.GetDatePartsAtEndOfMonth(y, m);
        return new DateTemplate(ymd);
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
    public int CountDaysSince(DateTemplate other) => Arithmetic.CountDaysBetween(other._bin, _bin);

    [Pure]
    public DateTemplate PlusDays(int days) => new(Arithmetic.AddDays(_bin, days));

    [Pure]
    public DateTemplate NextDay() => new(Arithmetic.NextDay(_bin));

    [Pure]
    public DateTemplate PreviousDay() => new(Arithmetic.PreviousDay(_bin));
}
