// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

public abstract class CalendarDayAdjusterFacts<TDataSet> :
    IDateAdjusterFacts<CalendarDayAdjustersAdapter, CalendarDay, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDayAdjusterFacts(SimpleCalendar calendar)
        : base(new CalendarDayAdjustersAdapter(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;
    }

    protected SimpleCalendar Calendar { get; }

    protected sealed override CalendarDay GetDate(int y, int m, int d) => Calendar.GetDate(y, m, d).ToCalendarDay();
    protected sealed override CalendarDay GetDate(int y, int doy) => Calendar.GetDate(y, doy).ToCalendarDay();
}

public abstract class CalendarDayProvidersFacts<TDataSet> :
    IDateProvidersFacts<CalendarDayProviders, CalendarDay, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDayProvidersFacts(SimpleCalendar calendar) : base(calendar) { }

    protected sealed override CalendarDay GetDate(int y, int m, int d) => Calendar.GetDate(y, m, d).ToCalendarDay();
    protected sealed override CalendarDay GetDate(int y, int doy) => Calendar.GetDate(y, doy).ToCalendarDay();
}
