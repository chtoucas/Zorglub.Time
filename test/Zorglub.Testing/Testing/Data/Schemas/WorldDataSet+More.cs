// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class WorldDataSet // Supplementary data
{
    // Year, month of the year, genuine daysInMonth.
    public static TheoryData<int, int, int> MoreMonthInfoData { get; } = new()
    {
        // Common year.
        { CommonYear, 1, 31 },
        { CommonYear, 2, 30 },
        { CommonYear, 3, 30 },
        { CommonYear, 4, 31 },
        { CommonYear, 5, 30 },
        { CommonYear, 6, 30 },
        { CommonYear, 7, 31 },
        { CommonYear, 8, 30 },
        { CommonYear, 9, 30 },
        { CommonYear, 10, 31 },
        { CommonYear, 11, 30 },
        { CommonYear, 12, 30 },
        // Leap year.
        { LeapYear, 1, 31 },
        { LeapYear, 2, 30 },
        { LeapYear, 3, 30 },
        { LeapYear, 4, 31 },
        { LeapYear, 5, 30 },
        { LeapYear, 6, 30 },
        { LeapYear, 7, 31 },
        { LeapYear, 8, 30 },
        { LeapYear, 9, 30 },
        { LeapYear, 10, 31 },
        { LeapYear, 11, 30 },
        { LeapYear, 12, 30 },
    };
}
