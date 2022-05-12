// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for <see cref="FrenchRepublican12Schema"/>.
/// </summary>
public sealed partial class FrenchRepublican12DataSet :
    CalendricalDataSet, IEpagomenalDataSet, ISingleton<FrenchRepublican12DataSet>
{
    public const int CommonYear = 1;
    public const int LeapYear = 4;
    public const int CommonCentury = 4000;
    public const int LeapCentury = 400;

    private FrenchRepublican12DataSet() : base(CommonYear, LeapYear) { }

    public static FrenchRepublican12DataSet Instance { get; } = new();

    public override TheoryData<YemodaAnd<int>> DaysInYearAfterDateData =>
        GetDaysInYearAfterDateData(DateInfoData, new FrenchRepublican12Schema());

    public override TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData =>
        GetDaysInMonthAfterDateData(DateInfoData, new FrenchRepublican12Schema());
}

public partial class FrenchRepublican12DataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??=
            CalCal.ConvertRataDieToDaysSinceEpochInfo(RataDieInfos, CalendarEpoch.FrenchRepublican);

    public override TheoryData<DateInfo> DateInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 30, 30, false, false),
        new(CommonYear, 2, 30, 60, false, false),
        new(CommonYear, 3, 30, 90, false, false),
        new(CommonYear, 4, 30, 120, false, false),
        new(CommonYear, 5, 30, 150, false, false),
        new(CommonYear, 6, 30, 180, false, false),
        new(CommonYear, 7, 30, 210, false, false),
        new(CommonYear, 8, 30, 240, false, false),
        new(CommonYear, 9, 30, 270, false, false),
        new(CommonYear, 10, 30, 300, false, false),
        new(CommonYear, 11, 30, 330, false, false),
        new(CommonYear, 12, 30, 360, false, false),
        new(CommonYear, 12, 31, 361, false, true),
        new(CommonYear, 12, 32, 362, false, true),
        new(CommonYear, 12, 33, 363, false, true),
        new(CommonYear, 12, 34, 364, false, true),
        new(CommonYear, 12, 35, 365, false, true),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 30, 30, false, false),
        new(LeapYear, 2, 30, 60, false, false),
        new(LeapYear, 3, 30, 90, false, false),
        new(LeapYear, 4, 30, 120, false, false),
        new(LeapYear, 5, 30, 150, false, false),
        new(LeapYear, 6, 30, 180, false, false),
        new(LeapYear, 7, 30, 210, false, false),
        new(LeapYear, 8, 30, 240, false, false),
        new(LeapYear, 9, 30, 270, false, false),
        new(LeapYear, 10, 30, 300, false, false),
        new(LeapYear, 11, 30, 330, false, false),
        new(LeapYear, 12, 30, 360, false, false),
        new(LeapYear, 12, 31, 361, false, true),
        new(LeapYear, 12, 32, 362, false, true),
        new(LeapYear, 12, 33, 363, false, true),
        new(LeapYear, 12, 34, 364, false, true),
        new(LeapYear, 12, 35, 365, false, true),
        new(LeapYear, 12, 36, 366, true, true),
        // Common century.
        new(CommonCentury, 1, 1, 1, false, false),
        new(CommonCentury, 1, 30, 30, false, false),
        new(CommonCentury, 2, 30, 60, false, false),
        new(CommonCentury, 3, 30, 90, false, false),
        new(CommonCentury, 4, 30, 120, false, false),
        new(CommonCentury, 5, 30, 150, false, false),
        new(CommonCentury, 6, 30, 180, false, false),
        new(CommonCentury, 7, 30, 210, false, false),
        new(CommonCentury, 8, 30, 240, false, false),
        new(CommonCentury, 9, 30, 270, false, false),
        new(CommonCentury, 10, 30, 300, false, false),
        new(CommonCentury, 11, 30, 330, false, false),
        new(CommonCentury, 12, 30, 360, false, false),
        new(CommonCentury, 12, 31, 361, false, true),
        new(CommonCentury, 12, 32, 362, false, true),
        new(CommonCentury, 12, 33, 363, false, true),
        new(CommonCentury, 12, 34, 364, false, true),
        new(CommonCentury, 12, 35, 365, false, true),
        // Leap century.
        new(LeapCentury, 1, 1, 1, false, false),
        new(LeapCentury, 1, 30, 30, false, false),
        new(LeapCentury, 2, 30, 60, false, false),
        new(LeapCentury, 3, 30, 90, false, false),
        new(LeapCentury, 4, 30, 120, false, false),
        new(LeapCentury, 5, 30, 150, false, false),
        new(LeapCentury, 6, 30, 180, false, false),
        new(LeapCentury, 7, 30, 210, false, false),
        new(LeapCentury, 8, 30, 240, false, false),
        new(LeapCentury, 9, 30, 270, false, false),
        new(LeapCentury, 10, 30, 300, false, false),
        new(LeapCentury, 11, 30, 330, false, false),
        new(LeapCentury, 12, 30, 360, false, false),
        new(LeapCentury, 12, 31, 361, false, true),
        new(LeapCentury, 12, 32, 362, false, true),
        new(LeapCentury, 12, 33, 363, false, true),
        new(LeapCentury, 12, 34, 364, false, true),
        new(LeapCentury, 12, 35, 365, false, true),
        new(LeapCentury, 12, 36, 366, true, true),
    };

    public override TheoryData<MonthInfo> MonthInfoData => new()
    {
        // Année commune.
        new(CommonYear, 1, 30, 0, false),
        new(CommonYear, 2, 30, 30, false),
        new(CommonYear, 3, 30, 60, false),
        new(CommonYear, 4, 30, 90, false),
        new(CommonYear, 5, 30, 120, false),
        new(CommonYear, 6, 30, 150, false),
        new(CommonYear, 7, 30, 180, false),
        new(CommonYear, 8, 30, 210, false),
        new(CommonYear, 9, 30, 240, false),
        new(CommonYear, 10, 30, 270, false),
        new(CommonYear, 11, 30, 300, false),
        new(CommonYear, 12, 35, 330, false),
        // Année bissextile.
        new(LeapYear, 1, 30, 0, false),
        new(LeapYear, 2, 30, 30, false),
        new(LeapYear, 3, 30, 60, false),
        new(LeapYear, 4, 30, 90, false),
        new(LeapYear, 5, 30, 120, false),
        new(LeapYear, 6, 30, 150, false),
        new(LeapYear, 7, 30, 180, false),
        new(LeapYear, 8, 30, 210, false),
        new(LeapYear, 9, 30, 240, false),
        new(LeapYear, 10, 30, 270, false),
        new(LeapYear, 11, 30, 300, false),
        new(LeapYear, 12, 36, 330, false),
        // Année séculaire commune.
        new(CommonCentury, 1, 30, 0, false),
        new(CommonCentury, 2, 30, 30, false),
        new(CommonCentury, 3, 30, 60, false),
        new(CommonCentury, 4, 30, 90, false),
        new(CommonCentury, 5, 30, 120, false),
        new(CommonCentury, 6, 30, 150, false),
        new(CommonCentury, 7, 30, 180, false),
        new(CommonCentury, 8, 30, 210, false),
        new(CommonCentury, 9, 30, 240, false),
        new(CommonCentury, 10, 30, 270, false),
        new(CommonCentury, 11, 30, 300, false),
        new(CommonCentury, 12, 35, 330, false),
        // Année séculaire bissextile.
        new(LeapCentury, 1, 30, 0, false),
        new(LeapCentury, 2, 30, 30, false),
        new(LeapCentury, 3, 30, 60, false),
        new(LeapCentury, 4, 30, 90, false),
        new(LeapCentury, 5, 30, 120, false),
        new(LeapCentury, 6, 30, 150, false),
        new(LeapCentury, 7, 30, 180, false),
        new(LeapCentury, 8, 30, 210, false),
        new(LeapCentury, 9, 30, 240, false),
        new(LeapCentury, 10, 30, 270, false),
        new(LeapCentury, 11, 30, 300, false),
        new(LeapCentury, 12, 36, 330, false),
    };

    public override TheoryData<YearInfo> YearInfoData => new()
    {
        new(-9, 12, 365, false),
        new(-8, 12, 366, true),
        new(-7, 12, 365, false),
        new(-6, 12, 365, false),
        new(-5, 12, 365, false),
        new(-4, 12, 366, true),
        new(-3, 12, 365, false),
        new(-2, 12, 365, false),
        new(-1, 12, 365, false),
        new(0, 12, 365, false), // année séculaire divisible par 400 et 4000.
        new(CommonYear, 12, 365, false),
        new(2, 12, 365, false),
        new(3, 12, 365, false),
        new(LeapYear, 12, 366, true),
        new(5, 12, 365, false),
        new(6, 12, 365, false),
        new(7, 12, 365, false),
        new(8, 12, 366, true),
        new(9, 12, 365, false),
        // Années séculaires non divisibles par 400.
        new(-900, 12, 365, false),
        new(-700, 12, 365, false),
        new(-600, 12, 365, false),
        new(-500, 12, 365, false),
        new(-300, 12, 365, false),
        new(-200, 12, 365, false),
        new(-100, 12, 365, false),
        new(100, 12, 365, false),
        new(200, 12, 365, false),
        new(300, 12, 365, false),
        new(500, 12, 365, false),
        new(600, 12, 365, false),
        new(700, 12, 365, false),
        new(900, 12, 365, false),
        // Années séculaires multiples de 400 mais pas de 4000.
        new(-800, 12, 366, true),
        new(-400, 12, 366, true),
        new(LeapCentury, 12, 366, true),
        new(800, 12, 366, true),
        // Années séculaires mutiples de 4000.
        new(-8000, 12, 365, false),
        new(-4000, 12, 365, false),
        new(CommonCentury, 12, 365, false),
        new(8000, 12, 365, false),
    };

    internal static List<(int RataDie, int Year, int Month, int Day)> RataDieInfos { get; } = new()
    {
        // Epoch.
        (654_415, 1, 1, 1),

        // D.&R. Appendix C.
        (-214_193, -2378, 11, 4),
        (-61_387, -1959, 3, 13),
        (25_469, -1721, 1, 2),
        (49_217, -1656, 1, 10),
        (171_307, -1322, 4, 18),
        (210_155, -1216, 9, 1),
        (253_427, -1097, 2, 19),
        (369_740, -779, 8, 4),
        (400_085, -696, 9, 5),
        (434_355, -602, 7, 1),
        (452_605, -552, 6, 20),
        (470_160, -504, 7, 13),
        (473_837, -494, 8, 8),
        (507_850, -401, 9, 23),
        (524_156, -356, 5, 13),
        (544_676, -300, 7, 19),
        (567_118, -239, 12, 31),
        (569_477, -232, 6, 14),
        (601_716, -144, 9, 22),
        (613_424, -112, 10, 12),
        (626_596, -76, 11, 6),
        (645_554, -24, 10, 1),
        (664_224, 27, 11, 14),
        (671_401, 47, 7, 6),
        (694_799, 111, 7, 29),
        (704_424, 137, 12, 7),
        (708_842, 150, 1, 7),
        (709_409, 151, 7, 29),
        (709_580, 152, 1, 15),
        (727_274, 200, 6, 27),
        (728_714, 204, 6, 7),
        (744_313, 247, 2, 20),
        (764_652, 302, 11, 1),
    };
}

