// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.TemporalConstants;

    // TODO(api): overflows (uint, ulong, etc.) From/ToDateTime() & year 2036, randomization.

    // Adapted from
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/sntp/Timestamp64.java
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/java/android/net/sntp/Timestamp64.java
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/tests/coretests/src/android/net/sntp/Timestamp64Test.java

    // See
    // - https://www.eecis.udel.edu/~mills/time.html
    // - https://www.eecis.udel.edu/~mills/y2k.html
    // - https://tickelton.gitlab.io/articles/ntp-timestamps/

    public readonly partial struct Timestamp64 :
        // Comparison
        IComparisonOperators<Timestamp64, Timestamp64>,
        IMinMaxFunctions<Timestamp64>,
        // Arithmetic
        ISubtractionOperators<Timestamp64, Timestamp64, Duration64>
    {
        /// <summary>
        /// Represents the minimum value of <see cref="SecondOfEra"/>.
        /// <para>This field is a constant equal to 0.</para>
        /// </summary>
        public const long MinSecondOfEra = 0;

        /// <summary>
        /// Represents the maximum value of <see cref="SecondOfEra"/>.
        /// <para>This field is a constant equal to 4_294_967_295.</para>
        /// </summary>
        public const long MaxSecondOfEra = (1L << 32) - 1;

        private static readonly DateTime s_Epoch = new(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // We use unsigned longs to simplify calculations (uint may overflow).
        private readonly ulong _secondOfEra;
        private readonly ulong _fractionalSecond;

        internal Timestamp64(uint secondOfEra, uint fractionalSecond)
        {
            // Useless, MaxSecondOfEra = UInt32.MaxValue.
            //if (secondOfEra > MaxSecondOfEra)
            //    Throw.ArgumentOutOfRange(nameof(secondOfEra));

            _secondOfEra = secondOfEra;
            _fractionalSecond = fractionalSecond;
        }

        public static Timestamp64 Zero { get; } = new(0, 0);

        internal uint SecondOfEraUnsigned => (uint)_secondOfEra;

        /// <summary>
        /// Gets the second of the NTP era, i.e. the number of elapsed seconds since <see cref="Zero"/>.
        /// </summary>
        public long SecondOfEra => (long)_secondOfEra;

        // 1 fractional second = 1 / 2^32 second
        // Relation to a subunit-of-second:
        // > subunit-of-second = (SubunitsPerSecond * fractional-second) / 2^32
        // > fractional-second = (2^32 * subunit-of-second) / SubunitsPerSecond
        // Precision is about 232 picoseconds.

        [CLSCompliant(false)]
        public uint FractionalSecond => (uint)_fractionalSecond;

        private ulong Value => (_secondOfEra << 32) | _fractionalSecond;

        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"{_secondOfEra}.{_fractionalSecond}");

        /// <summary>
        /// Counts the number of elapsed milliseconds since <see cref="Zero"/>.
        /// </summary>
        [Pure]
        public long CountMillisecondsSinceZero() => (long)(
            MillisecondsPerSecond * _secondOfEra
            // millisecond-of-second
            + ((MillisecondsPerSecond * _fractionalSecond) >> 32));

        /// <summary>
        /// Counts the number of elapsed nanoseconds since <see cref="Zero"/>.
        /// </summary>
        [Pure]
        public long CountNanosecondsSinceZero() => (long)(
            NanosecondsPerSecond * _secondOfEra
            // nanosecond-of-second
            + ((NanosecondsPerSecond * _fractionalSecond) >> 32));
    }

    public partial struct Timestamp64 // Conversions
    {
        [Pure]
        public static Timestamp64 FromDateTime(DateTime time)
        {
            var secondOfEra = (time - s_Epoch).TotalSeconds;
            var fractionOfSecond = FractionOfSecond.FromMillisecondOfSecond(time.Millisecond);

            return new Timestamp64((uint)secondOfEra, fractionOfSecond);
        }

        [Pure]
        public DateTime ToDateTime() =>
            s_Epoch + TimeSpan.FromMilliseconds(CountMillisecondsSinceZero());

        private static class FractionOfSecond
        {
            [Pure]
            public static uint FromMillisecondOfSecond(int millisecondOfSecond)
            {
                Debug.Assert(millisecondOfSecond >= 0);
                Debug.Assert(millisecondOfSecond < MillisecondsPerSecond);

                return (uint)(((ulong)millisecondOfSecond << 32) / MillisecondsPerSecond);
            }

            [Pure]
            public static uint FromNanosecondOfSecond(int nanosecondOfSecond)
            {
                Debug.Assert(nanosecondOfSecond >= 0);
                Debug.Assert(nanosecondOfSecond < NanosecondsPerSecond);

                return (uint)(((ulong)nanosecondOfSecond << 32) / NanosecondsPerSecond);
            }
        }
    }

    public partial struct Timestamp64 // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Timestamp64"/> are equal.
        /// </summary>
        public static bool operator ==(Timestamp64 left, Timestamp64 right) =>
            left._secondOfEra == right._secondOfEra
            && left._fractionalSecond == right._fractionalSecond;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Timestamp64"/> are not equal.
        /// </summary>
        public static bool operator !=(Timestamp64 left, Timestamp64 right) =>
            left._secondOfEra != right._secondOfEra
            || left._fractionalSecond != right._fractionalSecond;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Timestamp64 other) =>
            _secondOfEra == other._secondOfEra
            && _fractionalSecond == other._fractionalSecond;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Timestamp64 timestamp && Equals(timestamp);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(SecondOfEra, FractionalSecond);
    }

    public partial struct Timestamp64 // IComparable
    {
        public static bool operator <(Timestamp64 left, Timestamp64 right) =>
            left.Value < right.Value;

        public static bool operator <=(Timestamp64 left, Timestamp64 right) =>
            left.Value <= right.Value;

        public static bool operator >(Timestamp64 left, Timestamp64 right) =>
            left.Value > right.Value;

        public static bool operator >=(Timestamp64 left, Timestamp64 right) =>
            left.Value >= right.Value;

        [Pure]
        public static Timestamp64 Min(Timestamp64 x, Timestamp64 y) => x < y ? x : y;

        [Pure]
        public static Timestamp64 Max(Timestamp64 x, Timestamp64 y) => x > y ? x : y;

        [Pure]
        public int CompareTo(Timestamp64 other) => Value.CompareTo(other.Value);

        /// <inheritdoc />
        [Pure]
        public int CompareTo(object? obj) =>
            obj is null ? 1
            : obj is Timestamp64 timestamp ? CompareTo(timestamp)
            : Throw.NonComparable(typeof(Timestamp64), obj);
    }

    public partial struct Timestamp64 // Arithmetic
    {
        /// <summary>
        /// Subtracts the two specified timestamps and returns the duration between them.
        /// </summary>
        public static Duration64 operator -(Timestamp64 left, Timestamp64 right) => left.Subtract(right);

        [Pure]
        public Duration64 Subtract(Timestamp64 other)
        {
            ulong start = other.Value;
            ulong end = Value;

            return end > start ? new Duration64((long)(end - start))
                : new Duration64((long)(start - end));
        }
    }
}
