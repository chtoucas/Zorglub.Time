// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology
{
    // TODO(api): remplacer long par CivilTime dans Now().
    // Un CivilTime devra dépendre d'une échelle de temps.
    // Generic?
    //
    // Réalisation complémentaire: NtpTimepiece.
    // https://github.com/mattjohnsonpint/NodaTime.NetworkClock
    //
    // Leap Seconds.
    // The property Ticks "does not include the number of ticks that are
    // attributable to leap seconds".
    // https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-6.0
    //
    // UTC est basé sur le TAI, les secondes intercalaires permettent de garder
    // le décalage avec UT1 en dessous de 0,9 secondes SI. En ce moment,
    // TAI = UTC + 10 secondes initiales + 27 secondes intercalaires.
    // En .NET, DateTime.UtcNow correspond réellement au nombre de tics (dans
    // l'échelle) sans compter les secondes intercalaires et (je crois) initiales.
    //
    // https://stackoverflow.com/questions/30984599/how-does-the-oracle-java-jvm-know-a-leap-second-is-occurring
    // https://support.ntp.org/bin/view/Support/TimeScales
    // https://stackoverflow.com/questions/55964042/with-the-win-10-oct-2018-update-windows-is-leap-second-aware-is-nets-datetim
    //   -> https://github.com/dotnet/dotnet-api-docs/issues/966#issuecomment-434440807
    // https://docs.microsoft.com/en-us/dotnet/api/system.datetime.utcnow?view=net-6.0
    // Time4J claims to support leap seconds.
    // More interesting see
    // https://www.threeten.org/threeten-extra/apidocs/org.threeten.extra/org/threeten/extra/scale/package-summary.html
    // UTC-SLS, UTC with Smoothed Leap Seconds
    // https://www.cl.cam.ac.uk/~mgk25/time/utc-sls/
    // https://metacpan.org/pod/Time::UTC_SLS
    // "Workarounds for leap second problems" in https://en.wikipedia.org/wiki/Leap_second
    // PHK
    // http://phk.freebsd.dk/time/
    // http://phk.freebsd.dk/time/20151122/

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
