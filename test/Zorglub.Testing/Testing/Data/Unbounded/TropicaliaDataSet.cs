// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) "Tropicália" calendar.
/// </summary>
public sealed class UnboundedTropicaliaDataSet :
    UnboundedCalendarDataSet<TropicalistaDataSet>, ISingleton<UnboundedTropicaliaDataSet>
{
    private UnboundedTropicaliaDataSet() : base(TropicaliaDataSet.Instance, DayZero.NewStyle) { }

    public static UnboundedTropicaliaDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedTropicaliaDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(TropicaliaDataSet.DaysSinceRataDieInfos);
}

/// <summary>
/// Provides test data for the (unbounded) "Tropicália" calendar (30-31).
/// </summary>
public sealed class UnboundedTropicalia3031DataSet :
    UnboundedCalendarDataSet<Tropicalia3031DataSet>, ISingleton<UnboundedTropicalia3031DataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private UnboundedTropicalia3031DataSet() : base(Tropicalia3031DataSet.Instance, s_Epoch) { }

    public static UnboundedTropicalia3031DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedTropicalia3031DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Tropicalia3031DataSet.DaysSinceEpochInfos, s_Epoch);
}

/// <summary>
/// Provides test data for the (unbounded) "Tropicália" calendar (31-30).
/// </summary>
public sealed class UnboundedTropicalia3130DataSet :
    UnboundedCalendarDataSet<Tropicalia3130DataSet>, ISingleton<UnboundedTropicalia3130DataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private UnboundedTropicalia3130DataSet() : base(Tropicalia3130DataSet.Instance, s_Epoch) { }

    public static UnboundedTropicalia3130DataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedTropicalia3130DataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Tropicalia3130DataSet.DaysSinceEpochInfos, s_Epoch);
}
