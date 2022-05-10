// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Core.Schemas;

    // https://en.wikipedia.org/wiki/Calendar_era

    /// <summary>
    /// Provides static properties to obtain common calendar epochs.
    /// <para>See also <seealso cref="DayZero"/>.</para>
    /// </summary>
    public static class CalendarEpoch
    {
        /// <summary>
        /// Gets the epoch of the Egyptian calendar.
        /// <para>The 26th of February, 747 BC within the Julian calendar.</para>
        /// <para>This property matches also the epoch of the Era of Nabonassar.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Egyptian => FromJulian(Ord.First - 747, 2, 26);

        /// <summary>
        /// Gets the day before the Gregorian epoch.
        /// <para>Sunday 31th of December, 1 BC within the Gregorian calendar.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        /// <remarks>
        /// We use this epoch for the Pax and the World calendars.
        /// <list type="bullet">
        /// <item>The Pax epoch must be a Sunday (first day of a Pax week).</item>
        /// <item>As part of its definition, within the World calendar, a year must start on a
        /// Sunday.</item>
        /// </list>
        /// </remarks>
        public static DayNumber SundayBeforeGregorian { get; } = DayZero.NewStyle - 1;

        /// <summary>
        /// Gets the epoch of the Ethiopic calendar.
        /// <para>The 29th of August, 8 CE within the Julian calendar.</para>
        /// <para>This property matches also the epoch of the Era of the Incarnation of Jesus.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Ethiopic => FromJulian(Ord.Zeroth + 8, 8, 29);

        /// <summary>
        /// Gets the epoch of the Coptic calendar.
        /// <para>The 29th of August, 284 CE within the Julian calendar.</para>
        /// <para>This property matches also the epoch of the Era of the Martyrs (Anno Martyrum),
        /// aka the Diocletian Era (Anno Diocletiani).</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Coptic => FromJulian(Ord.Zeroth + 284, 8, 29);

        /// <summary>
        /// Gets the epoch of the Armenian calendar.
        /// <para>The 11th of July, 552 CE within the Julian calendar.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Armenian => FromJulian(Ord.Zeroth + 552, 7, 11);

        /// <summary>
        /// Gets the epoch of the Persian calendar.
        /// <para>The 19th of March, 622 CE within the Julian calendar.</para>
        /// <para>This property is also called Anno Persico or Anno Persarum.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Persian => FromJulian(Ord.Zeroth + 622, 3, 19);

        /// <summary>
        /// Gets the epoch of the Tabular Islamic calendar.
        /// <para>The 16th of July, 622 CE within the Julian calendar.</para>
        /// <para>This property matches also the epoch of the Hijri Era.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber TabularIslamic => FromJulian(Ord.Zeroth + 622, 7, 16);

        /// <summary>
        /// Gets the epoch of the Zoroastrian calendar.
        /// <para>The 26th of June, 632 CE within the Julian calendar.</para>
        /// <para>This property matches also the epoch of the Yazdegerd Era.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Zoroastrian => FromJulian(Ord.Zeroth + 632, 6, 16);

        /// <summary>
        /// Gets the epoch of the Positivist calendar.
        /// <para>The 1st of January, 1789 CE within the Gregorian calendar.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Positivist => FromGregorian(Ord.Zeroth + 1789, 1, 1);

        /// <summary>
        /// Gets the epoch of the French Republican calendar.
        /// <para>The 9th of September, 1792 CE within the Gregorian calendar.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        // Ère de la République française ou Ère des Français.
        // La terminologie semble fluctuante [Parisot et Suagher] :
        // - ère de la Liberté, après le 14 juillet 1789 ;
        // - ère de de l'Égalité, après le 10 août 1792 ;
        // - ère de la République française, après le 22 septembre 1792.
        // Calendrier révolutionnaire ou calendrier républicain français.
        public static DayNumber FrenchRepublican => FromGregorian(Ord.Zeroth + 1792, 9, 22);

        /// <summary>
        /// Creates an epoch number from the specified (signed) year of the era, month and day
        /// within the Gregorian calendar.
        /// </summary>
        [Pure]
        private static DayNumber FromGregorian(Ord yearOfEra, int m, int d) =>
            DayZero.NewStyle + GregorianFormulae.CountDaysSinceEpoch((int)yearOfEra, m, d);

        /// <summary>
        /// Creates an epoch number from the specified (signed) year of the era, month and day
        /// within the Julian calendar.
        /// </summary>
        [Pure]
        private static DayNumber FromJulian(Ord yearOfEra, int m, int d) =>
            DayZero.OldStyle + (int)JulianFormulae.CountDaysSinceEpoch((int)yearOfEra, m, d);
    }
}
