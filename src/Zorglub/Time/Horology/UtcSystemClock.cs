﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

    // WARNING: this clock does not use the UTC time scale, it ignores leap
    // seconds.

    /// <summary>
    /// Represents the system clock using the UTC time zone.
    /// <para>See <see cref="SystemClocks.Utc"/>.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class UtcSystemClock : IClock
    {
        /// <summary>
        /// Represents a singleton instance of the <see cref="UtcSystemClock"/> class.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly UtcSystemClock Instance = new();

        private UtcSystemClock() { }

        /// <inheritdoc/>
        [Pure]
        public Moment Now() => throw new NotImplementedException();

        /// <inheritdoc/>
        [Pure]
        public DayNumber Today()
        {
            // NB: the cast should always succeed.
            int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.UtcNow.Ticks);
            return new DayNumber(daysSinceZero);
        }
    }
}
