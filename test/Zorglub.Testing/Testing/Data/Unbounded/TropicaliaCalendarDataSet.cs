﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) "Tropicália" calendar.
/// </summary>
public sealed class TropicaliaCalendarDataSet :
    UnboundedCalendarDataSet<TropicalistaDataSet>, ISingleton<TropicaliaCalendarDataSet>
{
    private TropicaliaCalendarDataSet() : base(TropicaliaDataSet.Instance, DayZero.NewStyle) { }

    public static TropicaliaCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly TropicaliaCalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(TropicaliaDataSet.DaysSinceRataDieInfos);
}

/// <summary>
/// Provides test data for the (unbounded) "Tropicália" calendar (30-31).
/// </summary>
public sealed class Tropicalia3031CalendarDataSet :
    UnboundedCalendarDataSet<Tropicalia3031DataSet>, ISingleton<Tropicalia3031CalendarDataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private Tropicalia3031CalendarDataSet() : base(Tropicalia3031DataSet.Instance, s_Epoch) { }

    public static Tropicalia3031CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly Tropicalia3031CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Tropicalia3031DataSet.DaysSinceEpochInfos, s_Epoch);
}

/// <summary>
/// Provides test data for the (unbounded) "Tropicália" calendar (31-30).
/// </summary>
public sealed class Tropicalia3130CalendarDataSet :
    UnboundedCalendarDataSet<Tropicalia3130DataSet>, ISingleton<Tropicalia3130CalendarDataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private Tropicalia3130CalendarDataSet() : base(Tropicalia3130DataSet.Instance, s_Epoch) { }

    public static Tropicalia3130CalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly Tropicalia3130CalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Tropicalia3130DataSet.DaysSinceEpochInfos, s_Epoch);
}
