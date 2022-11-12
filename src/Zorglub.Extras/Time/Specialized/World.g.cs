﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable IDE0240 // Remove redundant nullable directive ✓
// Public API, fix RS0041 "Public members should not use oblivious types".
#nullable enable
#pragma warning restore IDE0240

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

/// <summary>Represents the World calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class WorldCalendar : SpecialCalendar<WorldDate>
{
    /// <summary>Initializes a new instance of the <see cref="WorldCalendar"/> class.</summary>
    public WorldCalendar() : this(new WorldSchema()) { }

    internal WorldCalendar(WorldSchema schema) : base("World", GetScope(schema))
    {
        OnInitializing(schema);
    }

    private static partial MinMaxYearScope GetScope(WorldSchema schema);

    partial void OnInitializing(WorldSchema schema);

    private protected sealed override WorldDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Provides common adjusters for <see cref="WorldDate"/>.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class WorldAdjuster : SpecialAdjuster<WorldDate>
{
    /// <summary>Initializes a new instance of the <see cref="WorldAdjuster"/> class.</summary>
    public WorldAdjuster() : base(WorldDate.Calendar.Scope) { }

    internal WorldAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected sealed override WorldDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Represents the World date.
/// <para><see cref="WorldDate"/> is an immutable struct.</para></summary>
public readonly partial struct WorldDate :
    IDate<WorldDate, WorldCalendar>,
    IAdjustable<WorldDate>
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly WorldSchema s_Schema = new();
    private static readonly WorldCalendar s_Calendar = new(s_Schema);
    private static readonly MinMaxYearScope s_Scope = s_Calendar.Scope;
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;
    private static readonly WorldAdjuster s_Adjuster = new(s_Scope);
    private static readonly WorldDate s_MinValue = new(s_Domain.Min - s_Epoch);
    private static readonly WorldDate s_MaxValue = new(s_Domain.Max - s_Epoch);

    private readonly int _daysSinceEpoch;

    /// <summary>Initializes a new instance of the <see cref="WorldDate"/> struct to the specified date parts.</summary>
    /// <exception cref="AoorException">The specified components do not form a valid date or
    /// <paramref name="year"/> is outside the range of supported years.</exception>
    public WorldDate(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>Initializes a new instance of the <see cref="WorldDate"/> struct to the specified ordinal date parts.</summary>
    /// <exception cref="AoorException">The specified components do not form a valid ordinal date or
    /// <paramref name="year"/> is outside the range of supported years.</exception>
    public WorldDate(int year, int dayOfYear)
    {
        s_Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>This constructor does NOT validate its parameter.</summary>
    internal WorldDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static WorldDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static WorldDate MaxValue => s_MaxValue;

    /// <summary>Gets the date adjuster.
    /// <para>This static property is thread-safe.</para></summary>
    public static WorldAdjuster Adjuster => s_Adjuster;

    /// <inheritdoc />
    public static WorldCalendar Calendar => s_Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

    /// <inheritdoc />
    public int DaysSinceEpoch => _daysSinceEpoch;

    /// <inheritdoc />
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <inheritdoc />
    public int Century => YearNumbering.GetCentury(Year);

    /// <inheritdoc />
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <inheritdoc />
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <inheritdoc />
    public int Year => s_Schema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = s_Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return d;
        }
    }

    /// <inheritdoc />
    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    /// <inheritdoc />
    public bool IsIntercalary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <summary>Returns a culture-independent string representation of the current instance.</summary>
    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = s_Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct WorldDate // Factories
{
    /// <summary>Creates a new instance of the <see cref="WorldDate"/> struct from the
    /// specified day number.</summary>
    /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
    /// supported values.</exception>
    public static WorldDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new WorldDate(dayNumber - s_Epoch);
    }
}

public partial struct WorldDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct WorldDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public WorldDate Adjust(Func<WorldDate, WorldDate> adjuster)
    {
        Requires.NotNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new WorldDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new WorldDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new WorldDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new WorldDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new WorldDate(dayNumber - s_Epoch);
    }
}

public partial struct WorldDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(WorldDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is WorldDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct WorldDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(WorldDate left, WorldDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static WorldDate Min(WorldDate x, WorldDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static WorldDate Max(WorldDate x, WorldDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(WorldDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is WorldDate date ? CompareTo(date)
        : Throw.NonComparable(typeof(WorldDate), obj);
}

public partial struct WorldDate // Math
{
    /// <summary>Subtracts the two specified dates and returns the number of days between them.</summary>
    public static int operator -(WorldDate left, WorldDate right) => left.CountDaysSince(right);

    /// <summary>Adds a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static WorldDate operator +(WorldDate value, int days) => value.PlusDays(days);

    /// <summary>Subtracts a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static WorldDate operator -(WorldDate value, int days) => value.PlusDays(-days);

    /// <summary>Adds one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the latest supported date.</exception>
    public static WorldDate operator ++(WorldDate value) => value.NextDay();

    /// <summary>Subtracts one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the earliest supported date.</exception>
    public static WorldDate operator --(WorldDate value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(WorldDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public WorldDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public WorldDate NextDay() =>
        this == s_MaxValue ? Throw.DateOverflow<WorldDate>()
        : new WorldDate(_daysSinceEpoch + 1);

    /// <inheritdoc />
    [Pure]
    public WorldDate PreviousDay() =>
        this == s_MinValue ? Throw.DateOverflow<WorldDate>()
        : new WorldDate(_daysSinceEpoch - 1);
}
