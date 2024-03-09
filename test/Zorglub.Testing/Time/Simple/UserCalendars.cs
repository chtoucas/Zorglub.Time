// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

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
    public static SimpleCalendar Gregorian { get; } =
        SimpleCatalog.Add("User Gregorian", new GregorianSchema(), DayZero.NewStyle, proleptic: false);

    /// <summary>
    /// Gets the <i>proleptic</i> user-defined Julian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static SimpleCalendar Julian { get; } =
        SimpleCatalog.Add("User Julian", new JulianSchema(), DayZero.OldStyle, proleptic: true);

    /// <summary>
    /// Gets the <i>non-proleptic</i> user-defined Positivist calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static SimpleCalendar Positivist { get; } =
        SimpleCatalog.Add("User Positivist", new PositivistSchema(), DayZero.Positivist, proleptic: false);

    /// <summary>
    /// Gets the <i>non-proleptic</i> user-defined Coptic13 calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static SimpleCalendar Coptic13 { get; } =
        SimpleCatalog.Add("User Coptic13", new Coptic13Schema(), DayZero.Coptic, proleptic: false);

    public static readonly DayNumber LunisolarEpoch = DayZero.NewStyle;

    /// <summary>
    /// Gets the <i>non-proleptic</i> user-defined Lunisolar calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static SimpleCalendar Lunisolar { get; } =
        SimpleCatalog.Add("User Lunisolar", new LunisolarSchema(), LunisolarEpoch, proleptic: false);
}
