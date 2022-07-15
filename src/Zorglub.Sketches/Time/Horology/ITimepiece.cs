// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // TODO: retourner un type temps UtcTime ou AtomicTime ? ou Y-M-D H-M-S.sss
    // Réalisation complémentaire : NTP.

    /// <summary>
    /// Represents an instrument for measuring time.
    /// </summary>
    public interface ITimepiece<out TDate>
    {
        /// <summary>
        /// Obtains the current time expressed as a number of nanoseconds since
        /// the january 1st, 1 CE (gregorian) at midnight (0h).
        /// </summary>
        public long Now();

        /// <summary>
        /// Obtains the current date.
        /// </summary>
        public TDate Today();
    }
}
