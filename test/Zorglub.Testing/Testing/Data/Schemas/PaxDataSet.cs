// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for <see cref="PaxSchema"/>.
/// </summary>
public sealed partial class PaxDataSet : SchemaDataSet, ISingleton<PaxDataSet>
{
    private const int CommonYear = 3;
    private const int LeapYear = 6;

    private PaxDataSet() : base(new PaxSchema(), CommonYear, LeapYear) { }

    public static PaxDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly PaxDataSet Instance = new();
        static Singleton() { }
    }
}

public partial class PaxDataSet // Infos
{
    public override DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; } =
        DataGroup.Create(DaysSinceEpochInfos);

    public override DataGroup<DateInfo> DateInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 28, 28, false, false),
        new(CommonYear, 2, 28, 56, false, false),
        new(CommonYear, 3, 28, 84, false, false),
        new(CommonYear, 4, 28, 112, false, false),
        new(CommonYear, 5, 28, 140, false, false),
        new(CommonYear, 6, 28, 168, false, false),
        new(CommonYear, 7, 28, 196, false, false),
        new(CommonYear, 8, 28, 224, false, false),
        new(CommonYear, 9, 28, 252, false, false),
        new(CommonYear, 10, 28, 280, false, false),
        new(CommonYear, 11, 28, 308, false, false),
        new(CommonYear, 12, 28, 336, false, false),
        new(CommonYear, 13, 28, 364, false, false),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 28, 28, false, false),
        new(LeapYear, 2, 28, 56, false, false),
        new(LeapYear, 3, 28, 84, false, false),
        new(LeapYear, 4, 28, 112, false, false),
        new(LeapYear, 5, 28, 140, false, false),
        new(LeapYear, 6, 28, 168, false, false),
        new(LeapYear, 7, 28, 196, false, false),
        new(LeapYear, 8, 28, 224, false, false),
        new(LeapYear, 9, 28, 252, false, false),
        new(LeapYear, 10, 28, 280, false, false),
        new(LeapYear, 11, 28, 308, false, false),
        new(LeapYear, 12, 28, 336, false, false),
        new(LeapYear, 13, 1, 337, false, false),
        new(LeapYear, 13, 2, 338, false, false),
        new(LeapYear, 13, 3, 339, false, false),
        new(LeapYear, 13, 4, 340, false, false),
        new(LeapYear, 13, 5, 341, false, false),
        new(LeapYear, 13, 6, 342, false, false),
        new(LeapYear, 13, 7, 343, false, false),
        new(LeapYear, 14, 28, 371, false, false),
    };

    public override DataGroup<MonthInfo> MonthInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 28, 0, 336, false),
        new(CommonYear, 2, 28, 28, 308, false),
        new(CommonYear, 3, 28, 56, 280, false),
        new(CommonYear, 4, 28, 84, 252, false),
        new(CommonYear, 5, 28, 112, 224, false),
        new(CommonYear, 6, 28, 140, 196, false),
        new(CommonYear, 7, 28, 168, 168, false),
        new(CommonYear, 8, 28, 196, 140, false),
        new(CommonYear, 9, 28, 224, 112, false),
        new(CommonYear, 10, 28, 252, 84, false),
        new(CommonYear, 11, 28, 280, 56, false),
        new(CommonYear, 12, 28, 308, 28, false),
        new(CommonYear, 13, 28, 336, 0, false),
        // Leap year.
        new(LeapYear, 1, 28, 0, 343, false),
        new(LeapYear, 2, 28, 28, 315, false),
        new(LeapYear, 3, 28, 56, 287, false),
        new(LeapYear, 4, 28, 84, 259, false),
        new(LeapYear, 5, 28, 112, 231, false),
        new(LeapYear, 6, 28, 140, 203, false),
        new(LeapYear, 7, 28, 168, 175, false),
        new(LeapYear, 8, 28, 196, 147, false),
        new(LeapYear, 9, 28, 224, 119, false),
        new(LeapYear, 10, 28, 252, 91, false),
        new(LeapYear, 11, 28, 280, 63, false),
        new(LeapYear, 12, 28, 308, 35, false),
        new(LeapYear, 13, 7, 336, 28, false),
        new(LeapYear, 14, 28, 343, 0, false),
    };

    public override DataGroup<YearInfo> YearInfoData { get; } = new()
    {
        new(1930, 14, 371, true),
        new(1936, 14, 371, true),
        new(1942, 14, 371, true),
        new(1948, 14, 371, true),
        new(1954, 14, 371, true),
        new(1960, 14, 371, true),
        new(1966, 14, 371, true),
        new(1972, 14, 371, true),
        new(1978, 14, 371, true),
        new(1984, 14, 371, true),
        new(1990, 14, 371, true),
        new(1996, 14, 371, true),
        new(1999, 14, 371, true), // ends with 99
        new(2006, 14, 371, true),
        new(2012, 14, 371, true),
        new(2018, 14, 371, true),
        new(2024, 14, 371, true),

        // Centuries.
        new(2000, 13, 364, false), // century multiple of 400
        new(2100, 14, 371, true),
        new(2200, 14, 371, true),
        new(2300, 14, 371, true),
    };

    internal static IEnumerable<DaysSinceEpochInfo> DaysSinceEpochInfos
    {
        get
        {
            yield return new(0, 1, 1, 1);
            yield return new(1, 1, 1, 2);
            yield return new(363, 1, 13, 28);
            yield return new(364, 2, 1, 1);
        }
    }
}

