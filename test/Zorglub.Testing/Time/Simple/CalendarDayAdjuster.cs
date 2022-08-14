// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Adjuster created to be able to re-use IDateAdjusterFacts.
public sealed class CalendarDayAdjuster : IDateAdjuster<CalendarDay>
{
    public CalendarDayAdjuster(SimpleCalendar calendar)
    {
        Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    public SimpleCalendar Calendar { get; }
    public CalendarScope Scope => Calendar.Scope;

    private void ValidateCuid(Cuid cuid)
    {
        if (cuid != Calendar.Id) ThrowHelpers.BadCuid("date", Calendar.Id, cuid);
    }

    public CalendarDay AdjustYear(CalendarDay date, int newYear)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.WithYear(date, newYear);
    }

    public CalendarDay AdjustMonth(CalendarDay date, int newMonth)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.WithMonth(date, newMonth);
    }

    public CalendarDay AdjustDay(CalendarDay date, int newDay)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.WithDay(date, newDay);
    }

    public CalendarDay AdjustDayOfYear(CalendarDay date, int newDayOfYear)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.WithDayOfYear(date, newDayOfYear);
    }

    public CalendarDay GetStartOfYear(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.GetStartOfYear(date);
    }

    public CalendarDay GetEndOfYear(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.GetEndOfYear(date);
    }

    public CalendarDay GetStartOfMonth(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.GetStartOfMonth(date);
    }

    public CalendarDay GetEndOfMonth(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDayAdjusters.GetEndOfMonth(date);
    }
}
