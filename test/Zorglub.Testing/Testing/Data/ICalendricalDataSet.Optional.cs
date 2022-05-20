// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

public interface IYearAdjustmentDataSet
{
    /// <summary>Date, invalid target year with the same month and day parts.</summary>
    DataGroup<YemodaAnd<int>> InvalidYearAdjustementData { get; }
    /// <summary>Date, valid target year with the same month and day parts.</summary>
    DataGroup<YemodaAnd<int>> YearAdjustementData { get; }
}

public interface IMathDataSet
{
    /// <summary>Date, expected result, days to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddDaysData { get; }

    /// <summary>Date, date after.</summary>
    DataGroup<YemodaPair> ConsecutiveDaysData { get; }
}

public interface IAdvancedMathDataSet
{
    /// <summary>Date, expected result, years to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddYearsData { get; }

    /// <summary>Date, expected result, months to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddMonthsData { get; }

    /// <summary>Start date, end date, exact diff between.</summary>
    DataGroup<DateDiff> DateDiffData { get; }
}

public interface IEpagomenalDataSet
{
    /// <summary>Date, epagomenal number.</summary>
    DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; }
}
