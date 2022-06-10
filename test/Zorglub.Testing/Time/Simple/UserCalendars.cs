// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology;

// NB: don't remove the proleptic calendar, it's useful to check that everything
// continues to work with negative years.

/// <summary>
/// Provides user-defined calendars.
/// </summary>
public static class UserCalendars
{
    /// <summary>
    /// Gets the <i>non-proleptic</i> user-defined Gregorian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Gregorian { get; } =
        CalendarCatalog.Add("User Gregorian", new GregorianSchema(), DayZero.NewStyle, proleptic: false);

    /// <summary>
    /// Gets the <i>proleptic</i> user-defined Julian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Julian { get; } =
        CalendarCatalog.Add("User Julian", new JulianSchema(), DayZero.OldStyle, proleptic: true);

    /// <summary>
    /// Gets the <i>non-proleptic</i> user-defined Positivist calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Positivist { get; } =
        CalendarCatalog.Add("User Positivist", new PositivistSchema(), CalendarEpoch.Positivist, proleptic: false);

    /// <summary>
    /// Gets the <i>non-proleptic</i> user-defined Coptic13 calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Coptic13 { get; } =
        CalendarCatalog.Add("User Coptic13", new Coptic13Schema(), CalendarEpoch.Coptic, proleptic: false);

    public static readonly DayNumber LunisolarEpoch = DayZero.NewStyle;

    /// <summary>
    /// Gets the <i>non-proleptic</i> user-defined Lunisolar calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Lunisolar { get; } =
        CalendarCatalog.Add("User Lunisolar", new LunisolarSchema(), LunisolarEpoch, proleptic: false);
}
