// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using Zorglub.Time.Core.Schemas;

// https://en.wikipedia.org/wiki/Calendar_era

/// <summary>Defines the (origin for) different styles of numbering days and common calendar epochs.
/// <para>This class cannot be inherited.</para></summary>
public static partial class DayZero
{
    /// <summary>Creates an epoch number from the specified (signed) year of the era, month and day
    /// within the Gregorian calendar.</summary>
    [Pure]
    private static DayNumber FromGregorian(Ord yearOfEra, int m, int d) =>
        NewStyle + GregorianFormulae.CountDaysSinceEpoch((int)yearOfEra, m, d);

    /// <summary>Creates an epoch number from the specified (signed) year of the era, month and day
    /// within the Julian calendar.</summary>
    [Pure]
    private static DayNumber FromJulian(Ord yearOfEra, int m, int d) =>
        OldStyle + JulianFormulae.CountDaysSinceEpoch((int)yearOfEra, m, d);
}

public partial class DayZero //
{
    /// <summary>Gets the Monday 1st of January, 1 CE within the Gregorian calendar, i.e. the epoch
    /// of the Gregorian calendar.
    /// <para>Matches the epoch of the Common Era, Current Era or Vulgar Era.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber NewStyle { get; } = DayNumber.Zero;

    /// <summary>Gets the Saturday 1st of January, 1 CE within the Julian calendar, i.e. the epoch
    /// of the Julian calendar.
    /// <para>Two days before <see cref="NewStyle"/>, the Gregorian epoch.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber OldStyle { get; } = DayNumber.Zero - 2;

    /// <summary>Gets the day before <see cref="NewStyle"/>.
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber RataDie { get; } = DayNumber.Zero - 1;
}

public partial class DayZero // Aliases
{
    /// <summary>Gets the epoch of the Julian calendar.
    /// <para>The 1st of January, 1 CE within the Julian calendar.</para>
    /// <para>This property is an alias for <see cref="OldStyle"/>.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Julian { get; } = OldStyle;

    /// <summary>Gets the epoch of the Gregorian calendar.
    /// <para>Monday 1st of January, 1 CE within the Gregorian calendar.</para>
    /// <para>This property is an alias for <see cref="NewStyle"/>.</para>
    /// <para>This property matches also the epoch of the Common Era, aka Current Era or Vulgar Era.
    /// </para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Gregorian { get; } = NewStyle;
}

public partial class DayZero //
{
    /// <summary>Gets the epoch of the Holocene calendar.
    /// <para>The 1st of January, 10,000 BC within the Gregorian calendar.</para>
    /// <para>This property matches also the epoch of the Holocene Era (HE) and the one of the Jōmon
    /// Era (JE).</para>
    /// <para>This static property is thread-safe.</para></summary>
    // This is NOT the start of the Holocene geological era.
    public static DayNumber Holocene => FromGregorian(Ord.First - 10_000, 1, 1);

    /// <summary>Gets the epoch of the Egyptian calendar.
    /// <para>The 26th of February, 747 BC within the Julian calendar.</para>
    /// <para>This property matches also the epoch of the Era of Nabonassar.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Egyptian => FromJulian(Ord.First - 747, 2, 26);

    /// <summary>Gets the day before the Gregorian epoch.
    /// <para>Sunday 31th of December, 1 BC within the Gregorian calendar.</para>
    /// <para>This static property is thread-safe.</para>
    /// We use this epoch for the Pax and the World calendars.
    /// <list type="bullet">
    /// <item>The Pax epoch must be a Sunday (first day of a Pax week).</item>
    /// <item>As part of its definition, within the World calendar, a year must start on a
    /// Sunday.</item>
    /// </list></summary>
    public static DayNumber SundayBeforeGregorian { get; } = NewStyle - 1;

    /// <summary>Gets the epoch of the Ethiopic calendar.
    /// <para>The 29th of August, 8 CE within the Julian calendar.</para>
    /// <para>This property matches also the epoch of the Era of the Incarnation of Jesus.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Ethiopic => FromJulian(Ord.Zeroth + 8, 8, 29);

    /// <summary>Gets the epoch of the Coptic calendar.
    /// <para>The 29th of August, 284 CE within the Julian calendar.</para>
    /// <para>This property matches also the epoch of the Era of the Martyrs (Anno Martyrum), aka
    /// the Diocletian Era (Anno Diocletiani).</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Coptic => FromJulian(Ord.Zeroth + 284, 8, 29);

    /// <summary>Gets the epoch of the Armenian calendar.
    /// <para>The 11th of July, 552 CE within the Julian calendar.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Armenian => FromJulian(Ord.Zeroth + 552, 7, 11);

    /// <summary>Gets the epoch of the Persian calendar.
    /// <para>The 19th of March, 622 CE within the Julian calendar.</para>
    /// <para>This property is also called Anno Persico or Anno Persarum.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Persian => FromJulian(Ord.Zeroth + 622, 3, 19);

    /// <summary>Gets the epoch of the Tabular Islamic calendar.
    /// <para>The 16th of July, 622 CE within the Julian calendar.</para>
    /// <para>This property matches also the epoch of the Hijri Era.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber TabularIslamic => FromJulian(Ord.Zeroth + 622, 7, 16);

    /// <summary>Gets the epoch of the Zoroastrian calendar.
    /// <para>The 26th of June, 632 CE within the Julian calendar.</para>
    /// <para>This property matches also the epoch of the Yazdegerd Era.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Zoroastrian => FromJulian(Ord.Zeroth + 632, 6, 16);

    /// <summary>Gets the epoch of the Positivist calendar.
    /// <para>The 1st of January, 1789 CE within the Gregorian calendar.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Positivist => FromGregorian(Ord.Zeroth + 1789, 1, 1);

    /// <summary>Gets the epoch of the French Republican calendar.
    /// <para>The 9th of September, 1792 CE within the Gregorian calendar.</para>
    /// <para>This static property is thread-safe.</para></summary>
    // Ère de la République française ou Ère des Français.
    // La terminologie semble fluctuante [Parisot et Suagher] :
    // - ère de la Liberté, après le 14 juillet 1789 ;
    // - ère de de l'Égalité, après le 10 août 1792 ;
    // - ère de la République française, après le 22 septembre 1792.
    // Calendrier révolutionnaire ou calendrier républicain français.
    public static DayNumber FrenchRepublican => FromGregorian(Ord.Zeroth + 1792, 9, 22);

    /// <summary>Gets the epoch of the Minguo calendar, aka the Republic of China calendar.
    /// <para>The 1st of January, 1912 CE within the Gregorian calendar.</para>
    /// <para>This static property is thread-safe.</para></summary>
    public static DayNumber Minguo => FromGregorian(Ord.Zeroth + 1912, 1, 1);

    /// <summary>Gets the epoch of the "Tropicália" calendar.
    /// <para>The 1st of January, 1968 CE within the Gregorian calendar.</para>
    /// <para>Let's call this the start of the Tropicalismo Era (TE)</para>
    /// <para>This static property is thread-safe.</para></summary>
    // 1968 = release year of "Tropicália ou Panis et Circencis" :-)
    // 1/1/1968 is a Monday for both Gregorian and Tropicália calendars.
    public static DayNumber Tropicalia => FromGregorian(Ord.Zeroth + 1968, 1, 1);
}
