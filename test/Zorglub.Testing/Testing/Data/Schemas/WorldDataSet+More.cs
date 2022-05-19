// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class WorldDataSet // Supplementary data
{
    /// <summary>Year, month of the year, genuine daysInMonth.</summary>
    public static DataGroup<YemoAnd<int>> MoreMonthInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 31),
        new(CommonYear, 2, 30),
        new(CommonYear, 3, 30),
        new(CommonYear, 4, 31),
        new(CommonYear, 5, 30),
        new(CommonYear, 6, 30),
        new(CommonYear, 7, 31),
        new(CommonYear, 8, 30),
        new(CommonYear, 9, 30),
        new(CommonYear, 10, 31),
        new(CommonYear, 11, 30),
        new(CommonYear, 12, 30),
        // Leap year.
        new(LeapYear, 1, 31),
        new(LeapYear, 2, 30),
        new(LeapYear, 3, 30),
        new(LeapYear, 4, 31),
        new(LeapYear, 5, 30),
        new(LeapYear, 6, 30),
        new(LeapYear, 7, 31),
        new(LeapYear, 8, 30),
        new(LeapYear, 9, 30),
        new(LeapYear, 10, 31),
        new(LeapYear, 11, 30),
        new(LeapYear, 12, 30),
    };
}
