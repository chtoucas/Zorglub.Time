// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

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
    // We use TheoryData, not DataGroup, because we shouldn't have to filter
    // these properties.

    /// <summary>Year, month; ONLY the latter is invalid.</summary>
    TheoryData<int, int> InvalidMonthFieldData { get; }
    /// <summary>Year, month, day; ONLY the latter is invalid.</summary>
    TheoryData<int, int, int> InvalidDayFieldData { get; }
    /// <summary>Year, dayOfYear; ONLY the latter is invalid.</summary>
    TheoryData<int, int> InvalidDayOfYearFieldData { get; }

    // NB: no need to add the reverse operations (e.g. -days), it's done
    // automatically by the tests.

    /// <summary>Date, expected result, days to be added.</summary>
    DataGroup<YemodaPairAnd<int>> AddDaysData { get; }
    /// <summary>Date, date after.</summary>
    DataGroup<YemodaPair> ConsecutiveDaysData { get; }

    /// <summary>Ordinal date, expected result, days to be added.</summary>
    DataGroup<YedoyPairAnd<int>> AddDaysOrdinalData { get; }
    /// <summary>Ordinal date, date after.</summary>
    DataGroup<YedoyPair> ConsecutiveDaysOrdinalData { get; }

    /// <summary>Date, expected result, years to be added.</summary>
    /// <remarks>
    /// <para>This property SHOULD NOT contain any ambiguous data.</para>
    /// <para>We can re-use this property to test the difference in years between two dates because
    /// the data is NOT ambiguous.</para>
    /// </remarks>
    DataGroup<YemodaPairAnd<int>> AddYearsData { get; }
    /// <summary>Date, expected result, months to be added.</summary>
    /// <remarks>
    /// <para>This property SHOULD NOT contain any ambiguous data.</para>
    /// <para>We can re-use this property to test the difference in months between two dates because
    /// the data is NOT ambiguous.</para>
    /// </remarks>
    DataGroup<YemodaPairAnd<int>> AddMonthsData { get; }
    /// <summary>Date, expected result, years to be added.</summary>
    /// <remarks>
    /// <para>This property SHOULD NOT contain any ambiguous data.</para>
    /// <para>We can re-use this property to test the difference in years between two dates because
    /// the data is NOT ambiguous.</para>
    /// </remarks>
    DataGroup<YedoyPairAnd<int>> AddYearsOrdinalData { get; }
}
