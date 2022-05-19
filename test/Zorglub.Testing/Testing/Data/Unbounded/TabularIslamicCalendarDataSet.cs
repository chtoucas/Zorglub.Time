﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Tabular Islamic calendar.
/// </summary>
public sealed class TabularIslamicCalendarDataSet :
    UnboundedCalendarDataSet<TabularIslamicDataSet>, ISingleton<TabularIslamicCalendarDataSet>
{
    private TabularIslamicCalendarDataSet() : base(TabularIslamicDataSet.Instance, CalendarEpoch.TabularIslamic) { }

    public static TabularIslamicCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly TabularIslamicCalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(TabularIslamicDataSet.DaysSinceRataDieInfos);
}
