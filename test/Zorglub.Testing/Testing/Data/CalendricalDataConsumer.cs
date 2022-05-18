// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// All datasets are read-only and initialize lazily a bunch of properties.
// To improve things (performance, memory), it might help a bit to constraint
// the datasets to be singleton.

/// <summary>
/// Provides static access to calendrical data.
/// </summary>
public abstract class CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CalendricalDataConsumer() { }

    protected static TDataSet DataSet { get; } = TDataSet.Instance;

    protected static int SampleCommonYear { get; } = DataSet.SampleCommonYear;
    protected static int SampleLeapYear { get; } = DataSet.SampleLeapYear;

    public static XunitData<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; } =
        DataSet.DaysSinceEpochInfoData.ToXunitData();

    public static XunitData<DateInfo> DateInfoData { get; } =
        DataSet.DateInfoData.ToXunitData();
    public static XunitData<MonthInfo> MonthInfoData { get; } =
        DataSet.MonthInfoData.ToXunitData();
    public static XunitData<YearInfo> YearInfoData { get; } =
        DataSet.YearInfoData.ToXunitData();
    public static XunitData<CenturyInfo> CenturyInfoData { get; } =
        DataSet.CenturyInfoData.ToXunitData();

    public static XunitData<YemodaAnd<int>> DaysInYearAfterDateData { get; } =
        DataSet.DaysInYearAfterDateData.ToXunitData();
    public static XunitData<YemodaAnd<int>> DaysInMonthAfterDateData { get; } =
        DataSet.DaysInMonthAfterDateData.ToXunitData();

    public static XunitData<Yemoda> StartOfYearPartsData { get; } =
        DataSet.StartOfYearPartsData.ToXunitData();
    public static XunitData<Yemoda> EndOfYearPartsData { get; } =
        DataSet.EndOfYearPartsData.ToXunitData();

    public static XunitData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } =
        DataSet.StartOfYearDaysSinceEpochData.ToXunitData();
    public static XunitData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData { get; } =
        DataSet.EndOfYearDaysSinceEpochData.ToXunitData();

    public static TheoryData<int, int> InvalidMonthFieldData => DataSet.InvalidMonthFieldData;
    public static TheoryData<int, int, int> InvalidDayFieldData => DataSet.InvalidDayFieldData;
    public static TheoryData<int, int> InvalidDayOfYearFieldData => DataSet.InvalidDayOfYearFieldData;
}
