// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp;

using System;
using System.Buffers.Binary;

using Zorglub.Time.Core;

using static Zorglub.Time.Core.TemporalConstants;

/// <summary>
/// Represents a 32-bit NTP short format; see RFC 5905, section 6.
/// <para><see cref="Duration32"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Duration32 :
    // Comparison
    IComparisonOperators<Duration32, Duration32>,
    IMinMaxValue<Duration32>,
    IMinMaxFunctions<Duration32>
{
    /// <summary>
    /// Represents the number of whole seconds.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly ushort _seconds;

    /// <summary>
    /// Represents the number of fractional seconds.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly ushort _fractionalSeconds;

    /// <summary>
    /// Initializes a new instance of the <see cref="Duration32"/> struct.
    /// </summary>
    [CLSCompliant(false)]
    public Duration32(ushort seconds, ushort fractionalSeconds)
    {
        _seconds = seconds;
        _fractionalSeconds = fractionalSeconds;
    }

    /// <summary>
    /// Gets a duration representing exactly zero fractional second.
    /// <para>This is the shortest duration.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration32 Zero { get; }

    /// <summary>
    /// Gets a duration representing exactly one fractional second.
    /// <para>This is the shortest duration longer than <see cref="Zero"/>.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration32 Epsilon { get; } = new(0, 1);

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Duration32"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration32 MinValue => Zero;

    /// <summary>
    /// Gets the largest possible value of a <see cref="Duration32"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Duration32 MaxValue { get; } = new(UInt16.MaxValue, UInt16.MaxValue);

    /// <summary>
    /// Gets the number of whole seconds in this duration.
    /// </summary>
    public int Seconds => _seconds;

    /// <summary>
    /// Gets the number of fractional seconds in this duration.
    /// </summary>
    public int FractionalSeconds => _fractionalSeconds;

    /// <summary>
    /// Gets the total number of seconds in this duration.
    /// </summary>
    [Pure]
    public double TotalSeconds =>
        _seconds + _fractionalSeconds / (double)0x1_0000;

    /// <summary>
    /// Gets the total number of milliseconds in this duration.
    /// </summary>
    [Pure]
    public double TotalMilliseconds =>
         MillisecondsPerSecond * _seconds
        + MillisecondsPerSecond * _fractionalSeconds / (double)0x1_0000;

    /// <summary>
    /// Gets the total number of nanoseconds in this duration.
    /// </summary>
    [Pure]
    public double TotalNanoseconds =>
         NanosecondsPerSecond * (ulong)_seconds
        + NanosecondsPerSecond * (ulong)_fractionalSeconds / (double)0x1_0000;

    /// <summary>
    /// Gets the total number of fractional seconds in this duration.
    /// </summary>
    private ulong TotalFractionalSeconds => ((ulong)_seconds << 16) | _fractionalSeconds;

    /// <summary>
    /// Returns a culture-independent string representation of the current instance.
    /// </summary>
    [Pure]
    public override string ToString() =>
        FormattableString.Invariant($"{_seconds}s+{_fractionalSeconds}");
}

public partial struct Duration32 // Binary helpers
{
    /// <summary>
    /// Reads a <see cref="Duration32"/> value from the beginning of a read-only span of bytes.
    /// </summary>
    /// <exception cref="AoorException"><paramref name="buf"/> is too small to contain a
    /// <see cref="Duration32"/>.</exception>
    [Pure]
    internal static Duration32 ReadFrom(ReadOnlySpan<byte> buf)
    {
        Debug.Assert(buf.Length >= 4);

        ushort seconds = BinaryPrimitives.ReadUInt16BigEndian(buf);
        ushort fractionalSeconds = BinaryPrimitives.ReadUInt16BigEndian(buf[2..]);

        return new Duration32(seconds, fractionalSeconds);
    }
}

public partial struct Duration32 // IEquatable
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="Duration32"/> are equal.
    /// </summary>
    public static bool operator ==(Duration32 left, Duration32 right) =>
        left._seconds == right._seconds
        && left._fractionalSeconds == right._fractionalSeconds;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Duration32"/> are not equal.
    /// </summary>
    public static bool operator !=(Duration32 left, Duration32 right) =>
        left._seconds != right._seconds
        || left._fractionalSeconds != right._fractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Duration32 other) =>
        _seconds == other._seconds
        && _fractionalSeconds == other._fractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Duration32 duration && Equals(duration);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_seconds, _fractionalSeconds);
}

public partial struct Duration32 // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Duration32 left, Duration32 right) =>
        left.TotalFractionalSeconds < right.TotalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator <=(Duration32 left, Duration32 right) =>
        left.TotalFractionalSeconds <= right.TotalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator >(Duration32 left, Duration32 right) =>
        left.TotalFractionalSeconds > right.TotalFractionalSeconds;

    /// <inheritdoc />
    public static bool operator >=(Duration32 left, Duration32 right) =>
        left.TotalFractionalSeconds >= right.TotalFractionalSeconds;

    /// <inheritdoc />
    [Pure]
    public static Duration32 Min(Duration32 x, Duration32 y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Duration32 Max(Duration32 x, Duration32 y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Duration32 other) =>
        TotalFractionalSeconds.CompareTo(other.TotalFractionalSeconds);

    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Duration32 duration ? CompareTo(duration)
        : Throw.NonComparable(typeof(Duration32), obj);
}
