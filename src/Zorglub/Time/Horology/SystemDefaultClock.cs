// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents the system timepiece using the default timezone.
    /// Represents the system clock.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SystemDefaultClock : ITimepiece
    {
        private SystemDefaultClock() { }

        /// <summary>
        /// Gets a singleton instance of the <see cref="SystemDefaultClock"/> class.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SystemDefaultClock Instance { get; } = new();

        /// <inheritdoc/>
        public long Now() => 100 * DateTime.Now.Ticks;

        /// <inheritdoc/>
        public DayNumber Today()
        {
            // NB: the cast should always succeed.
            int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.Now.Ticks);
            return new DayNumber(daysSinceZero);
        }
    }
}
