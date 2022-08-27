// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Horology.Astronomy
{
    using Zorglub.Time.Core;
    using Zorglub.Time.Core.Schemas;

    using static Zorglub.Time.Core.TemporalConstants;

    // TODO: À nettement améliorer (struct, layout, ranges & co).

    /// <summary>
    /// Represents a time retrieved from a clock.
    /// </summary>
    public partial class ClockTime0
    {
        /// <summary>
        /// Represents the smallest possible value of a year.
        /// <para>This field is constant.</para>
        /// </summary>
        // SOFA utilise -4799. Pourquoi cette valeur min ?
        // Origine de JD :
        // - à 12h le 1er janvier 4713 av. J.-C. (julien) ;
        // - à 12h le 24 novembre 4714 av. J.-C. (grégorien).
        // Pour le moment, on ne s'embête pas, on se limite aux dates après
        // minuit (0h) le 1er janvier 4713 av. J.-C. (grégorien).
        // Date/heure minimale = 1er janvier -4712 à 0h autrement dit JD = 37,5
        // et MJD = -2.399.963. Si on change cette valeur, màj SplitJD et QuasiJD.
        internal const int MinYear = -4712;

        /// <summary>
        /// Represents the largest possible value of a year.
        /// <para>This field is constant.</para>
        /// </summary>
        // Pour le moment, on va se restreindre à 9999 (comme avec les chronologies).
        // JD < 5.373.484,5 et MJD < 2.973.484, ces valeurs correspondent au
        // 1er janvier 10.000 apr. J-C. à 0h, càd le premier instant non pris
        // en charge. Si on change cette valeur, màj SplitJD et QuasiJD.
        internal const int MaxYear = 9999;

        /// <summary>
        /// Represents the year, month and day of this instance.
        /// </summary>
        private readonly Yemoda _ymd;

        /// <summary>
        /// Initializes a new instance of <see cref="ClockTime0"/> from the
        /// specified gregorian date.
        /// </summary>
        private ClockTime0(Yemoda ymd)
        {
            _ymd = ymd;
        }

        /// <summary>
        /// Gets the year, month and day from this instance.
        /// </summary>
        public Yemoda Yemoda => _ymd;

        /// <summary>
        /// Gets the algebraic year from this instance.
        /// </summary>
        public int Year => _ymd.Year;

        /// <summary>
        /// Gets the month from this instance.
        /// <para>The result is in the range from 1 to 12.</para>
        /// </summary>
        public int Month => _ymd.Month;

        /// <summary>
        /// Gets the day from this instance.
        /// <para>The result is in the range from 1 to 31.</para>
        /// </summary>
        public int Day => _ymd.Day;

        /// <summary>
        /// Gets the hour from this instance.
        /// <para>The result is in the range from 0 to 23.</para>
        /// </summary>
        public int Hour { get; }

        /// <summary>
        /// Gets the minute from this instance.
        /// <para>The result is in the range from 0 to 59.</para>
        /// </summary>
        public int Minute { get; }

        /// <summary>
        /// Gets the fractional-seconds from this instance.
        /// <para>The result is in the range from 0 (inclusive) to 61
        /// (exclusive).</para>
        /// </summary>
        public double FractionalSeconds { get; }

        /// <summary>
        /// Gets the second from this instance.
        /// <para>The result is in the range from 0 to 60.</para>
        /// </summary>
        public int Second { get; }

        /// <summary>
        /// Gets the fraction of the second from this instance.
        /// </summary>
        public int FractionOfSecond { get; }

        /// <summary>
        /// Gets the fraction of the day from this instance.
        /// </summary>
        public double FractionOfDay
        {
            get
            {
                double seconds = Hour * SecondsPerHour
                    + Minute * SecondsPerMinute
                    + FractionalSeconds;

                return seconds / SecondsPerDay;
            }
        }

        /// <summary>
        /// Deconstructs this instance into its components.
        /// </summary>
        public void Deconstruct(
            out Yemoda ymd,
            out int hour, out int minute, out double fractionalSeconds)
        {
            (ymd, hour, minute, fractionalSeconds)
                = (Yemoda, Hour, Minute, FractionalSeconds);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClockTime0"/> from the specified
        /// gregorian date.
        /// </summary>
        [Pure]
        public static ClockTime0 Create(int year, int month, int day)
        {
            ValidateGregorian(year, month, day);
            return new ClockTime0(new Yemoda(year, month, day));
        }
    }

    public partial class ClockTime0
    {
        /// <summary>
        /// Checks wether a gregorian date is well-formed and is in the supported
        /// range of years, or not.
        /// </summary>
        public static void ValidateGregorian(int year, int month, int day)
        {
            if (year < MinYear || year > MaxYear)
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month));
            }
            if (day < 1 || day > GregorianFormulae.CountDaysInMonth(year, month))
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }
        }

        /// <summary>
        /// Obtains the fraction of the day from the specified time of the day.
        /// </summary>
        [Pure]
        internal static double GetFractionOfDay(int hour, int minute, double fractionalSeconds)
        {
            if (hour < 0 || hour >= HoursPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(hour));
            }
            if (minute < 0 || minute >= MinutesPerHour)
            {
                throw new ArgumentOutOfRangeException(nameof(minute));
            }
            if (fractionalSeconds < 0 || fractionalSeconds > SecondsPerMinute)
            {
                throw new ArgumentOutOfRangeException(nameof(fractionalSeconds));
            }

            double seconds = hour * SecondsPerHour + minute * SecondsPerMinute + fractionalSeconds;

            return seconds / SecondsPerDay;
        }

        /// <remarks>Adapted from SOFA::iauD2tf.</remarks>
        [Pure]
        internal static (int hour, int minute, int second, int fractionOfSecond)
            GetTimeOfDay(double fractionOfDay, int decimalPlaces, bool isUtc)
        {
            // Attention, si on a des secondes intercalaires, |fractionOfDay|
            // peut être > 1, auquel cas on obtiendra une heure > 23.
            if (fractionOfDay < 0 || (!isUtc && fractionOfDay >= 1))
            {
                throw new ArgumentOutOfRangeException(nameof(fractionOfDay));
            }

            double totalSeconds = SecondsPerDay * fractionOfDay;

            // Pre-round if resolution coarser than 1s (then pretend ndp=1).
            if (decimalPlaces < 0)
            {
                int pow = 1;
                for (int i = 1; i <= -decimalPlaces; i++)
                {
                    pow *= (i == 2 || i == 4) ? 6 : 10;
                }
                totalSeconds = pow * MathOperations.RoundAwayFromZero(totalSeconds / pow);
            }

            // Express the unit of each field in resolution units.
            int unitsPerSecond = 1;
            for (int i = 1; i <= decimalPlaces; i++)
            {
                unitsPerSecond *= 10;
            }

            int unitsPerMinute = unitsPerSecond * SecondsPerMinute;
            int unitsPerHour = unitsPerMinute * MinutesPerHour;

            // Round the interval and express in resolution units.
            double fos = MathOperations.RoundAwayFromZero(totalSeconds * unitsPerSecond);

            // Heure.
            double hh = Math.Truncate(fos / unitsPerHour);
            fos -= unitsPerHour * hh;
            // Minute.
            double mm = Math.Truncate(fos / unitsPerMinute);
            fos -= unitsPerMinute * mm;
            // Seconde.
            double ss = Math.Truncate(fos / unitsPerSecond);
            fos -= unitsPerSecond * ss;

            // Résultat.
            return ((int)hh, (int)mm, (int)ss, (int)fos);
        }
    }
}
