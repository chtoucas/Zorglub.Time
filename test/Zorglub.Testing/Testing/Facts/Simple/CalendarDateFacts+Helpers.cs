// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

public abstract class CalendarDateAdjusterFacts<TDataSet> :
    IDateAdjusterFacts<CalendarDateAdjustersAdapter, CalendarDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateAdjusterFacts(SimpleCalendar calendar)
        : base(new CalendarDateAdjustersAdapter(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;
    }

    protected SimpleCalendar Calendar { get; }

    protected sealed override CalendarDate GetDate(int y, int m, int d) => Calendar.GetCalendarDate(y, m, d);
    protected sealed override CalendarDate GetDate(int y, int doy) => Calendar.GetOrdinalDate(y, doy).ToCalendarDate();
}

public abstract class CalendarDateProvidersFacts<TDataSet> :
    IDateProvidersFacts<CalendarDateProviders, CalendarDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateProvidersFacts(SimpleCalendar calendar) : base(calendar) { }

    protected sealed override CalendarDate GetDate(int y, int m, int d) => Calendar.GetCalendarDate(y, m, d);
    protected sealed override CalendarDate GetDate(int y, int doy) => Calendar.GetOrdinalDate(y, doy).ToCalendarDate();
}
