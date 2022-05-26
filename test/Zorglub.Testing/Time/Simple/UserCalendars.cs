// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

/// <summary>
/// Provides user-defined calendars.
/// </summary>
public static class UserCalendars
{
    /// <summary>
    /// Gets a <i>non-proleptic</i> user-defined Gregorian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Gregorian { get; } =
        CalendarCatalog.Add("User Gregorian", new GregorianSchema(), DayZero.NewStyle, proleptic: false);

    /// <summary>
    /// Gets a <i>proleptic</i> user-defined Julian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Calendar Julian { get; } =
        CalendarCatalog.Add("User Julian", new JulianSchema(), DayZero.OldStyle, proleptic: true);
}
