// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

    using static Zorglub.Time.Core.TemporalConstants;

    /// <summary>
    /// Represents the system clock using the current time zone setting on this machine.
    /// <para>See <see cref="SystemClocks.Local"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class LocalSystemClock : IClock
    {
        /// <summary>
        /// Represents a singleton instance of the <see cref="LocalSystemClock"/> class.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly LocalSystemClock Instance = new();

        private LocalSystemClock() { }

        /// <inheritdoc/>
        [Pure]
        public Moment Now()
        {
            var now = DateTime.Now;
            long ticksSinceZero = now.Ticks;
            int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(ticksSinceZero);
            //long ticksOfDay = now.TimeOfDay.Ticks;
            //long ticksOfDay = ticksSinceZero - TicksPerDay * daysSinceZero;
            long ticksOfDay = ticksSinceZero % TicksPerDay;
            int millisecondsOfDay = (int)(ticksOfDay / TicksPerMillisecond);
            var timeOfDay = TimeOfDay.FromMillisecondsSinceMidnight(millisecondsOfDay);

            //100 * DateTime.Now.Ticks;
            return new Moment(new DayNumber(daysSinceZero), timeOfDay);
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber Today()
        {
            // NB: the cast should always succeed.
            int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.Now.Ticks);
            return new DayNumber(daysSinceZero);
        }
    }
}
