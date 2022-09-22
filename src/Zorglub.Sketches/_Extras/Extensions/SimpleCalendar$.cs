// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions;

using Zorglub.Time.Simple;

// REVIEW(code): use DayNumber.Utc/Today().

public static class SimpleCalendarExtensions
{
    [Pure]
    public static CalendarDay Today(this SimpleCalendar calendar)
    {
        Requires.NotNull(calendar);
        return calendar.LocalClock.GetCurrentDay();
    }

    [Pure]
    public static CalendarDay UtcToday(this SimpleCalendar calendar)
    {
        Requires.NotNull(calendar);
        return calendar.UtcClock.GetCurrentDay();
    }
}
