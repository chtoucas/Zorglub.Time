// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // TODO(api): remplacer long par CivilTime dans Now().
    // Un CivilTime devra dépendre d'une échelle de temps.
    //
    // Réalisation complémentaire: NtpTimepiece.
    // https://github.com/mattjohnsonpint/NodaTime.NetworkClock
    //
    // Leap Seconds.
    // The property Ticks "does not include the number of ticks that are
    // attributable to leap seconds".
    // https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-6.0
    //
    // DateTime.UtcNow correspond réellement au nombre de tics sans
    // compter les secondes intercalaires, donc dans l'échelle atomique
    // internationale à condition d'ignore le delta initial de 10 secondes.
    // TAI = UTC + 10 secondes + 27 secondes intercalaires
    // UTC est basé sur le TAI et les secondes intercalaire permettent de garder
    // le décalage avec UT1 à moins de 0,9 secondes SI.
    //
    // https://support.ntp.org/bin/view/Support/TimeScales
    // https://stackoverflow.com/questions/55964042/with-the-win-10-oct-2018-update-windows-is-leap-second-aware-is-nets-datetim
    //   -> https://github.com/dotnet/dotnet-api-docs/issues/966#issuecomment-434440807
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
