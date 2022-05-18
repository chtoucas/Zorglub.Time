// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

public interface IYearAdjustmentDataSet
{
    DataGroup<YemodaAnd<int>> InvalidYearAdjustementData { get; }
    DataGroup<YemodaAnd<int>> YearAdjustementData { get; }
}

public interface IMathDataSet
{
    DataGroup<YemodaPairAnd<int>> AddDaysData { get; }
    DataGroup<YemodaPair> ConsecutiveDaysData { get; }
}

public interface IAdvancedMathDataSet : IMathDataSet
{
    TheoryData<Yemoda, Yemoda, int> AddYearsData { get; }
    TheoryData<Yemoda, Yemoda, int> AddMonthsData { get; }
    TheoryData<Yemoda, Yemoda, int, int, int> DiffData { get; }
}

public interface IEpagomenalDataSet
{
    DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; }
}
