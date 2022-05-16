// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) French Republican calendar and related date types.
/// </summary>
public sealed class FrenchRepublican12CalendarDataSet :
    CalendarDataSet<FrenchRepublican12DataSet>, IEpagomenalDataSet, ISingleton<FrenchRepublican12CalendarDataSet>
{
    private FrenchRepublican12CalendarDataSet()
        : base(FrenchRepublican12DataSet.Instance, CalendarEpoch.FrenchRepublican) { }

    public static FrenchRepublican12CalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            TheoryDataFactories.Create(FrenchRepublican12DataSet.DaysSinceRataDieInfos, DayNumberInfo.FromDaysSinceRataDieInfo);

    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) French Republican calendar (alternative form) and related date types.
/// </summary>
public sealed class FrenchRepublican13CalendarDataSet :
    CalendarDataSet<FrenchRepublican13DataSet>, IEpagomenalDataSet, ISingleton<FrenchRepublican13CalendarDataSet>
{
    private FrenchRepublican13CalendarDataSet()
        : base(FrenchRepublican13DataSet.Instance, CalendarEpoch.FrenchRepublican) { }

    public static FrenchRepublican13CalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            TheoryDataFactories.Create(FrenchRepublican13DataSet.DaysSinceRataDieInfos, DayNumberInfo.FromDaysSinceRataDieInfo);

    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}
