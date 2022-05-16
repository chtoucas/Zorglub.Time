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

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= TheoryDataDNInfo.FromDaysSinceEpochInfos(InternationalFixedDataSet.DaysSinceEpochInfos, Epoch);
}
