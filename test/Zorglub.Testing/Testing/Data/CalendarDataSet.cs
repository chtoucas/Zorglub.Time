// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Provides test data for an <i>unbounded</i> calendar and related date types.
/// </summary>
public abstract class CalendarDataSet<TDataSet> : ICalendarDataSet
    where TDataSet : ICalendricalDataSet
{
    protected CalendarDataSet(TDataSet dataSet, DayNumber epoch)
    {
        DataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        Epoch = epoch;
    }

    /// <summary>
    /// Gets the calendrical dataset.
    /// </summary>
    public TDataSet DataSet { get; }

    public DayNumber Epoch { get; }

    public abstract DataGroup<DayNumberInfo> DayNumberInfoData { get; }

    private DataGroup<YearDayNumber>? _startOfYearDayNumberData;
    public DataGroup<YearDayNumber> StartOfYearDayNumberData =>
        _startOfYearDayNumberData ??= InitDayNumberData(DataSet.StartOfYearDaysSinceEpochData);

    private DataGroup<YearDayNumber>? _endOfYearDayNumberData;
    public DataGroup<YearDayNumber> EndOfYearDayNumberData =>
        _endOfYearDayNumberData ??= InitDayNumberData(DataSet.EndOfYearDaysSinceEpochData);

    //
    // Affine data
    //

    public int SampleCommonYear => DataSet.SampleCommonYear;
    public int SampleLeapYear => DataSet.SampleLeapYear;

    public DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData => DataSet.DaysSinceEpochInfoData;

    public DataGroup<DateInfo> DateInfoData => DataSet.DateInfoData;
    public DataGroup<MonthInfo> MonthInfoData => DataSet.MonthInfoData;
    public DataGroup<YearInfo> YearInfoData => DataSet.YearInfoData;
    public DataGroup<CenturyInfo> CenturyInfoData => DataSet.CenturyInfoData;

    public DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => DataSet.DaysInYearAfterDateData;
    public DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => DataSet.DaysInMonthAfterDateData;

    public DataGroup<Yemoda> StartOfYearPartsData => DataSet.StartOfYearPartsData;
    public DataGroup<Yemoda> EndOfYearPartsData => DataSet.EndOfYearPartsData;

    public DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => DataSet.StartOfYearDaysSinceEpochData;
    public DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => DataSet.EndOfYearDaysSinceEpochData;

    public TheoryData<int, int> InvalidMonthFieldData => DataSet.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => DataSet.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => DataSet.InvalidDayOfYearFieldData;

    #region Helpers

    [Pure]
    private DataGroup<YearDayNumber> InitDayNumberData(DataGroup<YearDaysSinceEpoch> source)
    {
        Debug.Assert(source != null);

        var epoch = Epoch;
        return source.SelectT(Selector);

        YearDayNumber Selector(YearDaysSinceEpoch x) => x.ToYearDayNumber(epoch);
    }

    #endregion
}
