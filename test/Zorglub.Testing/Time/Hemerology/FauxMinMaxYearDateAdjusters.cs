// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core;
using Zorglub.Time.Hemerology.Scopes;

public sealed class FauxMinMaxYearDateAdjusters<TDate> : MinMaxYearDateAdjusters<TDate>
    where TDate : IDate<TDate>
{
    public FauxMinMaxYearDateAdjusters(ICalendar<TDate> calendar) : base(calendar)
    {
        Debug.Assert(calendar != null);

        Epoch = calendar.Epoch;
    }

    public DayNumber Epoch { get; }

    protected override TDate GetDate(int daysSinceEpoch) =>
        TDate.FromDayNumber(Epoch + daysSinceEpoch);
}
