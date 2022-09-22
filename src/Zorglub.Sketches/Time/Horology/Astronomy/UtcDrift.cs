// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// Adapted from SOFA source code.

namespace Zorglub.Time.Horology.Astronomy
{
    using Zorglub.Time.Core;

    // Dérive de UTC par rapport à TAI.
    // Voir UTC-TAI.history dans le répertoire "data".
    internal static class UtcDrift
    {
        // MJD - drift rate (s/day), pre leap seconds.
        private static readonly (double mjd, double rate)[] s_PreLeapSecondsDriftRate
            = new (double, double)[14]
        {
            // 14 premières entrées du tableau s_LeapSeconds.
            // MJD = 1er janvier 1961.
            ( 37300, 0.0012960 ),
            ( 37300, 0.0012960 ),
            ( 37300, 0.0012960 ),
            // MJD = 1er janvier 1962.
            ( 37665, 0.0011232 ),
            ( 37665, 0.0011232 ),
            // MJD = 1er janvier 1965.
            ( 38761, 0.0012960 ),
            ( 38761, 0.0012960 ),
            ( 38761, 0.0012960 ),
            ( 38761, 0.0012960 ),
            ( 38761, 0.0012960 ),
            ( 38761, 0.0012960 ),
            ( 38761, 0.0012960 ),
            // MJD = 1er janvier 1966.
            ( 39126, 0.0025920 ),
            ( 39126, 0.0025920 ),
        };

        // Première année prise en compte par l'UTC.
        private const int FirstYear = 1960;

        // Année de la dernière de mise à jour.
        [SuppressMessage("Performance", "CA1802:Use literals where appropriate")]
        private static readonly int LastYear = 2019;

        // The official leap seconds annoucements are the responsability of the
        // IERS.
        // The list below is taken from the SOFA source code.
        // A more readable source is provided by the IETF
        // https://www.ietf.org/timezones/data/leap-seconds.list
        // Another reference for historical data can be found at:
        // https://www.iers.org/IERS/EN/Science/Recommendations/resolutionB1.html
        // (see a verbatim in JulianDate.cs)
        private static readonly (int year, int month, double leap)[] s_LeapSeconds
            = new (int, int, double)[42]
        {
            ( 1960,  1,  1.4178180 ),
            ( 1961,  1,  1.4228180 ),
            ( 1961,  8,  1.3728180 ),
            ( 1962,  1,  1.8458580 ),
            ( 1963, 11,  1.9458580 ),
            ( 1964,  1,  3.2401300 ),
            ( 1964,  4,  3.3401300 ),
            ( 1964,  9,  3.4401300 ),
            ( 1965,  1,  3.5401300 ),
            ( 1965,  3,  3.6401300 ),
            ( 1965,  7,  3.7401300 ),
            ( 1965,  9,  3.8401300 ),
            ( 1966,  1,  4.3131700 ),
            ( 1968,  2,  4.2131700 ),
            // Leap seconds.
            ( 1972,  1, 10         ),
            ( 1972,  7, 11         ),
            ( 1973,  1, 12         ),
            ( 1974,  1, 13         ),
            ( 1975,  1, 14         ),
            ( 1976,  1, 15         ),
            ( 1977,  1, 16         ),
            ( 1978,  1, 17         ),
            ( 1979,  1, 18         ),
            ( 1980,  1, 19         ),
            ( 1981,  7, 20         ),
            ( 1982,  7, 21         ),
            ( 1983,  7, 22         ),
            ( 1985,  7, 23         ),
            ( 1988,  1, 24         ),
            ( 1990,  1, 25         ),
            ( 1991,  1, 26         ),
            ( 1992,  7, 27         ),
            ( 1993,  7, 28         ),
            ( 1994,  7, 29         ),
            ( 1996,  1, 30         ),
            ( 1997,  7, 31         ),
            ( 1999,  1, 32         ),
            ( 2006,  1, 33         ),
            ( 2009,  1, 34         ),
            ( 2012,  7, 35         ),
            ( 2015,  7, 36         ),
            ( 2017,  1, 37         )
        };

        /// <summary>
        /// Obtains the difference between TAI and UTC, ΔAT = TAI-UTC in seconds, available from
        /// IERS Bulletins.
        /// <para>ΔAT is simply the total count of leap seconds in UTC.</para>
        /// </summary>
        /// <remarks>Adapted from SOFA::iauDat.</remarks>
        [Pure]
        public static double GetDeltaAT(Yemoda ymd, double fractionOfDay)
        {
            var (year, month, day) = ymd;
            if (year < FirstYear || year > LastYear + 5)
            {
                // Précède l'introduction de l'UTC ou 5 années après la dernière
                // mise à jour des secondes intercalaires.
                Throw.ArgumentOutOfRange(nameof(ymd));
            }
            ClockTime0.ValidateGregorian(year, month, day);
            if (fractionOfDay < 0 || fractionOfDay >= 1)
            {
                Throw.ArgumentOutOfRange(nameof(fractionOfDay));
            }

            int ym = 12 * year + month;
            int idx = s_LeapSeconds.Length - 1;
            while (idx >= 0)
            {
                var (y, m, _) = s_LeapSeconds[idx];
                if (ym >= 12 * y + m)
                {
                    break;
                }
                idx--;
            }
            if (idx < 0)
            {
                throw new InvalidOperationException();
            }
            double drift = s_LeapSeconds[idx].leap;

            // Correction pour les années 1960 à 1971.
            if (idx < s_PreLeapSecondsDriftRate.Length)
            {
                double mjd = ModifiedJulianDate.FromGregorianTime(ymd, fractionOfDay);

                var (mjd0, rate) = s_PreLeapSecondsDriftRate[idx];
                drift += (mjd - mjd0) * rate;
            }

            return drift;
        }

        [Pure]
        public static double Compute(Yemoda ymd, Yemoda tomorrow)
        {
            // TAI-UTC at 0h today.
            double deltaAt0h = GetDeltaAT(ymd, 0);
            // TAI-UTC at 12h today (to detect drift).
            double deltaAt12h = GetDeltaAT(ymd, .5);
            // TAI-UTC at 0h tomorrow (to detect jumps).
            double deltaTomorrowAt0h = GetDeltaAT(tomorrow, 0);
            // Any sudden change in TAI-UTC (seconds) between today and tomorrow.
            return deltaTomorrowAt0h - (2 * deltaAt12h - deltaAt0h);
        }
    }
}
