// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides a method to obtain the current date on this machine, expressed in the UTC.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class UtcTodayProvider : ITodayProvider
    {
        private UtcTodayProvider() { }

        /// <summary>
        /// Gets a singleton instance of the <see cref="UtcTodayProvider"/> class.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static UtcTodayProvider Instance { get; } = new();

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
