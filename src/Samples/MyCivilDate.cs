// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

// MyCivilDate is like DayTemplate but with a companion calendar.

/// <summary>
/// Provides a Gregorian date based on the count of consecutive days since the Gregorian epoch.
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial struct MyCivilDate : IDate<MyCivilDate>, IMinMaxValue<MyCivilDate>
{
    private static readonly CivilSchema s_Schema = CivilSchema.GetInstance().Unbox();
    private static readonly MyCivilCalendar s_Calendar = new(s_Schema);

    private static readonly CalendarScope s_Scope = s_Calendar.Scope;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;

    private readonly int _daysSinceZero;

    public MyCivilDate(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceZero = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    public MyCivilDate(DayNumber dayNumber)
    {
        Debug.Assert(s_Calendar.Epoch == DayZero.NewStyle);

        s_Domain.Validate(dayNumber);

        _daysSinceZero = dayNumber.DaysSinceZero;
    }

    internal MyCivilDate(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    public static MyCivilDate MinValue { get; } = new(s_Domain.Min.DaysSinceZero);
    public static MyCivilDate MaxValue { get; } = new(s_Domain.Max.DaysSinceZero);

    public DayNumber DayNumber => DayZero.NewStyle + _daysSinceZero;

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => s_Schema.GetYear(_daysSinceZero);

    public int Month
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceZero, out _, out int m, out _);
            return m;
        }
    }

    public int DayOfYear
    {
        get
        {
            _ = s_Schema.GetYear(_daysSinceZero, out int doy);
            return doy;
        }
    }

    public int Day
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceZero, out _, out _, out int d);
            return d;
        }
    }

    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    public bool IsIntercalary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    public bool IsSupplementary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        s_Schema.GetDateParts(_daysSinceZero, out year, out month, out day);
}

public partial struct MyCivilDate // Conversions, adjustments...
{
    #region Factories

    [Pure]
    public static MyCivilDate Today() => new(DayNumber.Today().DaysSinceZero);

    #endregion
    #region Conversions

    [Pure]
    static MyCivilDate IFixedDay<MyCivilDate>.FromDayNumber(DayNumber dayNumber) =>
        new(dayNumber);

    [Pure]
    DayNumber IFixedDay.ToDayNumber() => DayNumber;

    #endregion
    #region Counting

    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceZero);

    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceZero);

    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceZero);

    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceZero);

    #endregion
    #region Adjust the day of the week

    [Pure]
    public MyCivilDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDate(dayNumber.DaysSinceZero);
    }

    [Pure]
    public MyCivilDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDate(dayNumber.DaysSinceZero);
    }

    [Pure]
    public MyCivilDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDate(dayNumber.DaysSinceZero);
    }

    [Pure]
    public MyCivilDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDate(dayNumber.DaysSinceZero);
    }

    [Pure]
    public MyCivilDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) { throw new OverflowException(); }
        return new MyCivilDate(dayNumber.DaysSinceZero);
    }

    #endregion
}

public partial struct MyCivilDate // IEquatable
{
    public static bool operator ==(MyCivilDate left, MyCivilDate right) =>
        left._daysSinceZero == right._daysSinceZero;
    public static bool operator !=(MyCivilDate left, MyCivilDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    [Pure]
    public bool Equals(MyCivilDate other) => _daysSinceZero == other._daysSinceZero;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MyCivilDate date && Equals(date);

    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct MyCivilDate // IComparable
{
    public static bool operator <(MyCivilDate left, MyCivilDate right) =>
        left._daysSinceZero < right._daysSinceZero;
    public static bool operator <=(MyCivilDate left, MyCivilDate right) =>
        left._daysSinceZero <= right._daysSinceZero;
    public static bool operator >(MyCivilDate left, MyCivilDate right) =>
        left._daysSinceZero > right._daysSinceZero;
    public static bool operator >=(MyCivilDate left, MyCivilDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    [Pure]
    public static MyCivilDate Min(MyCivilDate x, MyCivilDate y) => x < y ? x : y;

    [Pure]
    public static MyCivilDate Max(MyCivilDate x, MyCivilDate y) => x > y ? x : y;

    [Pure]
    public int CompareTo(MyCivilDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MyCivilDate date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(MyCivilDate).GetType()}.",
            nameof(obj));
}

public partial struct MyCivilDate // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(MyCivilDate left, MyCivilDate right) => left.CountDaysSince(right);
    public static MyCivilDate operator +(MyCivilDate value, int days) => value.PlusDays(days);
    public static MyCivilDate operator -(MyCivilDate value, int days) => value.PlusDays(-days);
    public static MyCivilDate operator ++(MyCivilDate value) => value.NextDay();
    public static MyCivilDate operator --(MyCivilDate value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(MyCivilDate other) =>
        // No need to use a checked context here.
        _daysSinceZero - other._daysSinceZero;

    [Pure]
    public MyCivilDate PlusDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);
        // We don't write:
        // > Domain.CheckOverflow(Epoch + daysSinceZero);
        // The addition may also overflow...
        s_Scope.DaysValidator.CheckOverflow(daysSinceZero);
        //if (daysSinceZero < MinValue._daysSinceZero || daysSinceZero > MaxValue._daysSinceZero)
        //{
        //    throw new OverflowException(nameof(days));
        //}
        return new(daysSinceZero);
    }

    [Pure]
    public MyCivilDate NextDay() =>
        this == MaxValue ? throw new OverflowException() : new MyCivilDate(_daysSinceZero + 1);

    [Pure]
    public MyCivilDate PreviousDay() =>
        this == MinValue ? throw new OverflowException() : new MyCivilDate(_daysSinceZero - 1);
}
