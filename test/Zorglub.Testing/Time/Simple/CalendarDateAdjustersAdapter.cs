// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Adapter created to be able to re-use IDateAdjusterFacts.
public sealed class CalendarDateAdjustersAdapter : IDateAdjuster<CalendarDate>
{
    public CalendarDateAdjustersAdapter(SimpleCalendar calendar)
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
        return date.WithDayOfYear(newDayOfYear);
    }

    public CalendarDate GetStartOfYear(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetStartOfYear();
    }

    public CalendarDate GetEndOfYear(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetEndOfYear();
    }

    public CalendarDate GetStartOfMonth(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetStartOfMonth();
    }

    public CalendarDate GetEndOfMonth(CalendarDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetEndOfMonth();
    }
}
