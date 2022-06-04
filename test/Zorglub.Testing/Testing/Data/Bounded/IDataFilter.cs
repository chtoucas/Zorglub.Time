// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

/// <summary>
/// Defines a data filter.
/// </summary>
public interface IDataFilter
{
    bool Filter(Yemoda x);
    bool Filter(Yemo x);
    bool Filter(Yedoy x);

    bool Filter(YearDaysSinceEpoch x);
    bool Filter(YearDayNumber x);

    bool Filter(DaysSinceEpochInfo x);
    bool Filter(DayNumberInfo x);

    bool Filter(DaysSinceEpochYewedaInfo x);
    bool Filter(DayNumberYewedaInfo x);

    bool Filter(DateInfo x);
    bool Filter(MonthInfo x);
    bool Filter(YearInfo x);
    bool Filter(CenturyInfo x);

    bool Filter<T>(YemodaAnd<T> x) where T : struct;
    bool Filter<T>(YemoAnd<T> x) where T : struct;
    bool Filter<T1, T2>(YemoAnd<T1, T2> x) where T1 : struct where T2 : struct;

    bool Filter(YemodaPair x);
    bool Filter<T>(YemodaPairAnd<T> x) where T : struct;

    bool Filter(YedoyPair x);
    bool Filter<T>(YedoyPairAnd<T> x) where T : struct;

    bool Filter(YemoPair x);

    bool Filter<T>(YearAnd<T> x) where T : struct;

    bool Filter(DateDiff x);
}
