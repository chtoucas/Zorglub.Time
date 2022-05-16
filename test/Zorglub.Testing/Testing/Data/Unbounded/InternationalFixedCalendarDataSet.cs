// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) International Fixed calendar and related date types.
/// </summary>
public sealed class InternationalFixedCalendarDataSet :
    CalendarDataSet<InternationalFixedDataSet>, ISingleton<InternationalFixedCalendarDataSet>
{
    private InternationalFixedCalendarDataSet() : base(InternationalFixedDataSet.Instance, DayZero.NewStyle) { }

    public static InternationalFixedCalendarDataSet Instance { get; } = new();

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            DataGroup.CreateDayNumberInfoData(InternationalFixedDataSet.DaysSinceEpochInfos, Epoch);
}
