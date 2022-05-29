// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for <see cref="PositivistSchema"/>.
/// </summary>
public sealed partial class PositivistDataSet : SchemaDataSet, ISingleton<PositivistDataSet>
{
    public const int CommonYear = 3;
    public const int LeapYear = 4;

    private PositivistDataSet() : base(new PositivistSchema(), CommonYear, LeapYear) { }

    public static PositivistDataSet Instance => Singleton.Instance;

    private static class Singleton
    {
        internal static readonly PositivistDataSet Instance = new();
        static Singleton() { }
    }
}

public partial class PositivistDataSet // Infos
{
    public override DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; } =
        DataGroup.CreateDaysSinceEpochInfoData(DaysSinceZeroInfos, CalendarEpoch.Positivist);

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
        new(CommonYear, 13, 29, 365, false, true),
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
        new(LeapYear, 13, 29, 365, false, true),
        new(LeapYear, 13, 30, 366, true, true),
    };

    public override DataGroup<MonthInfo> MonthInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 28, 0, 337, false),
        new(CommonYear, 2, 28, 28, 309, false),
        new(CommonYear, 3, 28, 56, 281, false),
        new(CommonYear, 4, 28, 84, 253, false),
        new(CommonYear, 5, 28, 112, 225, false),
        new(CommonYear, 6, 28, 140, 197, false),
        new(CommonYear, 7, 28, 168, 169, false),
        new(CommonYear, 8, 28, 196, 141, false),
        new(CommonYear, 9, 28, 224, 113, false),
        new(CommonYear, 10, 28, 252, 85, false),
        new(CommonYear, 11, 28, 280, 57, false),
        new(CommonYear, 12, 28, 308, 29, false),
        new(CommonYear, 13, 29, 336, 0, false),
        // Leap year.
        new(LeapYear, 1, 28, 0, 338, false),
        new(LeapYear, 2, 28, 28, 310, false),
        new(LeapYear, 3, 28, 56, 282, false),
        new(LeapYear, 4, 28, 84, 254, false),
        new(LeapYear, 5, 28, 112, 226, false),
        new(LeapYear, 6, 28, 140, 198, false),
        new(LeapYear, 7, 28, 168, 170, false),
        new(LeapYear, 8, 28, 196, 142, false),
        new(LeapYear, 9, 28, 224, 114, false),
        new(LeapYear, 10, 28, 252, 86, false),
        new(LeapYear, 11, 28, 280, 58, false),
        new(LeapYear, 12, 28, 308, 30, false),
        new(LeapYear, 13, 30, 336, 0, false),
    };

    public override DataGroup<YearInfo> YearInfoData { get; } = new()
    {
        // Leap years.
        new(LeapYear, 13, 366, true),
        // Centennial years.
        new(400, 13, 366, true),

        // Common years.
        new(CommonYear, 13, 365, false),
        // Centennial years.
        new(100, 13, 365, false),
    };

    internal static IEnumerable<DaysSinceZeroInfo> DaysSinceZeroInfos
    {
        get
        {
            yield return new(653_052, 0, 13, 29);
            yield return new(653_053, 0, 13, 30);
            yield return new(653_054, 1, 1, 1);
            yield return new(653_055, 1, 1, 2);
            yield return new(653_418, 1, 13, 29);
            yield return new(653_419, 2, 1, 1);
        }
    }
}

public partial class PositivistDataSet // Start and end of year
{
    public override DataGroup<Yemoda> EndOfYearPartsData { get; } = new()
    {
        new(CommonYear, 13, 29),
        new(LeapYear, 13, 30),
    };

    public override DataGroup<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
    {
        new(-399, -PositivistSchema.DaysPer400YearCycle),
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
        new(401, PositivistSchema.DaysPer400YearCycle)
    };
}

public partial class PositivistDataSet // Invalid date parts
{
    public override TheoryData<int, int> InvalidMonthFieldData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 14 },
        { LeapYear, 0 },
        { LeapYear, 14 },
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
        { CommonYear, 13, 30 },
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
        { LeapYear, 13, 31 },
    };
}

public partial class PositivistDataSet // Math data
{
    public override DataGroup<YemodaPair> ConsecutiveDaysData { get; } = new DataGroup<YemodaPair>()
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
        new(new(CommonYear, 13, 29), new(CommonYear + 1, 1, 1)),
        new(new(LeapYear, 13, 29), new(LeapYear, 13, 30)),
        new(new(LeapYear, 13, 30), new(LeapYear + 1, 1, 1)),
    }.ConcatT(ConsecutiveDaysSamples);

    public override DataGroup<YedoyPair> ConsecutiveDaysOrdinalData { get; } = new DataGroup<YedoyPair>()
    {
        // End of year.
        new(new(CommonYear, 365), new(CommonYear + 1, 1)),
        new(new(LeapYear, 365), new(LeapYear, 366)),
        new(new(LeapYear, 366), new(LeapYear + 1, 1)),
    }.ConcatT(ConsecutiveDaysOrdinalSamples);
}
