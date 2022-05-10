﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Ethiopic calendar (alternative form) and related date types.
/// </summary>
public sealed class Ethiopic13CalendarDataSet :
    CalendarDataSet<Coptic13DataSet>, IEpagomenalDataSet, ISingleton<Ethiopic13CalendarDataSet>
{
    private Ethiopic13CalendarDataSet() : base(Coptic13DataSet.Instance, CalendarEpoch.Ethiopic) { }

    public static Ethiopic13CalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            CalCal.ConvertRataDieToDayNumberInfo(Coptic13DataSet.RataDieInfos, CalendarEpoch.Coptic, Epoch);

    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Ethiopic calendar and related date types.
/// </summary>
public sealed class Ethiopic12CalendarDataSet :
    CalendarDataSet<Coptic12DataSet>, IEpagomenalDataSet, ISingleton<Ethiopic12CalendarDataSet>
{
    private Ethiopic12CalendarDataSet() : base(Coptic12DataSet.Instance, CalendarEpoch.Ethiopic) { }

    public static Ethiopic12CalendarDataSet Instance { get; } = new();

    private TheoryData<DayNumberInfo>? _dayNumberInfoData;
    public override TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??=
            CalCal.ConvertRataDieToDayNumberInfo(Coptic12DataSet.RataDieInfos, CalendarEpoch.Coptic, Epoch);

    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}
