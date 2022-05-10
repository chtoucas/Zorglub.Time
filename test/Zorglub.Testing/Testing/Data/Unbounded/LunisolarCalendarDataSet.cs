﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) faux Lunisolar calendar and related date types.
/// </summary>
public sealed class LunisolarCalendarDataSet :
    CalendarDataSet<LunisolarDataSet>, ISingleton<LunisolarCalendarDataSet>
{
    private LunisolarCalendarDataSet() : base(LunisolarDataSet.Instance, CalendarEpoch.Positivist) { }

    public static LunisolarCalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= CalCal.ConvertRataDieToDayNumberInfo(LunisolarDataSet.RataDieInfos);
}
