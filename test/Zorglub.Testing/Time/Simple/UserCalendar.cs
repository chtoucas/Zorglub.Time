// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public static class UserCalendar
{
    public const string GregorianKey = "User Gregorian";

    public static Calendar Gregorian { get; } =
        CalendarCatalog.Add(GregorianKey, new GregorianSchema(), DayZero.NewStyle, true);
}
