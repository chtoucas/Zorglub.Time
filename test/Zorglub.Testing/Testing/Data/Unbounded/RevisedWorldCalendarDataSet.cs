// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Revised World calendar.
/// <para>We use the revised version to simplify the creation of <see cref="DayNumberInfoData"/>.</para>
/// </summary>
public sealed class RevisedWorldCalendarDataSet :
    UnboundedCalendarDataSet<WorldDataSet>, ISingleton<RevisedWorldCalendarDataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private RevisedWorldCalendarDataSet() : base(WorldDataSet.Instance, s_Epoch) { }

    public static RevisedWorldCalendarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly RevisedWorldCalendarDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(WorldDataSet.DaysSinceEpochInfos, s_Epoch);
}
