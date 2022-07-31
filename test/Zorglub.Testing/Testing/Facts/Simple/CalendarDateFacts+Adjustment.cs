// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

public abstract partial class CalendarDateAdjustmentFacts<TDataSet> :
    IAdjustableDateFacts<CalendarDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateAdjustmentFacts(SimpleCalendar calendar)
        : base(calendar?.YearsValidator.Range ?? throw new ArgumentNullException(nameof(calendar)))
    {
        Debug.Assert(calendar != null);

        CalendarUT = calendar;
    }

    protected SimpleCalendar CalendarUT { get; }

    protected override CalendarDate GetDate(int y, int m, int d) => CalendarUT.GetCalendarDate(y, m, d);
}
