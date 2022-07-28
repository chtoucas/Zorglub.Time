﻿// SPDX-License-Identifier: BSD-3-Clause
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

// MyCivilDay is like DayTemplate but with a companion calendar.

/// <summary>
/// Provides a Gregorian date based on the count of consecutive days since the Gregorian epoch.
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial struct MyCivilDay : IDate<MyCivilDay>, IMinMaxValue<MyCivilDay>
{
    private static readonly CivilSchema s_Schema = CivilSchema.GetInstance().Unbox();
    private static readonly MyCivilCalendar s_Calendar = new(s_Schema);

    private static readonly CalendarScope s_Scope = s_Calendar.Scope;
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;

    private readonly int _daysSinceEpoch;

    public MyCivilDay(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    public MyCivilDay(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        _daysSinceEpoch = dayNumber - s_Epoch;
    }

    internal MyCivilDay(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    public static MyCivilDay MinValue { get; } = new(s_Domain.Min - s_Epoch);
    public static MyCivilDay MaxValue { get; } = new(s_Domain.Max - s_Epoch);

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

public partial struct MyCivilDay // Conversions, adjustments...
{
    #region Factories

    [Pure]
    public static MyCivilDay Today() => new(DayNumber.Today() - s_Epoch);

    #endregion
    #region Conversions

    [Pure]
    static MyCivilDay IFixedDay<MyCivilDay>.FromDayNumber(DayNumber dayNumber) =>
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
    #region Adjust the day of the week

    [Pure]
    public MyCivilDay Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDay(dayNumber - s_Epoch);
    }

    [Pure]
    public MyCivilDay PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDay(dayNumber - s_Epoch);
    }

    [Pure]
    public MyCivilDay Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDay(dayNumber - s_Epoch);
    }

    [Pure]
    public MyCivilDay NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDay(dayNumber - s_Epoch);
    }

    [Pure]
    public MyCivilDay Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDay(dayNumber - s_Epoch);
    }

    #endregion
}

public partial struct MyCivilDay // IEquatable
{
    public static bool operator ==(MyCivilDay left, MyCivilDay right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;
    public static bool operator !=(MyCivilDay left, MyCivilDay right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    [Pure]
    public bool Equals(MyCivilDay other) => _daysSinceEpoch == other._daysSinceEpoch;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MyCivilDay date && Equals(date);

    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct MyCivilDay // IComparable
{
    public static bool operator <(MyCivilDay left, MyCivilDay right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;
    public static bool operator <=(MyCivilDay left, MyCivilDay right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;
    public static bool operator >(MyCivilDay left, MyCivilDay right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;
    public static bool operator >=(MyCivilDay left, MyCivilDay right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    [Pure]
    public static MyCivilDay Min(MyCivilDay x, MyCivilDay y) => x < y ? x : y;

    [Pure]
    public static MyCivilDay Max(MyCivilDay x, MyCivilDay y) => x > y ? x : y;

    [Pure]
    public int CompareTo(MyCivilDay other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MyCivilDay date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(MyCivilDay).GetType()}.",
            nameof(obj));
}

public partial struct MyCivilDay // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(MyCivilDay left, MyCivilDay right) => left.CountDaysSince(right);
    public static MyCivilDay operator +(MyCivilDay value, int days) => value.PlusDays(days);
    public static MyCivilDay operator -(MyCivilDay value, int days) => value.PlusDays(-days);
    public static MyCivilDay operator ++(MyCivilDay value) => value.NextDay();
    public static MyCivilDay operator --(MyCivilDay value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(MyCivilDay other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    [Pure]
    public MyCivilDay PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // We don't write:
        // > Domain.CheckOverflow(Epoch + daysSinceEpoch);
        // The addition may also overflow...
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        //if (daysSinceEpoch < MinValue._daysSinceEpoch || daysSinceEpoch > MaxValue._daysSinceEpoch)
        //{
        //    throw new OverflowException(nameof(days));
        //}
        return new(daysSinceEpoch);
    }

    [Pure]
    public MyCivilDay NextDay() =>
        this == MaxValue ? throw new OverflowException() : new MyCivilDay(_daysSinceEpoch + 1);

    [Pure]
    public MyCivilDay PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new MyCivilDay(_daysSinceEpoch - 1);
}
