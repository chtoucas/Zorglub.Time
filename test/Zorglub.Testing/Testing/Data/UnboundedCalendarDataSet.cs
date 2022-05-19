// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

/// <summary>
/// Defines test data for a <i>unbounded</i> calendar and provides a base for derived classes.
/// </summary>
/// <typeparam name="TCalendricalDataSet">The type that represents the calendrical dataset.</typeparam>
public abstract class UnboundedCalendarDataSet<TCalendricalDataSet> : ICalendarDataSet
    where TCalendricalDataSet : ICalendricalDataSet
{
    protected UnboundedCalendarDataSet(TCalendricalDataSet dataSet, DayNumber epoch)
    {
        CalendricalDataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        Epoch = epoch;
    }

    /// <summary>
    /// Gets the calendrical dataset.
    /// </summary>
    public TCalendricalDataSet CalendricalDataSet { get; }

    public DayNumber Epoch { get; }

    public abstract DataGroup<DayNumberInfo> DayNumberInfoData { get; }

    public DataGroup<YearDayNumber> StartOfYearDayNumberData =>
        MapToDayNumberData(CalendricalDataSet.StartOfYearDaysSinceEpochData);

    public DataGroup<YearDayNumber> EndOfYearDayNumberData =>
        MapToDayNumberData(CalendricalDataSet.EndOfYearDaysSinceEpochData);

    //
    // Affine data
    //

    public int SampleCommonYear => CalendricalDataSet.SampleCommonYear;
    public int SampleLeapYear => CalendricalDataSet.SampleLeapYear;

    public DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData => CalendricalDataSet.DaysSinceEpochInfoData;

    public DataGroup<DateInfo> DateInfoData => CalendricalDataSet.DateInfoData;
    public DataGroup<MonthInfo> MonthInfoData => CalendricalDataSet.MonthInfoData;
    public DataGroup<YearInfo> YearInfoData => CalendricalDataSet.YearInfoData;
    public DataGroup<CenturyInfo> CenturyInfoData => CalendricalDataSet.CenturyInfoData;

    public DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => CalendricalDataSet.DaysInYearAfterDateData;
    public DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => CalendricalDataSet.DaysInMonthAfterDateData;

    public DataGroup<Yemoda> StartOfYearPartsData => CalendricalDataSet.StartOfYearPartsData;
    public DataGroup<Yemoda> EndOfYearPartsData => CalendricalDataSet.EndOfYearPartsData;

    public DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => CalendricalDataSet.StartOfYearDaysSinceEpochData;
    public DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => CalendricalDataSet.EndOfYearDaysSinceEpochData;

    public TheoryData<int, int> InvalidMonthFieldData => CalendricalDataSet.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => CalendricalDataSet.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => CalendricalDataSet.InvalidDayOfYearFieldData;

    //
    // Helpers
    //

    [Pure]
    private DataGroup<YearDayNumber> MapToDayNumberData(DataGroup<YearDaysSinceEpoch> source)
    {
        Requires.NotNull(source);

        var epoch = Epoch;
        return source.SelectT(Selector);

        YearDayNumber Selector(YearDaysSinceEpoch x) => x.ToYearDayNumber(epoch);
    }
}
