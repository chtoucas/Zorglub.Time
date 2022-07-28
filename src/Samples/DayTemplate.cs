// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

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
// See also CivilDay which is an "optimized" version of this type.

/// <summary>
/// Provides a Gregorian date based on the count of consecutive days since the Gregorian epoch.
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial struct DayTemplate :
    IDate<DayTemplate>,
    IYearEndpointsProvider<DayTemplate>,
    IMonthEndpointsProvider<DayTemplate>,
    IMinMaxValue<DayTemplate>
{
    private static readonly CalendricalSchema s_Schema = SchemaActivator.CreateInstance<CivilSchema>();
    private static readonly CalendarScope s_Scope = new StandardScope(s_Schema, DayZero.NewStyle);

    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }
}

public partial struct DayTemplate
{
    private readonly int _daysSinceEpoch;

    public DayTemplate(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    public DayTemplate(DayNumber dayNumber)
    {
        s_Scope.Domain.Validate(dayNumber);

        _daysSinceEpoch = dayNumber - s_Scope.Epoch;
    }

    private DayTemplate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    public static DayTemplate MinValue { get; } = new(s_Scope.Segment.SupportedDays.Min);
    public static DayTemplate MaxValue { get; } = new(s_Scope.Segment.SupportedDays.Max);

    public DayNumber DayNumber => s_Scope.Epoch + _daysSinceEpoch;

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

    public void Deconstruct(out int year, out int month, out int day) =>
        s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);
}

public partial struct DayTemplate // Conversions, adjustments...
{
    #region Factories

    [Pure]
    public static DayTemplate Today() => new(DayNumber.Today());

    #endregion
    #region Conversions

    [Pure]
    static DayTemplate IFixedDay<DayTemplate>.FromDayNumber(DayNumber dayNumber) =>
        new(dayNumber);

    [Pure]
    DayNumber IFixedDay.ToDayNumber() => DayNumber;

    #endregion
    #region Counting

    [Pure]
    public int CountElapsedDaysInYear() =>
        s_Schema.CountDaysInYearBefore(_daysSinceEpoch);

    [Pure]
    public int CountRemainingDaysInYear() =>
        s_Schema.CountDaysInYearAfter(_daysSinceEpoch);

    [Pure]
    public int CountElapsedDaysInMonth() =>
        s_Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    [Pure]
    public int CountRemainingDaysInMonth() =>
        s_Schema.CountDaysInMonthAfter(_daysSinceEpoch);

    #endregion
    #region Year and month boundaries

    [Pure]
    public static DayTemplate GetStartOfYear(DayTemplate day)
    {
        int daysSinceEpoch = s_Schema.GetStartOfYear(day.Year);
        return new DayTemplate(daysSinceEpoch);
    }

    [Pure]
    public static DayTemplate GetEndOfYear(DayTemplate day)
    {
        int daysSinceEpoch = s_Schema.GetEndOfYear(day.Year);
        return new DayTemplate(daysSinceEpoch);
    }

    [Pure]
    public static DayTemplate GetStartOfMonth(DayTemplate day)
    {
        s_Schema.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
        int daysSinceEpoch = s_Schema.GetStartOfMonth(y, m);
        return new DayTemplate(daysSinceEpoch);
    }

    [Pure]
    public static DayTemplate GetEndOfMonth(DayTemplate day)
    {
        s_Schema.GetDateParts(day._daysSinceEpoch, out int y, out int m, out _);
        int daysSinceEpoch = s_Schema.GetEndOfMonth(y, m);
        return new DayTemplate(daysSinceEpoch);
    }

    #endregion
    #region Adjust the day of the week

    [Pure]
    public DayTemplate Previous(DayOfWeek dayOfWeek) => new(DayNumber.Previous(dayOfWeek));

    [Pure]
    public DayTemplate PreviousOrSame(DayOfWeek dayOfWeek) => new(DayNumber.PreviousOrSame(dayOfWeek));

    [Pure]
    public DayTemplate Nearest(DayOfWeek dayOfWeek) => new(DayNumber.Nearest(dayOfWeek));

    [Pure]
    public DayTemplate NextOrSame(DayOfWeek dayOfWeek) => new(DayNumber.NextOrSame(dayOfWeek));

    [Pure]
    public DayTemplate Next(DayOfWeek dayOfWeek) => new(DayNumber.Next(dayOfWeek));

    #endregion
}

public partial struct DayTemplate // IEquatable
{
    public static bool operator ==(DayTemplate left, DayTemplate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;
    public static bool operator !=(DayTemplate left, DayTemplate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    [Pure]
    public bool Equals(DayTemplate other) => _daysSinceEpoch == other._daysSinceEpoch;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DayTemplate date && Equals(date);

    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct DayTemplate // IComparable
{
    public static bool operator <(DayTemplate left, DayTemplate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;
    public static bool operator <=(DayTemplate left, DayTemplate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;
    public static bool operator >(DayTemplate left, DayTemplate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;
    public static bool operator >=(DayTemplate left, DayTemplate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    [Pure]
    public static DayTemplate Min(DayTemplate x, DayTemplate y) => x < y ? x : y;

    [Pure]
    public static DayTemplate Max(DayTemplate x, DayTemplate y) => x > y ? x : y;

    [Pure]
    public int CompareTo(DayTemplate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DayTemplate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(DayTemplate).GetType()}.",
            nameof(obj));
}

public partial struct DayTemplate // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(DayTemplate left, DayTemplate right) => left.CountDaysSince(right);
    public static DayTemplate operator +(DayTemplate value, int days) => value.PlusDays(days);
    public static DayTemplate operator -(DayTemplate value, int days) => value.PlusDays(-days);
    public static DayTemplate operator ++(DayTemplate value) => value.NextDay();
    public static DayTemplate operator --(DayTemplate value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(DayTemplate other) =>
        checked(_daysSinceEpoch - other._daysSinceEpoch);

    [Pure]
    public DayTemplate PlusDays(int days)
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
    public DayTemplate NextDay() =>
        this == MaxValue ? throw new OverflowException() : new DayTemplate(_daysSinceEpoch + 1);

    [Pure]
    public DayTemplate PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new DayTemplate(_daysSinceEpoch - 1);
}
