// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Time.Core.Intervals;

public class MinMaxYearCalendarDataSet<TDataSet> : BoundedCalendarDataSet<TDataSet>
    where TDataSet : ICalendarDataSet
{
    public MinMaxYearCalendarDataSet(TDataSet inner, int minYear, int maxYear)
        : this(inner, Range.Create(minYear, maxYear)) { }

    public MinMaxYearCalendarDataSet(TDataSet inner, Range<int> supportedYears) : base(inner)
    {
        SupportedYears = supportedYears;

        if (supportedYears.Contains(inner.SampleCommonYear) == false)
        {
            throw new ArgumentException("SampleCommonYear is out of range", nameof(inner));
        }
        if (supportedYears.Contains(inner.SampleLeapYear) == false)
        {
            throw new ArgumentException("SampleLeapYear is out of range", nameof(inner));
        }

        _DaysSinceEpochInfoData = Filter(Inner.DaysSinceEpochInfoData, Filter);
        _DayNumberInfoData = Filter(Inner.DayNumberInfoData, Filter);

        _DateInfoData = Filter(Inner.DateInfoData, Filter);
        _MonthInfoData = Filter(Inner.MonthInfoData, Filter);
        _YearInfoData = Filter(Inner.YearInfoData, Filter);
        _CenturyInfoData = Filter(Inner.CenturyInfoData, Filter);

        _DaysInYearAfterDateData = Filter(Inner.DaysInYearAfterDateData, Filter);
        _DaysInMonthAfterDateData = Filter(Inner.DaysInMonthAfterDateData, Filter);

        _StartOfYearPartsData = Filter(Inner.StartOfYearPartsData, Filter);
        _EndOfYearPartsData = Filter(Inner.EndOfYearPartsData, Filter);

        _StartOfYearDaysSinceEpochData = Filter(Inner.StartOfYearDaysSinceEpochData, Filter);
        _EndOfYearDaysSinceEpochData = Filter(Inner.EndOfYearDaysSinceEpochData, Filter);

        _StartOfYearDayNumberData = Filter(Inner.StartOfYearDayNumberData, Filter);
        _EndOfYearDayNumberData = Filter(Inner.EndOfYearDayNumberData, Filter);
    }

    public Range<int> SupportedYears { get; }

    protected bool Filter(Yemoda x) => SupportedYears.Contains(x.Year);

    protected bool Filter(YearDaysSinceEpoch x) => SupportedYears.Contains(x.Year);
    protected bool Filter(YearDayNumber x) => SupportedYears.Contains(x.Year);

    protected bool Filter(DaysSinceEpochInfo x) => SupportedYears.Contains(x.Year);
    protected bool Filter(DayNumberInfo x) => SupportedYears.Contains(x.Year);

    protected bool Filter(DateInfo x) => SupportedYears.Contains(x.Yemoda.Year);
    protected bool Filter(MonthInfo x) => SupportedYears.Contains(x.Yemo.Year);
    protected bool Filter(YearInfo x) => SupportedYears.Contains(x.Year);
    protected bool Filter(CenturyInfo x) => SupportedYears.Contains(x.Year);

    protected bool Filter(EpagomenalDayInfo x) => SupportedYears.Contains(x.Year);

    protected bool Filter<T>(YemodaAnd<T> x) where T : struct => SupportedYears.Contains(x.Yemoda.Year);
    protected bool Filter<T>(YemoAnd<T> x) where T : struct => SupportedYears.Contains(x.Yemo.Year);
}
