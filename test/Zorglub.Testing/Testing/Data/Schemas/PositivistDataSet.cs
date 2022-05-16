// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

using Zorglub.Time.Hemerology;

/// <summary>
/// Provides test data for <see cref="PositivistSchema"/>.
/// </summary>
public sealed partial class PositivistDataSet : CalendricalDataSet, ISingleton<PositivistDataSet>
{
    public const int CommonYear = 3;
    public const int LeapYear = 4;

    private PositivistDataSet() : base(new PositivistSchema(), CommonYear, LeapYear) { }

    public static PositivistDataSet Instance { get; } = new();
}

public partial class PositivistDataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??=
            DaysSinceEpochInfoDataGroup.Create(DaysSinceZeroInfos, CalendarEpoch.Positivist);

    public override TheoryData<DateInfo> DateInfoData => new()
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

    public override TheoryData<MonthInfo> MonthInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 28, 0, false),
        new(CommonYear, 2, 28, 28, false),
        new(CommonYear, 3, 28, 56, false),
        new(CommonYear, 4, 28, 84, false),
        new(CommonYear, 5, 28, 112, false),
        new(CommonYear, 6, 28, 140, false),
        new(CommonYear, 7, 28, 168, false),
        new(CommonYear, 8, 28, 196, false),
        new(CommonYear, 9, 28, 224, false),
        new(CommonYear, 10, 28, 252, false),
        new(CommonYear, 11, 28, 280, false),
        new(CommonYear, 12, 28, 308, false),
        new(CommonYear, 13, 29, 336, false),
        // Leap year.
        new(LeapYear, 1, 28, 0, false),
        new(LeapYear, 2, 28, 28, false),
        new(LeapYear, 3, 28, 56, false),
        new(LeapYear, 4, 28, 84, false),
        new(LeapYear, 5, 28, 112, false),
        new(LeapYear, 6, 28, 140, false),
        new(LeapYear, 7, 28, 168, false),
        new(LeapYear, 8, 28, 196, false),
        new(LeapYear, 9, 28, 224, false),
        new(LeapYear, 10, 28, 252, false),
        new(LeapYear, 11, 28, 280, false),
        new(LeapYear, 12, 28, 308, false),
        new(LeapYear, 13, 30, 336, false),
    };

    public override TheoryData<YearInfo> YearInfoData => new()
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
    public override TheoryData<Yemoda> EndOfYearPartsData => new()
    {
        new(CommonYear, 13, 29),
        new(LeapYear, 13, 30),
    };

    public override TheoryData<YearDaysSinceEpoch> StartOfYearDaysSinceEpochData { get; } = new()
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
    public override TheoryData<int, int> InvalidMonthFieldData => new()
    {
        { CommonYear, 0 },
        { CommonYear, 14 },
        { LeapYear, 0 },
        { LeapYear, 14 },
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
