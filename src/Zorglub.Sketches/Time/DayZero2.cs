// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time
{
    using Zorglub.Time.Core.Schemas;

    /// <summary>
    /// Provides static methods to obtain common calendar epochs.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class DayZero2
    {
        #region "Disabled"
        // Props désactivées pour éviter tout risque de confusion.
        // Julian et DayZero.OldStyle sont identiques et cela pourrait perturber
        // certaines personnes (laquelle choisir ?). Idem avec Gregorian et
        // DayZero.NewStyle.

        /// <summary>
        /// Gets the epoch of the Julian calendar.
        /// <para>The 1st of January, 1 CE within the Julian calendar.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Julian { get; } = DayZero.OldStyle;

        /// <summary>
        /// Gets the epoch of the Gregorian calendar.
        /// <para>Monday 1st of January, 1 CE within the Gregorian calendar.</para>
        /// <para>This property matches also the epoch of the Common Era, aka
        /// Current Era or Vulgar Era.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Gregorian { get; } = DayZero.NewStyle;

        #endregion

        /// <summary>
        /// Gets the epoch of the Holocene calendar.
        /// <para>The 1st of January, 10,000 BC within the Gregorian calendar.</para>
        /// <para>This property matches also the epoch of the Holocene Era (HE)
        /// and the one of the Jōmon Era (JE).</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        // This is NOT the start of the Holocene geological era.
        public static DayNumber Holocene => FromGregorian(Ord.First - 10_000, 1, 1);

        /// <summary>
        /// Gets the epoch of the "Tropicália" calendar.
        /// <para>The 1st of January, 1968 CE within the Gregorian calendar.</para>
        /// <para>Let's call this the start of the Tropicalismo Era (TE)</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        // 1968 = release year of "Tropicália ou Panis et Circencis" :-)
        // 1/1/1968 is a Monday for both Gregorian and Tropicália calendars.
        public static DayNumber Tropicalia => FromGregorian(Ord.Zeroth + 1968, 1, 1);

        /// <summary>
        /// Gets the epoch of the Minguo calendar, aka the Republic of China calendar.
        /// <para>The 1st of January, 1912 CE within the Gregorian calendar.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static DayNumber Minguo => FromGregorian(Ord.Zeroth + 1912, 1, 1);

        [Pure]
        private static DayNumber FromGregorian(Ord yearOfEra, int m, int d) =>
            DayZero.NewStyle
            + GregorianFormulae.CountDaysSinceEpoch((int)yearOfEra, m, d);
    }
}
