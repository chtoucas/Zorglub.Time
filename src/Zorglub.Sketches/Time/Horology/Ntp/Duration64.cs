// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System;
    using System.Buffers.Binary;

    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.TemporalConstants;

    // Adapted from
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/sntp/Duration64.java
    // GitHub mirror:
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/java/android/net/sntp/Duration64.java
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/tests/coretests/src/android/net/sntp/Duration64Test.java

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

        internal Duration64(long fractionalSeconds)
        {
            // TODO(code): fix boundaries (63-bit signed integer).
            _fractionalSeconds = fractionalSeconds;
        }

        public static Duration64 Zero { get; }

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
        /// Gets the total number of fractional seconds.
        /// </summary>
        public long TotalFractionalSeconds => _fractionalSeconds;

        /// <summary>
        /// Gets the total number of nanoseconds in this duration.
        /// </summary>
        [Pure]
        public long TotalNanoseconds => FractionalSeconds.ToNanoseconds(_fractionalSeconds);

        /// <summary>
        /// Gets the total number of milliseconds in this duration.
        /// </summary>
        [Pure]
        public long TotalMilliseconds => FractionalSeconds.ToMilliseconds(_fractionalSeconds);

        /// <summary>
        /// Gets the total number of seconds in this duration.
        /// </summary>
        [Pure]
        public int TotalSeconds => (int)FractionalSeconds.ToSeconds(_fractionalSeconds);

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
        internal static Duration64 ReadFrom(ReadOnlySpan<byte> buf)
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

        [Pure]
        public Duration64 DivideBy(int value) =>
            new(checked(_fractionalSeconds / value));
    }
}
