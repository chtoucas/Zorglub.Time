// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#pragma warning disable IDE1006 // Naming Styles

namespace Zorglub.Testing.Data.Bounded;

using System.Linq;

using static Zorglub.Testing.Data.Extensions.TheoryDataHelpers;

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

        return q.ToTheoryData();
    }

    private static InvalidOperationException BadProperty(string propertyName) =>
        new($"The feature can not be tested: the property \"{propertyName}\" has not been initialized.");

    #region ICalendarDataSet

    public DayNumber Epoch { get; }

    public TheoryData<DayNumberInfo> DayNumberInfoData =>
        _DayNumberInfoData ?? throw BadProperty(nameof(_DayNumberInfoData));

    public TheoryData<YearDayNumber> StartOfYearDayNumberData =>
        _StartOfYearDayNumberData ?? throw BadProperty(nameof(StartOfYearDayNumberData));
    public TheoryData<YearDayNumber> EndOfYearDayNumberData =>
        _EndOfYearDayNumberData ?? throw BadProperty(nameof(_EndOfYearDayNumberData));

    protected TheoryData<DayNumberInfo>? _DayNumberInfoData { get; init; }
    protected TheoryData<YearDayNumber>? _StartOfYearDayNumberData { get; init; }
    protected TheoryData<YearDayNumber>? _EndOfYearDayNumberData { get; init; }

    #endregion
    #region ICalendricalDataSet

    public int SampleCommonYear { get; }
    public int SampleLeapYear { get; }

    public TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _DaysSinceEpochInfoData ?? throw BadProperty(nameof(_DaysSinceEpochInfoData));

    public TheoryData<DateInfo> DateInfoData =>
        _DateInfoData ?? throw BadProperty(nameof(_DateInfoData));
    public TheoryData<MonthInfo> MonthInfoData =>
        _MonthInfoData ?? throw BadProperty(nameof(_MonthInfoData));
    public TheoryData<YearInfo> YearInfoData =>
        _YearInfoData ?? throw BadProperty(nameof(_YearInfoData));
    public TheoryData<CenturyInfo> CenturyInfoData =>
        _CenturyInfoData ?? throw BadProperty(nameof(_CenturyInfoData));

    public TheoryData<YemodaAnd<int>> DaysInYearAfterDateData =>
        _DaysInYearAfterDateData ?? throw BadProperty(nameof(_DaysInYearAfterDateData));
    public TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        _DaysInMonthAfterDateData ?? throw BadProperty(nameof(_DaysInMonthAfterDateData));

    public TheoryData<Yemoda> StartOfYearPartsData =>
        _StartOfYearPartsData ?? throw BadProperty(nameof(_StartOfYearPartsData));
    public TheoryData<Yemoda> EndOfYearPartsData =>
        _EndOfYearPartsData ?? throw BadProperty(nameof(_EndOfYearPartsData));

    public TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData =>
        _StartOfYearDaysSinceEpochData ?? throw BadProperty(nameof(_StartOfYearDaysSinceEpochData));
    public TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData =>
        _EndOfYearDaysSinceEpochData ?? throw BadProperty(nameof(_EndOfYearDaysSinceEpochData));

    // Normally, we don't have to filter the three following properties.
    public TheoryData<int, int> InvalidMonthFieldData => Inner.InvalidMonthFieldData;
    public TheoryData<int, int, int> InvalidDayFieldData => Inner.InvalidDayFieldData;
    public TheoryData<int, int> InvalidDayOfYearFieldData => Inner.InvalidDayOfYearFieldData;

    protected TheoryData<DaysSinceEpochInfo>? _DaysSinceEpochInfoData { get; init; }
    protected TheoryData<DateInfo>? _DateInfoData { get; init; }
    protected TheoryData<MonthInfo>? _MonthInfoData { get; init; }
    protected TheoryData<YearInfo>? _YearInfoData { get; init; }
    protected TheoryData<CenturyInfo>? _CenturyInfoData { get; init; }
    protected TheoryData<YemodaAnd<int>>? _DaysInYearAfterDateData { get; init; }
    protected TheoryData<YemodaAnd<int>>? _DaysInMonthAfterDateData { get; init; }
    protected TheoryData<Yemoda>? _StartOfYearPartsData { get; init; }
    protected TheoryData<Yemoda>? _EndOfYearPartsData { get; init; }
    protected TheoryData<YearDaysSinceEpoch>? _StartOfYearDaysSinceEpochData { get; init; }
    protected TheoryData<YearDaysSinceEpoch>? _EndOfYearDaysSinceEpochData { get; init; }

    #endregion
}
