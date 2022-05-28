// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) faux Lunisolar calendar.
/// </summary>
public sealed class UnboundedLunisolarDataSet :
    UnboundedCalendarDataSet<LunisolarDataSet>, IMathDataSet, ISingleton<UnboundedLunisolarDataSet>
{
    private UnboundedLunisolarDataSet() : base(LunisolarDataSet.Instance, CalendarEpoch.Positivist) { }

    public static UnboundedLunisolarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedLunisolarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(LunisolarDataSet.DaysSinceRataDieInfos);

    // IMathDataSet
    public DataGroup<YemodaPairAnd<int>> AddDaysData => SchemaDataSet.AddDaysData;
    public DataGroup<YemodaPair> ConsecutiveDaysData => SchemaDataSet.ConsecutiveDaysData;
    public DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData => SchemaDataSet.AddDaysOrdinalData;
    public DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => SchemaDataSet.ConsecutiveDaysOrdinalData;
}
