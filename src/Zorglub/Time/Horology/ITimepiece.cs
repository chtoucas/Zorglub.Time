// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // TODO(api): retourner un type temps UtcTime ou AtomicTime ? ou Y-M-D H-M-S.sss
    // Réalisation complémentaire: NtpTimepiece.
    // Remplace long par ??? dans Now().
    // Prop Timescale.

    /// <summary>
    /// Represents an instrument for measuring time.
    /// </summary>
    public interface ITimepiece
    {
        /// <summary>
        /// Obtains the current time expressed as a number of nanoseconds since the january 1st,
        /// 1 CE (gregorian) at midnight (0h).
        /// </summary>
        [Pure] long Now();

        /// <summary>
        /// Obtains a <see cref="DayNumber"/> value representing the current date.
        /// </summary>
        [Pure] DayNumber Today();
    }
}
