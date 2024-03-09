// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data;

// What's in ICalendarDataSet? Everything related to DayNumber, including the
// Epoch and DayOfWeek.
// We have three properties in ICalendricalDataSet that can be translated in
// terms of DayNumber's.

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
