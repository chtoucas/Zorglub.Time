// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// TODO(data): one interface per rule? Separate data for subtraction?
// To be used as a provider for -ambiguous- data.
public interface IAdvancedMathDataSet
{
    /// <summary>
    /// Gets the rules employed to resolve ambiguities.
    /// </summary>
    AdditionRules AdditionRules { get; }

    /// <summary>Date, expected result, years to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddYearsData { get; }
    /// <summary>Date, expected result, months to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddMonthsData { get; }
    /// <summary>Date, expected result, years to be added.</summary>
    DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData { get; }

    ///// <summary>Start date, end date, exact diff between.</summary>
    //DataGroup<DateDiff> DateDiffData { get; }
}

public interface IEpagomenalDataSet
{
    /// <summary>Date, epagomenal number.</summary>
    DataGroup<YemodaAnd<int>> EpagomenalDayInfoData { get; }
}
