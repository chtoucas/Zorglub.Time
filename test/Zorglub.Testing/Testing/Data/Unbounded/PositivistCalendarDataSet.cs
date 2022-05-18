// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Positivist calendar and related date types.
/// </summary>
public sealed class PositivistCalendarDataSet :
    CalendarDataSet<PositivistDataSet>, ISingleton<PositivistCalendarDataSet>
{
    private PositivistCalendarDataSet() : base(PositivistDataSet.Instance, CalendarEpoch.Positivist) { }

    public static PositivistCalendarDataSet Instance { get; } = new();

    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        DataGroup.CreateDayNumberInfoData(PositivistDataSet.DaysSinceZeroInfos);
}
