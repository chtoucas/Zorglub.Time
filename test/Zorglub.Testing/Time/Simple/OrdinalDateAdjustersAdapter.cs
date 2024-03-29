﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Adapter created to be able to re-use IDateAdjusterFacts.
public sealed class OrdinalDateAdjustersAdapter : IDateAdjuster<OrdinalDate>
{
    public OrdinalDateAdjustersAdapter(SimpleCalendar calendar)
    {
        Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    public SimpleCalendar Calendar { get; }
    public CalendarScope Scope => Calendar.Scope;

    private void ValidateCuid(Cuid cuid)
    {
        if (cuid != Calendar.Id) ThrowHelpers.BadCuid("date", Calendar.Id, cuid);
    }

    public OrdinalDate AdjustYear(OrdinalDate date, int newYear)
    {
        ValidateCuid(date.Cuid);
        return date.WithYear(newYear);
    }

    public OrdinalDate AdjustMonth(OrdinalDate date, int newMonth)
    {
        ValidateCuid(date.Cuid);
        return date.WithMonth(newMonth);
    }

    public OrdinalDate AdjustDay(OrdinalDate date, int newDay)
    {
        ValidateCuid(date.Cuid);
        return date.WithDay(newDay);
    }

    public OrdinalDate AdjustDayOfYear(OrdinalDate date, int newDayOfYear)
    {
        ValidateCuid(date.Cuid);
        return date.WithDayOfYear(newDayOfYear);
    }

    public OrdinalDate GetStartOfYear(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetStartOfYear();
    }

    public OrdinalDate GetEndOfYear(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetEndOfYear();
    }

    public OrdinalDate GetStartOfMonth(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetStartOfMonth();
    }

    public OrdinalDate GetEndOfMonth(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return date.GetEndOfMonth();
    }
}
