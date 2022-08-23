// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for <see cref="TabularIslamicSchema"/>.
/// </summary>
public sealed partial class TabularIslamicDataSet : SchemaDataSet, ISingleton<TabularIslamicDataSet>
{
    public const int CommonYear = 1;
    public const int LeapYear = 2;

    private TabularIslamicDataSet() : base(new TabularIslamicSchema(), CommonYear, LeapYear) { }

    public static TabularIslamicDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly TabularIslamicDataSet Instance = new();
        static Singleton() { }
    }
}

public partial class TabularIslamicDataSet // Infos
{
    public override DataGroup<MonthsSinceEpochInfo> MonthsSinceEpochInfoData { get; } =
        GenMonthsSinceEpochInfoData(12);

    public override DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; } =
        DataGroup.CreateDaysSinceEpochInfoData(DaysSinceRataDieInfos, DayZero.TabularIslamic);

    public override DataGroup<DateInfo> DateInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 30, 30, false, false),
        new(CommonYear, 2, 29, 59, false, false),
        new(CommonYear, 3, 30, 89, false, false),
        new(CommonYear, 4, 29, 118, false, false),
        new(CommonYear, 5, 30, 148, false, false),
        new(CommonYear, 6, 29, 177, false, false),
        new(CommonYear, 7, 30, 207, false, false),
        new(CommonYear, 8, 29, 236, false, false),
        new(CommonYear, 9, 30, 266, false, false),
        new(CommonYear, 10, 29, 295, false, false),
        new(CommonYear, 11, 30, 325, false, false),
        new(CommonYear, 12, 29, 354, false, false),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 30, 30, false, false),
        new(LeapYear, 2, 29, 59, false, false),
        new(LeapYear, 3, 30, 89, false, false),
        new(LeapYear, 4, 29, 118, false, false),
        new(LeapYear, 5, 30, 148, false, false),
        new(LeapYear, 6, 29, 177, false, false),
        new(LeapYear, 7, 30, 207, false, false),
        new(LeapYear, 8, 29, 236, false, false),
        new(LeapYear, 9, 30, 266, false, false),
        new(LeapYear, 10, 29, 295, false, false),
        new(LeapYear, 11, 30, 325, false, false),
        new(LeapYear, 12, 29, 354, false, false),
        new(LeapYear, 12, 30, 355, true, false),
    };

    public override DataGroup<MonthInfo> MonthInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 30, 0, 324, false),
        new(CommonYear, 2, 29, 30, 295, false),
        new(CommonYear, 3, 30, 59, 265, false),
        new(CommonYear, 4, 29, 89, 236, false),
        new(CommonYear, 5, 30, 118, 206, false),
        new(CommonYear, 6, 29, 148, 177, false),
        new(CommonYear, 7, 30, 177, 147, false),
        new(CommonYear, 8, 29, 207, 118, false),
        new(CommonYear, 9, 30, 236, 88, false),
        new(CommonYear, 10, 29, 266, 59, false),
        new(CommonYear, 11, 30, 295, 29, false),
        new(CommonYear, 12, 29, 325, 0, false),
        // Leap year.
        new(LeapYear, 1, 30, 0, 325, false),
        new(LeapYear, 2, 29, 30, 296, false),
        new(LeapYear, 3, 30, 59, 266, false),
        new(LeapYear, 4, 29, 89, 237, false),
        new(LeapYear, 5, 30, 118, 207, false),
        new(LeapYear, 6, 29, 148, 178, false),
        new(LeapYear, 7, 30, 177, 148, false),
        new(LeapYear, 8, 29, 207, 119, false),
        new(LeapYear, 9, 30, 236, 89, false),
        new(LeapYear, 10, 29, 266, 60, false),
        new(LeapYear, 11, 30, 295, 30, false),
        new(LeapYear, 12, 30, 325, 0, false),
    };

    public override DataGroup<YearInfo> YearInfoData { get; } = new()
    {
        new(-30, 12, 354, false),
        // Cycle de 30 ans.
        new(-29, 12, 354, false),
        new(-28, 12, 355, true),  // 2e année
        new(-27, 12, 354, false),
        new(-26, 12, 354, false),
        new(-25, 12, 355, true),  // 5e année
        new(-24, 12, 354, false),
        new(-23, 12, 355, true),  // 7e année
        new(-22, 12, 354, false),
        new(-21, 12, 354, false),
        new(-20, 12, 355, true),  // 10e année
        new(-19, 12, 354, false),
        new(-18, 12, 354, false),
        new(-17, 12, 355, true),  // 13e année
        new(-16, 12, 354, false),
        new(-15, 12, 354, false),
        new(-14, 12, 355, true),  // 16e année
        new(-13, 12, 354, false),
        new(-12, 12, 355, true),  // 18e année
        new(-11, 12, 354, false),
        new(-10, 12, 354, false),
        new(-9, 12, 355, true),  // 21e année
        new(-8, 12, 354, false),
        new(-7, 12, 354, false),
        new(-6, 12, 355, true),  // 24e année
        new(-5, 12, 354, false),
        new(-4, 12, 355, true),  // 26e année
        new(-3, 12, 354, false),
        new(-2, 12, 354, false),
        new(-1, 12, 355, true),  // 29e année
        new(0, 12, 354, false),
        // Cycle de 30 ans.
        new(CommonYear, 12, 354, false),
        new(LeapYear, 12, 355, true),
        new(3, 12, 354, false),
        new(4, 12, 354, false),
        new(5, 12, 355, true),
        new(6, 12, 354, false),
        new(7, 12, 355, true),
        new(8, 12, 354, false),
        new(9, 12, 354, false),
        new(10, 12, 355, true),
        new(11, 12, 354, false),
        new(12, 12, 354, false),
        new(13, 12, 355, true),
        new(14, 12, 354, false),
        new(15, 12, 354, false),
        new(16, 12, 355, true),
        new(17, 12, 354, false),
        new(18, 12, 355, true),
        new(19, 12, 354, false),
        new(20, 12, 354, false),
        new(21, 12, 355, true),
        new(22, 12, 354, false),
        new(23, 12, 354, false),
        new(24, 12, 355, true),
        new(25, 12, 354, false),
        new(26, 12, 355, true),
        new(27, 12, 354, false),
        new(28, 12, 354, false),
        new(29, 12, 355, true),
        new(30, 12, 354, false),
    };

    internal static IEnumerable<DaysSinceRataDieInfo> DaysSinceRataDieInfos
    {
        get
        {
            // Epoch.
            yield return new(227_015, 1, 1, 1);

            // D.&R. Appendix C.
            yield return new(-214_193, -1245, 12, 9);
            yield return new(-61_387, -813, 2, 23);
            yield return new(25_469, -568, 4, 1);
            yield return new(49_217, -501, 4, 6);
            yield return new(171_307, -157, 10, 17);
            yield return new(210_155, -47, 6, 3);
            yield return new(253_427, 75, 7, 13);
            yield return new(369_740, 403, 10, 5);
            yield return new(400_085, 489, 5, 22);
            yield return new(434_355, 586, 2, 7);
            yield return new(452_605, 637, 8, 7);
            yield return new(470_160, 687, 2, 20);
            yield return new(473_837, 697, 7, 7);
            yield return new(507_850, 793, 7, 1);
            yield return new(524_156, 839, 7, 6);
            yield return new(544_676, 897, 6, 1);
            yield return new(567_118, 960, 9, 30);
            yield return new(569_477, 967, 5, 27);
            yield return new(601_716, 1058, 5, 18);
            yield return new(613_424, 1091, 6, 2);
            yield return new(626_596, 1128, 8, 4);
            yield return new(645_554, 1182, 2, 3);
            yield return new(664_224, 1234, 10, 10);
            yield return new(671_401, 1255, 1, 11);
            yield return new(694_799, 1321, 1, 21);
            yield return new(704_424, 1348, 3, 19);
            yield return new(708_842, 1360, 9, 8);
            yield return new(709_409, 1362, 4, 13);
            yield return new(709_580, 1362, 10, 7);
            yield return new(727_274, 1412, 9, 13);
            yield return new(728_714, 1416, 10, 5);
            yield return new(744_313, 1460, 10, 12);
            yield return new(764_652, 1518, 3, 5);
        }
    }
}

