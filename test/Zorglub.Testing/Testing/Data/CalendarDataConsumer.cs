// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Provides static access to calendar data.
/// </summary>
public abstract class CalendarDataConsumer<TDataSet> : CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDataConsumer() { }

    protected static DayNumber Epoch { get; } = DataSet.Epoch;

    public static TheoryData<DayNumberInfo> DayNumberInfoData => DataSet.DayNumberInfoData;

    public static TheoryData<YearDayNumber> StartOfYearDayNumberData => DataSet.StartOfYearDayNumberData;
    public static TheoryData<YearDayNumber> EndOfYearDayNumberData => DataSet.EndOfYearDayNumberData;
}
