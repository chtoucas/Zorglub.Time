// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDay"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)] // Indirect tests
public abstract class CalendarDayFacts<TDataSet> : IDateFacts<CalendarDay, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected CalendarDayFacts(Calendar calendar) : this(calendar, CreateCtorArgs(calendar)) { }

    private CalendarDayFacts(Calendar calendar, CtorArgs args) : base(args)
    {
        CalendarUT = calendar;

        (MinDate, MaxDate) = calendar.MinMaxDay;
    }

    protected Calendar CalendarUT { get; }

    protected sealed override CalendarDay MinDate { get; }
    protected sealed override CalendarDay MaxDate { get; }

    protected sealed override CalendarDay CreateDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
}