public partial class TabularIslamicDataSet // Start and end of year
{
    public override DataGroup<Yemoda> EndOfYearPartsData { get; } = new()
    {
        new(CommonYear, 12, 29),
        new(LeapYear, 12, 30),
    };

    public override DataGroup<YearMonthsSinceEpoch> StartOfYearMonthsSinceEpochData { get; } =
        GenStartOfYearMonthsSinceEpochData(12);

    public override DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-30, -10985),

        // Cycle of 30 years.
        new(-29, -TabularIslamicSchema.DaysPer30YearCycle),
        new(-28, -10277),  // leap year
        new(-27, -9922),
        new(-26, -9568),
        new(-25, -9214),  // leap year
        new(-24, -8859),
        new(-23, -8505),  // leap year
        new(-22, -8150),
        new(-21, -7796),
        new(-20, -7442),  // leap year
        new(-19, -7087),
        new(-18, -6733),
        new(-17, -6379),  // leap year
        new(-16, -6024),
        new(-15, -5670),
        new(-14, -5316),  // leap year
        new(-13, -4961),
        new(-12, -4607),  // leap year
        new(-11, -4252),
        new(-10, -3898),
        new(-9, -3544),  // leap year
        new(-8, -3189),
        new(-7, -2835),
        new(-6, -2481),  // leap year
        new(-5, -2126),
        new(-4, -1772),  // leap year
        new(-3, -1417),
        new(-2, -1063),
        new(-1, -709), // leap year
        new(0, -354),

        // Cycle of 30 years.
        new(1, 0),
        new(2, 354),
        new(3, 709), // leap year
        new(4, 1063),
        new(5, 1417), // leap year
        new(6, 1772),
        new(7, 2126), // leap year
        new(8, 2481),
        new(9, 2835),
        new(10, 3189), // leap year
        new(11, 3544),
        new(12, 3898),
        new(13, 4252), // leap year
        new(14, 4607),
        new(15, 4961),
        new(16, 5315), // leap year
        new(17, 5670),
        new(18, 6024), // leap year
        new(19, 6379),
        new(20, 6733),
        new(21, 7087), // leap year
        new(22, 7442),
        new(23, 7796),
        new(24, 8150), // leap year
        new(25, 8505),
        new(26, 8859), // leap year
        new(27, 9214),
        new(28, 9568),
        new(29, 9922), // leap year
        new(30, 10_277),

        new(31, TabularIslamicSchema.DaysPer30YearCycle),
    };
}

