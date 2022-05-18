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

    public static DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData => DataSet.DaysSinceEpochInfoData;

    public static DataGroup<DateInfo> DateInfoData => DataSet.DateInfoData;
    public static DataGroup<MonthInfo> MonthInfoData => DataSet.MonthInfoData;
    public static DataGroup<YearInfo> YearInfoData => DataSet.YearInfoData;
    public static DataGroup<CenturyInfo> CenturyInfoData => DataSet.CenturyInfoData;

    public static DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => DataSet.DaysInYearAfterDateData;
    public static DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => DataSet.DaysInMonthAfterDateData;

    public static DataGroup<Yemoda> StartOfYearPartsData => DataSet.StartOfYearPartsData;
    public static DataGroup<Yemoda> EndOfYearPartsData => DataSet.EndOfYearPartsData;

    public static DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => DataSet.StartOfYearDaysSinceEpochData;
    public static DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => DataSet.EndOfYearDaysSinceEpochData;

    public static TheoryData<int, int> InvalidMonthFieldData => DataSet.InvalidMonthFieldData;
    public static TheoryData<int, int, int> InvalidDayFieldData => DataSet.InvalidDayFieldData;
    public static TheoryData<int, int> InvalidDayOfYearFieldData => DataSet.InvalidDayOfYearFieldData;
}
