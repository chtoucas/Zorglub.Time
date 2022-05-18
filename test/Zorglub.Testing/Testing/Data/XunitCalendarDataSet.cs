// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

public class XunitCalendarDataSet<TDataSet> : XunitCalendricalDataSet<TDataSet>
    where TDataSet : ICalendarDataSet
{
    public XunitCalendarDataSet(TDataSet dataSet) : base(dataSet) { }

    public DayNumber Epoch => DataSet.Epoch;

    private XunitData<DayNumberInfo>? _dayNumberInfoData;
    public XunitData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= DataSet.DayNumberInfoData.ToXunitData();

    private XunitData<YearDayNumber>? _startOfYearDayNumberData;
    public XunitData<YearDayNumber> StartOfYearDayNumberData =>
        _startOfYearDayNumberData ??= DataSet.StartOfYearDayNumberData.ToXunitData();

    private XunitData<YearDayNumber>? _endOfYearDayNumberData;
    public XunitData<YearDayNumber> EndOfYearDayNumberData =>
        _endOfYearDayNumberData ??= DataSet.EndOfYearDayNumberData.ToXunitData();
}
