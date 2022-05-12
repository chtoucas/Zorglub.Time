﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for <see cref="TropicaliaSchema"/>.
/// </summary>
public sealed partial class TropicaliaDataSet : TropicalistaDataSet, ISingleton<TropicaliaDataSet>
{
    private TropicaliaDataSet() { }

    public static TropicaliaDataSet Instance { get; } = new();

    public override TheoryData<YemodaAnd<int>> DaysInYearAfterDateData =>
        GetDaysInYearAfterDateData(new TropicaliaSchema());
    public override TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        GetDaysInMonthAfterDateData(new TropicaliaSchema());
}

public partial class TropicaliaDataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??= CalCal.ConvertRataDieToDaysSinceEpochInfo(RataDieInfos, DayZero.NewStyle);

    public override TheoryData<DateInfo> DateInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 15, 15, false, false),
        new(CommonYear, 1, 31, 31, false, false),
        new(CommonYear, 2, 1, 32, false, false),
        new(CommonYear, 2, 15, 46, false, false),
        new(CommonYear, 2, 28, 59, false, false),
        new(CommonYear, 3, 1, 60, false, false),
        new(CommonYear, 3, 15, 74, false, false),
        new(CommonYear, 3, 31, 90, false, false),
        new(CommonYear, 4, 1, 91, false, false),
        new(CommonYear, 4, 15, 105, false, false),
        new(CommonYear, 4, 30, 120, false, false),
        new(CommonYear, 5, 1, 121, false, false),
        new(CommonYear, 5, 15, 135, false, false),
        new(CommonYear, 5, 31, 151, false, false),
        new(CommonYear, 6, 1, 152, false, false),
        new(CommonYear, 6, 15, 166, false, false),
        new(CommonYear, 6, 30, 181, false, false),
        new(CommonYear, 7, 1, 182, false, false),
        new(CommonYear, 7, 15, 196, false, false),
        new(CommonYear, 7, 31, 212, false, false),
        new(CommonYear, 8, 1, 213, false, false),
        new(CommonYear, 8, 15, 227, false, false),
        new(CommonYear, 8, 31, 243, false, false),
        new(CommonYear, 9, 1, 244, false, false),
        new(CommonYear, 9, 15, 258, false, false),
        new(CommonYear, 9, 30, 273, false, false),
        new(CommonYear, 10, 1, 274, false, false),
        new(CommonYear, 10, 15, 288, false, false),
        new(CommonYear, 10, 31, 304, false, false),
        new(CommonYear, 11, 1, 305, false, false),
        new(CommonYear, 11, 15, 319, false, false),
        new(CommonYear, 11, 30, 334, false, false),
        new(CommonYear, 12, 1, 335, false, false),
        new(CommonYear, 12, 15, 349, false, false),
        new(CommonYear, 12, 31, 365, false, false),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 15, 15, false, false),
        new(LeapYear, 1, 31, 31, false, false),
        new(LeapYear, 2, 1, 32, false, false),
        new(LeapYear, 2, 15, 46, false, false),
        new(LeapYear, 2, 28, 59, false, false),
        new(LeapYear, 2, 29, 60, true, false), // intercalary day
        new(LeapYear, 3, 1, 61, false, false),
        new(LeapYear, 3, 15, 75, false, false),
        new(LeapYear, 3, 31, 91, false, false),
        new(LeapYear, 4, 1, 92, false, false),
        new(LeapYear, 4, 15, 106, false, false),
        new(LeapYear, 4, 30, 121, false, false),
        new(LeapYear, 5, 1, 122, false, false),
        new(LeapYear, 5, 15, 136, false, false),
        new(LeapYear, 5, 31, 152, false, false),
        new(LeapYear, 6, 1, 153, false, false),
        new(LeapYear, 6, 15, 167, false, false),
        new(LeapYear, 6, 30, 182, false, false),
        new(LeapYear, 7, 1, 183, false, false),
        new(LeapYear, 7, 15, 197, false, false),
        new(LeapYear, 7, 31, 213, false, false),
        new(LeapYear, 8, 1, 214, false, false),
        new(LeapYear, 8, 15, 228, false, false),
        new(LeapYear, 8, 31, 244, false, false),
        new(LeapYear, 9, 1, 245, false, false),
        new(LeapYear, 9, 15, 259, false, false),
        new(LeapYear, 9, 30, 274, false, false),
        new(LeapYear, 10, 1, 275, false, false),
        new(LeapYear, 10, 15, 289, false, false),
        new(LeapYear, 10, 31, 305, false, false),
        new(LeapYear, 11, 1, 306, false, false),
        new(LeapYear, 11, 15, 320, false, false),
        new(LeapYear, 11, 30, 335, false, false),
        new(LeapYear, 12, 1, 336, false, false),
        new(LeapYear, 12, 15, 350, false, false),
        new(LeapYear, 12, 31, 366, false, false),
    };

    public override TheoryData<MonthInfo> MonthInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 31, 0, false),
        new(CommonYear, 2, 28, 31, false),
        new(CommonYear, 3, 31, 59, false),
        new(CommonYear, 4, 30, 90, false),
        new(CommonYear, 5, 31, 120, false),
        new(CommonYear, 6, 30, 151, false),
        new(CommonYear, 7, 31, 181, false),
        new(CommonYear, 8, 31, 212, false),
        new(CommonYear, 9, 30, 243, false),
        new(CommonYear, 10, 31, 273, false),
        new(CommonYear, 11, 30, 304, false),
        new(CommonYear, 12, 31, 334, false),
        // Leap year.
        new(LeapYear, 1, 31, 0, false),
        new(LeapYear, 2, 29, 31, false),
        new(LeapYear, 3, 31, 60, false),
        new(LeapYear, 4, 30, 91, false),
        new(LeapYear, 5, 31, 121, false),
        new(LeapYear, 6, 30, 152, false),
        new(LeapYear, 7, 31, 182, false),
        new(LeapYear, 8, 31, 213, false),
        new(LeapYear, 9, 30, 244, false),
        new(LeapYear, 10, 31, 274, false),
        new(LeapYear, 11, 30, 305, false),
        new(LeapYear, 12, 31, 335, false),
    };

    internal static List<(int RataDie, int Year, int Month, int Day)> RataDieInfos { get; } = new()
    {
        (-1, 0, 12, 30),
        (0, 0, 12, 31),
        (1, 1, 1, 1),
        (2, 1, 1, 2),
        (365, 1, 12, 31),
        (366, 2, 1, 1),
        (731, 3, 1, 1),
        (1096, 4, 1, 1),
        (1462, 5, 1, 1),

        // 128-year cycle.
        (46_751, 128, 12, 31),
        (93_502, 256, 12, 31),
        (140_253, 384, 12, 31),
        (187_004, 512, 12, 31),
        (233_755, 640, 12, 31),
        (280_506, 768, 12, 31),
        (327_257, 896, 12, 31),
        (374_008, 1024, 12, 31),
        (420_759, 1152, 12, 31),
        (467_510, 1280, 12, 31),

        // REVIEW(data): taken from the Gregorian calendar.
        // D.&R. p. 49.
        (710_347, 1945, 11, 12),
        // D.&R. Appendix C.
        (25_469, 70, 9, 24),
        (49_217, 135, 10, 2),
        (171_307, 470, 1, 8),
        (210_155, 576, 5, 20),
        (253_427, 694, 11, 10),
        (369_740, 1013, 4, 24), // -1 day
        (400_085, 1096, 5, 24),
        (434_355, 1190, 3, 23),
        (452_605, 1240, 3, 10),
        (470_160, 1288, 4, 3), // +1 day
        (473_837, 1298, 4, 28), // +1 day
        (507_850, 1391, 6, 12),
        (524_156, 1436, 2, 3),
        (544_676, 1492, 4, 9),
        (567_118, 1553, 9, 19),
        (569_477, 1560, 3, 5),
        (601_716, 1648, 6, 10),
        (613_424, 1680, 7, 1), // +1 day
        (626_596, 1716, 7, 24),
        (645_554, 1768, 6, 19),
        (664_224, 1819, 8, 2),
        (671_401, 1839, 3, 27),
        (694_799, 1903, 4, 18), // -1 day
        (704_424, 1929, 8, 25),
        (708_842, 1941, 9, 29),
        (709_409, 1943, 4, 19),
        (709_580, 1943, 10, 7),
        (727_274, 1992, 3, 17),
        (728_714, 1996, 2, 25),
        (744_313, 2038, 11, 10),
        (764_652, 2094, 7, 19), // +1 day
    };
}

