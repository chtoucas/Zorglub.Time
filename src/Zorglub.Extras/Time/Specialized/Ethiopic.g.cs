﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.42000
// Microsoft.VisualStudio.TextTemplating: 17.0
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
using Zorglub.Time.Horology;

/// <summary>Represents the Ethiopic calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class EthiopicCalendar : SpecialCalendar<EthiopicDate>
{
    /// <summary>Initializes a new instance of the <see cref="EthiopicCalendar"/> class.</summary>
    public EthiopicCalendar() : this(new Coptic12Schema()) { }

    private protected sealed override EthiopicDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Provides common adjusters for <see cref="EthiopicDate"/>.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class EthiopicAdjuster : SpecialAdjuster<EthiopicDate>
{
    /// <summary>Initializes a new instance of the <see cref="EthiopicAdjuster"/> class.</summary>
    public EthiopicAdjuster() : base(EthiopicDate.Calendar.Scope) { }

    internal EthiopicAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected sealed override EthiopicDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Represents a clock for the Ethiopic calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class EthiopicClock
{
    private readonly IClock _clock;
    private readonly DayNumber _epoch;

    /// <summary>Initializes a new instance of the <see cref="EthiopicClock"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    public EthiopicClock(IClock clock) : this(EthiopicDate.Calendar.Epoch, clock) { }

    private EthiopicClock(DayNumber epoch, IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _epoch = epoch;
    }

    /// <summary>Gets an instance of the <see cref="EthiopicClock"/> class for the system clock
    /// using the current time zone setting on this machine.</summary>
    public static EthiopicClock Local { get; } = new(SystemClocks.Local);

    /// <summary>Gets an instance of the <see cref="EthiopicClock"/> class for the system clock
    /// using the Coordinated Universal Time (UTC).</summary>
    public static EthiopicClock Utc { get; } = new(SystemClocks.Utc);

    /// <summary>Obtains an instance of the <see cref="EthiopicClock"/> class for the specified clock.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    [Pure]
    public static EthiopicClock GetClock(IClock clock) => new(clock);

    /// <summary>Obtains a <see cref="EthiopicDate"/> value representing the current date.</summary>
    [Pure]
    public EthiopicDate GetCurrentDate() => new(_clock.Today() - _epoch);
}

/// <summary>Represents the Ethiopic date.
/// <para><see cref="EthiopicDate"/> is an immutable struct.</para></summary>
public partial struct EthiopicDate :
    IDate<EthiopicDate, EthiopicCalendar>,
    IAdjustable<EthiopicDate>
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly Coptic12Schema s_Schema = new();
    private static readonly EthiopicCalendar s_Calendar = new(s_Schema);
    private static readonly MinMaxYearScope s_Scope = s_Calendar.Scope;
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;
    private static readonly EthiopicAdjuster s_Adjuster = new(s_Scope);
    private static readonly EthiopicDate s_MinValue = new(s_Domain.Min - s_Epoch);
    private static readonly EthiopicDate s_MaxValue = new(s_Domain.Max - s_Epoch);

    private readonly int _daysSinceEpoch;

    /// <summary>Initializes a new instance of the <see cref="EthiopicDate"/> struct to the specified date parts.</summary>
    /// <exception cref="AoorException">The specified components do not form a valid date or
    /// <paramref name="year"/> is outside the range of supported years.</exception>
    public EthiopicDate(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>Initializes a new instance of the <see cref="EthiopicDate"/> struct to the specified ordinal date parts.</summary>
    /// <exception cref="AoorException">The specified components do not form a valid ordinal date or
    /// <paramref name="year"/> is outside the range of supported years.</exception>
    public EthiopicDate(int year, int dayOfYear)
    {
        s_Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>Initializes a new instance of the <see cref="EthiopicDate"/> struct to the specified day number.</summary>
    /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of supported values.</exception>
    public EthiopicDate(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        _daysSinceEpoch = dayNumber - s_Epoch;
    }

    /// <summary>This constructor does NOT validate its parameter.</summary>
    internal EthiopicDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static EthiopicDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static EthiopicDate MaxValue => s_MaxValue;

    /// <summary>Gets the date adjuster.
    /// <para>This static property is thread-safe.</para></summary>
    public static EthiopicAdjuster Adjuster => s_Adjuster;

    /// <inheritdoc />
    public static EthiopicCalendar Calendar => s_Calendar;

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

public partial struct EthiopicDate // Conversions, adjustments...
{
    #region Counting

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

    #endregion
    #region Adjustments

    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public EthiopicDate Adjust(Func<EthiopicDate, EthiopicDate> adjuster)
    {
        Requires.NotNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new EthiopicDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new EthiopicDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new EthiopicDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new EthiopicDate(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new EthiopicDate(dayNumber - s_Epoch);
    }

    #endregion
}

public partial struct EthiopicDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(EthiopicDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is EthiopicDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct EthiopicDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(EthiopicDate left, EthiopicDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static EthiopicDate Min(EthiopicDate x, EthiopicDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static EthiopicDate Max(EthiopicDate x, EthiopicDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(EthiopicDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is EthiopicDate date ? CompareTo(date)
        : Throw.NonComparable(typeof(EthiopicDate), obj);
}

public partial struct EthiopicDate // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>Subtracts the two specified dates and returns the number of days between them.</summary>
    public static int operator -(EthiopicDate left, EthiopicDate right) => left.CountDaysSince(right);

    /// <summary>Adds a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static EthiopicDate operator +(EthiopicDate value, int days) => value.PlusDays(days);

    /// <summary>Subtracts a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static EthiopicDate operator -(EthiopicDate value, int days) => value.PlusDays(-days);

    /// <summary>Adds one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the latest supported date.</exception>
    public static EthiopicDate operator ++(EthiopicDate value) => value.NextDay();

    /// <summary>Subtracts one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the earliest supported date.</exception>
    public static EthiopicDate operator --(EthiopicDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(EthiopicDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public EthiopicDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public EthiopicDate NextDay() =>
        this == s_MaxValue ? Throw.DateOverflow<EthiopicDate>()
        : new EthiopicDate(_daysSinceEpoch + 1);

    /// <inheritdoc />
    [Pure]
    public EthiopicDate PreviousDay() =>
        this == s_MinValue ? Throw.DateOverflow<EthiopicDate>()
        : new EthiopicDate(_daysSinceEpoch - 1);
}
