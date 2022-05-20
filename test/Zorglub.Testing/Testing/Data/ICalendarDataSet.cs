// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

// REVIEW(data): do we still need Start/EndOfYearDayNumberData?

// What's in ICalendarDataSet? Everything related to DayNumber, including the
// Epoch and DayOfWeek.

public interface ICalendarDataSet : ICalendricalDataSet
{
    /// <summary>Gets the epoch.</summary>
    DayNumber Epoch { get; }

    /// <summary>Day number of a date.</summary>
    DataGroup<DayNumberInfo> DayNumberInfoData { get; }

    /// <summary>Year, day number at the start of the year.</summary>
    DataGroup<YearDayNumber> StartOfYearDayNumberData { get; }
    /// <summary>Year, day number at the end of the year.</summary>
    DataGroup<YearDayNumber> EndOfYearDayNumberData { get; }
}
