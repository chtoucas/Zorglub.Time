// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using static Zorglub.Time.Core.TemporalConstants;

    // Adapted from
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/java/android/net/sntp/Timestamp64.java
    // See
    // - https://www.eecis.udel.edu/~mills/time.html
    // - https://tickelton.gitlab.io/articles/ntp-timestamps/

    public readonly partial struct NtpTimestamp :
        IEqualityOperators<NtpTimestamp, NtpTimestamp>
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

        internal NtpTimestamp(uint secondOfEra, uint fractionalSecond)
        {
            // Useless, MaxSecondOfEra = UInt32.MaxValue.
            //if (secondOfEra > MaxSecondOfEra)
            //    Throw.ArgumentOutOfRange(nameof(secondOfEra));

            _secondOfEra = secondOfEra;
            _fractionalSecond = fractionalSecond;
        }

        public static NtpTimestamp Zero { get; } = new(0, 0);

        /// <summary>
        /// Gets the second of the NTP era, i.e. number of elapsed seconds since <see cref="Zero"/>.
        /// </summary>
        public long SecondOfEra => (long)_secondOfEra;

        // 1 fractional second = 1 / 2^32 second
        // Relation to a subunit-of-second:
        // > subunit-of-second = (SubunitsPerSecond * fractional-second) / 2^32
        // > fractional-second = (2^32 * subunit-of-second) / SubunitsPerSecond
        // Precision is about 200 picoseconds.

        [CLSCompliant(false)]
        public uint FractionalSecond => (uint)_fractionalSecond;

        [Pure]
        public override string ToString() =>
            FormattableString.Invariant($"{SecondOfEra}.{FractionalSecond}");

        /// <summary>
        /// Counts the number of elapsed milliseconds since <see cref="Zero"/>.
        /// </summary>
        [Pure]
        public long CountMillisecondsSinceZero() =>
            MillisecondsPerSecond * SecondOfEra
            // millisecond-of-second
            + (long)((MillisecondsPerSecond * _fractionalSecond) >> 32);

        /// <summary>
        /// Counts the number of elapsed nanoseconds since <see cref="Zero"/>.
        /// </summary>
        [Pure]
        public long CountNanosecondsSinceZero() =>
            NanosecondsPerSecond * SecondOfEra
            // nanosecond-of-second
            + (long)((NanosecondsPerSecond * _fractionalSecond) >> 32);

        internal static class FractionOfSecond
        {
            [Pure]
            public static uint FromMillisecondOfSecond(ulong millisecondOfSecond)
            {
                if (millisecondOfSecond > MillisecondsPerSecond) Throw.ArgumentOutOfRange(nameof(millisecondOfSecond));
                return (uint)((millisecondOfSecond << 32) / MillisecondsPerSecond);
            }

            [Pure]
            public static uint FromNanosecondOfSecond(ulong nanosecondOfSecond)
            {
                if (nanosecondOfSecond > NanosecondsPerSecond) Throw.ArgumentOutOfRange(nameof(nanosecondOfSecond));
                return (uint)((nanosecondOfSecond << 32) / NanosecondsPerSecond);
            }
        }

        // FIXME(code): after 2036.
        [Pure]
        public DateTime ToDateTime() =>
            s_Epoch + TimeSpan.FromMilliseconds(CountMillisecondsSinceZero());
    }

    public partial struct NtpTimestamp // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="NtpTimestamp"/> are equal.
        /// </summary>
        public static bool operator ==(NtpTimestamp left, NtpTimestamp right) =>
            left._secondOfEra == right._secondOfEra
            && left._fractionalSecond == right._fractionalSecond;

        /// <summary>
        /// Determines whether two specified instances of <see cref="NtpTimestamp"/> are not equal.
        /// </summary>
        public static bool operator !=(NtpTimestamp left, NtpTimestamp right) =>
            left._secondOfEra != right._secondOfEra
            || left._fractionalSecond != right._fractionalSecond;

        /// <inheritdoc />
        [Pure]
        public bool Equals(NtpTimestamp other) =>
            _secondOfEra == other._secondOfEra
            & _fractionalSecond == other._fractionalSecond;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is NtpTimestamp timestamp && Equals(timestamp);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(SecondOfEra, FractionalSecond);
    }
}
