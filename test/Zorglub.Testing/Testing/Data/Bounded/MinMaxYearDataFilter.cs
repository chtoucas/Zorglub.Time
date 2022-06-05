// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

using Zorglub.Time.Core.Intervals;

public sealed class MinMaxYearDataFilter : IDataFilter
{
    public MinMaxYearDataFilter(Range<int> supportedYears)
    {
        SupportedYears = supportedYears;
    }

    public Range<int> SupportedYears { get; }

    public bool Filter(Yemoda x) => SupportedYears.Contains(x.Year);
    public bool Filter(Yemo x) => SupportedYears.Contains(x.Year);
    public bool Filter(Yedoy x) => SupportedYears.Contains(x.Year);

    public bool Filter(YearDaysSinceEpoch x) => SupportedYears.Contains(x.Year);
    public bool Filter(YearDayNumber x) => SupportedYears.Contains(x.Year);

    public bool Filter(DaysSinceEpochInfo x) => SupportedYears.Contains(x.Yemoda.Year);
    public bool Filter(DayNumberInfo x) => SupportedYears.Contains(x.Yemoda.Year);

    public bool Filter(DaysSinceEpochYewedaInfo x) => SupportedYears.Contains(x.Yewe.Year);
    public bool Filter(DayNumberYewedaInfo x) => SupportedYears.Contains(x.Yewe.Year);

    public bool Filter(DateInfo x) => SupportedYears.Contains(x.Yemoda.Year);
    public bool Filter(MonthInfo x) => SupportedYears.Contains(x.Yemo.Year);
    public bool Filter(YearInfo x) => SupportedYears.Contains(x.Year);
    public bool Filter(CenturyInfo x) => SupportedYears.Contains(x.Year);

    public bool Filter<T>(YemodaAnd<T> x) where T : struct => SupportedYears.Contains(x.Yemoda.Year);
    public bool Filter<T>(YemoAnd<T> x) where T : struct => SupportedYears.Contains(x.Yemo.Year);
    public bool Filter<T1, T2>(YemoAnd<T1, T2> x) where T1 : struct where T2 : struct =>
        SupportedYears.Contains(x.Yemo.Year);

    public bool Filter(YemodaPair x) =>
        SupportedYears.Contains(x.First.Year) && SupportedYears.Contains(x.Second.Year);
    public bool Filter<T>(YemodaPairAnd<T> x) where T : struct =>
        SupportedYears.Contains(x.First.Year) && SupportedYears.Contains(x.Second.Year);

    public bool Filter(YedoyPair x) =>
        SupportedYears.Contains(x.First.Year) && SupportedYears.Contains(x.Second.Year);
    public bool Filter<T>(YedoyPairAnd<T> x) where T : struct =>
        SupportedYears.Contains(x.First.Year) && SupportedYears.Contains(x.Second.Year);

    public bool Filter(YemoPair x) =>
        SupportedYears.Contains(x.First.Year) && SupportedYears.Contains(x.Second.Year);
    public bool Filter<T>(YemoPairAnd<T> x) where T : struct =>
        SupportedYears.Contains(x.First.Year) && SupportedYears.Contains(x.Second.Year);

    public bool Filter<T>(YearAnd<T> x) where T : struct => SupportedYears.Contains(x.Year);

    public bool Filter(DateDiff x) =>
        x is not null && SupportedYears.Contains(x.Start.Year) && SupportedYears.Contains(x.End.Year);
}
