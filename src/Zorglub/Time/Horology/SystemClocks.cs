// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides system clocks.
    /// </summary>
    public static class SystemClocks
    {
        /// <summary>
        /// Gets an instance of the system clock using the current time zone setting on this machine.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ITimepiece Local { get; } = new LocalClock();

        /// <summary>
        /// Gets an instance of the system clock using the UTC time zone.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ITimepiece Utc { get; } = new UtcClock();

        /// <summary>
        /// Represents the system clock using the current time zone setting on this machine.
        /// </summary>
        private sealed class LocalClock : ITimepiece
        {
            public LocalClock() { }

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
        private sealed class UtcClock : ITimepiece
        {
            public UtcClock() { }

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
}
