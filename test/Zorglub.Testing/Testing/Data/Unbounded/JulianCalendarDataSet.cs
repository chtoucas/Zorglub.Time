// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for the (unbounded) Julian calendar and related date types.
/// </summary>
public sealed class JulianCalendarDataSet :
    CalendarDataSet<JulianDataSet>, ISingleton<JulianCalendarDataSet>
{
    private JulianCalendarDataSet() : base(JulianDataSet.Instance, DayZero.OldStyle) { }

    public static JulianCalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= TheoryDataDNInfo.FromDaysSinceRataDieInfos(JulianDataSet.DaysSinceRataDieInfos);
}
