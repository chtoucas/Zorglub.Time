// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Persian calendar (proposed arithmetical form) and related date types.
/// </summary>
public sealed class Persian2820CalendarDataSet :
    CalendarDataSet<Persian2820DataSet>, ISingleton<Persian2820CalendarDataSet>
{
    private Persian2820CalendarDataSet() : base(Persian2820DataSet.Instance, CalendarEpoch.Persian) { }

    public static Persian2820CalendarDataSet Instance { get; } = new();

    public override DataGroup<DayNumberInfo> DayNumberInfoData { get; } =
        DataGroup.CreateDayNumberInfoData(Persian2820DataSet.DaysSinceRataDieInfos);
}

