// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Extensions;

using Zorglub.Time.Hemerology;

// REVIEW(code): use DayNumber.Utc/Today().

public static class ZCalendarExtensions
{
    [Pure]
    public static ZDate Today(this ZCalendar calendar)
    {
        Requires.NotNull(calendar);
        return calendar.LocalClock.GetCurrentDate();
    }

    [Pure]
    public static ZDate UtcToday(this ZCalendar calendar)
    {
        Requires.NotNull(calendar);
        return calendar.UtcClock.GetCurrentDate();
    }
}
