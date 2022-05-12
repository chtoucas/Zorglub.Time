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

    TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    TheoryData<DateInfo> DateInfoData { get; }
    TheoryData<MonthInfo> MonthInfoData { get; }
    TheoryData<YearInfo> YearInfoData { get; }
    TheoryData<CenturyInfo> CenturyInfoData { get; }

    TheoryData<YemodaAnd<int>> DaysInYearAfterDateData { get; }
    TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData { get; }

    TheoryData<Yemoda> StartOfYearPartsData { get; }
    TheoryData<Yemoda> EndOfYearPartsData { get; }

    TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }
    TheoryData<YearDaysSinceEpoch> EndOfYearDaysSinceEpochData { get; }

    // NB: for the next three properties, don't use Yemoda, Yemo or Yedoy;
    // any integer value may be used.

    /// <summary>Year, month; ONLY the latter is invalid.</summary>
    TheoryData<int, int> InvalidMonthFieldData { get; }
    /// <summary>Year, month, day; ONLY the latter is invalid.</summary>
    TheoryData<int, int, int> InvalidDayFieldData { get; }
    /// <summary>Year, dayOfYear; ONLY the latter is invalid.</summary>
    TheoryData<int, int> InvalidDayOfYearFieldData { get; }
}
