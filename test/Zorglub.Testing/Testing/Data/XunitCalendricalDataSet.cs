// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

public class XunitCalendricalDataSet<TDataSet>
    where TDataSet : ICalendricalDataSet
{
    public XunitCalendricalDataSet(TDataSet dataSet)
    {
        DataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
    }

    public TDataSet DataSet { get; }

    public int SampleCommonYear => DataSet.SampleCommonYear;
    public int SampleLeapYear => DataSet.SampleLeapYear;

    private XunitData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public XunitData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??= DataSet.DaysSinceEpochInfoData.ToXunitData();

    private XunitData<DateInfo>? _dateInfoData;
    public XunitData<DateInfo> DateInfoData =>
        _dateInfoData ??= DataSet.DateInfoData.ToXunitData();

    private XunitData<MonthInfo>? _monthInfoData;
    public XunitData<MonthInfo> MonthInfoData =>
        _monthInfoData ??= DataSet.MonthInfoData.ToXunitData();

    private XunitData<YearInfo>? _yearInfoData;
    public XunitData<YearInfo> YearInfoData =>
        _yearInfoData ??= DataSet.YearInfoData.ToXunitData();

    private XunitData<CenturyInfo>? _centuryInfoData;
    public XunitData<CenturyInfo> CenturyInfoData =>
        _centuryInfoData ??= DataSet.CenturyInfoData.ToXunitData();

    private XunitData<YemodaAnd<int>>? _daysInYearAfterDateData;
    public XunitData<YemodaAnd<int>> DaysInYearAfterDateData =>
        _daysInYearAfterDateData ??= DataSet.DaysInYearAfterDateData.ToXunitData();

    private XunitData<YemodaAnd<int>>? _daysInMonthAfterDateData;
    public XunitData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        _daysInMonthAfterDateData ??= DataSet.DaysInMonthAfterDateData.ToXunitData();

    private XunitData<Yemoda>? _startOfYearPartsData;
    public XunitData<Yemoda> StartOfYearPartsData =>
        _startOfYearPartsData ??= DataSet.StartOfYearPartsData.ToXunitData();

    private XunitData<Yemoda>? _endOfYearPartsData;
    public XunitData<Yemoda> EndOfYearPartsData =>
        _endOfYearPartsData ??= DataSet.EndOfYearPartsData.ToXunitData();

    private XunitData<YearDaysSinceEpoch>? _startOfYearDaysSinceEpochData;
    public XunitData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData =>
        _startOfYearDaysSinceEpochData ??= DataSet.StartOfYearDaysSinceEpochData.ToXunitData();

    private XunitData<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
    public XunitData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        _endOfYearDaysSinceEpochData ??= DataSet.EndOfYearDaysSinceEpochData.ToXunitData();

    public TheoryData<int, int> InvalidMonthFieldData => DataSet.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => DataSet.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => DataSet.InvalidDayOfYearFieldData;
}
