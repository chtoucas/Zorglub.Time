// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Buffers.Binary;

    using Zorglub.Time.Core;

    // Adapted from
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/sntp/Duration64.java
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/java/android/net/sntp/Duration64.java
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/tests/coretests/src/android/net/sntp/Duration64Test.java

    public readonly partial struct Duration64 :
        IComparisonOperators<Duration64, Duration64>,
        IMinMaxValue<Duration64>,
        IMinMaxFunctions<Duration64>
    {
        private readonly long _value;

        public Duration64(long value)
        {
            _value = value;
        }

        public static Duration64 Zero { get; }

        public static Duration64 MinValue { get; } = new(Int64.MinValue);
        public static Duration64 MaxValue { get; } = new(Int64.MaxValue);

        public long Value => _value;

        [Pure]
        public override string ToString() => FormattableString.Invariant($"{_value}");

        /// <summary>
        /// Counts the number of seconds in this duration.
        /// </summary>
        [Pure]
        public int CountSeconds() => (int)(_value >> 32);

        //return (double)_value / 0x10000;
    }

    public partial struct Duration64
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
            left._value == right._value;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Duration64"/> are not equal.
        /// </summary>
        public static bool operator !=(Duration64 left, Duration64 right) =>
            left._value != right._value;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Duration64 other) => _value == other._value;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Duration64 duration && Equals(duration);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => _value.GetHashCode();
    }

    public partial struct Duration64 // IComparable
    {
        public static bool operator <(Duration64 left, Duration64 right) =>
            left._value < right._value;

        public static bool operator <=(Duration64 left, Duration64 right) =>
            left._value <= right._value;

        public static bool operator >(Duration64 left, Duration64 right) =>
            left._value > right._value;

        public static bool operator >=(Duration64 left, Duration64 right) =>
            left._value >= right._value;

        [Pure]
        public static Duration64 Min(Duration64 x, Duration64 y) => x < y ? x : y;

        [Pure]
        public static Duration64 Max(Duration64 x, Duration64 y) => x > y ? x : y;

        [Pure]
        public int CompareTo(Duration64 other) => _value.CompareTo(other._value);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Duration64 duration ? CompareTo(duration)
            : Throw.NonComparable(typeof(Duration64), obj);
    }

}
