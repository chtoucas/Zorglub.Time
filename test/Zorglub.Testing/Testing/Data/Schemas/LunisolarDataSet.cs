// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for <see cref="LunisolarSchema"/>.
/// </summary>
public sealed partial class LunisolarDataSet : SchemaDataSet, ISingleton<LunisolarDataSet>
{
    public const int CommonYear = 1;
    public const int LeapYear = 4;

    private LunisolarDataSet() : base(new LunisolarSchema(), CommonYear, LeapYear) { }

    public static LunisolarDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly LunisolarDataSet Instance = new();
        static Singleton() { }
    }
}

public partial class LunisolarDataSet // Infos
{
    public override DataGroup<MonthsSinceEpochInfo> MonthsSinceEpochInfoData { get; } = new()
    {
        new(-25, -1, 1),
        new(-13, 0, 1), // leap year
        new(0, 1, 1),
        new(1, 1, 2),
        new(2, 1, 3),
        new(3, 1, 4),
        new(4, 1, 5),
        new(5, 1, 6),
        new(6, 1, 7),
        new(7, 1, 8),
        new(8, 1, 9),
        new(9, 1, 10),
        new(10, 1, 11),
        new(11, 1, 12),
        new(12, 2, 1),
        new(24, 3, 1),
        new(36, 4, 1), // leap year
        new(49, 5, 1),
    };

    public override DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; } =
        DataGroup.CreateDaysSinceEpochInfoData(DaysSinceRataDieInfos, DayZero.NewStyle);

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
        new(LeapYear, 13, 30, 384, false, false),
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
        new(LeapYear, 1, 30, 0, 354, false),
        new(LeapYear, 2, 29, 30, 325, false),
        new(LeapYear, 3, 30, 59, 295, false),
        new(LeapYear, 4, 29, 89, 266, false),
        new(LeapYear, 5, 30, 118, 236, false),
        new(LeapYear, 6, 29, 148, 207, false),
        new(LeapYear, 7, 30, 177, 177, false),
        new(LeapYear, 8, 29, 207, 148, false),
        new(LeapYear, 9, 30, 236, 118, false),
        new(LeapYear, 10, 29, 266, 89, false),
        new(LeapYear, 11, 30, 295, 59, false),
        new(LeapYear, 12, 29, 325, 30, false),
        new(LeapYear, 13, 30, 354, 0, true),
    };

    public override DataGroup<YearInfo> YearInfoData { get; } = new()
    {
        new(CommonYear, 12, 354, false),
        new(LeapYear, 13, 384, true),
    };

    internal static IEnumerable<DaysSinceRataDieInfo> DaysSinceRataDieInfos
    {
        get
        {
            // Year -4 yield return new(leap).
            yield return new(-1829, -4, 1, 1);
            yield return new(-1476, -4, 12, 29);
            yield return new(-1475, -4, 13, 1);
            yield return new(-1446, -4, 13, 30);

            // Year -3.
            yield return new(-1445, -3, 1, 1);
            yield return new(-1092, -3, 12, 29);

            // Year -2.
            yield return new(-1091, -2, 1, 1);
            yield return new(-738, -2, 12, 29);

            // Year -1.
            yield return new(-737, -1, 1, 1);
            yield return new(-384, -1, 12, 29);

            // Year 0 yield return new(leap).
            yield return new(-383, 0, 1, 1);
            yield return new(-30, 0, 12, 29);
            yield return new(-29, 0, 13, 1);
            yield return new(0, 0, 13, 30);

            // Year 1.
            yield return new(1, 1, 1, 1);
            yield return new(30, 1, 1, 30);
            yield return new(31, 1, 2, 1);
            yield return new(59, 1, 2, 29);
            yield return new(60, 1, 3, 1);
            yield return new(89, 1, 3, 30);
            yield return new(90, 1, 4, 1);
            yield return new(118, 1, 4, 29);
            yield return new(119, 1, 5, 1);
            yield return new(148, 1, 5, 30);
            yield return new(149, 1, 6, 1);
            yield return new(177, 1, 6, 29);
            yield return new(178, 1, 7, 1);
            yield return new(207, 1, 7, 30);
            yield return new(208, 1, 8, 1);
            yield return new(236, 1, 8, 29);
            yield return new(237, 1, 9, 1);
            yield return new(266, 1, 9, 30);
            yield return new(267, 1, 10, 1);
            yield return new(295, 1, 10, 29);
            yield return new(296, 1, 11, 1);
            yield return new(325, 1, 11, 30);
            yield return new(326, 1, 12, 1);
            yield return new(354, 1, 12, 29);

            // Year 2.
            yield return new(355, 2, 1, 1);
            yield return new(708, 2, 12, 29);

            // Year 3.
            yield return new(709, 3, 1, 1);
            yield return new(1062, 3, 12, 29);

            // Year 4 yield return new(leap).
            yield return new(1063, 4, 1, 1);
            yield return new(1416, 4, 12, 29);
            yield return new(1417, 4, 13, 1);
            yield return new(1446, 4, 13, 30);

            // Year 5.
            yield return new(1447, 5, 1, 1);
            yield return new(1800, 5, 12, 29);
        }
    }
}

