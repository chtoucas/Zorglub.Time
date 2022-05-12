// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public abstract class TropicalistaDataSet : CalendricalDataSet
{
    public const int CommonYear = 3;
    public const int LeapYear = 4;

    protected TropicalistaDataSet(TropicalistaSchema schema) : base(schema, CommonYear, LeapYear) { }

    public sealed override TheoryData<YearInfo> YearInfoData => new()
    {
        // Leap years.
        new(-8, 12, 366, true),
        new(-4, 12, 366, true),
        new(4, 12, 366, true),
        new(8, 12, 366, true),
        new(12, 12, 366, true),
        new(16, 12, 366, true),
        new(20, 12, 366, true),
        new(40, 12, 366, true),
        new(60, 12, 366, true),
        new(80, 12, 366, true),
        // Centennial years.
        new(200, 12, 366, true),
        new(300, 12, 366, true),
        new(400, 12, 366, true),
        new(500, 12, 366, true),
        new(600, 12, 366, true),
        new(700, 12, 366, true),
        new(800, 12, 366, true),
        new(900, 12, 366, true),
        new(1000, 12, 366, true),
        new(1100, 12, 366, true),
        new(1200, 12, 366, true),
        new(1300, 12, 366, true),
        new(1400, 12, 366, true),
        new(1500, 12, 366, true),
        new(1600, 12, 366, true),
        new(1700, 12, 366, true),
        new(1800, 12, 366, true),
        new(1900, 12, 366, true),
        new(2000, 12, 366, true),

        // Common years.
        new(-7, 12, 365, false),
        new(-6, 12, 365, false),
        new(-5, 12, 365, false),
        new(-3, 12, 365, false),
        new(-2, 12, 365, false),
        new(-1, 12, 365, false),
        new(1, 12, 365, false),
        new(2, 12, 365, false),
        new(3, 12, 365, false),
        new(5, 12, 365, false),
        new(6, 12, 365, false),
        new(7, 12, 365, false),
        new(9, 12, 365, false),
        new(10, 12, 365, false),
        new(11, 12, 365, false),
        new(13, 12, 365, false),
        new(14, 12, 365, false),
        new(15, 12, 365, false),
        new(17, 12, 365, false),
        new(18, 12, 365, false),
        new(19, 12, 365, false),
        new(30, 12, 365, false),
        new(50, 12, 365, false),
        new(70, 12, 365, false),
        new(90, 12, 365, false),
        // Multiple of 128.
        new(0, 12, 365, false),
        new(128, 12, 365, false),
        new(256, 12, 365, false),
        new(384, 12, 365, false),
        new(512, 12, 365, false),
    };

    public sealed override TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-127, -TropicaliaSchema.DaysPer128YearCycle),
        new(-10, -11 * 365 - 2),
        new(-9, -10 * 365 - 2),
        new(-8, -9 * 365 - 2), // leap year
        new(-7, -8 * 365 - 1),
        new(-6, -7 * 365 - 1),
        new(-5, -6 * 365 - 1),
        new(-4, -5 * 365 - 1), // leap year
        new(-3, -4 * 365),
        new(-2, -3 * 365),
        new(-1, -2 * 365),
        new(0, -365),
        new(1, 0),
        new(2, 365),
        new(3, 2 * 365),
        new(4, 3 * 365), // leap year
        new(5, 4 * 365 + 1),
        new(6, 5 * 365 + 1),
        new(7, 6 * 365 + 1),
        new(8, 7 * 365 + 1), // leap year
        new(9, 8 * 365 + 2),
        new(10, 9 * 365 + 2),
        new(129, TropicaliaSchema.DaysPer128YearCycle),
    };

    public sealed override TheoryData<int, int> InvalidMonthFieldData => new()
    {
        { CommonYear, 0 },
        { CommonYear, 13 },
        { LeapYear, 0 },
        { LeapYear, 13 },
    };

    public sealed override TheoryData<int, int> InvalidDayOfYearFieldData => new()
    {
        { CommonYear, 0 },
        { CommonYear, 366 },
        { LeapYear, 0 },
        { LeapYear, 367 },
    };
}
