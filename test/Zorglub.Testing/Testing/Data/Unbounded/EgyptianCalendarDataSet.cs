// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Egyptian calendar.
/// </summary>
public sealed class Egyptian12CalendarDataSet :
    UnboundedCalendarDataSet<Egyptian12DataSet>, IEpagomenalDataSet, ISingleton<Egyptian12CalendarDataSet>
{
    private Egyptian12CalendarDataSet() : base(Egyptian12DataSet.Instance, CalendarEpoch.Egyptian) { }

    public static Egyptian12CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly Egyptian12CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian12DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Egyptian calendar (alternative form).
/// </summary>
public sealed class Egyptian13CalendarDataSet :
    UnboundedCalendarDataSet<Egyptian13DataSet>, IEpagomenalDataSet, ISingleton<Egyptian13CalendarDataSet>
{
    private Egyptian13CalendarDataSet() : base(Egyptian13DataSet.Instance, CalendarEpoch.Egyptian) { }

    public static Egyptian13CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly Egyptian13CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Egyptian13DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}
