// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// TODO(data): Filter supplementary data: see XXXGregorianDataSet.
// GregorianMinMaxYearCalendarTests à améliorer
// Do we still need Start/EndOfYearDayNumberData?
// Move from List<> to IEnumerable<> for intermediate data.

// What's in ICalendarDataSet? Everything related to DayNumber, including the
// Epoch and DayOfWeek.

public interface ICalendarDataSet : ICalendricalDataSet
{
    /// <summary>
    /// Gets the epoch.
    /// </summary>
    DayNumber Epoch { get; }

    TheoryData<DayNumberInfo> DayNumberInfoData { get; }

    TheoryData<YearDayNumber> StartOfYearDayNumberData { get; }
    TheoryData<YearDayNumber> EndOfYearDayNumberData { get; }
}
