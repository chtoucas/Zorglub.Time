// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using static Zorglub.Time.Core.TemporalConstants;

    // Unit of 1 fraction of second = 1 / 2^32 second
    // Relation to a subunit-of-second:
    // > subunit-of-second = (SubunitsPerSecond * fraction-of-second) / 2^32
    // > fraction-of-second = (2^32 * subunit-of-second) / SubunitsPerSecond
    // Precision is about 232 picoseconds.
    //
    // WARNING
    // To divide an integer by 2^32, do NOT use the integer division from C#.
    // It truncates toward minus infinity, which is not what we want for
    // negative numbers.

    internal static partial class FractionalSeconds { }

    internal static partial class FractionalSeconds // MillisecondOfSecond
    {
        /// <summary>
        /// Converts a millisecond of the second to a fraction of the second.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong FromMillisecondOfSecond(uint millisecondOfSecond)
        {
            Debug.Assert(millisecondOfSecond >= 0);
            Debug.Assert(millisecondOfSecond < MillisecondsPerSecond);

            return ((ulong)millisecondOfSecond << 32) / MillisecondsPerSecond;
        }

        /// <summary>
        /// Converts a fraction of the second to a millisecond of the second.
        /// <para>The result is in the range from 0 (included) to 1000 (excluded).</para>
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToMillisecondOfSecond(uint fractionOfSecond)
        {
            ulong millisecondOfSecond = (MillisecondsPerSecond * (ulong)fractionOfSecond) >> 32;

            Debug.Assert(millisecondOfSecond >= 0);
            Debug.Assert(millisecondOfSecond < MillisecondsPerSecond);

            return millisecondOfSecond;
        }
    }

    internal static partial class FractionalSeconds // NanosecondOfSecond
    {
        /// <summary>
        /// Converts a fraction of the second to a nanosecond of the second.
        /// <para>The result is in the range from 0 (included) to 1_000_000_000 (excluded).</para>
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToNanosecondOfSecond(uint fractionOfSecond)
        {
            ulong nanosecondOfSecond = (NanosecondsPerSecond * (ulong)fractionOfSecond) >> 32;

            Debug.Assert(nanosecondOfSecond >= 0);
            Debug.Assert(nanosecondOfSecond < NanosecondsPerSecond);

            return nanosecondOfSecond;
        }
    }

    internal static partial class FractionalSeconds // FractionalSeconds
    {
        /// <summary>
        /// Converts a second of the era to a number of fractional seconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong FromSeconds(uint secondOfEra) => (ulong)secondOfEra << 32;

        /// <summary>
        /// Converts a number of fractional seconds to a number of whole seconds.
        /// <para>The result is in the range of <see cref="Int32"/>.</para>
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToSeconds(long fractionalSeconds) => fractionalSeconds >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to a number of whole milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToMilliseconds(long fractionalSeconds) =>
            (MillisecondsPerSecond * fractionalSeconds) >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to a number of whole nanoseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToNanoseconds(long fractionalSeconds) =>
            (NanosecondsPerSecond * fractionalSeconds) >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to a number of seconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToTotalSeconds(long fractionalSeconds) =>
            fractionalSeconds / (double)0x1_0000_0000L;

        /// <summary>
        /// Converts a number of fractional seconds to a number of milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToTotalMilliseconds(long fractionalSeconds) =>
            (MillisecondsPerSecond * fractionalSeconds) / (double)0x1_0000_0000L;

        /// <summary>
        /// Converts a number of fractional seconds to a number of nanoseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToTotalNanoseconds(long fractionalSeconds) =>
            (NanosecondsPerSecond * fractionalSeconds) / (double)0x1_0000_0000L;
    }
}
