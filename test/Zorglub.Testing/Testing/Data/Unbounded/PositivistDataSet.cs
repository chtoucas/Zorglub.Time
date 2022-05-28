// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Positivist calendar.
/// </summary>
public sealed class UnboundedPositivistDataSet :
    UnboundedCalendarDataSet<PositivistDataSet>, IMathDataSet, ISingleton<UnboundedPositivistDataSet>
{
    private UnboundedPositivistDataSet() : base(PositivistDataSet.Instance, CalendarEpoch.Positivist) { }

    public static UnboundedPositivistDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedPositivistDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(PositivistDataSet.DaysSinceZeroInfos);

    // IMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddDaysData => SchemaDataSet.AddDaysData;
    public DataGroup<YemodaPair> ConsecutiveDaysData => SchemaDataSet.ConsecutiveDaysData;
    public DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => SchemaDataSet.AddDaysOrdinalData;
    public DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => SchemaDataSet.ConsecutiveDaysOrdinalData;
}
