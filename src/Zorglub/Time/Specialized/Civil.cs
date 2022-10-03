﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Horology;

// We use GregorianStandardScope instead of s_Calendar.Scope because they
// are strictly equivalent.
// We use daysSinceZero instead of daysSinceEpoch because s_Calendar.Epoch
// is equal to DayNumber.Zero.

/// <summary>Represents the Civil calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed class CivilCalendar :
    SpecialCalendar<CivilDate>,
    IRegularFeaturette
{
    /// <summary>Initializes a new instance of the <see cref="CivilCalendar"/> class.</summary>
    public CivilCalendar() : this(new CivilSchema()) { }

    internal CivilCalendar(CivilSchema schema)
        : base("Gregorian", StandardScope.Create(schema, DayZero.NewStyle))
    { }

    /// <inheritdoc />
    public int MonthsInYear => GJSchema.MonthsPerYear;

    [Pure]
    private protected sealed override CivilDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Provides common adjusters for <see cref="CivilDate"/>.
/// <para>This class cannot be inherited.</para></summary>
public sealed class CivilAdjuster : SpecialAdjuster<CivilDate>
{
    /// <summary>Initializes a new instance of the <see cref="CivilAdjuster"/> class.</summary>
    public CivilAdjuster() : base(CivilDate.Calendar.Scope) { }

    internal CivilAdjuster(MinMaxYearScope scope) : base(scope) { }

    [Pure]
    private protected sealed override CivilDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Represents a clock for the Civil calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed class CivilClock
{
    private readonly IClock _clock;

    /// <summary>Initializes a new instance of the <see cref="CivilClock"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    public CivilClock(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    /// <summary>Gets an instance of the <see cref="CivilClock"/> class for the system clock using
    /// the current time zone setting on this machine.</summary>
    public static CivilClock Local { get; } = new(SystemClocks.Local);

    /// <summary>Gets an instance of the <see cref="CivilClock"/> class for the system clock using
    /// the Coordinated Universal Time (UTC).</summary>
    public static CivilClock Utc { get; } = new(SystemClocks.Utc);

    /// <summary>Obtains an instance of the <see cref="CivilClock"/> class for the specified clock.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    [Pure]
    public static CivilClock GetClock(IClock clock) => new(clock);

    /// <summary>Obtains a <see cref="CivilDate"/> value representing the current date.</summary>
    [Pure]
    public CivilDate GetCurrentDate() => new(_clock.Today().DaysSinceZero);
}

/// <summary>Represents the Civil date.
/// <para><see cref="CivilDate"/> is an immutable struct.</para></summary>
public readonly partial struct CivilDate :
    IDate<CivilDate, CivilCalendar>,
    IAdjustable<CivilDate>
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly CivilSchema s_Schema = new();
    private static readonly CivilCalendar s_Calendar = new(s_Schema);
    private static readonly MinMaxYearScope s_Scope = s_Calendar.Scope;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;
    private static readonly CivilAdjuster s_Adjuster = new(s_Scope);
    private static readonly CivilDate s_MinValue = new(s_Domain.Min.DaysSinceZero);
    private static readonly CivilDate s_MaxValue = new(s_Domain.Max.DaysSinceZero);

    private readonly int _daysSinceEpoch;

    /// <summary>Initializes a new instance of the <see cref="CivilDate"/> struct to the specified
    /// date parts.</summary>
    /// <exception cref="AoorException">The specified components do not form a valid date or
    /// <paramref name="year"/> is outside the range of supported years.</exception>
    public CivilDate(int year, int month, int day)
    {
        GregorianStandardScope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = CivilFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>Initializes a new instance of the <see cref="CivilDate"/> struct to the specified
    /// ordinal date parts.</summary>
    /// <exception cref="AoorException">The specified components do not form a valid ordinal date or
    /// <paramref name="year"/> is outside the range of supported years.</exception>
    public CivilDate(int year, int dayOfYear)
    {
        GregorianStandardScope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>Initializes a new instance of the <see cref="CivilDate"/> struct.</summary>
    /// <exception cref="AoorException"><paramref name="dayNumber"/> is outside the range of
    /// supported values.</exception>
    public CivilDate(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        _daysSinceEpoch = dayNumber.DaysSinceZero;
    }

    /// <summary>Initializes a new instance of the <see cref="CivilDate"/> struct.
    /// <para>This method does NOT validate its parameter.</para></summary>
    internal CivilDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static CivilDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static CivilDate MaxValue => s_MaxValue;

    /// <summary>Gets the date adjuster.
    /// <para>This static property is thread-safe.</para></summary>
    public static CivilAdjuster Adjuster => s_Adjuster;

    /// <inheritdoc />
    public static CivilCalendar Calendar => s_Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceEpoch);

    /// <summary>Gets the count of days since the Gregorian epoch.</summary>
    public int DaysSinceZero => _daysSinceEpoch;

    int IFixedDay.DaysSinceEpoch => _daysSinceEpoch;

    /// <inheritdoc />
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <inheritdoc />
    public int Century => YearNumbering.GetCentury(Year);

    /// <inheritdoc />
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <inheritdoc />
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <inheritdoc />
    public int Year => CivilFormulae.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            CivilFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = CivilFormulae.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            CivilFormulae.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            CivilFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
            return GregorianFormulae.IsIntercalaryDay(m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary => false;

    /// <summary>Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        CivilFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        CivilFormulae.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = CivilFormulae.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct CivilDate // Conversions, adjustments...
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
    public CivilDate Adjust(Func<CivilDate, CivilDate> adjuster)
    {
        Requires.NotNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new CivilDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new CivilDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new CivilDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new CivilDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new CivilDate(dayNumber.DaysSinceZero);
    }

    #endregion
}

public partial struct CivilDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilDate left, CivilDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(CivilDate left, CivilDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct CivilDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(CivilDate left, CivilDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(CivilDate left, CivilDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(CivilDate left, CivilDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(CivilDate left, CivilDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Min(CivilDate x, CivilDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Max(CivilDate x, CivilDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilDate date ? CompareTo(date)
        : Throw.NonComparable(typeof(CivilDate), obj);
}

public partial struct CivilDate // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>Subtracts the two specified dates and returns the number of days between them.
    /// </summary>
    public static int operator -(CivilDate left, CivilDate right) => left.CountDaysSince(right);

    /// <summary>Adds a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static CivilDate operator +(CivilDate value, int days) => value.PlusDays(days);

    /// <summary>Subtracts a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static CivilDate operator -(CivilDate value, int days) => value.PlusDays(-days);

    /// <summary>Adds one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the latest supported date.
    /// </exception>
    public static CivilDate operator ++(CivilDate value) => value.NextDay();

    /// <summary>Subtracts one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the earliest supported date.
    /// </exception>
    public static CivilDate operator --(CivilDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(CivilDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public CivilDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        GregorianStandardScope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public CivilDate NextDay() =>
        this == s_MaxValue ? Throw.DateOverflow<CivilDate>()
        : new CivilDate(_daysSinceEpoch + 1);

    /// <inheritdoc />
    [Pure]
    public CivilDate PreviousDay() =>
        this == s_MinValue ? Throw.DateOverflow<CivilDate>()
        : new CivilDate(_daysSinceEpoch - 1);
}
