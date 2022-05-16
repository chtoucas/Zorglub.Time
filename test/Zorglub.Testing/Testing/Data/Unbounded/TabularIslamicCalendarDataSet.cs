// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

using static Zorglub.Testing.Data.Extensions.TheoryDataHelpers;

/// <summary>
/// Provides test data for the (unbounded) Tabular Islamic calendar and related date types.
/// </summary>
public sealed class TabularIslamicCalendarDataSet :
    CalendarDataSet<TabularIslamicDataSet>, ISingleton<TabularIslamicCalendarDataSet>
{
    private TabularIslamicCalendarDataSet() : base(TabularIslamicDataSet.Instance, CalendarEpoch.TabularIslamic) { }

    public static TabularIslamicCalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            TabularIslamicDataSet.DaysSinceRataDieInfos.MapToTheoryData(DayNumberInfo.FromDaysSinceRataDieInfo);
}
