// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

// Exploring the idea of a date type without a companion calendar type
// and built upon DayNumber instead of Yemoda.
// Pros:
// - Some operations are simpler and even sometimes faster: everything
//   related to the day of the week, interconversion. In theory, the adding
//   days is also faster, but it is (more on less) only the case when days
//   is greater than MinDaysInYear.
// - Maybe closer to what we would do if we created a time object.
//   By the way, this is the how DateTime works internally.
// Cons:
// - Slower for methods linked to the y/m/d concept, the most obvious one
//   being the construction of a new date object.
//
// See also CivilDate which is an "optimized" version of this type.

/// <summary>
/// Provides a Gregorian date based on the count of consecutive days since the Gregorian epoch.
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial struct DateTemplate :
    IDate<DateTemplate>,
    IYearEndpointsProvider<DateTemplate>,
    IMonthEndpointsProvider<DateTemplate>,
    IMinMaxValue<DateTemplate>
{
    private static readonly CivilSchema s_Schema = CivilSchema.GetInstance().Unbox();
    private static readonly CalendarTemplate s_Calendar = new(s_Schema);

    private static readonly CalendarScope s_Scope = s_Calendar.Scope;
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;

    private readonly int _daysSinceEpoch;

    public DateTemplate(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    public DateTemplate(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        _daysSinceEpoch = dayNumber - s_Epoch;
    }

    internal DateTemplate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    public static DateTemplate MinValue { get; } = new(s_Domain.Min - s_Epoch);
    public static DateTemplate MaxValue { get; } = new(s_Domain.Max - s_Epoch);

    public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => s_Schema.GetYear(_daysSinceEpoch);

    public int Month
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    public int DayOfYear
    {
        get
        {
            _ = s_Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    public int Day
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return d;
        }
    }

    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    public bool IsIntercalary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    public bool IsSupplementary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);
}

public partial struct DateTemplate // Conversions, adjustments...
{
    #region Factories

    [Pure]
    public static DateTemplate Today() => new(DayNumber.Today() - s_Epoch);

    #endregion
    #region Conversions

    [Pure]
    static DateTemplate IFixedDay<DateTemplate>.FromDayNumber(DayNumber dayNumber) =>
        new(dayNumber);

    [Pure]
    DayNumber IFixedDay.ToDayNumber() => DayNumber;

    #endregion
    #region Counting

    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceEpoch);

    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceEpoch);

    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceEpoch);

    #endregion
    #region Year and month boundaries

    [Pure]
    public static DateTemplate GetStartOfYear(DateTemplate day)
    {
        int daysSinceEpoch = s_Schema.GetStartOfYear(day.Year);
        return new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public static DateTemplate GetEndOfYear(DateTemplate day)
    {
        int daysSinceEpoch = s_Schema.GetEndOfYear(day.Year);
        return new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public static DateTemplate GetStartOfMonth(DateTemplate day)
    {
        s_Schema.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
        int daysSinceEpoch = s_Schema.GetStartOfMonth(y, m);
        return new DateTemplate(daysSinceEpoch);
    }

    [Pure]
    public static DateTemplate GetEndOfMonth(DateTemplate day)
    {
        s_Schema.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
        int daysSinceEpoch = s_Schema.GetEndOfMonth(y, m);
        return new DateTemplate(daysSinceEpoch);
    }

    #endregion
    #region Adjust the day of the week

    [Pure]
    public DateTemplate Previous(DayOfWeek dayOfWeek) => new(DayNumber.Previous(dayOfWeek));

    [Pure]
    public DateTemplate PreviousOrSame(DayOfWeek dayOfWeek) => new(DayNumber.PreviousOrSame(dayOfWeek));

    [Pure]
    public DateTemplate Nearest(DayOfWeek dayOfWeek) => new(DayNumber.Nearest(dayOfWeek));

    [Pure]
    public DateTemplate NextOrSame(DayOfWeek dayOfWeek) => new(DayNumber.NextOrSame(dayOfWeek));

    [Pure]
    public DateTemplate Next(DayOfWeek dayOfWeek) => new(DayNumber.Next(dayOfWeek));

    #endregion
}

public partial struct DateTemplate // IEquatable
{
    public static bool operator ==(DateTemplate left, DateTemplate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;
    public static bool operator !=(DateTemplate left, DateTemplate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    [Pure]
    public bool Equals(DateTemplate other) => _daysSinceEpoch == other._daysSinceEpoch;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DateTemplate date && Equals(date);

    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct DateTemplate // IComparable
{
    public static bool operator <(DateTemplate left, DateTemplate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;
    public static bool operator <=(DateTemplate left, DateTemplate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;
    public static bool operator >(DateTemplate left, DateTemplate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;
    public static bool operator >=(DateTemplate left, DateTemplate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    [Pure]
    public static DateTemplate Min(DateTemplate x, DateTemplate y) => x < y ? x : y;

    [Pure]
    public static DateTemplate Max(DateTemplate x, DateTemplate y) => x > y ? x : y;

    [Pure]
    public int CompareTo(DateTemplate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

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
    public int CountDaysSince(DateTemplate other) =>
        checked(_daysSinceEpoch - other._daysSinceEpoch);

    [Pure]
    public DateTemplate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // We don't write:
        // > Domain.CheckOverflow(Epoch + daysSinceEpoch);
        // The addition may also overflow...
        if (daysSinceEpoch < MinValue._daysSinceEpoch || daysSinceEpoch > MaxValue._daysSinceEpoch)
        {
            throw new OverflowException(nameof(days));
        }
        return new(daysSinceEpoch);
    }

    [Pure]
    public DateTemplate NextDay() =>
        this == MaxValue ? throw new OverflowException() : new DateTemplate(_daysSinceEpoch + 1);

    [Pure]
    public DateTemplate PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new DateTemplate(_daysSinceEpoch - 1);
}
