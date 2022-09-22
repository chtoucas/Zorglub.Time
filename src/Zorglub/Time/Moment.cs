// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using System.Numerics;

using static Zorglub.Time.Core.TemporalConstants;

// REVIEW(api): implement IFixedDay? Math ops? etc.

/// <summary>Represents a moment with millisecond precision.
/// <para><see cref="Moment"/> is an immutable struct.</para></summary>
public readonly partial struct Moment :
    // Comparison
    IComparisonOperators<Moment, Moment>,
    IMinMaxValue<Moment>
{
    /// <summary>Represents the day number.</summary>
    private readonly DayNumber _dayNumber;
    /// <summary>Represents the time of the day with millisecond precision.</summary>
    private readonly TimeOfDay _timeOfDay;

    /// <summary>Initializes a new instance of the <see cref="Moment"/> struct from the specified
    /// day number and time of the day.</summary>
    public Moment(DayNumber dayNumber, TimeOfDay timeOfDay)
    {
        _dayNumber = dayNumber;
        _timeOfDay = timeOfDay;
    }

    /// <summary>Gets the origin.
    /// <para>The Monday 1st of January, 1 CE within the Gregorian calendar at midnight (0h).</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static Moment Zero { get; }

    /// <summary>Gets the smallest possible value of a <see cref="Moment"/>.
    /// <para>This static property is thread-safe.</para></summary>
    public static Moment MinValue { get; } = new(DayNumber.MinValue, TimeOfDay.MinValue);

    /// <summary>Gets the largest possible value of a <see cref="Moment"/>.
    /// <para>This static property is thread-safe.</para></summary>
    public static Moment MaxValue { get; } = new(DayNumber.MaxValue, TimeOfDay.MaxValue);

    /// <summary>Gets the day number.</summary>
    public DayNumber DayNumber => _dayNumber;

    /// <summary>Gets the time of the day with millisecond precision.</summary>
    public TimeOfDay TimeOfDay => _timeOfDay;

    /// <summary>Gets the number of elapsed seconds since <see cref="Zero"/>.</summary>
    public long SecondsSinceZero =>
        _dayNumber.DaysSinceZero * (long)SecondsPerDay
        + _timeOfDay.MillisecondOfDay / MillisecondsPerSecond;

    /// <summary>Gets the number of elapsed milliseconds since <see cref="Zero"/>.</summary>
    public long MillisecondsSinceZero =>
        _dayNumber.DaysSinceZero * (long)MillisecondsPerDay
        + _timeOfDay.MillisecondOfDay;

    /// <summary>Returns a culture-independent string representation of this instance.</summary>
    [Pure]
    public override string ToString() =>
        FormattableString.Invariant($"{DayNumber}+{TimeOfDay}");

    /// <summary>Deconstructs this instance into its components.</summary>
    public void Deconstruct(out DayNumber dayNumber, out TimeOfDay timeOfDay) =>
        (dayNumber, timeOfDay) = (DayNumber, TimeOfDay);
}

public partial struct Moment // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(Moment left, Moment right) =>
        left._dayNumber == right._dayNumber && left._timeOfDay == right._timeOfDay;

    /// <inheritdoc />
    public static bool operator !=(Moment left, Moment right) =>
        left._dayNumber != right._dayNumber || left._timeOfDay != right._timeOfDay;

    /// <inheritdoc />
    public bool Equals(Moment other) =>
        _dayNumber == other._dayNumber && _timeOfDay == other._timeOfDay;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Moment moment && Equals(moment);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_dayNumber, _timeOfDay);
}

public partial struct Moment // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Moment left, Moment right) => left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(Moment left, Moment right) => left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(Moment left, Moment right) => left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(Moment left, Moment right) => left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Moment other) =>
        MillisecondsSinceZero.CompareTo(other.MillisecondsSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Moment moment ? CompareTo(moment)
        : Throw.NonComparable(typeof(Moment), obj);
}
