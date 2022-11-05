// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

using System;
using System.Numerics;

using static Zorglub.Time.Core.TemporalConstants;

// Adapted from
// https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/sntp/Duration64.java

/// <summary>
/// Represents a 64-bit duration.
/// <para>This type uses the unit for fractional seconds defined in <see cref="Timestamp64"/>.</para>
/// <para><see cref="Duration64"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Duration64 :
    // Comparison
    IComparisonOperators<Duration64, Duration64>,
    IMinMaxValue<Duration64>,
    IMinMaxFunction<Duration64>,
    // Arithmetic
    IAdditionOperators<Duration64, Duration64, Duration64>,
    ISubtractionOperators<Duration64, Duration64, Duration64>,
    IDivisionOperators<Duration64, int, Duration64>
{
    /// <summary>
    /// Represents the total number of fractional seconds in this duration.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly long _totalFractionalSeconds;

    /// <summary>
    /// Initializes a new instance of the <see cref="Duration64"/> struct.
    /// </summary>
    public Duration64(long fractionalSeconds)
    {
        _totalFractionalSeconds = fractionalSeconds;
    }

    /// <summary>
    /// Gets a duration representing exactly zero fractional second.
    /// <para>This is the shortest duration.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration64 Zero { get; }

    /// <summary>
    /// Gets a duration representing exactly one fractional second.
    /// <para>This is the shortest duration greater than <see cref="Zero"/>.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration64 Epsilon { get; } = new(1);

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Duration64"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration64 MinValue { get; } = new(Int64.MinValue);

    /// <summary>
    /// Gets the largest possible value of a <see cref="Duration64"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration64 MaxValue { get; } = new(Int64.MaxValue);

    /// <summary>
    /// Gets the total number of fractional seconds in this duration.
    /// </summary>
    public long TotalFractionalSeconds => _totalFractionalSeconds;

    // *** WARNING ***
    // Do NOT use >> 32 to divide a signed integer by 2^32. Indeed, >> rounds
    // towards minus infinity, e.g. (-2^32 - 1) >> 32 = -2 which is not what
    // we want that is -1. The integer division gives the correct result
    // since it rounds towards zero, (-2^32 - 1) / 2^32 = -1

    /// <summary>
    /// Gets the total number of nanoseconds in this duration.
    /// </summary>
    [Pure]
    public double TotalNanoseconds =>
        NanosecondsPerSecond * _totalFractionalSeconds / (double)Timestamp64.FractionalSecondsPerSecond;

    /// <summary>
    /// Gets the total number of milliseconds in this duration.
    /// </summary>
    [Pure]
    public double TotalMilliseconds =>
        MillisecondsPerSecond * _totalFractionalSeconds / (double)Timestamp64.FractionalSecondsPerSecond;

    /// <summary>
    /// Gets the total number of seconds in this duration.
    /// </summary>
    [Pure]
    public double TotalSeconds => _totalFractionalSeconds / (double)Timestamp64.FractionalSecondsPerSecond;

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString() => FormattableString.Invariant($"{_totalFractionalSeconds}");
}

public partial struct Duration64 // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Duration64"/> are equal.
    /// </summary>
    public static bool operator ==(Duration64 left, Duration64 right) =>
        left._totalFractionalSeconds == right._totalFractionalSeconds;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Duration64"/> are not equal.
    /// </summary>
    public static bool operator !=(Duration64 left, Duration64 right) =>
        left._totalFractionalSeconds != right._totalFractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Duration64 other) => _totalFractionalSeconds == other._totalFractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Duration64 duration && Equals(duration);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _totalFractionalSeconds.GetHashCode();
}

public partial struct Duration64 // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Duration64 left, Duration64 right) =>
        left._totalFractionalSeconds < right._totalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator <=(Duration64 left, Duration64 right) =>
        left._totalFractionalSeconds <= right._totalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator >(Duration64 left, Duration64 right) =>
        left._totalFractionalSeconds > right._totalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator >=(Duration64 left, Duration64 right) =>
        left._totalFractionalSeconds >= right._totalFractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public static Duration64 Min(Duration64 x, Duration64 y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Duration64 Max(Duration64 x, Duration64 y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Duration64 other) =>
        _totalFractionalSeconds.CompareTo(other._totalFractionalSeconds);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Duration64 duration ? CompareTo(duration)
        : Throw.NonComparable(typeof(Duration64), obj);
}

public partial struct Duration64 // Arithmetic
{
    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the range of
    /// <see cref="Int64"/>.</exception>
    public static Duration64 operator +(Duration64 left, Duration64 right) =>
        new(checked(left._totalFractionalSeconds + right._totalFractionalSeconds));

    /// <inheritdoc />
    /// <exception cref="OverflowException">The operation would overflow the range of
    /// <see cref="Int64"/>.</exception>
    public static Duration64 operator -(Duration64 left, Duration64 right) =>
        new(checked(left._totalFractionalSeconds - right._totalFractionalSeconds));

#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    /// <inheritdoc />
    public static Duration64 operator /(Duration64 left, int right) =>
        new(left._totalFractionalSeconds / right);

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Adds a duration to the current instance, yielding a new duration.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of
    /// <see cref="Int64"/>.</exception>
    [Pure]
    public Duration64 Add(Duration64 other) =>
        new(checked(_totalFractionalSeconds + other._totalFractionalSeconds));

    /// <summary>
    /// Subtracts a duration from the current, yielding a new duration.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range of
    /// <see cref="Int64"/>.</exception>
    [Pure]
    public Duration64 Subtract(Duration64 other) =>
        new(checked(_totalFractionalSeconds - other._totalFractionalSeconds));
}
