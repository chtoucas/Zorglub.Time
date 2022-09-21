// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

using System;

using static Zorglub.Time.Core.TemporalConstants;

// https://datatracker.ietf.org/doc/draft-stenn-ntp-leap-smear-refid/

/// <summary>
/// Represents a 24-bit duration.
/// <para><see cref="Duration24"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Duration24 :
    // Comparison
    IComparisonOperators<Duration24, Duration24>,
    IMinMaxValue<Duration24>,
    IMinMaxFunctions<Duration24>
{
    /// <summary>
    /// Represents the minimum value of <see cref="Seconds"/>.
    /// <para>This field is a constant equal to -4.</para>
    /// </summary>
    [CLSCompliant(false)]
    public const sbyte MinSeconds = -4;

    /// <summary>
    /// Represents the maximum value of <see cref="Seconds"/>.
    /// <para>This field is a constant equal to 3.</para>
    /// </summary>
    [CLSCompliant(false)]
    public const sbyte MaxSeconds = 3;

    /// <summary>
    /// Represents the minimum value of <see cref="FractionalSeconds"/>.
    /// <para>One second equals 2^22 fractional seconds.</para>
    /// <para>This field is a constant equal to 0.</para>
    /// </summary>
    [CLSCompliant(false)]
    public const uint MinFractionalSeconds = 0;

    /// <summary>
    /// Represents the maximum value of <see cref="FractionalSeconds"/>.
    /// <para>One second equals 2^22 fractional seconds.</para>
    /// <para>This field is a constant equal to 4_194_303.</para>
    /// </summary>
    [CLSCompliant(false)]
    public const uint MaxFractionalSeconds = (1 << 22) - 1;

    /// <summary>
    /// Represents the number of whole seconds.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly sbyte _seconds;

    /// <summary>
    /// Represents the number of fractional seconds.
    /// <para>One second equals 2^22 fractional seconds.</para>
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly uint _fractionalSeconds;

    /// <summary>
    /// Initializes a new instance of the <see cref="Duration24"/> struct.
    /// <para>One second equals 2^22 fractional seconds.</para>
    /// </summary>
    [CLSCompliant(false)]
    public Duration24(sbyte seconds, uint fractionalSeconds)
    {
        if (seconds < MinSeconds || seconds > MaxSeconds)
            Throw.ArgumentOutOfRange(nameof(seconds));
        if (fractionalSeconds < MinFractionalSeconds || fractionalSeconds > MaxFractionalSeconds)
            Throw.ArgumentOutOfRange(nameof(fractionalSeconds));

        _seconds = seconds;
        _fractionalSeconds = fractionalSeconds;
    }

    /// <summary>
    /// Gets a duration representing exactly zero fractional second.
    /// <para>This is the shortest duration.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration24 Zero { get; }

    /// <summary>
    /// Gets a duration representing exactly one fractional second.
    /// <para>One second equals 2^22 fractional seconds.</para>
    /// <para>This is the shortest duration longer than <see cref="Zero"/>.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration24 Epsilon { get; } = new(0, 1);

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Duration24"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration24 MinValue { get; } = new(MinSeconds, MinFractionalSeconds);

    /// <summary>
    /// Gets the largest possible value of a <see cref="Duration24"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration24 MaxValue { get; } = new(MaxSeconds, MaxFractionalSeconds);

    /// <summary>
    /// Gets the number of whole seconds in this duration.
    /// </summary>
    public int Seconds => _seconds;

    /// <summary>
    /// Gets the number of fractional seconds in this duration.
    /// <para>One second equals 2^22 fractional seconds.</para>
    /// </summary>
    public int FractionalSeconds => (int)_fractionalSeconds;

    /// <summary>
    /// Gets the total number of seconds in this duration.
    /// </summary>
    [Pure]
    public double TotalSeconds =>
        _seconds + _fractionalSeconds / (double)0x40_0000;

    /// <summary>
    /// Gets the total number of milliseconds in this duration.
    /// </summary>
    [Pure]
    public double TotalMilliseconds =>
         MillisecondsPerSecond * _seconds
        + MillisecondsPerSecond * _fractionalSeconds / (double)0x40_0000;

    /// <summary>
    /// Gets the total number of nanoseconds in this duration.
    /// </summary>
    [Pure]
    public double TotalNanoseconds =>
         NanosecondsPerSecond * _seconds
        + NanosecondsPerSecond * (long)_fractionalSeconds / (double)0x40_0000;

    /// <summary>
    /// Gets the total number of fractional seconds in this duration.
    /// <para>One second equals 2^22 fractional seconds.</para>
    /// </summary>
    private int TotalFractionalSeconds => (_seconds << 22) | (int)_fractionalSeconds;

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString() =>
        FormattableString.Invariant($"{_seconds}s+{_fractionalSeconds}");
}

public partial struct Duration24 // Binary helpers
{
    ///// <summary>
    ///// Reads a <see cref="Duration24"/> value from the beginning of a read-only span of bytes.
    ///// </summary>
    ///// <exception cref="AoorException"><paramref name="buf"/> is too small to contain a
    ///// <see cref="Duration24"/>.</exception>
    //[Pure]
    //internal static Duration24 ReadFrom(ReadOnlySpan<byte> buf)
    //{
    //    Debug.Assert(buf.Length >= 4);

    //    ushort seconds = BinaryPrimitives.ReadUInt16BigEndian(buf);
    //    ushort fractionalSeconds = BinaryPrimitives.ReadUInt16BigEndian(buf[2..]);

    //    return new Duration24(seconds, fractionalSeconds);
    //}
}

public partial struct Duration24 // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Duration24"/> are equal.
    /// </summary>
    public static bool operator ==(Duration24 left, Duration24 right) =>
        left._seconds == right._seconds
        && left._fractionalSeconds == right._fractionalSeconds;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Duration24"/> are not equal.
    /// </summary>
    public static bool operator !=(Duration24 left, Duration24 right) =>
        left._seconds != right._seconds
        || left._fractionalSeconds != right._fractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Duration24 other) =>
        _seconds == other._seconds
        && _fractionalSeconds == other._fractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Duration24 duration && Equals(duration);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_seconds, _fractionalSeconds);
}

public partial struct Duration24 // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Duration24 left, Duration24 right) =>
        left.TotalFractionalSeconds < right.TotalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator <=(Duration24 left, Duration24 right) =>
        left.TotalFractionalSeconds <= right.TotalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator >(Duration24 left, Duration24 right) =>
        left.TotalFractionalSeconds > right.TotalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator >=(Duration24 left, Duration24 right) =>
        left.TotalFractionalSeconds >= right.TotalFractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public static Duration24 Min(Duration24 x, Duration24 y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Duration24 Max(Duration24 x, Duration24 y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Duration24 other) =>
        TotalFractionalSeconds.CompareTo(other.TotalFractionalSeconds);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Duration24 duration ? CompareTo(duration)
        : Throw.NonComparable(typeof(Duration24), obj);
}
