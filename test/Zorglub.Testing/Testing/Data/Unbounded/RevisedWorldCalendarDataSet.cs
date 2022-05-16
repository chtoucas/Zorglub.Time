// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Revised World calendar and related date types.
/// <para>We use the revised version to simplify the creation of <see cref="DayNumberInfoData"/>.</para>
/// </summary>
public sealed class RevisedWorldCalendarDataSet :
    CalendarDataSet<WorldDataSet>, ISingleton<RevisedWorldCalendarDataSet>
{
    private RevisedWorldCalendarDataSet() : base(WorldDataSet.Instance, DayZero.NewStyle) { }

    public static RevisedWorldCalendarDataSet Instance { get; } = new();

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= DataGroup.CreateDayNumberInfoData(WorldDataSet.DaysSinceEpochInfos, Epoch);
}
