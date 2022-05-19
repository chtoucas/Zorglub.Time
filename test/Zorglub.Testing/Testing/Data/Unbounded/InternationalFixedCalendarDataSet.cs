// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) International Fixed calendar.
/// </summary>
public sealed class InternationalFixedCalendarDataSet :
    UnboundedCalendarDataSet<InternationalFixedDataSet>, ISingleton<InternationalFixedCalendarDataSet>
{
    private static readonly DayNumber s_Epoch = DayZero.NewStyle;

    private InternationalFixedCalendarDataSet() : base(InternationalFixedDataSet.Instance, s_Epoch) { }

    public static InternationalFixedCalendarDataSet Instance { get; } = new();

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(InternationalFixedDataSet.DaysSinceEpochInfos, s_Epoch);
}
