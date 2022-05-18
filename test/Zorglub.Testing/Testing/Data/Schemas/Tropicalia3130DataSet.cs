// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for <see cref="Tropicalia3130Schema"/>.
/// </summary>
public sealed partial class Tropicalia3130DataSet : TropicalistaDataSet, ISingleton<Tropicalia3130DataSet>
{
    private Tropicalia3130DataSet() : base(new Tropicalia3130Schema()) { }

    public static Tropicalia3130DataSet Instance { get; } = new();
}

public partial class Tropicalia3130DataSet // Infos
{
    public override DataGroup<DaysSinceEpochInfo> DaysSinceEpochInfoData { get; } =
        DataGroup.Create(DaysSinceEpochInfos);

    public override DataGroup<DateInfo> DateInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 31, 31, false, false),
        new(CommonYear, 2, 30, 61, false, false),
        new(CommonYear, 3, 31, 92, false, false),
        new(CommonYear, 4, 30, 122, false, false),
        new(CommonYear, 5, 31, 153, false, false),
        new(CommonYear, 6, 30, 183, false, false),
        new(CommonYear, 7, 31, 214, false, false),
        new(CommonYear, 8, 30, 244, false, false),
        new(CommonYear, 9, 31, 275, false, false),
        new(CommonYear, 10, 30, 305, false, false),
        new(CommonYear, 11, 31, 336, false, false),
        new(CommonYear, 12, 29, 365, false, false),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 31, 31, false, false),
        new(LeapYear, 2, 30, 61, false, false),
        new(LeapYear, 3, 31, 92, false, false),
        new(LeapYear, 4, 30, 122, false, false),
        new(LeapYear, 5, 31, 153, false, false),
        new(LeapYear, 6, 30, 183, false, false),
        new(LeapYear, 7, 31, 214, false, false),
        new(LeapYear, 8, 30, 244, false, false),
        new(LeapYear, 9, 31, 275, false, false),
        new(LeapYear, 10, 30, 305, false, false),
        new(LeapYear, 11, 31, 336, false, false),
        new(LeapYear, 12, 29, 365, false, false),
        new(LeapYear, 12, 30, 366, true, false),
    };

    public override DataGroup<MonthInfo> MonthInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 31, 0, false),
        new(CommonYear, 2, 30, 31, false),
        new(CommonYear, 3, 31, 61, false),
        new(CommonYear, 4, 30, 92, false),
        new(CommonYear, 5, 31, 122, false),
        new(CommonYear, 6, 30, 153, false),
        new(CommonYear, 7, 31, 183, false),
        new(CommonYear, 8, 30, 214, false),
        new(CommonYear, 9, 31, 244, false),
        new(CommonYear, 10, 30, 275, false),
        new(CommonYear, 11, 31, 305, false),
        new(CommonYear, 12, 29, 336, false),
        // Leap year.
        new(LeapYear, 1, 31, 0, false),
        new(LeapYear, 2, 30, 31, false),
        new(LeapYear, 3, 31, 61, false),
        new(LeapYear, 4, 30, 92, false),
        new(LeapYear, 5, 31, 122, false),
        new(LeapYear, 6, 30, 153, false),
        new(LeapYear, 7, 31, 183, false),
        new(LeapYear, 8, 30, 214, false),
        new(LeapYear, 9, 31, 244, false),
        new(LeapYear, 10, 30, 275, false),
        new(LeapYear, 11, 31, 305, false),
        new(LeapYear, 12, 30, 336, false),
    };

    internal static IEnumerable<DaysSinceEpochInfo> DaysSinceEpochInfos
    {
        get
        {
            yield return new(-2, 0, 12, 28);
            yield return new(-1, 0, 12, 29);
            yield return new(0, 1, 1, 1); // Epoch
            yield return new(1, 1, 1, 2);
            yield return new(364, 1, 12, 29);
            yield return new(365, 2, 1, 1);

            // 128-year cycle.
            yield return new(46_750, 128, 12, 29);
            yield return new(93_501, 256, 12, 29);
            yield return new(140_252, 384, 12, 29);
            yield return new(187_003, 512, 12, 29);
            yield return new(233_754, 640, 12, 29);
            yield return new(280_505, 768, 12, 29);
            yield return new(327_256, 896, 12, 29);
            yield return new(374_007, 1024, 12, 29);
            yield return new(420_758, 1152, 12, 29);
            yield return new(467_509, 1280, 12, 29);
        }
    }
}

public partial class Tropicalia3130DataSet // Start and end of year
{
    public override DataGroup<Yemoda> EndOfYearPartsData { get; } = new()
    {
        new(CommonYear, 12, 29),
        new(LeapYear, 12, 30),
    };
}

public partial class Tropicalia3130DataSet // Invalid date parts
{
    public override TheoryData<int, int, int> InvalidDayFieldData { get; } = new()
    {
        // Common year.
        { CommonYear, 1, 0 },
        { CommonYear, 1, 32 },
        { CommonYear, 2, 0 },
        { CommonYear, 2, 31 },
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
        { CommonYear, 8, 31 },
        { CommonYear, 9, 0 },
        { CommonYear, 9, 32 },
        { CommonYear, 10, 0 },
        { CommonYear, 10, 31 },
        { CommonYear, 11, 0 },
        { CommonYear, 11, 32 },
        { CommonYear, 12, 0 },
        { CommonYear, 12, 30 },
        // Leap year.
        { LeapYear, 1, 0 },
        { LeapYear, 1, 32 },
        { LeapYear, 2, 0 },
        { LeapYear, 2, 31 },
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
        { LeapYear, 8, 31 },
        { LeapYear, 9, 0 },
        { LeapYear, 9, 32 },
        { LeapYear, 10, 0 },
        { LeapYear, 10, 31 },
        { LeapYear, 11, 0 },
        { LeapYear, 11, 32 },
        { LeapYear, 12, 0 },
        { LeapYear, 12, 31 },
    };
}
