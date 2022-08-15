// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

public abstract class CalendarDateAdjustmentFacts<TDataSet> :
    IDateAdjusterFacts<CalendarDateAdjuster, CalendarDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateAdjustmentFacts(SimpleCalendar calendar)
        : base(new CalendarDateAdjuster(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;
    }

    protected SimpleCalendar Calendar { get; }

    protected sealed override CalendarDate GetDate(int y, int m, int d) => Calendar.GetCalendarDate(y, m, d);
    protected sealed override CalendarDate GetDate(int y, int doy) => Calendar.GetOrdinalDate(y, doy).ToCalendarDate();
}
