// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// We rely on Yemoda/Yedoy/Yemo to define our test data, which means that we
// cannot use (and test) parts (year, month, day, dayOfYear) outside the ranges
// supported by these types.

public interface ICalendricalDataSet
{
    /// <summary>Gets a sample common year.</summary>
    int SampleCommonYear { get; }

    /// <summary>Gets a sample leap year.</summary>
    int SampleLeapYear { get; }

    /// <summary>Number of consecutive days from the epoch to a date.</summary>
    DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; }

    /// <summary>Date informations.</summary>
    DataGroup<DateInfo> DateInfoData { get; }
    /// <summary>Month informations.</summary>
    DataGroup<MonthInfo> MonthInfoData { get; }
    /// <summary>Year informations.</summary>
    DataGroup<YearInfo> YearInfoData { get; }
    /// <summary>Century informations.</summary>
    DataGroup<CenturyInfo> CenturyInfoData { get; }

    // No DaysInYearBeforeMonthData, the information is given by MonthInfo.
    /// <summary>Month, daysInYearAfterMonth.</summary>
    DataGroup<YemoAnd<int>> DaysInYearAfterMonthData { get; }
    /// <summary>Date, daysInYearAfter.</summary>
    DataGroup<YemodaAnd<int>> DaysInYearAfterDateData { get; }
    /// <summary>Date, daysInMonthAfter.</summary>
    DataGroup<YemodaAnd<int>> DaysInMonthAfterDateData { get; }

    /// <summary>Date at the start of a year.</summary>
    DataGroup<Yemoda> StartOfYearPartsData { get; }
    /// <summary>Date at the end of a year.</summary>
    DataGroup<Yemoda> EndOfYearPartsData { get; }

    /// <summary>Year, number of consecutive days from the epoch to the start of the year.</summary>
    DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; }
    /// <summary>Year, number of consecutive days from the epoch to the end of the year.</summary>
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
