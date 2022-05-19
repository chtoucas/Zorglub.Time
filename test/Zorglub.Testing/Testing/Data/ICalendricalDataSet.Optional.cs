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
    /// <summary>
    /// Date, expected result, days to be added.
    /// </summary>
    DataGroup<YemodaPairAnd<int>> AddDaysData { get; }

    DataGroup<YemodaPair> ConsecutiveDaysData { get; }
}

public interface IAdvancedMathDataSet : IMathDataSet
{
    /// <summary>
    /// Date, expected result, years to be added.
    /// </summary>
    DataGroup<YemodaPairAnd<int>> AddYearsData { get; }

    /// <summary>
    /// Date, expected result, months to be added.
    /// </summary>
    DataGroup<YemodaPairAnd<int>> AddMonthsData { get; }

    TheoryData<Yemoda, Yemoda, int, int, int> DiffData { get; }
}

public interface IEpagomenalDataSet
{
    DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; }
}
