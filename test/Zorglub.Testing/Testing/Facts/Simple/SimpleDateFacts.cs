// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Simple;

// REVIEW(fact): ISimpleDate is internal, create and use a SimpleDateProxy?
// In derived classes, use (1, 1, 1) or (1, 1) instead of theories since we know
// that the first years are always complete.

/// <summary>
/// Provides facts about <see cref="CalendarDate"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)]
public abstract class SimpleDateFacts<TDate, TDataSet> : MultiDateFacts<TDate, Calendar, TDataSet>
    // ISimpleDate is internal.
    where TDate : struct, IDate<TDate>, ISerializable<TDate, int>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected SimpleDateFacts(Calendar calendar!!, Calendar otherCalendar!!)
        : base(calendar, otherCalendar) { }
}
