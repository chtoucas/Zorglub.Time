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

    public abstract TheoryData<DayNumberInfo> DayNumberInfoData { get; }

    private TheoryData<YearDayNumber>? _startOfYearDayNumberData;
    public TheoryData<YearDayNumber> StartOfYearDayNumberData =>
        _startOfYearDayNumberData ??= MapToYearDayNumberData(DataSet.StartOfYearDaysSinceEpochData, Epoch);

    private TheoryData<YearDayNumber>? _endOfYearDayNumberData;
    public TheoryData<YearDayNumber> EndOfYearDayNumberData =>
        _endOfYearDayNumberData ??= GetEndOfYearFromStartOfYear(StartOfYearDayNumberData);

    #region ICalendricalDataSet

    public int SampleCommonYear => DataSet.SampleCommonYear;
    public int SampleLeapYear => DataSet.SampleLeapYear;

    public TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData => DataSet.DaysSinceEpochInfoData;

    public TheoryData<DateInfo> DateInfoData => DataSet.DateInfoData;
    public TheoryData<MonthInfo> MonthInfoData => DataSet.MonthInfoData;
    public TheoryData<YearInfo> YearInfoData => DataSet.YearInfoData;
    public TheoryData<CenturyInfo> CenturyInfoData => DataSet.CenturyInfoData;

    public TheoryData<Yemoda> StartOfYearPartsData => DataSet.StartOfYearPartsData;
    public TheoryData<Yemoda> EndOfYearPartsData => DataSet.EndOfYearPartsData;

    public TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => DataSet.StartOfYearDaysSinceEpochData;
    public TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => DataSet.EndOfYearDaysSinceEpochData;

    public TheoryData<int, int> InvalidMonthFieldData => DataSet.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => DataSet.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => DataSet.InvalidDayOfYearFieldData;

    public TheoryData<YemodaAnd<int>> DaysInYearAfterDateData => DataSet.DaysInYearAfterDateData;
    public TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData => DataSet.DaysInMonthAfterDateData;

    #endregion
    #region Helpers

    /// <summary>
    /// Converts a set of data of type <see cref="YearDaysSinceEpoch"/>to a set of data of type
    /// <see cref="YearDayNumber"/>.
    /// </summary>
    [Pure]
    private static TheoryData<YearDayNumber> MapToYearDayNumberData(
        TheoryData<YearDaysSinceEpoch> source, DayNumber epoch)
    {
        Requires.NotNull(source);

        var data = new TheoryData<YearDayNumber>();
        foreach (var item in source)
        {
            var (y, daysSinceEpoch) = (YearDaysSinceEpoch)item[0];
            data.Add(new YearDayNumber(y, epoch + daysSinceEpoch));
        }
        return data;
    }

    [Pure]
    private static TheoryData<YearDayNumber> GetEndOfYearFromStartOfYear(TheoryData<YearDayNumber> source)
    {
        Requires.NotNull(source);

        var data = new TheoryData<YearDayNumber>();
        foreach (var item in source)
        {
            var (y, dayNumber) = (YearDayNumber)item[0];
            data.Add(new YearDayNumber(y - 1, dayNumber - 1));
        }
        return data;
    }

    #endregion
}
