// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology;

using System.Numerics;

using static Zorglub.Time.Core.TemporalConstants;

// REVIEW(code): size of GregorianInstant.

/// <summary>
/// Represents a Gregorian instant with nanosecond precision.
/// <para><see cref="GregorianInstant"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct GregorianInstant :
    // Comparison
    IComparisonOperators<GregorianInstant, GregorianInstant>,
    IMinMaxValue<GregorianInstant>
{
    private readonly DayNumber _dayNumber;
    private readonly InstantOfDay _instantOfDay;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianInstant"/> struct from the specified day
    /// number and instant of the day.
    /// </summary>
    public GregorianInstant(DayNumber dayNumber, InstantOfDay instantOfDay)
    {
        _dayNumber = dayNumber;
        _instantOfDay = instantOfDay;
    }

    /// <summary>
    /// Gets the origin.
    /// <para>The Monday 1st of January, 1 CE within the Gregorian calendar at 0h.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianInstant Zero { get; }

    public static GregorianInstant MinValue { get; } = new(DayNumber.MinValue, InstantOfDay.MinValue);
    public static GregorianInstant MaxValue { get; } = new(DayNumber.MaxValue, InstantOfDay.MaxValue);

    public DayNumber DayNumber => _dayNumber;

    public InstantOfDay InstantOfDay => _instantOfDay;

    public long SecondsSinceZero =>
        _dayNumber.DaysSinceZero * (long)SecondsPerDay
        + _instantOfDay.MillisecondOfDay / MillisecondsPerSecond;

    public long MillisecondsSinceZero =>
        _dayNumber.DaysSinceZero * (long)MillisecondsPerDay
        + _instantOfDay.MillisecondOfDay;

    // FIXME(code): may overflow => not suitable for comparison (see below).
    // Reduce the range of DayNumber's?
    public long NanosecondsSinceZero =>
        checked(
            _dayNumber.DaysSinceZero * NanosecondsPerDay
            + _instantOfDay.NanosecondOfDay);

    /// <summary>
    /// Returns a culture-independent string representation of this instance.
    /// </summary>
    public override string ToString() =>
        FormattableString.Invariant($"{DayNumber}+{InstantOfDay}ns");

    /// <summary>
    /// Deconstructs this instance into its components.
    /// </summary>
    public void Deconstruct(out DayNumber dayNumber, out InstantOfDay timeOfDay) =>
        (dayNumber, timeOfDay) = (DayNumber, InstantOfDay);
}

public partial struct GregorianInstant // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="GregorianInstant"/> are equal.
    /// </summary>
    public static bool operator ==(GregorianInstant left, GregorianInstant right) =>
        left._dayNumber == right._dayNumber && left._instantOfDay == right._instantOfDay;

    /// <summary>
    /// Determines whether two specified instances of <see cref="GregorianInstant"/> are not equal.
    /// </summary>
    public static bool operator !=(GregorianInstant left, GregorianInstant right) =>
        left._dayNumber != right._dayNumber || left._instantOfDay != right._instantOfDay;

    /// <inheritdoc />
    public bool Equals(GregorianInstant other) =>
        _dayNumber == other._dayNumber && _instantOfDay == other._instantOfDay;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is GregorianInstant moment && Equals(moment);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_dayNumber, _instantOfDay);
}

public partial struct GregorianInstant // IComparable
{
    /// <summary>
    /// Compares the two specified moments to see if the left one is strictly earlier than the
    /// right one.
    /// </summary>
    public static bool operator <(GregorianInstant left, GregorianInstant right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Compares the two specified moments to see if the left one is earlier than or equal to
    /// the right one.
    /// </summary>
    public static bool operator <=(GregorianInstant left, GregorianInstant right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Compares the two specified moments to see if the left one is strictly later than the
    /// right one.
    /// </summary>
    public static bool operator >(GregorianInstant left, GregorianInstant right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Compares the two specified moments to see if the left one is later than or equal to the
    /// right one.
    /// </summary>
    public static bool operator >=(GregorianInstant left, GregorianInstant right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Indicates whether this moment instance is earlier, later or the same as the specified one.
    /// </summary>
    [Pure]
    public int CompareTo(GregorianInstant other) =>
        NanosecondsSinceZero.CompareTo(other.NanosecondsSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianInstant moment ? CompareTo(moment)
        : Throw.NonComparable(typeof(GregorianInstant), obj);
}