public partial class FrenchRepublican12DataSet // Start and end of year
{
    public override TheoryData<Yemoda> EndOfYearPartsData => new()
    {
        new(CommonYear, 12, 35),
        new(LeapYear, 12, 36),
        new(CommonCentury, 12, 35),
        new(LeapCentury, 12, 36),
    };

    public override TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-3999, -(int)FrenchRepublican12Schema.DaysPer4000YearCycle),
        new(-10, -4017),
        new(-9, -3652),
        new(-8, -3287), // leap year
        new(-7, -2921),
        new(-6, -2556),
        new(-5, -2191),
        new(-4, -1826), // leap year
        new(-3, -1460),
        new(-2, -1095),
        new(-1, -730),
        new(0, -365),
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
        new(4001, (int)FrenchRepublican12Schema.DaysPer4000YearCycle),
    };
}

public partial class FrenchRepublican12DataSet // Invalid date parts
{
    public override TheoryData<int, int> InvalidMonthFieldData => new()
    {
        { CommonYear, 0 },
        { CommonYear, 13 },
        { LeapYear, 0 },
        { LeapYear, 13 },
        { CommonCentury, 0 },
        { CommonCentury, 13 },
        { LeapCentury, 0 },
        { LeapCentury, 13 },
    };

    public override TheoryData<int, int> InvalidDayOfYearFieldData => new()
    {
        { CommonYear, 0 },
        { CommonYear, 366 },
        { LeapYear, 0 },
        { LeapYear, 367 },
        { CommonCentury, 0 },
        { CommonCentury, 366 },
        { LeapCentury, 0 },
        { LeapCentury, 367 },
    };

