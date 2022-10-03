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

using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;
using Zorglub.Time.Horology;

/// <summary>Represents the Gregorian calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class GregorianCalendar : SpecialCalendar<GregorianDate>
{
    /// <summary>Initializes a new instance of the <see cref="GregorianCalendar"/> class.</summary>
    public GregorianCalendar() : this(new GregorianSchema()) { }

    private protected sealed override GregorianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Provides common adjusters for <see cref="GregorianDate"/>.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class GregorianAdjuster : SpecialAdjuster<GregorianDate>
{
    /// <summary>Initializes a new instance of the <see cref="GregorianAdjuster"/> class.</summary>
    public GregorianAdjuster() : base(GregorianDate.Calendar.Scope) { }

    internal GregorianAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected sealed override GregorianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>Represents a clock for the Gregorian calendar.
/// <para>This class cannot be inherited.</para></summary>
public sealed partial class GregorianClock
{
    private readonly IClock _clock;

    /// <summary>Initializes a new instance of the <see cref="GregorianClock"/> class.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    public GregorianClock(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    /// <summary>Gets an instance of the <see cref="GregorianClock"/> class for the system clock
    /// using the current time zone setting on this machine.</summary>
    public static GregorianClock Local { get; } = new(SystemClocks.Local);

    /// <summary>Gets an instance of the <see cref="GregorianClock"/> class for the system clock
    /// using the Coordinated Universal Time (UTC).</summary>
    public static GregorianClock Utc { get; } = new(SystemClocks.Utc);

    /// <summary>Obtains an instance of the <see cref="GregorianClock"/> class for the specified clock.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is null.</exception>
    [Pure]
    public static GregorianClock GetClock(IClock clock) => new(clock);

    /// <summary>Obtains a <see cref="GregorianDate"/> value representing the current date.</summary>
    [Pure]
    public GregorianDate GetCurrentDate() => new(_clock.Today().DaysSinceZero);
}

/// <summary>Represents the Gregorian date.
/// <para><see cref="GregorianDate"/> is an immutable struct.</para></summary>
public readonly partial struct GregorianDate :
    IDate<GregorianDate, GregorianCalendar>,
    IAdjustable<GregorianDate>
{ }

public partial struct GregorianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct GregorianDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public GregorianDate Adjust(Func<GregorianDate, GregorianDate> adjuster)
    {
        Requires.NotNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new GregorianDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new GregorianDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new GregorianDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new GregorianDate(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (s_Domain.Contains(dayNumber) == false) Throw.DateOverflow();
        return new GregorianDate(dayNumber.DaysSinceZero);
    }
}

public partial struct GregorianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(GregorianDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is GregorianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct GregorianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator <=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static GregorianDate Min(GregorianDate x, GregorianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static GregorianDate Max(GregorianDate x, GregorianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(GregorianDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianDate date ? CompareTo(date)
        : Throw.NonComparable(typeof(GregorianDate), obj);
}

public partial struct GregorianDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>Subtracts the two specified dates and returns the number of days between them.</summary>
    public static int operator -(GregorianDate left, GregorianDate right) => left.CountDaysSince(right);

    /// <summary>Adds a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static GregorianDate operator +(GregorianDate value, int days) => value.PlusDays(days);

    /// <summary>Subtracts a number of days to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow either the capacity of
    /// <see cref="Int32"/> or the range of supported dates.</exception>
    public static GregorianDate operator -(GregorianDate value, int days) => value.PlusDays(-days);

    /// <summary>Adds one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the latest supported date.</exception>
    public static GregorianDate operator ++(GregorianDate value) => value.NextDay();

    /// <summary>Subtracts one day to the specified date, yielding a new date.</summary>
    /// <exception cref="OverflowException">The operation would overflow the earliest supported date.</exception>
    public static GregorianDate operator --(GregorianDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(GregorianDate other) =>
        // No need to use a checked context here.
        _daysSinceZero - other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public GregorianDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceZero + days);
        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextDay() =>
        this == s_MaxValue ? Throw.DateOverflow<GregorianDate>()
        : new GregorianDate(_daysSinceZero + 1);

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousDay() =>
        this == s_MinValue ? Throw.DateOverflow<GregorianDate>()
        : new GregorianDate(_daysSinceZero - 1);
}