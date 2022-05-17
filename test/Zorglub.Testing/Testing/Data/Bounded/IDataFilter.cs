// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Bounded;

public interface IDataFilter
{
    bool Filter(Yemoda x);

    bool Filter(YearDaysSinceEpoch x);
    bool Filter(YearDayNumber x);

    bool Filter(DaysSinceEpochInfo x);
    bool Filter(DayNumberInfo x);

    bool Filter(DateInfo x);
    bool Filter(MonthInfo x);
    bool Filter(YearInfo x);
    bool Filter(CenturyInfo x);

    bool Filter(EpagomenalDayInfo x);

    bool Filter<T>(YemodaAnd<T> x) where T : struct;
    bool Filter<T>(YemoAnd<T> x) where T : struct;
}
