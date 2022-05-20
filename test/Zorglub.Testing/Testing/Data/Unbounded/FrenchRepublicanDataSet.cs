// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) French Republican calendar.
/// </summary>
public sealed class UnboundedFrenchRepublican12DataSet :
    UnboundedCalendarDataSet<FrenchRepublican12DataSet>, IEpagomenalDataSet, ISingleton<UnboundedFrenchRepublican12DataSet>
{
    private UnboundedFrenchRepublican12DataSet()
        : base(FrenchRepublican12DataSet.Instance, CalendarEpoch.FrenchRepublican) { }

    public static UnboundedFrenchRepublican12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedFrenchRepublican12DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(FrenchRepublican12DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) French Republican calendar (alternative form).
/// </summary>
public sealed class UnboundedFrenchRepublican13DataSet :
    UnboundedCalendarDataSet<FrenchRepublican13DataSet>, IEpagomenalDataSet, ISingleton<UnboundedFrenchRepublican13DataSet>
{
    private UnboundedFrenchRepublican13DataSet()
        : base(FrenchRepublican13DataSet.Instance, CalendarEpoch.FrenchRepublican) { }

    public static UnboundedFrenchRepublican13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedFrenchRepublican13DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(FrenchRepublican13DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}
