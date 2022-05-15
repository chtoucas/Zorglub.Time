// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for <see cref="WorldSchema"/>.
/// </summary>
public sealed partial class WorldDataSet : CalendricalDataSet, ISingleton<WorldDataSet>
{
    public const int CommonYear = 3;
    public const int LeapYear = 4;

    private WorldDataSet() : base(new WorldSchema(), CommonYear, LeapYear) { }

    public static WorldDataSet Instance { get; } = new();
}

public partial class WorldDataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??= MapToDaysSinceEpochInfoData(DaysSinceEpochInfos);

    public override TheoryData<DateInfo> DateInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 31, 31, false, false),
        new(CommonYear, 2, 30, 61, false, false),
        new(CommonYear, 3, 30, 91, false, false),
        new(CommonYear, 4, 31, 122, false, false),
        new(CommonYear, 5, 30, 152, false, false),
        new(CommonYear, 6, 30, 182, false, false),
        new(CommonYear, 7, 31, 213, false, false),
        new(CommonYear, 8, 30, 243, false, false),
        new(CommonYear, 9, 30, 273, false, false),
        new(CommonYear, 10, 31, 304, false, false),
        new(CommonYear, 11, 30, 334, false, false),
        new(CommonYear, 12, 30, 364, false, false),
        new(CommonYear, 12, 31, 365, false, true),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 31, 31, false, false),
        new(LeapYear, 2, 30, 61, false, false),
        new(LeapYear, 3, 30, 91, false, false),
        new(LeapYear, 4, 31, 122, false, false),
        new(LeapYear, 5, 30, 152, false, false),
        new(LeapYear, 6, 30, 182, false, false),
        new(LeapYear, 6, 31, 183, true, true),
        new(LeapYear, 7, 31, 214, false, false),
        new(LeapYear, 8, 30, 244, false, false),
        new(LeapYear, 9, 30, 274, false, false),
        new(LeapYear, 10, 31, 305, false, false),
        new(LeapYear, 11, 30, 335, false, false),
        new(LeapYear, 12, 30, 365, false, false),
        new(LeapYear, 12, 31, 366, false, true),
    };

    public override TheoryData<MonthInfo> MonthInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 31, 0, false),
        new(CommonYear, 2, 30, 31, false),
        new(CommonYear, 3, 30, 61, false),
        new(CommonYear, 4, 31, 91, false),
        new(CommonYear, 5, 30, 122, false),
        new(CommonYear, 6, 30, 152, false),
        new(CommonYear, 7, 31, 182, false),
        new(CommonYear, 8, 30, 213, false),
        new(CommonYear, 9, 30, 243, false),
        new(CommonYear, 10, 31, 273, false),
        new(CommonYear, 11, 30, 304, false),
        new(CommonYear, 12, 31, 334, false),
        // Leap year.
        new(LeapYear, 1, 31, 0, false),
        new(LeapYear, 2, 30, 31, false),
        new(LeapYear, 3, 30, 61, false),
        new(LeapYear, 4, 31, 91, false),
        new(LeapYear, 5, 30, 122, false),
        new(LeapYear, 6, 31, 152, false),
        new(LeapYear, 7, 31, 183, false),
        new(LeapYear, 8, 30, 214, false),
        new(LeapYear, 9, 30, 244, false),
        new(LeapYear, 10, 31, 274, false),
        new(LeapYear, 11, 30, 305, false),
        new(LeapYear, 12, 31, 335, false),
    };

    public override TheoryData<YearInfo> YearInfoData => new()
    {
        // Leap years.
        new(LeapYear, 12, 366, true),
        // Centennial years.
        new(400, 12, 366, true),

        // Common years.
        new(CommonYear, 12, 365, false),
        // Centennial years.
        new(100, 12, 365, false),
    };

    internal static List<(int DaysSinceEpoch, int Year, int Month, int Day)> DaysSinceEpochInfos { get; } = new()
    {
        (-2, 0, 12, 30),
        (-1, 0, 12, 31),
        (0, 1, 1, 1), // Epoch
        (1, 1, 1, 2),
        (364, 1, 12, 31),
        (365, 2, 1, 1),
    };
}

public partial class WorldDataSet // Start and end of year
{
    public override TheoryData<Yemoda> EndOfYearPartsData => new()
    {
        new(CommonYear, 12, 31),
        new(LeapYear, 12, 31),
    };

    public override TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-399, -WorldSchema.DaysPer400YearCycle),
        new(1, 0),
        new(2, 365),
        new(3, 730),
        new(4, 1095), // leap year
        new(5, 1461),
        new(6, 1826),
        new(7, 2191),
        new(8, 2556), // leap year
        new(9, 2922),
        new(10, 3287),
        new(401, WorldSchema.DaysPer400YearCycle),
    };
}

public partial class WorldDataSet // Invalid date parts
{
    public override TheoryData<int, int> InvalidMonthFieldData => new()
    {
        { CommonYear, 0 },
        { CommonYear, 13 },
        { LeapYear, 0 },
        { LeapYear, 13 },
    };

    public override TheoryData<int, int> InvalidDayOfYearFieldData => new()
    {
        { CommonYear, 0 },
        { CommonYear, 366 },
        { LeapYear, 0 },
        { LeapYear, 367 },
    };

    public override TheoryData<int, int, int> InvalidDayFieldData => new()
    {
        // Common year.
        { CommonYear, 1, 0 },
        { CommonYear, 1, 32 },
        { CommonYear, 2, 0 },
        { CommonYear, 2, 31 },
        { CommonYear, 3, 0 },
        { CommonYear, 3, 31 },
        { CommonYear, 4, 0 },
        { CommonYear, 4, 32 },
        { CommonYear, 5, 0 },
        { CommonYear, 5, 31 },
        { CommonYear, 6, 0 },
        { CommonYear, 6, 31 },
        { CommonYear, 7, 0 },
        { CommonYear, 7, 32 },
        { CommonYear, 8, 0 },
        { CommonYear, 8, 31 },
        { CommonYear, 9, 0 },
        { CommonYear, 9, 31 },
        { CommonYear, 10, 0 },
        { CommonYear, 10, 32 },
        { CommonYear, 11, 0 },
        { CommonYear, 11, 31 },
        { CommonYear, 12, 0 },
        { CommonYear, 12, 32 },
        // Leap year.
        { LeapYear, 1, 0 },
        { LeapYear, 1, 32 },
        { LeapYear, 2, 0 },
        { LeapYear, 2, 31 },
        { LeapYear, 3, 0 },
        { LeapYear, 3, 31 },
        { LeapYear, 4, 0 },
        { LeapYear, 4, 32 },
        { LeapYear, 5, 0 },
        { LeapYear, 5, 32 },
        { LeapYear, 6, 0 },
        { LeapYear, 6, 32 },
        { LeapYear, 7, 0 },
        { LeapYear, 7, 32 },
        { LeapYear, 8, 0 },
        { LeapYear, 8, 31 },
        { LeapYear, 9, 0 },
        { LeapYear, 9, 31 },
        { LeapYear, 10, 0 },
        { LeapYear, 10, 32 },
        { LeapYear, 11, 0 },
        { LeapYear, 11, 31 },
        { LeapYear, 12, 0 },
        { LeapYear, 12, 32 },
    };
}

public partial class WorldDataSet // Supplementary data
{
    // Year, month of the year, genuine daysInMonth.
    public static readonly TheoryData<int, int, int> MoreMonthInfoData = new()
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
