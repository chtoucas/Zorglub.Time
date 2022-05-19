// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Coptic calendar.
/// </summary>
public sealed class Coptic12CalendarDataSet :
    UnboundedCalendarDataSet<Coptic12DataSet>, IEpagomenalDataSet, ISingleton<Coptic12CalendarDataSet>
{
    private Coptic12CalendarDataSet() : base(Coptic12DataSet.Instance, CalendarEpoch.Coptic) { }

    public static Coptic12CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly Coptic12CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Coptic12DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Coptic calendar (alternative form).
/// </summary>
public sealed class Coptic13CalendarDataSet :
    UnboundedCalendarDataSet<Coptic13DataSet>, IEpagomenalDataSet, ISingleton<Coptic13CalendarDataSet>
{
    private Coptic13CalendarDataSet() : base(Coptic13DataSet.Instance, CalendarEpoch.Coptic) { }

    public static Coptic13CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly Coptic13CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Coptic13DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => CalendricalDataSet.EpagomenalDayInfoData;
}
