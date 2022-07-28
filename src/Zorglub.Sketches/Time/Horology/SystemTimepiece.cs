// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    using Zorglub.Time.Specialized;

    /// <summary>
    /// Represents the system clock.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class SystemTimepiece : ITimepiece<CivilPrototype>
    {
        /// <inheritdoc />
        public long Now() =>
            // Correspond réellement au nombre de tics sans compter les secondes
            // intercalaires, donc dans l'échelle atomique internationale.
            100 * DateTime.UtcNow.Ticks;

        /// <inheritdoc />
        public CivilPrototype Today()
        {
            var time = DateTime.UtcNow;
            return new CivilPrototype(time.Year, time.Month, time.Day);
        }
    }
}
