// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Tabular Islamic calendar.
/// </summary>
public sealed class UnboundedTabularIslamicDataSet :
    UnboundedCalendarDataSet<TabularIslamicDataSet>, ISingleton<UnboundedTabularIslamicDataSet>
{
    private UnboundedTabularIslamicDataSet() : base(TabularIslamicDataSet.Instance, DayZero.TabularIslamic) { }

    public static UnboundedTabularIslamicDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedTabularIslamicDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(TabularIslamicDataSet.DaysSinceRataDieInfos);
}
