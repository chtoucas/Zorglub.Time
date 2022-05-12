// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// TODO(data): Move DaysInYearAfterMonthData to MonthInfoData.
public interface IDaysAfterDataSet
{
    // CountDaysInYear(y) - CountDaysInMonth(y, m) - CountDaysInYearBeforeMonth(y, m);
    TheoryData<YemoAnd<int>> DaysInYearAfterMonthData { get; }
}

public interface IYearAdjustmentDataSet
{
    TheoryData<YemodaAnd<int>> InvalidYearAdjustementData { get; }
    TheoryData<YemodaAnd<int>> YearAdjustementData { get; }
}

public interface IMathDataSet
{
    TheoryData<Yemoda, Yemoda, int> AddDaysData { get; }
    TheoryData<Yemoda, Yemoda> ConsecutiveDaysData { get; }
}

public interface IAdvancedMathDataSet : IMathDataSet
{
    TheoryData<Yemoda, Yemoda, int> AddYearsData { get; }
    TheoryData<Yemoda, Yemoda, int> AddMonthsData { get; }
    TheoryData<Yemoda, Yemoda, int, int, int> DiffData { get; }
}

public interface IEpagomenalDataSet
{
    TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData { get; }
}
