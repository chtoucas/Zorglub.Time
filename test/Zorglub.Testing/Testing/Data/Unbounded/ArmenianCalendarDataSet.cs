// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Amenian calendar.
/// </summary>
public sealed class Armenian12CalendarDataSet :
    UnboundedCalendarDataSet<Egyptian12DataSet>, IEpagomenalDataSet, ISingleton<Armenian12CalendarDataSet>
{
    private static readonly DayNumber s_Epoch = CalendarEpoch.Armenian;

    private Armenian12CalendarDataSet() : base(Egyptian12DataSet.Instance, s_Epoch) { }

    public static Armenian12CalendarDataSet Instance { get; } = new();

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian12DataSet.DaysSinceRataDieInfos, CalendarEpoch.Egyptian, s_Epoch);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Amenian calendar (alternative form).
/// </summary>
public sealed class Armenian13CalendarDataSet :
    UnboundedCalendarDataSet<Egyptian13DataSet>, IEpagomenalDataSet, ISingleton<Armenian13CalendarDataSet>
{
    private static readonly DayNumber s_Epoch = CalendarEpoch.Armenian;

    private Armenian13CalendarDataSet() : base(Egyptian13DataSet.Instance, s_Epoch) { }

    public static Armenian13CalendarDataSet Instance { get; } = new();

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian13DataSet.DaysSinceRataDieInfos, CalendarEpoch.Egyptian, s_Epoch);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}
