﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

using static Zorglub.Testing.Data.Extensions.TheoryDataHelpers;

/// <summary>
/// Provides test data for the (unbounded) Egyptian calendar and related date types.
/// </summary>
public sealed class Egyptian12CalendarDataSet :
    CalendarDataSet<Egyptian12DataSet>, IEpagomenalDataSet, ISingleton<Egyptian12CalendarDataSet>
{
    private Egyptian12CalendarDataSet() : base(Egyptian12DataSet.Instance, CalendarEpoch.Egyptian) { }

    public static Egyptian12CalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            Egyptian12DataSet.DaysSinceRataDieInfos.MapToTheoryData(DayNumberInfo.FromDaysSinceRataDieInfo);

    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Egyptian calendar (alternative form) and related date types.
/// </summary>
public sealed class Egyptian13CalendarDataSet :
    CalendarDataSet<Egyptian13DataSet>, IEpagomenalDataSet, ISingleton<Egyptian13CalendarDataSet>
{
    private Egyptian13CalendarDataSet() : base(Egyptian13DataSet.Instance, CalendarEpoch.Egyptian) { }

    public static Egyptian13CalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            Egyptian13DataSet.DaysSinceRataDieInfos.MapToTheoryData(DayNumberInfo.FromDaysSinceRataDieInfo);

    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}
