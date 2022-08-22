// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Core;

    /// <summary>
    /// Represents the system clock.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SystemUtcTimepiece : ITimepiece
    {
        private SystemUtcTimepiece() { }

        /// <summary>
        /// Gets a singleton instance of the <see cref="SystemUtcTimepiece"/> class.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static SystemUtcTimepiece Instance { get; } = new();

        /// <inheritdoc />
        public long Now() =>
            // Correspond réellement au nombre de tics sans compter les secondes
            // intercalaires, donc dans l'échelle atomique internationale.
            100 * DateTime.UtcNow.Ticks;

        /// <inheritdoc />
        public DayNumber Today()
        {
            // NB: the cast should always succeed.
            int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.UtcNow.Ticks);
            return new DayNumber(daysSinceZero);
        }
    }
}
