﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Adapter created to be able to re-use IDateAdjusterFacts.
public sealed class CalendarDayAdjustersAdapter : IDateAdjuster<CalendarDay>
{
    public CalendarDayAdjustersAdapter(SimpleCalendar calendar)
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
        return date.WithYear(newYear);
    }

    public CalendarDay AdjustMonth(CalendarDay date, int newMonth)
    {
        ValidateCuid(date.Cuid);
        return date.WithMonth(newMonth);
    }

    public CalendarDay AdjustDay(CalendarDay date, int newDay)
    {
        ValidateCuid(date.Cuid);
        return date.WithDay(newDay);
    }

    public CalendarDay AdjustDayOfYear(CalendarDay date, int newDayOfYear)
    {
        ValidateCuid(date.Cuid);
        return date.WithDayOfYear(newDayOfYear);
    }

    public CalendarDay GetStartOfYear(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return date.GetStartOfYear();
    }

    public CalendarDay GetEndOfYear(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return date.GetEndOfYear();
    }

    public CalendarDay GetStartOfMonth(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return date.GetStartOfMonth();
    }

    public CalendarDay GetEndOfMonth(CalendarDay date)
    {
        ValidateCuid(date.Cuid);
        return date.GetEndOfMonth();
    }
}
