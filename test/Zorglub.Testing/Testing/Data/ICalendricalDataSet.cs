// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// We rely on Yemoda/Yedoy/Yemo to define our test data, which means that we
// cannot use (and test) parts (year, month, day, dayOfYear) outside the ranges
// supported by these types.

public interface ICalendricalDataSet
{
    /// <summary>
    /// Gets a sample common year.
    /// </summary>
    int SampleCommonYear { get; }

    /// <summary>
    /// Gets a sample leap year.
    /// </summary>
    int SampleLeapYear { get; }

    DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    DataGroup<DateInfo> DateInfoData { get; }
    DataGroup<MonthInfo> MonthInfoData { get; }
    DataGroup<YearInfo> YearInfoData { get; }
    DataGroup<CenturyInfo> CenturyInfoData { get; }

    DataGroup<YemodaAnd<int>> DaysInYearAfterDateData { get; }
    DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData { get; }

    DataGroup<Yemoda> StartOfYearPartsData { get; }
    DataGroup<Yemoda> EndOfYearPartsData { get; }

    DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }
    DataGroup<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData { get; }

    // NB: for the next three properties, don't use Yemoda, Yemo or Yedoy;
    // any integer value may be used.
    // We use TheoryData, not DataGroup, ebcause we shouldn't have to filter
    // these properties.

    /// <summary>Year, month; ONLY the latter is invalid.</summary>
    TheoryData<int, int> InvalidMonthFieldData { get; }
    /// <summary>Year, month, day; ONLY the latter is invalid.</summary>
    TheoryData<int, int, int> InvalidDayFieldData { get; }
    /// <summary>Year, dayOfYear; ONLY the latter is invalid.</summary>
    TheoryData<int, int> InvalidDayOfYearFieldData { get; }
}
