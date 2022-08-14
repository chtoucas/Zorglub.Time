// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Adjuster created to be able to re-use IDateAdjusterFacts.
public sealed class OrdinalDateAdjuster : IDateAdjuster<OrdinalDate>
{
    public OrdinalDateAdjuster(SimpleCalendar calendar)
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
        return OrdinalDateAdjusters.WithMonth(date, newMonth);
    }

    public OrdinalDate AdjustDay(OrdinalDate date, int newDay)
    {
        ValidateCuid(date.Cuid);
        return OrdinalDateAdjusters.WithDay(date, newDay);
    }

    public OrdinalDate AdjustDayOfYear(OrdinalDate date, int newDayOfYear)
    {
        ValidateCuid(date.Cuid);
        return date.WithDayOfYear(newDayOfYear);
    }

    public OrdinalDate GetStartOfYear(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return OrdinalDateAdjusters.GetStartOfYear(date);
    }

    public OrdinalDate GetEndOfYear(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return OrdinalDateAdjusters.GetEndOfYear(date);
    }

    public OrdinalDate GetStartOfMonth(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return OrdinalDateAdjusters.GetStartOfMonth(date);
    }

    public OrdinalDate GetEndOfMonth(OrdinalDate date)
    {
        ValidateCuid(date.Cuid);
        return OrdinalDateAdjusters.GetEndOfMonth(date);
    }
}
