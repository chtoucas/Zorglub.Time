// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using System.Linq;

public class BoundedCalendarDataSet<TDataSet> : ICalendarDataSet
    where TDataSet : ICalendarDataSet
{
    public BoundedCalendarDataSet(TDataSet inner, IDataFilter dataFilter)
    {
        Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        DataFilter = dataFilter ?? throw new ArgumentNullException(nameof(dataFilter));

        Epoch = inner.Epoch;
        SampleCommonYear = inner.SampleCommonYear;
        SampleLeapYear = inner.SampleLeapYear;
    }

    /// <summary>
    /// Gets the original dataset.
    /// </summary>
    public TDataSet Inner { get; }

    public IDataFilter DataFilter { get; }

    protected static DataGroup<T> FilterData<T>(TheoryData<T> data, Func<T, bool> filter)
    {
        var q = from item in data
                let value = (T)item[0]
                where filter(value)
                select value;

        Debug.Assert(q.Any());

        return DataGroup.Create(q);
    }

    // This version is faster, no boxing!
    protected static DataGroup<T> FilterData<T>(DataGroup<T> data, Func<T, bool> filter)
    {
        var q = data.AsEnumerable().Where(filter);

        Debug.Assert(q.Any());

        return DataGroup.Create(q);
    }

    #region ICalendarDataSet

    public DayNumber Epoch { get; }

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public TheoryData<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= FilterData(Inner.DayNumberInfoData, DataFilter.Filter);

    private DataGroup<YearDayNumber>? _startOfYearDayNumberData;
    public TheoryData<YearDayNumber> StartOfYearDayNumberData =>
        _startOfYearDayNumberData ??= FilterData(Inner.StartOfYearDayNumberData, DataFilter.Filter);

    private DataGroup<YearDayNumber>? _endOfYearDayNumberData;
    public TheoryData<YearDayNumber> EndOfYearDayNumberData =>
        _endOfYearDayNumberData ??= FilterData(Inner.EndOfYearDayNumberData, DataFilter.Filter);

    #endregion
    #region ICalendricalDataSet

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    private DataGroup<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??= FilterData(Inner.DaysSinceEpochInfoData, DataFilter.Filter);

    private DataGroup<DateInfo>? _dateInfoData;
    public TheoryData<DateInfo> DateInfoData =>
        _dateInfoData ??= FilterData(Inner.DateInfoData, DataFilter.Filter);

    private DataGroup<MonthInfo>? _monthInfoData;
    public TheoryData<MonthInfo> MonthInfoData =>
        _monthInfoData ??= FilterData(Inner.MonthInfoData, DataFilter.Filter);

    private DataGroup<YearInfo>? _yearInfoData;
    public TheoryData<YearInfo> YearInfoData =>
        _yearInfoData ??= FilterData(Inner.YearInfoData, DataFilter.Filter);

    private DataGroup<CenturyInfo>? _centuryInfoData;
    public TheoryData<CenturyInfo> CenturyInfoData =>
        _centuryInfoData ??= FilterData(Inner.CenturyInfoData, DataFilter.Filter);

    private DataGroup<YemodaAnd<int>>? _daysInYearAfterDateDataInit;
    public TheoryData<YemodaAnd<int>> DaysInYearAfterDateData =>
        _daysInYearAfterDateDataInit ??= FilterData(Inner.DaysInYearAfterDateData, DataFilter.Filter);

    private DataGroup<YemodaAnd<int>>? _daysInMonthAfterDateData;
    public TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        _daysInMonthAfterDateData ??= FilterData(Inner.DaysInMonthAfterDateData, DataFilter.Filter);

    private DataGroup<Yemoda>? _startOfYearPartsData;
    public TheoryData<Yemoda> StartOfYearPartsData =>
        _startOfYearPartsData ??= FilterData(Inner.StartOfYearPartsData, DataFilter.Filter);

    private DataGroup<Yemoda>? _endOfYearPartsData;
    public TheoryData<Yemoda> EndOfYearPartsData =>
        _endOfYearPartsData ??= FilterData(Inner.EndOfYearPartsData, DataFilter.Filter);

    private DataGroup<YearDaysSinceEpoch>? _startOfYearDaysSinceEpochData;
    public TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData =>
        _startOfYearDaysSinceEpochData ??= FilterData(Inner.StartOfYearDaysSinceEpochData, DataFilter.Filter);

    private DataGroup<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
    public TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        _endOfYearDaysSinceEpochData ??= FilterData(Inner.EndOfYearDaysSinceEpochData, DataFilter.Filter);

    // Normally, we don't have to filter the three following properties.
    public TheoryData<int, int> InvalidMonthFieldData => Inner.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => Inner.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => Inner.InvalidDayOfYearFieldData;

    #endregion
}
