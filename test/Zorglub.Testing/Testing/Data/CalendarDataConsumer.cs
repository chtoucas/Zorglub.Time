// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Provides static access to calendar data.
/// </summary>
public abstract class CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDataConsumer() { }

    protected static XunitCalendarDataSet<TDataSet> XunitDataSet { get; } = new(TDataSet.Instance);

    protected static DayNumber Epoch { get; } = XunitDataSet.Epoch;

    public static XunitData<DayNumberInfo> DayNumberInfoData => XunitDataSet.DayNumberInfoData;

    public static XunitData<YearDayNumber> StartOfYearDayNumberData => XunitDataSet.StartOfYearDayNumberData;
    public static XunitData<YearDayNumber> EndOfYearDayNumberData => XunitDataSet.EndOfYearDayNumberData;

    //
    // Affine data
    //

    protected static int SampleCommonYear { get; } = XunitDataSet.SampleCommonYear;
    protected static int SampleLeapYear { get; } = XunitDataSet.SampleLeapYear;

    public static XunitData<DaysSinceEpochInfo> DaysSinceEpochInfoData => XunitDataSet.DaysSinceEpochInfoData;

    public static XunitData<DateInfo> DateInfoData => XunitDataSet.DateInfoData;
    public static XunitData<MonthInfo> MonthInfoData => XunitDataSet.MonthInfoData;
    public static XunitData<YearInfo> YearInfoData => XunitDataSet.YearInfoData;
    public static XunitData<CenturyInfo> CenturyInfoData => XunitDataSet.CenturyInfoData;

    public static XunitData<YemodaAnd<int>> DaysInYearAfterDateData => XunitDataSet.DaysInYearAfterDateData;
    public static XunitData<YemodaAnd<int>> DaysInMonthAfterDateData => XunitDataSet.DaysInMonthAfterDateData;

    public static XunitData<Yemoda> StartOfYearPartsData => XunitDataSet.StartOfYearPartsData;
    public static XunitData<Yemoda> EndOfYearPartsData => XunitDataSet.EndOfYearPartsData;

    public static XunitData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => XunitDataSet.StartOfYearDaysSinceEpochData;
    public static XunitData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => XunitDataSet.EndOfYearDaysSinceEpochData;

    public static TheoryData<int, int> InvalidMonthFieldData => XunitDataSet.InvalidMonthFieldData;
    public static TheoryData<int, int, int> InvalidDayFieldData => XunitDataSet.InvalidDayFieldData;
    public static TheoryData<int, int> InvalidDayOfYearFieldData => XunitDataSet.InvalidDayOfYearFieldData;
}
