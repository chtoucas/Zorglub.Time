// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System;
    using System.Buffers.Binary;

    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.TemporalConstants;

    // REVIEW(code): fix boundaries (63-bit signed integer).

    // Adapted from
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/sntp/Duration64.java

    public readonly partial struct Duration64 :
        // Comparison
        IComparisonOperators<Duration64, Duration64>,
        IMinMaxValue<Duration64>,
        IMinMaxFunctions<Duration64>,
        // Arithmetic
        IAdditionOperators<Duration64, Duration64, Duration64>,
        ISubtractionOperators<Duration64, Duration64, Duration64>,
        IDivisionOperators<Duration64, int, Duration64>
    {
        private readonly long _fractionalSeconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration64"/> struct.
        /// </summary>
        public Duration64(long fractionalSeconds)
        {
            _fractionalSeconds = fractionalSeconds;
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
        /// Gets a duration representing exactly one second.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        internal static Duration64 OneSecond { get; } = new(1L << 32);

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
        /// Gets the number of fractional seconds.
        /// </summary>
        public long FractionalSeconds => _fractionalSeconds;

        // *** WARNING ***
        // Do NOT use >> 32 to divide a signed integer by 2^32. Indeed, >> rounds
        // towards minus infinity, e.g. (-2^32 - 1) >> 32 = -2 which is not what
        // we want that is -1.
        // The integer division gives the correct result since it rounds towards
        // zero, (-2^32 - 1) / 2^32 = -1

        /// <summary>
        /// Gets the total number of nanoseconds in this duration.
        /// </summary>
        [Pure]
        public double TotalNanoseconds =>
            NanosecondsPerSecond * _fractionalSeconds / (double)0x1_0000_0000L;

        /// <summary>
        /// Gets the total number of milliseconds in this duration.
        /// </summary>
        [Pure]
        public double TotalMilliseconds =>
            MillisecondsPerSecond * _fractionalSeconds / (double)0x1_0000_0000L;

        /// <summary>
        /// Gets the total number of seconds in this duration.
        /// </summary>
        [Pure]
        public double TotalSeconds => _fractionalSeconds / (double)0x1_0000_0000L;

        /// <summary>
        /// Returns a culture-independent string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => FormattableString.Invariant($"{_fractionalSeconds}");
    }

    public partial struct Duration64 // Binary helpers
    {
        /// <summary>
        /// Reads a <see cref="Duration64"/> from the beginning of a read-only span of bytes.
        /// </summary>
        [Pure]
        internal static Duration64 ReadFourBytesFrom(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 4);

            int value = BinaryPrimitives.ReadInt32BigEndian(buf);

            return new Duration64(value);
        }
    }

    public partial struct Duration64
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Duration64"/> are equal.
        /// </summary>
        public static bool operator ==(Duration64 left, Duration64 right) =>
            left._fractionalSeconds == right._fractionalSeconds;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Duration64"/> are not equal.
        /// </summary>
        public static bool operator !=(Duration64 left, Duration64 right) =>
            left._fractionalSeconds != right._fractionalSeconds;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Duration64 other) => _fractionalSeconds == other._fractionalSeconds;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Duration64 duration && Equals(duration);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _fractionalSeconds.GetHashCode();
    }

    public partial struct Duration64 // IComparable
    {
        public static bool operator <(Duration64 left, Duration64 right) =>
            left._fractionalSeconds < right._fractionalSeconds;

        public static bool operator <=(Duration64 left, Duration64 right) =>
            left._fractionalSeconds <= right._fractionalSeconds;

        public static bool operator >(Duration64 left, Duration64 right) =>
            left._fractionalSeconds > right._fractionalSeconds;

        public static bool operator >=(Duration64 left, Duration64 right) =>
            left._fractionalSeconds >= right._fractionalSeconds;

        [Pure]
        public static Duration64 Min(Duration64 x, Duration64 y) => x < y ? x : y;

        [Pure]
        public static Duration64 Max(Duration64 x, Duration64 y) => x > y ? x : y;

        [Pure]
        public int CompareTo(Duration64 other) =>
            _fractionalSeconds.CompareTo(other._fractionalSeconds);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Duration64 duration ? CompareTo(duration)
            : Throw.NonComparable(typeof(Duration64), obj);
    }

    public partial struct Duration64 // Arithmetic
    {
        public static Duration64 operator +(Duration64 left, Duration64 right) =>
            new(checked(left._fractionalSeconds + right._fractionalSeconds));

        public static Duration64 operator -(Duration64 left, Duration64 right) =>
            new(checked(left._fractionalSeconds - right._fractionalSeconds));

#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

        public static Duration64 operator /(Duration64 left, int right) =>
            new(checked(left._fractionalSeconds / right));

#pragma warning restore CA2225 // Operator overloads have named alternates

        [Pure]
        public Duration64 Add(Duration64 other) =>
            new(checked(_fractionalSeconds + other._fractionalSeconds));

        [Pure]
        public Duration64 Subtract(Duration64 other) =>
            new(checked(_fractionalSeconds - other._fractionalSeconds));
    }
}