public partial class LunisolarDataSet // Start and end of year
{
    public override DataGroup<Yemoda> EndOfYearPartsData { get; } = new()
    {
        new(CommonYear, 12, 29),
        new(LeapYear, 13, 30),
    };

    // TODO(data): à compléter.
    public override DataGroup<YearMonthsSinceEpoch> StartOfYearMonthsSinceEpochData { get; } = new()
    {
        new(-1, -25),
        new(0, -13), // leap year
        new(1, 0),
        new(2, 12),
        new(3, 24),
        new(4, 36), // leap year
        new(5, 49),
    };

    public override DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-9, -3630),
        new(-8, -3276), // leap year
        new(-7, -2892),
        new(-6, -2538),
        new(-5, -2184),
        new(-4, -1830), // leap year
        new(-3, -1446),
        new(-2, -1092),
        new(-1, -738),
        new(0, -384), // leap year
        new(1, 0),
        new(2, 354),
        new(3, 708),
        new(4, 1062), // leap year
        new(5, 1446),
        new(6, 1800),
        new(7, 2154),
        new(8, 2508), // leap year
        new(9, 2892),
    };
}

public partial class LunisolarDataSet // Invalid date parts
{
    public override TheoryData<int, int> InvalidMonthFieldData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 13 },
        { LeapYear, 0 },
        { LeapYear, 14 },
    };

    public override TheoryData<int, int> InvalidDayOfYearFieldData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 355 },
        { LeapYear, 0 },
        { LeapYear, 385 },
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
        { LeapYear, 12, 30 },
        { LeapYear, 13, 0 },
        { LeapYear, 13, 31 },
    };
}

public partial class LunisolarDataSet // Math data
{
    //
    // Data for Next() and Previous()
    //

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
        new(new(LeapYear, 12, 29), new(LeapYear, 13, 1)),
        new(new(LeapYear, 13, 30), new(LeapYear + 1, 1, 1)),
    }.ConcatT(base.ConsecutiveDaysData);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 354), new(CommonYear + 1, 1)),
        new(new(LeapYear, 354), new(LeapYear, 355)),
        new(new(LeapYear, 384), new(LeapYear + 1, 1)),
    }.ConcatT(base.ConsecutiveDaysOrdinalData);

    public override DataGroup<YemoPair> ConsecutiveMonthsData => new DataGroup<YemoPair>()
    {
        new(new(CommonYear, 12), new(CommonYear + 1, 1)),
        new(new(LeapYear, 12), new(LeapYear, 13)),
        new(new(LeapYear, 13), new(LeapYear + 1, 1)),
    }.ConcatT(base.ConsecutiveMonthsData);

    //
    // Data for the additions
    //

    public override DataGroup<YemodaPairAnd<int>> AddDaysData => new DataGroup<YemodaPairAnd<int>>()
    {
        //
        // Branch AddDaysViaDayOfYear()
        //
        // MinDaysInMonth = 29 < |days| <= MinDaysInYear = 353

        // Change of month.
        new(new(3, 4, 1), new(3, 5, 1), 29),

        // December, common year.
        new(new(CommonYear, 12, 29), new(CommonYear, 1, 1), -353),
        new(new(CommonYear, 12, 29), new(CommonYear + 1, 12, 28), 353),
        // December, leap year.
        new(new(LeapYear, 12, 29), new(LeapYear, 1, 1), -353),
        new(new(LeapYear, 12, 29), new(LeapYear + 1, 11, 28), 353),
        // December + 1, leap year.
        new(new(LeapYear, 13, 30), new(LeapYear, 2, 1), -353),
        new(new(LeapYear, 13, 30), new(LeapYear + 1, 12, 28), 353),
    }.ConcatT(base.AddDaysData);

    public override DataGroup<YemoPairAnd<int>> AddYearsMonthData => new DataGroup<YemoPairAnd<int>>()
    {
        new(new(CommonYear, 1), new(CommonYear, 2), 1),
        new(new(CommonYear, 1), new(CommonYear, 12), 11),
        new(new(CommonYear, 1), new(CommonYear + 1, 1), 12),

        new(new(LeapYear, 1), new(LeapYear, 2), 1),
        new(new(LeapYear, 1), new(LeapYear, 12), 11),
        new(new(LeapYear, 1), new(LeapYear, 13), 12),
        new(new(LeapYear, 1), new(LeapYear + 1, 1), 13),

        new(new(LeapYear, 13), new(LeapYear + 1, 12), 12),
        new(new(LeapYear, 13), new(LeapYear + 2, 1), 13),
    }.ConcatT(base.AddYearsMonthData);
}
