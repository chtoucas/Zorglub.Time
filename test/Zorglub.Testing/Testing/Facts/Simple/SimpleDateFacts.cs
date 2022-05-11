// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

// REVIEW(fact): ISimpleDate is internal, create and use a SimpleDateProxy?
// TODO(fact): in derived classes, use (1, 1, 1) or (1, 1) instead of theories
// since we know that the first years are always complete.

/// <summary>
/// Provides facts about <see cref="CalendarDate"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)]
public abstract class SimpleDateFacts<TDate, TDataSet> : IDateFacts<TDate, TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IAdvancedMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected SimpleDateFacts(Calendar calendar, Calendar otherCalendar)
        : this(calendar, otherCalendar, CreateCtorArgs(calendar)) { }

    private SimpleDateFacts(Calendar calendar, Calendar otherCalendar!!, CtorArgs args) : base(args)
    {
        if (otherCalendar == calendar)
        {
            throw new ArgumentException(
                "\"otherCalendar\" should not be equal to \"calendar\"", nameof(otherCalendar));
        }

        CalendarUT = calendar;
        OtherCalendar = otherCalendar;
    }

    protected Calendar CalendarUT { get; }
    protected Calendar OtherCalendar { get; }

    public static TheoryData<Yemoda, Yemoda, int> AddYearsData => DataSet.AddYearsData;
    public static TheoryData<Yemoda, Yemoda, int> AddMonthsData => DataSet.AddMonthsData;
}
