// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Coptic calendar.
/// </summary>
public sealed class UnboundedCoptic12DataSet :
    UnboundedCalendarDataSet<Coptic12DataSet>, IEpagomenalDataSet, ISingleton<UnboundedCoptic12DataSet>
{
    private UnboundedCoptic12DataSet() : base(Coptic12DataSet.Instance, CalendarEpoch.Coptic) { }

    public static UnboundedCoptic12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedCoptic12DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Coptic12DataSet.DaysSinceRataDieInfos);

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Coptic calendar (alternative form).
/// </summary>
public sealed class UnboundedCoptic13DataSet :
    UnboundedCalendarDataSet<Coptic13DataSet>,
    IMathDataSet,
    IEpagomenalDataSet,
    ISingleton<UnboundedCoptic13DataSet>
{
    private UnboundedCoptic13DataSet() : base(Coptic13DataSet.Instance, CalendarEpoch.Coptic) { }

    public static UnboundedCoptic13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedCoptic13DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Coptic13DataSet.DaysSinceRataDieInfos);

    // IMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddDaysData => SchemaDataSet.AddDaysData;
    public DataGroup<YemodaPair> ConsecutiveDaysData => SchemaDataSet.ConsecutiveDaysData;
    public DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => SchemaDataSet.AddDaysOrdinalData;
    public DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => SchemaDataSet.ConsecutiveDaysOrdinalData;

    // IEpagomenalDataSet
    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}
