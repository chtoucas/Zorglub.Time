// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology;

using System.Numerics;

// TODO(doc): describe the time scale in use here, it should match the BCL
// time scale.
// Advantages: a day is exactly 86.400 seconds long.
// Disadvantages: no leap seconds, a second is not a SI second.
// On systems supporting leap seconds, DateTime maps 23:59:60 to 23:59:59,
// therefore repeating it. Regarding the count of Ticks, I don't know what
// it does if one day a leap second is negative (it seems to me that it will
// get Tick wrong).
// See also https://docs.oracle.com/javase/8/docs/api/java/time/Instant.html

/// <summary>
/// Represents a instant on the global time-line with nanosecond precision.
/// <para><see cref="Instant"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Instant :
    // Comparison
    IComparisonOperators<Instant, Instant>,
    IMinMaxValue<Instant>,
    IMinMaxFunctions<Instant>
{
    /// <summary>
    /// Represents the number of elapsed nanoseconds since 1970-01-01T00:00:00Z (UTC).
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly long _nanosecondsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Instant"/> struct.
    /// </summary>
    public Instant(long nanosecondsSinceEpoch)
    {
        _nanosecondsSinceEpoch = nanosecondsSinceEpoch;
    }

    public static Instant Epoch { get; }

    public static Instant MinValue { get; } = new(Int64.MinValue);
    public static Instant MaxValue { get; } = new(Int64.MaxValue);

    public long NanosecondsSinceEpoch => _nanosecondsSinceEpoch;

    /// <summary>
    /// Returns a culture-independent string representation of this instance.
    /// </summary>
    public override string ToString() =>
        FormattableString.Invariant($"{NanosecondsSinceEpoch}ns");
}

public partial struct Instant // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Instant"/> are equal.
    /// </summary>
    public static bool operator ==(Instant left, Instant right) =>
        left._nanosecondsSinceEpoch == right._nanosecondsSinceEpoch;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Instant"/> are not equal.
    /// </summary>
    public static bool operator !=(Instant left, Instant right) =>
        left._nanosecondsSinceEpoch != right._nanosecondsSinceEpoch;

    /// <inheritdoc />
    public bool Equals(Instant other) =>
        _nanosecondsSinceEpoch == other._nanosecondsSinceEpoch;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Instant instant && Equals(instant);

    /// <inheritdoc />
    public override int GetHashCode() => _nanosecondsSinceEpoch.GetHashCode();
}

public partial struct Instant // IComparable
{
    /// <summary>
    /// Compares the two specified instants to see if the left one is strictly earlier than the
    /// right one.
    /// </summary>
    public static bool operator <(Instant left, Instant right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Compares the two specified instants to see if the left one is earlier than or equal to
    /// the right one.
    /// </summary>
    public static bool operator <=(Instant left, Instant right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Compares the two specified instants to see if the left one is strictly later than the
    /// right one.
    /// </summary>
    public static bool operator >(Instant left, Instant right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Compares the two specified instants to see if the left one is later than or equal to the
    /// right one.
    /// </summary>
    public static bool operator >=(Instant left, Instant right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Obtains the earlier instant of two specified instants.
    /// </summary>
    [Pure]
    public static Instant Min(Instant x, Instant y) => x < y ? x : y;

    /// <summary>
    /// Obtains the later instant of two specified instants.
    /// </summary>
    [Pure]
    public static Instant Max(Instant x, Instant y) => x > y ? x : y;

    /// <summary>
    /// Indicates whether this instant instance is earlier, later or the same as the specified one.
    /// </summary>
    public int CompareTo(Instant other) =>
        _nanosecondsSinceEpoch.CompareTo(other._nanosecondsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Instant instant ? CompareTo(instant)
        : Throw.NonComparable(typeof(Instant), obj);
}
