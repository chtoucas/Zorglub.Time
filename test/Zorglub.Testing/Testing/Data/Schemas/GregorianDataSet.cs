// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for <see cref="GregorianSchema"/>.
/// </summary>
public sealed partial class GregorianDataSet :
    CalendricalDataSet,
    IYearAdjustmentDataSet,
    IAdvancedMathDataSet,
    ISingleton<GregorianDataSet>
{
    public const int CommonYear = 3;
    public const int LeapYear = 4;

    private GregorianDataSet() : base(CommonYear, LeapYear) { }

    public static GregorianDataSet Instance { get; } = new();
}

public partial class GregorianDataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??= CalCal.ConvertRataDieToDaysSinceEpochInfo(RataDieInfos, DayZero.NewStyle);

    public override TheoryData<DateInfo> DateInfoData => new()
    {
        // First & last dates.
        new(CivilDate.MinYear, 1, 1, 1, false, false),
        new(CivilDate.MaxYear, 12, 31, 365, false, false),
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

    public override TheoryData<YearInfo> YearInfoData => new()
    {
        // Leap years.
        new(-8, 12, 366, true),
        new(-4, 12, 366, true),
        new(0, 12, 366, true),
        new(4, 12, 366, true),
        new(8, 12, 366, true),
        new(12, 12, 366, true),
        new(16, 12, 366, true),
        new(20, 12, 366, true),
        new(40, 12, 366, true),
        new(60, 12, 366, true),
        new(80, 12, 366, true),
        // Centennial years.
        new(400, 12, 366, true),
        new(800, 12, 366, true),
        new(1200, 12, 366, true),
        new(1600, 12, 366, true),
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
        // Centennial years.
        new(100, 12, 365, false),
        new(200, 12, 365, false),
        new(300, 12, 365, false),
        new(500, 12, 365, false),
        new(600, 12, 365, false),
        new(700, 12, 365, false),
        new(900, 12, 365, false),
        new(1000, 12, 365, false),
        new(1100, 12, 365, false),
        new(1300, 12, 365, false),
        new(1400, 12, 365, false),
        new(1500, 12, 365, false),
        new(1700, 12, 365, false),
        new(1800, 12, 365, false),
        new(1900, 12, 365, false),
    };

    public override TheoryData<YemodaAnd<int>> DaysInYearAfterDateData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 31, 334),
        new(CommonYear, 2, 28, 306),
        new(CommonYear, 3, 31, 275),
        new(CommonYear, 4, 30, 245),
        new(CommonYear, 5, 31, 214),
        new(CommonYear, 6, 30, 184),
        new(CommonYear, 7, 31, 153),
        new(CommonYear, 8, 31, 122),
        new(CommonYear, 9, 30, 92),
        new(CommonYear, 10, 31, 61),
        new(CommonYear, 11, 30, 31),
        new(CommonYear, 12, 31, 0),
        // Leap year.
        new(LeapYear, 1, 31, 335),
        new(LeapYear, 2, 29, 306),
        new(LeapYear, 3, 31, 275),
        new(LeapYear, 4, 30, 245),
        new(LeapYear, 5, 31, 214),
        new(LeapYear, 6, 30, 184),
        new(LeapYear, 7, 31, 153),
        new(LeapYear, 8, 31, 122),
        new(LeapYear, 9, 30, 92),
        new(LeapYear, 10, 31, 61),
        new(LeapYear, 11, 30, 31),
        new(LeapYear, 12, 31, 0),
    };

    public override TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData { get; } = new()
    {
        new(CommonYear, 1, 1, 30),
        new(CommonYear, 1, 31, 0),
        new(CommonYear, 2, 1, 27),
        new(CommonYear, 2, 28, 0),
        new(LeapYear, 2, 1, 28),
        new(LeapYear, 2, 28, 1),
        new(LeapYear, 2, 29, 0),
        new(CommonYear, 3, 1, 30),
        new(CommonYear, 3, 31, 0),
        new(CommonYear, 4, 1, 29),
        new(CommonYear, 4, 30, 0),
        new(CommonYear, 5, 1, 30),
        new(CommonYear, 5, 31, 0),
        new(CommonYear, 6, 1, 29),
        new(CommonYear, 6, 30, 0),
        new(CommonYear, 7, 1, 30),
        new(CommonYear, 7, 31, 0),
        new(CommonYear, 8, 1, 30),
        new(CommonYear, 8, 31, 0),
        new(CommonYear, 9, 1, 29),
        new(CommonYear, 9, 30, 0),
        new(CommonYear, 10, 1, 30),
        new(CommonYear, 10, 31, 0),
        new(CommonYear, 11, 1, 29),
        new(CommonYear, 11, 30, 0),
        new(CommonYear, 12, 1, 30),
        new(CommonYear, 12, 31, 0),
    };

    internal static List<(int RataDie, int Year, int Month, int Day)> RataDieInfos { get; } = new()
    {
        // See also JulianData.DayNumbers.
        (-1, 0, 12, 30),
        (0, 0, 12, 31),

        // Epoch.
        (1, 1, 1, 1),
        (31, 1, 1, 31),
        (365, 1, 12, 31),

        // 400-year cycle.
        (146_097, 400, 12, 31),
        (292_194, 800, 12, 31),
        (438_291, 1200, 12, 31),
        (584_388, 1600, 12, 31),
        (730_485, 2000, 12, 31),
        (876_582, 2400, 12, 31),
        (1460970, 4000, 12, 31),
        (2921940, 8000, 12, 31),

        // D.&R. p. 49.
        (710_347, 1945, 11, 12),

        // D.&R. Appendix C.
        (-214_193, -586, 7, 24),
        (-61_387, -168, 12, 5),
        (25_469, 70, 9, 24),
        (49_217, 135, 10, 2),
        (171_307, 470, 1, 8),
        (210_155, 576, 5, 20),
        (253_427, 694, 11, 10),
        (369_740, 1013, 4, 25),
        (400_085, 1096, 5, 24),
        (434_355, 1190, 3, 23),
        (452_605, 1240, 3, 10),
        (470_160, 1288, 4, 2),
        (473_837, 1298, 4, 27),
        (507_850, 1391, 6, 12),
        (524_156, 1436, 2, 3),
        (544_676, 1492, 4, 9),
        (567_118, 1553, 9, 19),
        (569_477, 1560, 3, 5),
        (601_716, 1648, 6, 10),
        (613_424, 1680, 6, 30),
        (626_596, 1716, 7, 24),
        (645_554, 1768, 6, 19),
        (664_224, 1819, 8, 2),
        (671_401, 1839, 3, 27),
        (694_799, 1903, 4, 19),
        (704_424, 1929, 8, 25),
        (708_842, 1941, 9, 29),
        (709_409, 1943, 4, 19),
        (709_580, 1943, 10, 7),
        (727_274, 1992, 3, 17),
        (728_714, 1996, 2, 25),
        (744_313, 2038, 11, 10),
        (764_652, 2094, 7, 18),
    };
}

