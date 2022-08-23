// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) World calendar.
/// </summary>
public sealed class UnboundedWorldDataSet :
    UnboundedCalendarDataSet<WorldDataSet>, ISingleton<UnboundedWorldDataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.SundayBeforeGregorian;

    private UnboundedWorldDataSet() : base(WorldDataSet.Instance, s_Epoch) { }

    public static UnboundedWorldDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedWorldDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(WorldDataSet.DaysSinceEpochInfos, s_Epoch);
}