public partial class PaxDataSet // Start and end of year
{
    public override DataGroup<Yemoda> EndOfYearPartsData { get; } = new()
    {
        new(CommonYear, 13, 28),
        new(LeapYear, 14, 28),
    };

    public override DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(1, 0),
        new(2, 364),
        new(3, 728),
        new(4, 1092),
        new(5, 1456),
        new(6, 1820), // leap year
        new(7, 2191),
        new(8, 2555),
        new(9, 2919),
        new(10, 3283),
        new(11, 3647),
        new(12, 4011), // leap year
        new(13, 4382),

        new(18, 2 * 7 + 17 * 364), // leap year
        new(19, 3 * 7 + 18 * 364),

        new(84, 13 * 7 + 83 * 364), // leap year
        new(85, 14 * 7 + 84 * 364),

        new(90, 14 * 7 + 89 * 364), // leap year
        new(91, 15 * 7 + 90 * 364),
        new(92, 15 * 7 + 91 * 364),
        new(93, 15 * 7 + 92 * 364),
        new(94, 15 * 7 + 93 * 364),
        new(95, 15 * 7 + 94 * 364),
        new(96, 15 * 7 + 95 * 364), // leap year
        new(97, 16 * 7 + 96 * 364),
        new(98, 16 * 7 + 97 * 364),
        new(99, 16 * 7 + 98 * 364), // leap year

        new(100, 17 * 7 + 99 * 364), // leap year
        new(101, 18 * 7 + 100 * 364),

        new(401, PaxSchema.DaysPer400YearCycle),
    };
}

