// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Ethiopic calendar (alternative form).
/// </summary>
public sealed class UnboundedEthiopic13DataSet :
    UnboundedCalendarDataSet<Coptic13DataSet>, IEpagomenalDataSet, ISingleton<UnboundedEthiopic13DataSet>
{
    private static readonly DayNumber s_Epoch = CalendarEpoch.Ethiopic;

    private UnboundedEthiopic13DataSet() : base(Coptic13DataSet.Instance, s_Epoch) { }

    public static UnboundedEthiopic13DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedEthiopic13DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Coptic13DataSet.DaysSinceRataDieInfos, CalendarEpoch.Coptic, s_Epoch);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Ethiopic calendar.
/// </summary>
public sealed class UnboundedEthiopic12DataSet :
    UnboundedCalendarDataSet<Coptic12DataSet>, IEpagomenalDataSet, ISingleton<UnboundedEthiopic12DataSet>
{
    private static readonly DayNumber s_Epoch = CalendarEpoch.Ethiopic;

    private UnboundedEthiopic12DataSet() : base(Coptic12DataSet.Instance, s_Epoch) { }

    public static UnboundedEthiopic12DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedEthiopic12DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Coptic12DataSet.DaysSinceRataDieInfos, CalendarEpoch.Coptic, s_Epoch);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => SchemaDataSet.EpagomenalDayInfoData;
}
