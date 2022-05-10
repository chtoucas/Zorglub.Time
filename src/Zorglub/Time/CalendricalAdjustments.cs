// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    /// <summary>
    /// Specifies the kind of adjustments the calendar may perform to synchronize
    /// itself with the lunar and solar cycles.
    /// </summary>
    [Flags]
    public enum CalendricalAdjustments
    {
        /// <summary>
        /// The calendar does not employ any mechanism in order to achieve any
        /// kind of luni-solar synchronicity. We also use this value for calendars
        /// that do periodic adjustments not related to the lunar and solar cycles.
        /// </summary>
        None = 0,

        /// <summary>
        /// The calendar may add one or more intercalary days.
        /// <para>This is usually the case with solar calendars.</para>
        /// </summary>
        Days = 1 << 0,

        /// <summary>
        /// The calendar may add one or more weeks.
        /// <para>This is usually the case with perennial calendars.</para>
        /// </summary>
        Weeks = 1 << 1,

        /// <summary>
        /// The calendar may add one or more embolismic months.
        /// <para>This is usually the case with lunisolar calendars.</para>
        /// </summary>
        Months = 1 << 2,

        /// <summary>
        /// The calendar may add one or more months or days.
        /// </summary>
        // Exemple : calendrier bouddhiste.
        DaysAndMonths = Days | Months,
    }
}