public partial class PaxDataSet // Invalid date parts
{
    public override TheoryData<int, int> InvalidMonthFieldData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 14 },
        { LeapYear, 0 },
        { LeapYear, 15 },
    };

    public override TheoryData<int, int> InvalidDayOfYearFieldData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 365 },
        { LeapYear, 0 },
        { LeapYear, 372 },
    };

    public override TheoryData<int, int, int> InvalidDayFieldData { get; } = new()
    {
        // Common year.
        { CommonYear, 1, 0 },
        { CommonYear, 1, 29 },
        { CommonYear, 2, 0 },
        { CommonYear, 2, 29 },
        { CommonYear, 3, 0 },
        { CommonYear, 3, 29 },
        { CommonYear, 4, 0 },
        { CommonYear, 4, 29 },
        { CommonYear, 5, 0 },
        { CommonYear, 5, 29 },
        { CommonYear, 6, 0 },
        { CommonYear, 6, 29 },
        { CommonYear, 7, 0 },
        { CommonYear, 7, 29 },
        { CommonYear, 8, 0 },
        { CommonYear, 8, 29 },
        { CommonYear, 9, 0 },
        { CommonYear, 9, 29 },
        { CommonYear, 10, 0 },
        { CommonYear, 10, 29 },
        { CommonYear, 11, 0 },
        { CommonYear, 11, 29 },
        { CommonYear, 12, 0 },
        { CommonYear, 12, 29 },
        { CommonYear, 13, 0 },
        { CommonYear, 13, 29 },
        // Leap year.
        { LeapYear, 1, 0 },
        { LeapYear, 1, 29 },
        { LeapYear, 2, 0 },
        { LeapYear, 2, 29 },
        { LeapYear, 3, 0 },
        { LeapYear, 3, 29 },
        { LeapYear, 4, 0 },
        { LeapYear, 4, 29 },
        { LeapYear, 5, 0 },
        { LeapYear, 5, 29 },
        { LeapYear, 6, 0 },
        { LeapYear, 6, 29 },
        { LeapYear, 7, 0 },
        { LeapYear, 7, 29 },
        { LeapYear, 8, 0 },
        { LeapYear, 8, 29 },
        { LeapYear, 9, 0 },
        { LeapYear, 9, 29 },
        { LeapYear, 10, 0 },
        { LeapYear, 10, 29 },
        { LeapYear, 11, 0 },
        { LeapYear, 11, 29 },
        { LeapYear, 12, 0 },
        { LeapYear, 12, 29 },
        { LeapYear, 13, 0 },
        { LeapYear, 13, 8 },
        { LeapYear, 14, 0 },
        { LeapYear, 14, 29 },
    };
}

public partial class PaxDataSet // Math data
{
    public override DataGroup<YemodaPair> ConsecutiveDaysData => new DataGroup<YemodaPair>()
    {
        // End of month.
        new(new(CommonYear, 1, 28), new(CommonYear, 2, 1)),
        new(new(CommonYear, 2, 28), new(CommonYear, 3, 1)),
        new(new(CommonYear, 3, 28), new(CommonYear, 4, 1)),
        new(new(CommonYear, 4, 28), new(CommonYear, 5, 1)),
        new(new(CommonYear, 5, 28), new(CommonYear, 6, 1)),
        new(new(CommonYear, 6, 28), new(CommonYear, 7, 1)),
        new(new(CommonYear, 7, 28), new(CommonYear, 8, 1)),
        new(new(CommonYear, 8, 28), new(CommonYear, 9, 1)),
        new(new(CommonYear, 9, 28), new(CommonYear, 10, 1)),
        new(new(CommonYear, 10, 28), new(CommonYear, 11, 1)),
        new(new(CommonYear, 11, 28), new(CommonYear, 12, 1)),
        new(new(CommonYear, 12, 28), new(CommonYear, 13, 1)),
        new(new(CommonYear, 13, 28), new(CommonYear + 1, 1, 1)),
        new(new(LeapYear, 13, 7), new(LeapYear, 14, 1)),
        new(new(LeapYear, 14, 28), new(LeapYear + 1, 1, 1)),
    }.ConcatT(base.ConsecutiveDaysData);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData => new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 364), new(CommonYear + 1, 1)),
        new(new(LeapYear, 364), new(LeapYear, 365)),
        new(new(LeapYear, 365), new(LeapYear, 366)),
        new(new(LeapYear, 366), new(LeapYear, 367)),
        new(new(LeapYear, 367), new(LeapYear, 368)),
        new(new(LeapYear, 368), new(LeapYear, 369)),
        new(new(LeapYear, 369), new(LeapYear, 370)),
        new(new(LeapYear, 370), new(LeapYear, 371)),
        new(new(LeapYear, 371), new(LeapYear + 1, 1)),
    }.ConcatT(base.ConsecutiveDaysOrdinalData);
}
