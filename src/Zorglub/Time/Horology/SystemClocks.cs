// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

    // Timescale: internally the BCL uses the UTC timescale but it ignores leap
    // seconds.

    /// <summary>
    /// Provides system clocks.
    /// </summary>
    public static class SystemClocks
    {
        /// <summary>
        /// Gets an instance of the system clock using the current time zone setting on this machine.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SystemLocalClock Local { get; } = new();

        /// <summary>
        /// Gets an instance of the system clock using the UTC time zone.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SystemUtcClock Utc { get; } = new();
    }

    /// <summary>
    /// Represents the system clock using the current time zone setting on this machine.
    /// </summary>
    public sealed class SystemLocalClock : ITimepiece
    {
        internal SystemLocalClock() { }

        [Pure]
        public long Now() => 100 * DateTime.Now.Ticks;

        [Pure]
        public DayNumber Today()
        {
            // NB: the cast should always succeed.
            int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.Now.Ticks);
            return new DayNumber(daysSinceZero);
        }
    }

    /// <summary>
    /// Represents the system clock using the UTC time zone.
    /// </summary>
    public sealed class SystemUtcClock : ITimepiece
    {
        internal SystemUtcClock() { }

        [Pure]
        public long Now() => 100 * DateTime.UtcNow.Ticks;

        [Pure]
        public DayNumber Today()
        {
            // NB: the cast should always succeed.
            int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.UtcNow.Ticks);
            return new DayNumber(daysSinceZero);
        }
    }
}