    public override TheoryData<int, int, int> InvalidDayFieldData => new()
    {
        // Common year.
        { CommonYear, 1, 0 },
        { CommonYear, 1, 31 },
        { CommonYear, 2, 0 },
        { CommonYear, 2, 31 },
        { CommonYear, 3, 0 },
        { CommonYear, 3, 31 },
        { CommonYear, 4, 0 },
        { CommonYear, 4, 31 },
        { CommonYear, 5, 0 },
        { CommonYear, 5, 31 },
        { CommonYear, 6, 0 },
        { CommonYear, 6, 31 },
        { CommonYear, 7, 0 },
        { CommonYear, 7, 31 },
        { CommonYear, 8, 0 },
        { CommonYear, 8, 31 },
        { CommonYear, 9, 0 },
        { CommonYear, 9, 31 },
        { CommonYear, 10, 0 },
        { CommonYear, 10, 31 },
        { CommonYear, 11, 0 },
        { CommonYear, 11, 31 },
        { CommonYear, 12, 0 },
        { CommonYear, 12, 36 },
        // Leap year.
        { LeapYear, 1, 0 },
        { LeapYear, 1, 31 },
        { LeapYear, 2, 0 },
        { LeapYear, 2, 31 },
        { LeapYear, 3, 0 },
        { LeapYear, 3, 31 },
        { LeapYear, 4, 0 },
        { LeapYear, 4, 31 },
        { LeapYear, 5, 0 },
        { LeapYear, 5, 31 },
        { LeapYear, 6, 0 },
        { LeapYear, 6, 31 },
        { LeapYear, 7, 0 },
        { LeapYear, 7, 31 },
        { LeapYear, 8, 0 },
        { LeapYear, 8, 31 },
        { LeapYear, 9, 0 },
        { LeapYear, 9, 31 },
        { LeapYear, 10, 0 },
        { LeapYear, 10, 31 },
        { LeapYear, 11, 0 },
        { LeapYear, 11, 31 },
        { LeapYear, 12, 0 },
        { LeapYear, 12, 37 },
        // Common century.
        { CommonCentury, 1, 0 },
        { CommonCentury, 1, 31 },
        { CommonCentury, 2, 0 },
        { CommonCentury, 2, 31 },
        { CommonCentury, 3, 0 },
        { CommonCentury, 3, 31 },
        { CommonCentury, 4, 0 },
        { CommonCentury, 4, 31 },
        { CommonCentury, 5, 0 },
        { CommonCentury, 5, 31 },
        { CommonCentury, 6, 0 },
        { CommonCentury, 6, 31 },
        { CommonCentury, 7, 0 },
        { CommonCentury, 7, 31 },
        { CommonCentury, 8, 0 },
        { CommonCentury, 8, 31 },
        { CommonCentury, 9, 0 },
        { CommonCentury, 9, 31 },
        { CommonCentury, 10, 0 },
        { CommonCentury, 10, 31 },
        { CommonCentury, 11, 0 },
        { CommonCentury, 11, 31 },
        { CommonCentury, 12, 0 },
        { CommonCentury, 12, 36 },
        // Leap century.
        { LeapCentury, 1, 0 },
        { LeapCentury, 1, 31 },
        { LeapCentury, 2, 0 },
        { LeapCentury, 2, 31 },
        { LeapCentury, 3, 0 },
        { LeapCentury, 3, 31 },
        { LeapCentury, 4, 0 },
        { LeapCentury, 4, 31 },
        { LeapCentury, 5, 0 },
        { LeapCentury, 5, 31 },
        { LeapCentury, 6, 0 },
        { LeapCentury, 6, 31 },
        { LeapCentury, 7, 0 },
        { LeapCentury, 7, 31 },
        { LeapCentury, 8, 0 },
        { LeapCentury, 8, 31 },
        { LeapCentury, 9, 0 },
        { LeapCentury, 9, 31 },
        { LeapCentury, 10, 0 },
        { LeapCentury, 10, 31 },
        { LeapCentury, 11, 0 },
        { LeapCentury, 11, 31 },
        { LeapCentury, 12, 0 },
        { LeapCentury, 12, 37 },
    };
}

public partial class FrenchRepublican12DataSet // Supplementary data
{
    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 12, 31, 1),
        new(CommonYear, 12, 32, 2),
        new(CommonYear, 12, 33, 3),
        new(CommonYear, 12, 34, 4),
        new(CommonYear, 12, 35, 5),
        // Leap year.
        new(LeapYear, 12, 31, 1),
        new(LeapYear, 12, 32, 2),
        new(LeapYear, 12, 33, 3),
        new(LeapYear, 12, 34, 4),
        new(LeapYear, 12, 35, 5),
        new(LeapYear, 12, 36, 6),
    };
}
