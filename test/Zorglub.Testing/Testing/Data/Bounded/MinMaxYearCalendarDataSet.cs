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

        DaysSinceEpochInfoDataInit = Filter(Inner.DaysSinceEpochInfoData, Filter);
        DayNumberInfoDataInit = Filter(Inner.DayNumberInfoData, Filter);

        DateInfoDataInit = Filter(Inner.DateInfoData, Filter);
        MonthInfoDataInit = Filter(Inner.MonthInfoData, Filter);
        YearInfoDataInit = Filter(Inner.YearInfoData, Filter);
        CenturyInfoDataInit = Filter(Inner.CenturyInfoData, Filter);

        DaysInYearAfterDateDataInit = Filter(Inner.DaysInYearAfterDateData, Filter);
        DaysInMonthAfterDateDataInit = Filter(Inner.DaysInMonthAfterDateData, Filter);

        StartOfYearPartsDataInit = Filter(Inner.StartOfYearPartsData, Filter);
        EndOfYearPartsDataInit = Filter(Inner.EndOfYearPartsData, Filter);

        StartOfYearDaysSinceEpochDataInit = Filter(Inner.StartOfYearDaysSinceEpochData, Filter);
        EndOfYearDaysSinceEpochDataInit = Filter(Inner.EndOfYearDaysSinceEpochData, Filter);

        StartOfYearDayNumberDataInit = Filter(Inner.StartOfYearDayNumberData, Filter);
        EndOfYearDayNumberDataInit = Filter(Inner.EndOfYearDayNumberData, Filter);
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