public partial class GregorianDataSet // Start and end of year
{
    public override TheoryData<Yemoda> EndOfYearPartsData => new()
    {
        new(CommonYear, 12, 31),
        new(LeapYear, 12, 31),
    };

    public override TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-401, -GregorianSchema.DaysPer400YearCycle - 366 - 365),
        new(-400, -GregorianSchema.DaysPer400YearCycle - 366), // leap year

        // Start of century -3: 36_524 days long.
        new(-399, -GregorianSchema.DaysPer400YearCycle),
        new(-301, -GregorianSchema.DaysPer100YearSubcycle * 3 - 1 - 365 * 2),
        new(-300, -GregorianSchema.DaysPer100YearSubcycle * 3 - 1 - 365), // common year

        // Start of century -2: 36_524 days long.
        new(-299, -GregorianSchema.DaysPer100YearSubcycle * 3 - 1),
        new(-201, -GregorianSchema.DaysPer100YearSubcycle * 2 - 1 - 365 * 2),
        new(-200, -GregorianSchema.DaysPer100YearSubcycle * 2 - 1 - 365), // common year

        // Start of century -1: 36_524 days long.
        new(-199, -GregorianSchema.DaysPer100YearSubcycle * 2 - 1),
        new(-101, -GregorianSchema.DaysPer100YearSubcycle - 1 - 365 * 2),
        new(-100, -GregorianSchema.DaysPer100YearSubcycle - 1 - 365), // common year

        // Start of century 0: 36_525 days long.
        new(-99, -GregorianSchema.DaysPer100YearSubcycle - 1),
        new(-10, -4018),
        new(-9, -3653),
        new(-8, -3288), // leap year
        new(-7, -2922),
        new(-6, -2557),
        new(-5, -2192),
        new(-4, -1827), // leap year
        new(-3, -1461),
        new(-2, -1096),
        new(-1, -731),
        new(0, -366), // leap year

        // Start of century 1: 36_524 days long.
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
        new(99, GregorianSchema.DaysPer100YearSubcycle - 365 * 2),
        new(100, GregorianSchema.DaysPer100YearSubcycle - 365), // common year

        // Start of century 2: 36_524 days long.
        new(101, GregorianSchema.DaysPer100YearSubcycle),
        new(200, GregorianSchema.DaysPer100YearSubcycle * 2 - 365), // common year

        // Start of century 3: 36_524 days long.
        new(201, GregorianSchema.DaysPer100YearSubcycle * 2),
        new(300, GregorianSchema.DaysPer100YearSubcycle * 3 - 365), // common year

        // Start of century 4: 36_525 days long.
        new(301, GregorianSchema.DaysPer100YearSubcycle * 3),
        new(400, GregorianSchema.DaysPer400YearCycle - 366), // leap year

        // Start of century 5: 36_524 days long.
        new(401, GregorianSchema.DaysPer400YearCycle),
        new(402, GregorianSchema.DaysPer400YearCycle + 365),
    };
}

