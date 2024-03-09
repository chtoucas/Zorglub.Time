// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Revised World calendar.
/// <para>We use the revised version to simplify the creation of <see cref="DayNumberInfoData"/>.</para>
/// </summary>
public sealed class UnboundedRevisedWorldDataSet :
    UnboundedCalendarDataSet<WorldDataSet>, ISingleton<UnboundedRevisedWorldDataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private UnboundedRevisedWorldDataSet() : base(WorldDataSet.Instance, s_Epoch) { }

    public static UnboundedRevisedWorldDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly UnboundedRevisedWorldDataSet Instance = new();
        static Singleton() { }
    }

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(WorldDataSet.DaysSinceEpochInfos, s_Epoch);
}
