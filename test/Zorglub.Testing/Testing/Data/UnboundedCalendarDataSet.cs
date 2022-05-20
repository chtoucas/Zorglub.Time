﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// For unbounded datasets, do not copy the static props from the calendrical
// dataset, it's unnecessary as one can always call them directly.

/// <summary>
/// Defines test data for a <i>unbounded</i> calendar and provides a base for derived classes.
/// </summary>
/// <typeparam name="TCalendricalDataSet">The type that represents the calendrical dataset.</typeparam>
public abstract class UnboundedCalendarDataSet<TCalendricalDataSet> : UnboundedCalendarDataSet
    where TCalendricalDataSet : ICalendricalDataSet
{
    protected UnboundedCalendarDataSet(TCalendricalDataSet dataSet, DayNumber epoch)
        : base(dataSet, epoch)
    {
        Debug.Assert(dataSet != null);

        CalendricalDataSet = dataSet;
    }

    /// <summary>
    /// Gets the calendrical dataset.
    /// </summary>
    public TCalendricalDataSet CalendricalDataSet { get; }
}

/// <summary>
/// Defines test data for a <i>unbounded</i> calendar and provides a base for derived classes.
/// </summary>
public abstract class UnboundedCalendarDataSet : ICalendarDataSet
{
    /// <summary>
    /// Represents the calendrical dataset.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly ICalendricalDataSet _dataSet;

    protected UnboundedCalendarDataSet(ICalendricalDataSet dataSet, DayNumber epoch)
    {
        _dataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        Epoch = epoch;
    }

    public DayNumber Epoch { get; }

    public abstract DataGroup<DayNumberInfo> DayNumberInfoData { get; }

    public DataGroup<YearDayNumber> StartOfYearDayNumberData =>
        MapToDayNumberData(_dataSet.StartOfYearDaysSinceEpochData);

    public DataGroup<YearDayNumber> EndOfYearDayNumberData =>
        MapToDayNumberData(_dataSet.EndOfYearDaysSinceEpochData);

    //
    // Affine data
    //

    public int SampleCommonYear => _dataSet.SampleCommonYear;
    public int SampleLeapYear => _dataSet.SampleLeapYear;

    public DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData => _dataSet.DaysSinceEpochInfoData;

    public DataGroup<DateInfo> DateInfoData => _dataSet.DateInfoData;
    public DataGroup<MonthInfo> MonthInfoData => _dataSet.MonthInfoData;
    public DataGroup<YearInfo> YearInfoData => _dataSet.YearInfoData;
    public DataGroup<CenturyInfo> CenturyInfoData => _dataSet.CenturyInfoData;

    public DataGroup<YemoAnd<int>> DaysInYearAfterMonthData => _dataSet.DaysInYearAfterMonthData;
    public DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => _dataSet.DaysInYearAfterDateData;
    public DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => _dataSet.DaysInMonthAfterDateData;

    public DataGroup<Yemoda> StartOfYearPartsData => _dataSet.StartOfYearPartsData;
    public DataGroup<Yemoda> EndOfYearPartsData => _dataSet.EndOfYearPartsData;

    public DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => _dataSet.StartOfYearDaysSinceEpochData;
    public DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => _dataSet.EndOfYearDaysSinceEpochData;

    public TheoryData<int, int> InvalidMonthFieldData => _dataSet.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => _dataSet.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => _dataSet.InvalidDayOfYearFieldData;

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
