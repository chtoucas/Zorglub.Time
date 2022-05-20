// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

/// <summary>
/// Defines test data for a <i>bounded</i> calendar and provides a base for derived classes.
/// </summary>
/// <typeparam name="TDataSet">The type that represents the original calendar dataset.</typeparam>
public class BoundedCalendarDataSet<TDataSet> : ICalendarDataSet
    where TDataSet : UnboundedCalendarDataSet
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

    public DayNumber Epoch { get; }

    public DataGroup<DayNumberInfo> DayNumberInfoData => Inner.DayNumberInfoData.WhereT(DataFilter.Filter);

    public DataGroup<YearDayNumber> StartOfYearDayNumberData => Inner.StartOfYearDayNumberData.WhereT(DataFilter.Filter);
    public DataGroup<YearDayNumber> EndOfYearDayNumberData => Inner.EndOfYearDayNumberData.WhereT(DataFilter.Filter);

    //
    // Affine data
    //

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData => Inner.DaysSinceEpochInfoData.WhereT(DataFilter.Filter);

    public DataGroup<DateInfo> DateInfoData => Inner.DateInfoData.WhereT(DataFilter.Filter);
    public DataGroup<MonthInfo> MonthInfoData => Inner.MonthInfoData.WhereT(DataFilter.Filter);
    public DataGroup<YearInfo> YearInfoData => Inner.YearInfoData.WhereT(DataFilter.Filter);
    public DataGroup<CenturyInfo> CenturyInfoData => Inner.CenturyInfoData.WhereT(DataFilter.Filter);

    public DataGroup<YemodaAnd<int>> DaysInYearAfterDateData => Inner.DaysInYearAfterDateData.WhereT(DataFilter.Filter);
    public DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData => Inner.DaysInMonthAfterDateData.WhereT(DataFilter.Filter);

    public DataGroup<Yemoda> StartOfYearPartsData => Inner.StartOfYearPartsData.WhereT(DataFilter.Filter);
    public DataGroup<Yemoda> EndOfYearPartsData => Inner.EndOfYearPartsData.WhereT(DataFilter.Filter);

    public DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData => Inner.StartOfYearDaysSinceEpochData.WhereT(DataFilter.Filter);
    public DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData => Inner.EndOfYearDaysSinceEpochData.WhereT(DataFilter.Filter);

    // Normally, we don't have to filter the three following properties.
    public TheoryData<int, int> InvalidMonthFieldData => Inner.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => Inner.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => Inner.InvalidDayOfYearFieldData;
}
