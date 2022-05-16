// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using System.Linq;

//using static Zorglub.Testing.Data.Extensions.TheoryDataHelpers;

public class BoundedCalendarDataSet<TDataSet> : ICalendarDataSet
    where TDataSet : ICalendarDataSet
{
    public BoundedCalendarDataSet(TDataSet inner)
    {
        Inner = inner ?? throw new ArgumentNullException(nameof(inner));

        Epoch = Inner.Epoch;
        SampleCommonYear = Inner.SampleCommonYear;
        SampleLeapYear = Inner.SampleLeapYear;
    }

    /// <summary>
    /// Gets the original dataset.
    /// </summary>
    public TDataSet Inner { get; }

    protected static TheoryData<T> Filter<T>(TheoryData<T> data, Func<T, bool> filter)
    {
        var q = from item in data
                let value = (T)item[0]
                where filter(value)
                select value;

        Debug.Assert(q.Any());

        return TheoryDataFactories.Create(q);
    }

    private static InvalidOperationException BadProperty(string propertyName) =>
        new($"The feature can not be tested: the property \"{propertyName}\" has not been initialized.");

    #region ICalendarDataSet

    public DayNumber Epoch { get; }

    public TheoryData<DayNumberInfo> DayNumberInfoData =>
        DayNumberInfoDataInit ?? throw BadProperty(nameof(DayNumberInfoDataInit));

    public TheoryData<YearDayNumber> StartOfYearDayNumberData =>
        StartOfYearDayNumberDataInit ?? throw BadProperty(nameof(StartOfYearDayNumberData));
    public TheoryData<YearDayNumber> EndOfYearDayNumberData =>
        EndOfYearDayNumberDataInit ?? throw BadProperty(nameof(EndOfYearDayNumberDataInit));

    protected TheoryData<DayNumberInfo>? DayNumberInfoDataInit { get; init; }
    protected TheoryData<YearDayNumber>? StartOfYearDayNumberDataInit { get; init; }
    protected TheoryData<YearDayNumber>? EndOfYearDayNumberDataInit { get; init; }

    #endregion
    #region ICalendricalDataSet

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        DaysSinceEpochInfoDataInit ?? throw BadProperty(nameof(DaysSinceEpochInfoDataInit));

    public TheoryData<DateInfo> DateInfoData =>
        DateInfoDataInit ?? throw BadProperty(nameof(DateInfoDataInit));
    public TheoryData<MonthInfo> MonthInfoData =>
        MonthInfoDataInit ?? throw BadProperty(nameof(MonthInfoDataInit));
    public TheoryData<YearInfo> YearInfoData =>
        YearInfoDataInit ?? throw BadProperty(nameof(YearInfoDataInit));
    public TheoryData<CenturyInfo> CenturyInfoData =>
        CenturyInfoDataInit ?? throw BadProperty(nameof(CenturyInfoDataInit));

    public TheoryData<YemodaAnd<int>> DaysInYearAfterDateData =>
        DaysInYearAfterDateDataInit ?? throw BadProperty(nameof(DaysInYearAfterDateDataInit));
    public TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        DaysInMonthAfterDateDataInit ?? throw BadProperty(nameof(DaysInMonthAfterDateDataInit));

    public TheoryData<Yemoda> StartOfYearPartsData =>
        StartOfYearPartsDataInit ?? throw BadProperty(nameof(StartOfYearPartsDataInit));
    public TheoryData<Yemoda> EndOfYearPartsData =>
        EndOfYearPartsDataInit ?? throw BadProperty(nameof(EndOfYearPartsDataInit));

    public TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData =>
        StartOfYearDaysSinceEpochDataInit ?? throw BadProperty(nameof(StartOfYearDaysSinceEpochDataInit));
    public TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        EndOfYearDaysSinceEpochDataInit ?? throw BadProperty(nameof(EndOfYearDaysSinceEpochDataInit));

    // Normally, we don't have to filter the three following properties.
    public TheoryData<int, int> InvalidMonthFieldData => Inner.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => Inner.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => Inner.InvalidDayOfYearFieldData;

    protected TheoryData<DaysSinceEpochInfo>? DaysSinceEpochInfoDataInit { get; init; }
    protected TheoryData<DateInfo>? DateInfoDataInit { get; init; }
    protected TheoryData<MonthInfo>? MonthInfoDataInit { get; init; }
    protected TheoryData<YearInfo>? YearInfoDataInit { get; init; }
    protected TheoryData<CenturyInfo>? CenturyInfoDataInit { get; init; }
    protected TheoryData<YemodaAnd<int>>? DaysInYearAfterDateDataInit { get; init; }
    protected TheoryData<YemodaAnd<int>>? DaysInMonthAfterDateDataInit { get; init; }
    protected TheoryData<Yemoda>? StartOfYearPartsDataInit { get; init; }
    protected TheoryData<Yemoda>? EndOfYearPartsDataInit { get; init; }
    protected TheoryData<YearDaysSinceEpoch>? StartOfYearDaysSinceEpochDataInit { get; init; }
    protected TheoryData<YearDaysSinceEpoch>? EndOfYearDaysSinceEpochDataInit { get; init; }

    #endregion
}
