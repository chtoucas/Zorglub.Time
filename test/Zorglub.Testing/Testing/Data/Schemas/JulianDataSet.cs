// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for <see cref="JulianSchema"/>.
/// </summary>
public sealed partial class JulianDataSet : CalendricalDataSet, ISingleton<JulianDataSet>
{
    public const int CommonYear = 1;
    public const int LeapYear = 4;

    private JulianDataSet() : base(new JulianSchema(), CommonYear, LeapYear) { }

    public static JulianDataSet Instance { get; } = new();
}

public partial class JulianDataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??=
            DataGroup.CreateDaysSinceEpochInfoData(DaysSinceRataDieInfos, DayZero.OldStyle);

    public override TheoryData<DateInfo> DateInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 31, 31, false, false),
        new(CommonYear, 2, 1, 32, false, false),
        new(CommonYear, 2, 28, 59, false, false),
        new(CommonYear, 3, 1, 60, false, false),
        new(CommonYear, 3, 2, 61, false, false),
        new(CommonYear, 4, 1, 91, false, false),
        new(CommonYear, 5, 1, 121, false, false),
        new(CommonYear, 6, 1, 152, false, false),
        new(CommonYear, 7, 1, 182, false, false),
        new(CommonYear, 8, 1, 213, false, false),
        new(CommonYear, 9, 1, 244, false, false),
        new(CommonYear, 10, 1, 274, false, false),
        new(CommonYear, 11, 1, 305, false, false),
        new(CommonYear, 12, 1, 335, false, false),
        new(CommonYear, 12, 31, 365, false, false),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 31, 31, false, false),
        new(LeapYear, 2, 1, 32, false, false),
        new(LeapYear, 2, 28, 59, false, false),
        new(LeapYear, 2, 29, 60, true, false), // intercalary day
        new(LeapYear, 3, 1, 61, false, false),
        new(LeapYear, 4, 1, 92, false, false),
        new(LeapYear, 5, 1, 122, false, false),
        new(LeapYear, 6, 1, 153, false, false),
        new(LeapYear, 7, 1, 183, false, false),
        new(LeapYear, 8, 1, 214, false, false),
        new(LeapYear, 9, 1, 245, false, false),
        new(LeapYear, 10, 1, 275, false, false),
        new(LeapYear, 11, 1, 306, false, false),
        new(LeapYear, 12, 1, 336, false, false),
        new(LeapYear, 12, 31, 366, false, false),
    };

    public override TheoryData<MonthInfo> MonthInfoData { get; } = new()
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

    public override TheoryData<YearInfo> YearInfoData { get; } = new()
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
        new(0, 12, 366, true),
        new(CommonYear, 12, 365, false),
        new(2, 12, 365, false),
        new(3, 12, 365, false),
        new(LeapYear, 12, 366, true),
        new(5, 12, 365, false),
        new(6, 12, 365, false),
        new(7, 12, 365, false),
        new(8, 12, 366, true),
        new(9, 12, 365, false),
    };

    internal static IEnumerable<DaysSinceRataDieInfo> DaysSinceRataDieInfos
    {
        get
        {
            // Epoch.
            yield return new(-1, 1, 1, 1);
            yield return new(0, 1, 1, 2);
            yield return new(1, 1, 1, 3);

            // D.&R. Appendix C.
            yield return new(-214_193, -586, 7, 30);
            yield return new(-61_387, -168, 12, 8);
            yield return new(25_469, 70, 9, 26);
            yield return new(49_217, 135, 10, 3);
            yield return new(171_307, 470, 1, 7);
            yield return new(210_155, 576, 5, 18);
            yield return new(253_427, 694, 11, 7);
            yield return new(369_740, 1013, 4, 19);
            yield return new(400_085, 1096, 5, 18);
            yield return new(434_355, 1190, 3, 16);
            yield return new(452_605, 1240, 3, 3);
            yield return new(470_160, 1288, 3, 26);
            yield return new(473_837, 1298, 4, 20);
            yield return new(507_850, 1391, 6, 4);
            yield return new(524_156, 1436, 1, 25);
            yield return new(544_676, 1492, 3, 31);
            yield return new(567_118, 1553, 9, 9);
            yield return new(569_477, 1560, 2, 24);
            yield return new(601_716, 1648, 5, 31);
            yield return new(613_424, 1680, 6, 20);
            yield return new(626_596, 1716, 7, 13);
            yield return new(645_554, 1768, 6, 8);
            yield return new(664_224, 1819, 7, 21);
            yield return new(671_401, 1839, 3, 15);
            yield return new(694_799, 1903, 4, 6);
            yield return new(704_424, 1929, 8, 12);
            yield return new(708_842, 1941, 9, 16);
            yield return new(709_409, 1943, 4, 6);
            yield return new(709_580, 1943, 9, 24);
            yield return new(727_274, 1992, 3, 4);
            yield return new(728_714, 1996, 2, 12);
            yield return new(744_313, 2038, 10, 28);
            yield return new(764_652, 2094, 7, 5);
        }
    }
}

public partial class JulianDataSet // Start and end of year
{
    public override TheoryData<Yemoda> EndOfYearPartsData { get; } = new()
    {
        new(CommonYear, 12, 31),
        new(LeapYear, 12, 31),
    };

    public override TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-10, -4018),
        new(-9, -3653),
        new(-8, -3288), // leap year
        new(-7, -2922),
        new(-6, -2557),
        new(-5, -2192),
        new(-4, -1827), // leap year
        new(-3, -JulianSchema.DaysPer4YearCycle),
        new(-2, -1096),
        new(-1, -731),
        new(0, -366), // leap year
        new(1, 0),
        new(2, 365),
        new(3, 730),
        new(4, 1095), // leap year
        new(5, JulianSchema.DaysPer4YearCycle),
        new(6, 1826),
        new(7, 2191),
        new(8, 2556), // leap year
        new(9, 2922),
        new(10, 3287),
    };
}

public partial class JulianDataSet // Invalid date parts
{
    public override TheoryData<int, int> InvalidMonthFieldData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 13 },
        { LeapYear, 0 },
        { LeapYear, 13 },
    };

    public override TheoryData<int, int> InvalidDayOfYearFieldData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 366 },
        { LeapYear, 0 },
        { LeapYear, 367 },
    };

    public override TheoryData<int, int, int> InvalidDayFieldData { get; } = new()
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
