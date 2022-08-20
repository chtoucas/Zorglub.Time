// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Provides a method to obtain the current date on this machine, expressed in the local time.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class LocalTodayProvider : ITodayProvider
    {
        private LocalTodayProvider() { }

        /// <summary>
        /// Gets a singleton instance of the <see cref="LocalTodayProvider"/> class.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static LocalTodayProvider Instance { get; } = new();

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
