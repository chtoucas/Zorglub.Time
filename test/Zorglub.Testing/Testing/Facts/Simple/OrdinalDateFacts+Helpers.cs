// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Simple;

public abstract class OrdinalDateAdjusterFacts<TDataSet> :
    IDateAdjusterFacts<OrdinalDateAdjustersAdapter, OrdinalDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateAdjusterFacts(SimpleCalendar calendar)
        : base(new OrdinalDateAdjustersAdapter(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;
    }

    protected SimpleCalendar Calendar { get; }

    protected sealed override OrdinalDate GetDate(int y, int m, int d) => Calendar.GetDate(y, m, d).ToOrdinalDate();
    protected sealed override OrdinalDate GetDate(int y, int doy) => Calendar.GetDate(y, doy);
}

public abstract class OrdinalDateProvidersFacts<TDataSet> :
    IDateProvidersFacts<OrdinalDateProviders, OrdinalDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateProvidersFacts(SimpleCalendar calendar) : base(calendar) { }

    protected sealed override OrdinalDate GetDate(int y, int m, int d) => Calendar.GetDate(y, m, d).ToOrdinalDate();
    protected sealed override OrdinalDate GetDate(int y, int doy) => Calendar.GetDate(y, doy);
}
