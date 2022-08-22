// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides system clocks.
    /// </summary>
    public static class SystemClock
    {
        /// <summary>
        /// Gets an instance of the system clock using the default timezone.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ITimepiece Default { get; } = new DefaultClock();

        /// <summary>
        /// Gets an instance of the system clock using the UTC timezone.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ITimepiece Utc { get; } = new UtcClock();

        /// <summary>
        /// Represents the system clock using the default timezone.
        /// </summary>
        private sealed class DefaultClock : ITimepiece
        {
            public DefaultClock() { }

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
        /// Represents the system clock using the UTC timezone.
        /// </summary>
        private sealed class UtcClock : ITimepiece
        {
            public UtcClock() { }

            // DateTime.UtcNow correspond réellement au nombre de tics sans
            // compter les secondes intercalaires, donc dans l'échelle atomique
            // internationale.

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
