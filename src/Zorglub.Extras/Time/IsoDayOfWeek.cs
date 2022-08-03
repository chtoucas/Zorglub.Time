﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    // REVIEW(api): avoir une valeur par défaut illégale ne me plaît pas des masses.

    // Nom: IsoDayOfWeek ou IsoWeekday ? En fait, j'imagine un autre usage à
    // Weekday (CalendarWeekday).

    /// <summary>
    /// Specifies the ISO day of the week.
    /// <para>The value is in the range from 1 to 7, 1 being attributed to
    /// Monday.</para>
    /// </summary>
    public enum IsoDayOfWeek
    {
        /// <summary>
        /// Indicates an unknown ISO day of the week.
        /// <para>We never use this value, and neither should you.</para>
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates Monday.
        /// </summary>
        Monday = 1,

        /// <summary>
        /// Indicates Tuesday.
        /// </summary>
        Tuesday = 2,

        /// <summary>
        /// Indicates Wednesday.
        /// </summary>
        Wednesday = 3,

        /// <summary>
        /// Indicates Thursday.
        /// </summary>
        Thursday = 4,

        /// <summary>
        /// Indicates Friday.
        /// </summary>
        Friday = 5,

        /// <summary>
        /// Indicates Saturday.
        /// </summary>
        Saturday = 6,

        /// <summary>
        /// Indicates Sunday.
        /// </summary>
        Sunday = 7
    }
}