public partial class TabularIslamicDataSet // Invalid date parts
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
        { CommonYear, 355 },
        { LeapYear, 0 },
        { LeapYear, 356 },
    };

    public override TheoryData<int, int, int> InvalidDayFieldData { get; } = new()
    {
        // Common year.
        { CommonYear, 1, 0 },
        { CommonYear, 1, 31 },
        { CommonYear, 2, 0 },
        { CommonYear, 2, 30 },
        { CommonYear, 3, 0 },
        { CommonYear, 3, 31 },
        { CommonYear, 4, 0 },
        { CommonYear, 4, 30 },
        { CommonYear, 5, 0 },
        { CommonYear, 5, 31 },
        { CommonYear, 6, 0 },
        { CommonYear, 6, 30 },
        { CommonYear, 7, 0 },
        { CommonYear, 7, 31 },
        { CommonYear, 8, 0 },
        { CommonYear, 8, 30 },
        { CommonYear, 9, 0 },
        { CommonYear, 9, 31 },
        { CommonYear, 10, 0 },
        { CommonYear, 10, 30 },
        { CommonYear, 11, 0 },
        { CommonYear, 11, 31 },
        { CommonYear, 12, 0 },
        { CommonYear, 12, 30 },
        // Leap year.
        { LeapYear, 1, 0 },
        { LeapYear, 1, 31 },
        { LeapYear, 2, 0 },
        { LeapYear, 2, 30 },
        { LeapYear, 3, 0 },
        { LeapYear, 3, 31 },
        { LeapYear, 4, 0 },
        { LeapYear, 4, 30 },
        { LeapYear, 5, 0 },
        { LeapYear, 5, 31 },
        { LeapYear, 6, 0 },
        { LeapYear, 6, 30 },
        { LeapYear, 7, 0 },
        { LeapYear, 7, 31 },
        { LeapYear, 8, 0 },
        { LeapYear, 8, 30 },
        { LeapYear, 9, 0 },
        { LeapYear, 9, 31 },
        { LeapYear, 10, 0 },
        { LeapYear, 10, 30 },
        { LeapYear, 11, 0 },
        { LeapYear, 11, 31 },
        { LeapYear, 12, 0 },
        { LeapYear, 12, 31 },
    };
}

public partial class TabularIslamicDataSet // Math data
{
    public override DataGroup<YemodaPairAnd<int>> AddDaysData => new DataGroup<YemodaPairAnd<int>>()
    {
        //
        // Branch AddDaysViaDayOfYear()
        //
        // MinDaysInMonth = 29 < |days| <= MinDaysInYear = 354

        // Change of month.
        new(new(3, 4, 1), new(3, 5, 1), 29),

        // December, common year.
        new(new(CommonYear, 12, 29), new(CommonYear - 1, 12, 29), -354),
        new(new(CommonYear, 12, 29), new(CommonYear + 1, 12, 29), 354),
        // December, leap year.
        new(new(LeapYear, 12, 29), new(LeapYear - 1, 12, 29), -354),
        new(new(LeapYear, 12, 29), new(LeapYear + 1, 12, 28), 354),
        new(new(LeapYear, 12, 30), new(LeapYear, 1, 1), -354),
        new(new(LeapYear, 12, 30), new(LeapYear + 1, 12, 29), 354),
    }.ConcatT(base.AddDaysData);

    public override DataGroup<YemodaPair> ConsecutiveDaysData => new DataGroup<YemodaPair>()
    {
        // End of month.
        new(new(CommonYear, 1, 30), new(CommonYear, 2, 1)),
        new(new(CommonYear, 2, 29), new(CommonYear, 3, 1)),
        new(new(CommonYear, 3, 30), new(CommonYear, 4, 1)),
        new(new(CommonYear, 4, 29), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 30), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 29), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 30), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 29), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 30), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 29), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 30), new(CommonYear, 12, 1)),
        new(new(CommonYear, 12, 29), new(CommonYear + 1, 1, 1)),
        new(new(LeapYear, 12, 29), new(LeapYear, 12, 30)),
        new(new(LeapYear, 12, 30), new(LeapYear + 1, 1, 1)),
    }.ConcatT(base.ConsecutiveDaysData);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 354), new(CommonYear + 1, 1)),
        new(new(LeapYear, 354), new(LeapYear, 355)),
        new(new(LeapYear, 355), new(LeapYear + 1, 1)),
    }.ConcatT(base.ConsecutiveDaysOrdinalData);
}
