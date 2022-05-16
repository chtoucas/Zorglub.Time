// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for <see cref="Egyptian12Schema"/>.
/// </summary>
public sealed partial class Egyptian12DataSet :
    CalendricalDataSet, IEpagomenalDataSet, ISingleton<Egyptian12DataSet>
{
    public const int SampleYear = 3;

    private Egyptian12DataSet() : base(new Egyptian12Schema(), SampleYear, SampleYear) { }

    public static Egyptian12DataSet Instance { get; } = new();
}

public partial class Egyptian12DataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??=
            DataGroup.OfDaysSinceEpochInfo.Create(DaysSinceRataDieInfos, CalendarEpoch.Egyptian);

    public override TheoryData<DateInfo> DateInfoData => new()
    {
        new(SampleYear, 1, 1, 1, false, false),
        new(SampleYear, 1, 30, 30, false, false),
        new(SampleYear, 2, 1, 31, false, false),
        new(SampleYear, 2, 30, 60, false, false),
        new(SampleYear, 3, 1, 61, false, false),
        new(SampleYear, 3, 30, 90, false, false),
        new(SampleYear, 4, 1, 91, false, false),
        new(SampleYear, 4, 30, 120, false, false),
        new(SampleYear, 5, 1, 121, false, false),
        new(SampleYear, 5, 30, 150, false, false),
        new(SampleYear, 6, 1, 151, false, false),
        new(SampleYear, 6, 30, 180, false, false),
        new(SampleYear, 7, 1, 181, false, false),
        new(SampleYear, 7, 30, 210, false, false),
        new(SampleYear, 8, 1, 211, false, false),
        new(SampleYear, 8, 30, 240, false, false),
        new(SampleYear, 9, 1, 241, false, false),
        new(SampleYear, 9, 30, 270, false, false),
        new(SampleYear, 10, 1, 271, false, false),
        new(SampleYear, 10, 30, 300, false, false),
        new(SampleYear, 11, 1, 301, false, false),
        new(SampleYear, 11, 30, 330, false, false),
        new(SampleYear, 12, 1, 331, false, false),
        new(SampleYear, 12, 30, 360, false, false),
        // Epagomenal days.
        new(SampleYear, 12, 31, 361, false, true),
        new(SampleYear, 12, 32, 362, false, true),
        new(SampleYear, 12, 33, 363, false, true),
        new(SampleYear, 12, 34, 364, false, true),
        new(SampleYear, 12, 35, 365, false, true),
    };

    public override TheoryData<MonthInfo> MonthInfoData => new()
    {
        new(SampleYear, 1, 30, 0, false),
        new(SampleYear, 2, 30, 30, false),
        new(SampleYear, 3, 30, 60, false),
        new(SampleYear, 4, 30, 90, false),
        new(SampleYear, 5, 30, 120, false),
        new(SampleYear, 6, 30, 150, false),
        new(SampleYear, 7, 30, 180, false),
        new(SampleYear, 8, 30, 210, false),
        new(SampleYear, 9, 30, 240, false),
        new(SampleYear, 10, 30, 270, false),
        new(SampleYear, 11, 30, 300, false),
        new(SampleYear, 12, 35, 330, false),
    };

    public override TheoryData<YearInfo> YearInfoData => new()
    {
        new(SampleYear, 12, 365, false),
    };

    internal static IEnumerable<DaysSinceRataDieInfo> DaysSinceRataDieInfos
    {
        get
        {
            // Epoch.
            yield return new(-272_787, 1, 1, 1);

            // Epagomenal days.
            yield return new(613_428, 2428, 12, 31);
            yield return new(613_429, 2428, 12, 32);
            yield return new(613_430, 2428, 12, 33);
            yield return new(613_431, 2428, 12, 34);
            yield return new(613_432, 2428, 12, 35);

            // D.&R. Appendix C.
            yield return new(-214_193, 161, 7, 15);
            yield return new(-61_387, 580, 3, 6);
            yield return new(25_469, 818, 2, 22);
            yield return new(49_217, 883, 3, 15);
            yield return new(171_307, 1217, 9, 15);
            yield return new(210_155, 1324, 2, 18);
            yield return new(253_427, 1442, 9, 10);
            yield return new(369_740, 1761, 5, 8);
            yield return new(400_085, 1844, 6, 28);
            yield return new(434_355, 1938, 5, 18);
            yield return new(452_605, 1988, 5, 18);
            yield return new(470_160, 2036, 6, 23);
            yield return new(473_837, 2046, 7, 20);
            yield return new(507_850, 2139, 9, 28);
            yield return new(524_156, 2184, 5, 29);
            yield return new(544_676, 2240, 8, 19);
            yield return new(567_118, 2302, 2, 11);
            yield return new(569_477, 2308, 7, 30);
            yield return new(601_716, 2396, 11, 29);
            yield return new(613_424, 2428, 12, 27);
            yield return new(626_596, 2465, 1, 24);
            yield return new(645_554, 2517, 1, 2);
            yield return new(664_224, 2568, 2, 27);
            yield return new(671_401, 2587, 10, 29);
            yield return new(694_799, 2651, 12, 7);
            yield return new(704_424, 2678, 4, 17);
            yield return new(708_842, 2690, 5, 25);
            yield return new(709_409, 2691, 12, 17);
            yield return new(709_580, 2692, 6, 3);
            yield return new(727_274, 2740, 11, 27);
            yield return new(728_714, 2744, 11, 7);
            yield return new(744_313, 2787, 8, 1);
            yield return new(764_652, 2843, 4, 20);
        }
    }
}

