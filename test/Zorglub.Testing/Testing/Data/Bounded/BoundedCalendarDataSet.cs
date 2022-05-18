// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

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

    #region ICalendarDataSet

    public DayNumber Epoch { get; }

    private DataGroup<DayNumberInfo>? _dayNumberInfoData;
    public DataGroup<DayNumberInfo> DayNumberInfoData =>
        _dayNumberInfoData ??= Inner.DayNumberInfoData.WhereT(DataFilter.Filter);

    private DataGroup<YearDayNumber>? _startOfYearDayNumberData;
    public DataGroup<YearDayNumber> StartOfYearDayNumberData =>
        _startOfYearDayNumberData ??= Inner.StartOfYearDayNumberData.WhereT(DataFilter.Filter);

    private DataGroup<YearDayNumber>? _endOfYearDayNumberData;
    public DataGroup<YearDayNumber> EndOfYearDayNumberData =>
        _endOfYearDayNumberData ??= Inner.EndOfYearDayNumberData.WhereT(DataFilter.Filter);

    #endregion
    #region ICalendricalDataSet

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    private DataGroup<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??= Inner.DaysSinceEpochInfoData.WhereT(DataFilter.Filter);

    private DataGroup<DateInfo>? _dateInfoData;
    public DataGroup<DateInfo> DateInfoData =>
        _dateInfoData ??= Inner.DateInfoData.WhereT(DataFilter.Filter);

    private DataGroup<MonthInfo>? _monthInfoData;
    public DataGroup<MonthInfo> MonthInfoData =>
        _monthInfoData ??= Inner.MonthInfoData.WhereT(DataFilter.Filter);

    private DataGroup<YearInfo>? _yearInfoData;
    public DataGroup<YearInfo> YearInfoData =>
        _yearInfoData ??= Inner.YearInfoData.WhereT(DataFilter.Filter);

    private DataGroup<CenturyInfo>? _centuryInfoData;
    public DataGroup<CenturyInfo> CenturyInfoData =>
        _centuryInfoData ??= Inner.CenturyInfoData.WhereT(DataFilter.Filter);

    private DataGroup<YemodaAnd<int>>? _daysInYearAfterDateDataInit;
    public DataGroup<YemodaAnd<int>> DaysInYearAfterDateData =>
        _daysInYearAfterDateDataInit ??= Inner.DaysInYearAfterDateData.WhereT(DataFilter.Filter);

    private DataGroup<YemodaAnd<int>>? _daysInMonthAfterDateData;
    public DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData =>
        _daysInMonthAfterDateData ??= Inner.DaysInMonthAfterDateData.WhereT(DataFilter.Filter);

    private DataGroup<Yemoda>? _startOfYearPartsData;
    public DataGroup<Yemoda> StartOfYearPartsData =>
        _startOfYearPartsData ??= Inner.StartOfYearPartsData.WhereT(DataFilter.Filter);

    private DataGroup<Yemoda>? _endOfYearPartsData;
    public DataGroup<Yemoda> EndOfYearPartsData =>
        _endOfYearPartsData ??= Inner.EndOfYearPartsData.WhereT(DataFilter.Filter);

    private DataGroup<YearDaysSinceEpoch>? _startOfYearDaysSinceEpochData;
    public DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData =>
        _startOfYearDaysSinceEpochData ??= Inner.StartOfYearDaysSinceEpochData.WhereT(DataFilter.Filter);

    private DataGroup<YearDaysSinceEpoch>? _endOfYearDaysSinceEpochData;
    public DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        _endOfYearDaysSinceEpochData ??= Inner.EndOfYearDaysSinceEpochData.WhereT(DataFilter.Filter);

    // Normally, we don't have to filter the three following properties.
    public TheoryData<int, int> InvalidMonthFieldData => Inner.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => Inner.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => Inner.InvalidDayOfYearFieldData;

    #endregion
}
