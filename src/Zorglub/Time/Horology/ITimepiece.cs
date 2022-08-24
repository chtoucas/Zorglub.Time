// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // TODO(api): retourner un type temps UtcTime ou AtomicTime ? ou Y-M-D H-M-S.sss
    // Réalisation complémentaire: NtpTimepiece.
    // Remplace long par ??? dans Now() with UTC Offset(?).
    // Prop Timescale.
    // https://github.com/mattjohnsonpint/NodaTime.NetworkClock
    //
    // Leap Seconds.
    // The property Ticks "does not include the number of ticks that are
    // attributable to leap seconds".
    // https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-6.0
    //
    // DateTime.UtcNow correspond réellement au nombre de tics sans
    // compter les secondes intercalaires, donc dans l'échelle atomique
    // internationale.
    //
    // https://stackoverflow.com/questions/55964042/with-the-win-10-oct-2018-update-windows-is-leap-second-aware-is-nets-datetim
    // https://docs.microsoft.com/en-us/dotnet/api/system.datetime.utcnow?view=net-6.0

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
