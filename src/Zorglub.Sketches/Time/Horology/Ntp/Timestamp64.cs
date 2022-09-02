// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using System.Buffers.Binary;
    using System.Net;

    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.TemporalConstants;

    // TODO(api): overflows (uint, ulong, etc.)
    // From/ToDateTime() & year 2036.
    // Randomization.
    // https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca5394

    // Adapted from
    // https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/sntp/Timestamp64.java
    // GitHub mirror:
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/java/android/net/sntp/Timestamp64.java
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/tests/coretests/src/android/net/sntp/Timestamp64Test.java

    // See
    // - https://www.eecis.udel.edu/~mills/time.html
    // - https://www.eecis.udel.edu/~mills/y2k.html
    // - https://tickelton.gitlab.io/articles/ntp-timestamps/

    public readonly partial struct Timestamp64 :
        // Comparison
        IComparisonOperators<Timestamp64, Timestamp64>,
        IMinMaxValue<Timestamp64>,
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

        /// <summary>
        /// Represents the minimum value of <see cref="FractionOfSecond"/>.
        /// <para>This field is a constant equal to 0.</para>
        /// </summary>
        public const long MinFractionOfSecond = 0;

        /// <summary>
        /// Represents the maximum value of <see cref="FractionOfSecond"/>.
        /// <para>This field is a constant equal to 4_294_967_295.</para>
        /// </summary>
        public const long MaxFractionOfSecond = (1L << 32) - 1;

        private readonly uint _secondOfEra;
        private readonly uint _fractionOfSecond;

        internal Timestamp64(uint secondOfEra, uint fractionOfSecond)
        {
            // Useless, MaxSecondOfEra = UInt32.MaxValue.
            //if (secondOfEra > MaxSecondOfEra)
            //    Throw.ArgumentOutOfRange(nameof(secondOfEra));

            _secondOfEra = secondOfEra;
            _fractionOfSecond = fractionOfSecond;
        }

        public static Timestamp64 Zero { get; }

        public static Timestamp64 MinValue => Zero;
        public static Timestamp64 MaxValue { get; } = new(UInt32.MaxValue, UInt32.MaxValue);

        /// <summary>
        /// Gets the second of the NTP era, i.e. the number of elapsed seconds since <see cref="Zero"/>.
        /// </summary>
        public long SecondOfEra => _secondOfEra;

        public long FractionOfSecond => _fractionOfSecond;

        private ulong Value => ((ulong)_secondOfEra << 32) | _fractionOfSecond;

        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"{_secondOfEra}.{_fractionOfSecond}");

        /// <summary>
        /// Counts the number of elapsed milliseconds since <see cref="Zero"/>.
        /// </summary>
        [Pure]
        public long CountMillisecondsSinceZero() => (long)(
            MillisecondsPerSecond * (ulong)_secondOfEra
            // millisecond-of-second
            + ((MillisecondsPerSecond * (ulong)_fractionOfSecond) >> 32));

        /// <summary>
        /// Counts the number of elapsed nanoseconds since <see cref="Zero"/>.
        /// </summary>
        [Pure]
        public long CountNanosecondsSinceZero() => (long)(
            NanosecondsPerSecond * (ulong)_secondOfEra
            // nanosecond-of-second
            + ((NanosecondsPerSecond * (ulong)_fractionOfSecond) >> 32));

        private static class FractionalSecond
        {
            // 1 fraction of second = 1 / 2^32 second
            // Relation to a subunit-of-second:
            // > subunit-of-second = (SubunitsPerSecond * fraction-of-second) / 2^32
            // > fraction-of-second = (2^32 * subunit-of-second) / SubunitsPerSecond
            // Precision is about 232 picoseconds.

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

    public partial struct Timestamp64 // Internal helpers
    {
        /// <summary>
        /// Reads a <see cref="Timestamp64"/> from the beginning of a read-only span of bytes.
        /// </summary>
        [Pure]
        internal static Timestamp64 ReadFrom(ReadOnlySpan<byte> buf)
        {
            Debug.Assert(buf.Length >= 8);

            uint secondOfEra = BinaryPrimitives.ReadUInt32BigEndian(buf);
            uint fractionOfSecond = BinaryPrimitives.ReadUInt32BigEndian(buf[4..]);

            return new Timestamp64(secondOfEra, fractionOfSecond);
        }

        /// <summary>
        /// Writes the current instance into a span of bytes.
        /// </summary>
        internal void WriteTo(Span<byte> buf)
        {
            Debug.Assert(buf.Length >= 8);

            BinaryPrimitives.WriteUInt32BigEndian(buf, _secondOfEra);
            BinaryPrimitives.WriteUInt32BigEndian(buf[4..], _fractionOfSecond);
        }

        internal Timestamp64 RandomizeSubmilliseconds(Random random)
        {
            // We randomize the submilliseconds part of fraction-of-second.
            //   1 millisecond = 2^32 / 1000 > 4_294_967 fraction-of-second
            // Therefore 2^22 (= 4_194_304) fraction-of-second < 1 millisecond.
            const int LowerBitsToRandomize = 22;

            Debug.Assert(random != null);

            uint fractionOfSecond = RandomizeSubmilliseconds(
                _fractionOfSecond,
                LowerBitsToRandomize,
                random);

            return new Timestamp64(_secondOfEra, fractionOfSecond);
        }

        private static uint RandomizeSubmilliseconds(uint fractionOfSecond, int index, Random random)
        {
            Debug.Assert(random != null);
            Debug.Assert(0 < index && index < 32);

            long lowerBitMask = (1L << index) - 1L;
            long upperBitMask = ~lowerBitMask;
            int randomValue = random.Next();

            return (uint)((fractionOfSecond & upperBitMask) | (randomValue & lowerBitMask));
        }
    }

    public partial struct Timestamp64 // Conversions
    {
        private static readonly DateTime s_Epoch = new(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Pure]
        public static Timestamp64 FromDateTime(DateTime time)
        {
            if (time.Kind != DateTimeKind.Utc) Throw.Argument(nameof(time));

            var secondOfEra = (time - s_Epoch).TotalSeconds;
            var fractionOfSecond = FractionalSecond.FromMillisecondOfSecond(time.Millisecond);

            return new Timestamp64((uint)secondOfEra, fractionOfSecond);
        }

        [Pure]
        public DateTime ToDateTime() =>
            s_Epoch + TimeSpan.FromMilliseconds(CountMillisecondsSinceZero());
    }

    public partial struct Timestamp64 // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Timestamp64"/> are equal.
        /// </summary>
        public static bool operator ==(Timestamp64 left, Timestamp64 right) =>
            left._secondOfEra == right._secondOfEra
            && left._fractionOfSecond == right._fractionOfSecond;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Timestamp64"/> are not equal.
        /// </summary>
        public static bool operator !=(Timestamp64 left, Timestamp64 right) =>
            left._secondOfEra != right._secondOfEra
            || left._fractionOfSecond != right._fractionOfSecond;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Timestamp64 other) =>
            _secondOfEra == other._secondOfEra
            && _fractionOfSecond == other._fractionOfSecond;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Timestamp64 timestamp && Equals(timestamp);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(_secondOfEra, _fractionOfSecond);
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