public partial class Egyptian12DataSet // Start and end of year
{
    public override TheoryData<Yemoda> EndOfYearPartsData => new()
    {
        new(SampleYear, 12, 35),
    };

    public override TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-10, -4015),
        new(-9, -3650),
        new(-8, -3285),
        new(-7, -2920),
        new(-6, -2555),
        new(-5, -2190),
        new(-4, -1825),
        new(-3, -1460),
        new(-2, -1095),
        new(-1, -730),
        new(0, -365),
        new(1, 0),
        new(2, 365),
        new(3, 730),
        new(4, 1095),
        new(5, 1460),
        new(6, 1825),
        new(7, 2190),
        new(8, 2555),
        new(9, 2920),
        new(10, 3285),
    };
}

public partial class Egyptian12DataSet // Invalid date parts
{
    public override TheoryData<int, int> InvalidMonthFieldData => new()
    {
        { SampleYear, 0 },
        { SampleYear, 13 },
    };

    public override TheoryData<int, int> InvalidDayOfYearFieldData => new()
    {
        { SampleYear, 0 },
        { SampleYear, 366 },
    };

    public override TheoryData<int, int, int> InvalidDayFieldData => new()
    {
        { SampleYear, 1, 0 },
        { SampleYear, 1, 31 },
        { SampleYear, 2, 0 },
        { SampleYear, 2, 31 },
        { SampleYear, 3, 0 },
        { SampleYear, 3, 31 },
        { SampleYear, 4, 0 },
        { SampleYear, 4, 31 },
        { SampleYear, 5, 0 },
        { SampleYear, 5, 31 },
        { SampleYear, 6, 0 },
        { SampleYear, 6, 31 },
        { SampleYear, 7, 0 },
        { SampleYear, 7, 31 },
        { SampleYear, 8, 0 },
        { SampleYear, 8, 31 },
        { SampleYear, 9, 0 },
        { SampleYear, 9, 31 },
        { SampleYear, 10, 0 },
        { SampleYear, 10, 31 },
        { SampleYear, 11, 0 },
        { SampleYear, 11, 31 },
        { SampleYear, 12, 0 },
        { SampleYear, 12, 36 },
    };
}

public partial class Egyptian12DataSet // Supplementary data
{
    public TheoryData<EpagomenalDayInfo> EpagomenalDayInfoData { get; } = new()
    {
        new(SampleYear, 12, 31, 1),
        new(SampleYear, 12, 32, 2),
        new(SampleYear, 12, 33, 3),
        new(SampleYear, 12, 34, 4),
        new(SampleYear, 12, 35, 5),
    };
}
