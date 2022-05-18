// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Amenian calendar and related date types.
/// </summary>
public sealed class Armenian12CalendarDataSet :
    CalendarDataSet<Egyptian12DataSet>, IEpagomenalDataSet, ISingleton<Armenian12CalendarDataSet>
{
    private Armenian12CalendarDataSet() : base(Egyptian12DataSet.Instance, CalendarEpoch.Armenian) { }

    public static Armenian12CalendarDataSet Instance { get; } = new();

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            DataGroup.CreateDayNumberInfoData(
                Egyptian12DataSet.DaysSinceRataDieInfos, CalendarEpoch.Egyptian, Epoch);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Amenian calendar (alternative form) and related date types.
/// </summary>
public sealed class Armenian13CalendarDataSet :
    CalendarDataSet<Egyptian13DataSet>, IEpagomenalDataSet, ISingleton<Armenian13CalendarDataSet>
{
    private Armenian13CalendarDataSet() : base(Egyptian13DataSet.Instance, CalendarEpoch.Armenian) { }

    public static Armenian13CalendarDataSet Instance { get; } = new();

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            DataGroup.CreateDayNumberInfoData(
                Egyptian13DataSet.DaysSinceRataDieInfos, CalendarEpoch.Egyptian, Epoch);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}
