// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Ntp
{
    using static Zorglub.Time.Core.TemporalConstants;

    internal static class FractionalSecondsUnit
    {
        /// <summary>
        /// Converts a number of seconds to fractional seconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong FromSeconds(uint seconds) => (ulong)seconds << 32;

        /// <summary>
        /// Converts a number of milliseconds to fractional seconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong FromMilliseconds(uint milliseconds) =>
            ((ulong)milliseconds << 32) / MillisecondsPerSecond;

        /// <summary>
        /// Converts a number of fractional seconds to milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToSeconds(uint fractionalSeconds) => fractionalSeconds >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToSeconds(long fractionalSeconds) => fractionalSeconds >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToMilliseconds(uint fractionalSeconds) =>
            (MillisecondsPerSecond * (ulong)fractionalSeconds) >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToMilliseconds(long fractionalSeconds) =>
            (MillisecondsPerSecond * fractionalSeconds) >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToNanoseconds(uint fractionalSeconds) =>
            (NanosecondsPerSecond * (ulong)fractionalSeconds) >> 32;

        /// <summary>
        /// Converts a number of fractional seconds to milliseconds.
        /// </summary>
        [Pure]
        // CIL code size = XXX bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToNanoseconds(long fractionalSeconds) =>
            (NanosecondsPerSecond * fractionalSeconds) >> 32;
    }
}
