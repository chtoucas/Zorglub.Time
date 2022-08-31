// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using static Zorglub.Time.Core.TemporalConstants;

    // Adapted from
    // https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/java/android/net/sntp/Timestamp64.java
    // See
    // https://www.eecis.udel.edu/~mills/time.html
    // https://tickelton.gitlab.io/articles/ntp-timestamps/

    public readonly partial struct Timestamp :
        IEqualityOperators<Timestamp, Timestamp>
    {
        public const long MinEraSeconds = 1;
        public const long MaxEraSeconds = (1 << 32) - 1; // 4_294_967_295

        private readonly long _secondOfEra;
        private readonly int _fractionBits;

        public Timestamp(long secondOfEra, int fractionBits)
        {
            if (secondOfEra < MinEraSeconds || secondOfEra > MaxEraSeconds)
                Throw.ArgumentOutOfRange(nameof(secondOfEra));

            _secondOfEra = secondOfEra;
            _fractionBits = fractionBits;
        }

        public static Timestamp Zero { get; } = new(0, 0);

        /// <summary>
        /// Gets the second of the NTP era, i.e. number of elapsed seconds since <see cref="Zero"/>.
        /// </summary>
        public long SecondOfEra => _secondOfEra;

        public int FractionBits => _fractionBits;

        internal static int ConvertFractionBitsToNanoseconds(int fractionBits) =>
            (int)(((fractionBits & 0xffff_ffffL) * NanosecondsPerSecond) >>> 32);

        internal static int ConvertNanosecondsToFractionBits(long nanoseconds)
        {
            if (nanoseconds > NanosecondsPerSecond) Throw.ArgumentOutOfRange(nameof(nanoseconds));

            return (int)((nanoseconds << 32) / NanosecondsPerSecond);
        }
    }

    public partial struct Timestamp // IEquatable
    {
        /// <summary>
        /// Determines whether two specified instances of <see cref="Timestamp"/> are equal.
        /// </summary>
        public static bool operator ==(Timestamp left, Timestamp right) =>
            left._secondOfEra == right._secondOfEra
            && left._fractionBits == right._fractionBits;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Timestamp"/> are not equal.
        /// </summary>
        public static bool operator !=(Timestamp left, Timestamp right) =>
            left._secondOfEra != right._secondOfEra
            || left._fractionBits != right._fractionBits;

        /// <inheritdoc />
        [Pure]
        public bool Equals(Timestamp other) =>
            _secondOfEra == other._secondOfEra
            & _fractionBits == other._fractionBits;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Timestamp timestamp && Equals(timestamp);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => HashCode.Combine(SecondOfEra, FractionBits);
    }
}
