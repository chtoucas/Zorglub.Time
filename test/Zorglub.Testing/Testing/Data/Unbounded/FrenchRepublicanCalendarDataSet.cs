// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) French Republican calendar.
/// </summary>
public sealed class FrenchRepublican12CalendarDataSet :
    UnboundedCalendarDataSet<FrenchRepublican12DataSet>, IEpagomenalDataSet, ISingleton<FrenchRepublican12CalendarDataSet>
{
    private FrenchRepublican12CalendarDataSet()
        : base(FrenchRepublican12DataSet.Instance, CalendarEpoch.FrenchRepublican) { }

    public static FrenchRepublican12CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly FrenchRepublican12CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(FrenchRepublican12DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) French Republican calendar (alternative form).
/// </summary>
public sealed class FrenchRepublican13CalendarDataSet :
    UnboundedCalendarDataSet<FrenchRepublican13DataSet>, IEpagomenalDataSet, ISingleton<FrenchRepublican13CalendarDataSet>
{
    private FrenchRepublican13CalendarDataSet()
        : base(FrenchRepublican13DataSet.Instance, CalendarEpoch.FrenchRepublican) { }

    public static FrenchRepublican13CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly FrenchRepublican13CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(FrenchRepublican13DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}
