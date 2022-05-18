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

    public static XunitData<DayNumberInfo> DayNumberInfoData { get; } =
        DataSet.DayNumberInfoData.ToXunitData();

    public static XunitData<YearDayNumber> StartOfYearDayNumberData { get; } =
        DataSet.StartOfYearDayNumberData.ToXunitData();
    public static XunitData<YearDayNumber> EndOfYearDayNumberData { get; } =
        DataSet.EndOfYearDayNumberData.ToXunitData();
}
