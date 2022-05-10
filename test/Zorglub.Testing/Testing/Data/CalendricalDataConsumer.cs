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

    public static TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData => DataSet.DaysSinceEpochInfoData;

    public static TheoryData<DateInfo> DateInfoData => DataSet.DateInfoData;
    public static TheoryData<MonthInfo> MonthInfoData => DataSet.MonthInfoData;
    public static TheoryData<YearInfo> YearInfoData => DataSet.YearInfoData;
    public static TheoryData<CenturyInfo> CenturyInfoData => DataSet.CenturyInfoData;

    public static TheoryData<Yemoda> StartOfYearPartsData => DataSet.StartOfYearPartsData;
    public static TheoryData<Yemoda> EndOfYearPartsData => DataSet.EndOfYearPartsData;

    public static TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => DataSet.StartOfYearDaysSinceEpochData;
    public static TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => DataSet.EndOfYearDaysSinceEpochData;

    public static TheoryData<int, int> InvalidMonthFieldData => DataSet.InvalidMonthFieldData;
    public static TheoryData<int, int, int> InvalidDayFieldData => DataSet.InvalidDayFieldData;
    public static TheoryData<int, int> InvalidDayOfYearFieldData => DataSet.InvalidDayOfYearFieldData;
}
