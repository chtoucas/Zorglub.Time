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

/// <summary>
/// Provides a Gregorian date based on the count of consecutive days since the Gregorian epoch.
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial struct MyDate :
    IDate<MyDate>,
    IMinMaxValue<MyDate>
{
    private static readonly CivilSchema s_Schema = SchemaActivator.CreateInstance<CivilSchema>();
    private static readonly MyCalendar s_Calendar = new(s_Schema);

    private static CalendarScope Scope { get; } = s_Calendar.Scope;

    private static CalendricalSegment Segment => Scope.Segment;
    private static Range<DayNumber> Domain => Scope.Domain;

    private readonly int _daysSinceEpoch;

    public MyDate(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    public MyDate(DayNumber dayNumber)
    {
        Domain.Validate(dayNumber);

        _daysSinceEpoch = dayNumber - Epoch;
    }

    internal MyDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    public static DayNumber Epoch { get; } = Scope.Epoch;
    public static MyDate MinValue { get; } = new(Segment.SupportedDays.Min);
    public static MyDate MaxValue { get; } = new(Segment.SupportedDays.Max);

    public DayNumber DayNumber => Epoch + _daysSinceEpoch;

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

    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }
}

public partial struct MyDate // Conversions, adjustments...
{
    #region Factories

    [Pure]
    public static MyDate Today() => new(DayNumber.Today());

    #endregion
    #region Conversions

    [Pure]
    static MyDate IFixedDay<MyDate>.FromDayNumber(DayNumber dayNumber) =>
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
    #region Adjust the day of the week

    [Pure]
    public MyDate Previous(DayOfWeek dayOfWeek) => new(DayNumber.Previous(dayOfWeek));

    [Pure]
    public MyDate PreviousOrSame(DayOfWeek dayOfWeek) => new(DayNumber.PreviousOrSame(dayOfWeek));

    [Pure]
    public MyDate Nearest(DayOfWeek dayOfWeek) => new(DayNumber.Nearest(dayOfWeek));

    [Pure]
    public MyDate NextOrSame(DayOfWeek dayOfWeek) => new(DayNumber.NextOrSame(dayOfWeek));

    [Pure]
    public MyDate Next(DayOfWeek dayOfWeek) => new(DayNumber.Next(dayOfWeek));

    #endregion
}

public partial struct MyDate // IEquatable
{
    public static bool operator ==(MyDate left, MyDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;
    public static bool operator !=(MyDate left, MyDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    [Pure]
    public bool Equals(MyDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MyDate date && Equals(date);

    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct MyDate // IComparable
{
    public static bool operator <(MyDate left, MyDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;
    public static bool operator <=(MyDate left, MyDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;
    public static bool operator >(MyDate left, MyDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;
    public static bool operator >=(MyDate left, MyDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    [Pure]
    public static MyDate Min(MyDate x, MyDate y) => x < y ? x : y;

    [Pure]
    public static MyDate Max(MyDate x, MyDate y) => x > y ? x : y;

    [Pure]
    public int CompareTo(MyDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

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
    public int CountDaysSince(MyDate other) =>
        checked(_daysSinceEpoch - other._daysSinceEpoch);

    [Pure]
    public MyDate PlusDays(int days)
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
    public MyDate NextDay() =>
        this == MaxValue ? throw new OverflowException() : new MyDate(_daysSinceEpoch + 1);

    [Pure]
    public MyDate PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new MyDate(_daysSinceEpoch - 1);
}