public partial class GregorianDataSet // Invalid date parts
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

public partial class GregorianDataSet //
{
    // TODO(data): Move DaysInYearAfterMonthData to MonthInfoData.
    // When done, remove DaysInYearAfterMonthData from the various Gregorian datasets.
    // CountDaysInYear(y) - CountDaysInMonth(y, m) - CountDaysInYearBeforeMonth(y, m);
    public TheoryData<YemoAnd<int>> DaysInYearAfterMonthData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 334),
        new(CommonYear, 2, 306),
        new(CommonYear, 3, 275),
        new(CommonYear, 4, 245),
        new(CommonYear, 5, 214),
        new(CommonYear, 6, 184),
        new(CommonYear, 7, 153),
        new(CommonYear, 8, 122),
        new(CommonYear, 9, 92),
        new(CommonYear, 10, 61),
        new(CommonYear, 11, 31),
        new(CommonYear, 12, 0),
        // Leap year.
        new(LeapYear, 1, 335),
        new(LeapYear, 2, 306),
        new(LeapYear, 3, 275),
        new(LeapYear, 4, 245),
        new(LeapYear, 5, 214),
        new(LeapYear, 6, 184),
        new(LeapYear, 7, 153),
        new(LeapYear, 8, 122),
        new(LeapYear, 9, 92),
        new(LeapYear, 10, 61),
        new(LeapYear, 11, 31),
        new(LeapYear, 12, 0),
    };
}

public partial class GregorianDataSet // IYearAdjustmentDataSet
{
    public TheoryData<YemodaAnd<int>> InvalidYearAdjustementData { get; } = new()
    {
        // Intercalary day mapped to a common year.
        new(LeapYear, 2, 29, CommonYear)
    };

    public TheoryData<YemodaAnd<int>> YearAdjustementData { get; } = new()
    {
        // Intercalary day mapped to another leap year.
        new(4, 2, 29, 8),
    };
}