public partial class TropicaliaDataSet // Start and end of year
{
    public override TheoryData<Yemoda> EndOfYearPartsData => new()
    {
        new(CommonYear, 12, 31),
        new(LeapYear, 12, 31),
    };
}

public partial class TropicaliaDataSet // Invalid date parts
{
    public override TheoryData<int, int, int> InvalidDayFieldData => new()
    {
        // Common year.
        { CommonYear, 1, 0 },
        { CommonYear, 1, 32 },
        { CommonYear, 2, 0 },
        { CommonYear, 2, 29 },
        { CommonYear, 3, 0 },
        { CommonYear, 3, 32 },
        { CommonYear, 4, 0 },
        { CommonYear, 4, 31 },
        { CommonYear, 5, 0 },
        { CommonYear, 5, 32 },
        { CommonYear, 6, 0 },
        { CommonYear, 6, 31 },
        { CommonYear, 7, 0 },
        { CommonYear, 7, 32 },
        { CommonYear, 8, 0 },
        { CommonYear, 8, 32 },
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
        { LeapYear, 2, 30 },
        { LeapYear, 3, 0 },
        { LeapYear, 3, 32 },
        { LeapYear, 4, 0 },
        { LeapYear, 4, 31 },
        { LeapYear, 5, 0 },
        { LeapYear, 5, 32 },
        { LeapYear, 6, 0 },
        { LeapYear, 6, 31 },
        { LeapYear, 7, 0 },
        { LeapYear, 7, 32 },
        { LeapYear, 8, 0 },
        { LeapYear, 8, 32 },
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
