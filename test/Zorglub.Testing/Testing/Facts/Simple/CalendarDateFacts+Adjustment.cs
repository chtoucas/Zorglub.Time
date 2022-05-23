// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

public abstract class CalendarDateAdjustmentFacts<TDataSet> :
    IAdjustableDateFacts<CalendarDate, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CalendarDateAdjustmentFacts(Calendar calendar)
        : base(calendar?.SupportedYears ?? throw new ArgumentNullException(nameof(calendar)))
    {
        Debug.Assert(calendar != null);

        CalendarUT = calendar;
    }

    protected Calendar CalendarUT { get; }

    protected override CalendarDate GetDate(int y, int m, int d) => CalendarUT.GetCalendarDate(y, m, d);
}
