// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Adjuster created to be able to re-use IDateAdjusterFacts.
public sealed class CalendarDateAdjuster : IDateAdjuster<CalendarDate>
{
    public CalendarDateAdjuster(SimpleCalendar calendar)
    {
        Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    public SimpleCalendar Calendar { get; }
    public CalendarScope Scope => Calendar.Scope;

    private void ValidateCuid(Cuid cuid)
    {
        if (cuid != Calendar.Id) ThrowHelpers.BadCuid("date", Calendar.Id, cuid);
    }

    public CalendarDate AdjustYear(CalendarDate date, int newYear)
    {
        ValidateCuid(date.Cuid);
        return date.WithYear(newYear);
    }

    public CalendarDate AdjustMonth(CalendarDate date, int newMonth)
    {
        ValidateCuid(date.Cuid);
        return date.WithMonth(newMonth);
    }

    public CalendarDate AdjustDay(CalendarDate date, int newDay)
    {
        ValidateCuid(date.Cuid);
        return date.WithDay(newDay);
    }

    public CalendarDate AdjustDayOfYear(CalendarDate date, int newDayOfYear)
    {
        ValidateCuid(date.Cuid);
        return CalendarDateAdjusters.WithDayOfYear(date, newDayOfYear);
    }

    public CalendarDate GetStartOfYear(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDateAdjusters.GetStartOfYear(date);
    }

    public CalendarDate GetEndOfYear(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDateAdjusters.GetEndOfYear(date);
    }

    public CalendarDate GetStartOfMonth(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDateAdjusters.GetStartOfMonth(date);
    }

    public CalendarDate GetEndOfMonth(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return CalendarDateAdjusters.GetEndOfMonth(date);
    }
}
