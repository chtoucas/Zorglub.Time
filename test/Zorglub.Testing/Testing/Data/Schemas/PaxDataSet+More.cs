// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class PaxDataSet // Supplementary data
{
    /// <summary>Year, invalid week of the year.</summary>
    public static TheoryData<int, int> InvalidWeekOfYearData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 53 },
        { LeapYear, 0 },
        { LeapYear, 54 },
    };

    /// <summary>Year, weeks in year.</summary>
    public static TheoryData<int, int> MoreYearInfoData { get; } = new()
    {
        { CommonYear, 52 },
        { LeapYear, 53 },
    };

    /// <summary>Year, month of the year (Yemo), isPaxMonth, isLastMonthOfYear.</summary>
    public static TheoryData<int, int, bool, bool> MoreMonthInfoData { get; } = new()
    {
        // Common year.
        { CommonYear, 1, false, false },
        { CommonYear, 2, false, false },
        { CommonYear, 3, false, false },
        { CommonYear, 4, false, false },
        { CommonYear, 5, false, false },
        { CommonYear, 6, false, false },
        { CommonYear, 7, false, false },
        { CommonYear, 8, false, false },
        { CommonYear, 9, false, false },
        { CommonYear, 10, false, false },
        { CommonYear, 11, false, false },
        { CommonYear, 12, false, false },
        { CommonYear, 13, false, true },
        // Leap year.
        { LeapYear, 1, false, false },
        { LeapYear, 2, false, false },
        { LeapYear, 3, false, false },
        { LeapYear, 4, false, false },
        { LeapYear, 5, false, false },
        { LeapYear, 6, false, false },
        { LeapYear, 7, false, false },
        { LeapYear, 8, false, false },
        { LeapYear, 9, false, false },
        { LeapYear, 10, false, false },
        { LeapYear, 11, false, false },
        { LeapYear, 12, false, false },
        { LeapYear, 13, true, false },
        { LeapYear, 14, false, true },
    };

    /// <summary>Year, week of the year (Yewe), isIntercalary.</summary>
    public static TheoryData<int, int, bool> WeekInfoData { get; } = new()
    {
        // Common year.
        { CommonYear, 1, false },
        { CommonYear, 48, false },
        { CommonYear, 49, false },
        { CommonYear, 50, false },
        { CommonYear, 51, false },
        { CommonYear, 52, false },
        // Leap year.
        { LeapYear, 1, false },
        { LeapYear, 48, false },
        { LeapYear, 49, true },
        { LeapYear, 50, false },
        { LeapYear, 51, false },
        { LeapYear, 52, false },
        { LeapYear, 53, false },
    };
}
