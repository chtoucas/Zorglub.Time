﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using Zorglub.Testing.Data.Schemas;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for the (unbounded) Coptic calendar and related date types.
/// </summary>
public sealed class Coptic12CalendarDataSet :
    CalendarDataSet<Coptic12DataSet>, IEpagomenalDataSet, ISingleton<Coptic12CalendarDataSet>
{
    private Coptic12CalendarDataSet() : base(Coptic12DataSet.Instance, CalendarEpoch.Coptic) { }

    public static Coptic12CalendarDataSet Instance { get; } = new();

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= DataGroup.CreateDayNumberInfoData(Coptic12DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}

/// <summary>
/// Provides test data for the (unbounded) Coptic calendar (alternative form) and related date types.
/// </summary>
public sealed class Coptic13CalendarDataSet :
    CalendarDataSet<Coptic13DataSet>, IEpagomenalDataSet, ISingleton<Coptic13CalendarDataSet>
{
    private Coptic13CalendarDataSet() : base(Coptic13DataSet.Instance, CalendarEpoch.Coptic) { }

    public static Coptic13CalendarDataSet Instance { get; } = new();

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public override DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= DataGroup.CreateDayNumberInfoData(Coptic13DataSet.DaysSinceRataDieInfos);

    public DataGroup<YemodaAnd<int>> EpagomenalDayInfoData => DataSet.EpagomenalDayInfoData;
}
