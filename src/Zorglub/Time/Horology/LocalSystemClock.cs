// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

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
        public long Now() => 100 * DateTime.Now.Ticks;

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
