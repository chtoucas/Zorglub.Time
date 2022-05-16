// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

/// <summary>
/// Provides test data for <see cref="Tropicalia3031Schema"/>.
/// </summary>
public sealed partial class Tropicalia3031DataSet : TropicalistaDataSet, ISingleton<Tropicalia3031DataSet>
{
    private Tropicalia3031DataSet() : base(new Tropicalia3031Schema()) { }

    public static Tropicalia3031DataSet Instance { get; } = new();
}

public partial class Tropicalia3031DataSet // Infos
{
    private TheoryData<DaysSinceEpochInfo>? _daysSinceEpochInfoData;
    public override TheoryData<DaysSinceEpochInfo> DaysSinceEpochInfoData =>
        _daysSinceEpochInfoData ??= DataGroup.Create(DaysSinceEpochInfos);

    public override TheoryData<DateInfo> DateInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 1, 1, false, false),
        new(CommonYear, 1, 30, 30, false, false),
        new(CommonYear, 2, 31, 61, false, false),
        new(CommonYear, 3, 30, 91, false, false),
        new(CommonYear, 4, 31, 122, false, false),
        new(CommonYear, 5, 30, 152, false, false),
        new(CommonYear, 6, 31, 183, false, false),
        new(CommonYear, 7, 30, 213, false, false),
        new(CommonYear, 8, 31, 244, false, false),
        new(CommonYear, 9, 30, 274, false, false),
        new(CommonYear, 10, 31, 305, false, false),
        new(CommonYear, 11, 30, 335, false, false),
        new(CommonYear, 12, 30, 365, false, false),
        // Leap year.
        new(LeapYear, 1, 1, 1, false, false),
        new(LeapYear, 1, 30, 30, false, false),
        new(LeapYear, 2, 31, 61, false, false),
        new(LeapYear, 3, 30, 91, false, false),
        new(LeapYear, 4, 31, 122, false, false),
        new(LeapYear, 5, 30, 152, false, false),
        new(LeapYear, 6, 31, 183, false, false),
        new(LeapYear, 7, 30, 213, false, false),
        new(LeapYear, 8, 31, 244, false, false),
        new(LeapYear, 9, 30, 274, false, false),
        new(LeapYear, 10, 31, 305, false, false),
        new(LeapYear, 11, 30, 335, false, false),
        new(LeapYear, 12, 30, 365, false, false),
        new(LeapYear, 12, 31, 366, true, false),
    };

    public override TheoryData<MonthInfo> MonthInfoData => new()
    {
        // Common year.
        new(CommonYear, 1, 30, 0, false),
        new(CommonYear, 2, 31, 30, false),
        new(CommonYear, 3, 30, 61, false),
        new(CommonYear, 4, 31, 91, false),
        new(CommonYear, 5, 30, 122, false),
        new(CommonYear, 6, 31, 152, false),
        new(CommonYear, 7, 30, 183, false),
        new(CommonYear, 8, 31, 213, false),
        new(CommonYear, 9, 30, 244, false),
        new(CommonYear, 10, 31, 274, false),
        new(CommonYear, 11, 30, 305, false),
        new(CommonYear, 12, 30, 335, false),
        // Leap year.
        new(LeapYear, 1, 30, 0, false),
        new(LeapYear, 2, 31, 30, false),
        new(LeapYear, 3, 30, 61, false),
        new(LeapYear, 4, 31, 91, false),
        new(LeapYear, 5, 30, 122, false),
        new(LeapYear, 6, 31, 152, false),
        new(LeapYear, 7, 30, 183, false),
        new(LeapYear, 8, 31, 213, false),
        new(LeapYear, 9, 30, 244, false),
        new(LeapYear, 10, 31, 274, false),
        new(LeapYear, 11, 30, 305, false),
        new(LeapYear, 12, 31, 335, false),
    };

    internal static IEnumerable<DaysSinceEpochInfo> DaysSinceEpochInfos
    {
        get
        {
            yield return new(-2, 0, 12, 29);
            yield return new(-1, 0, 12, 30);
            yield return new(0, 1, 1, 1); // Epoch
            yield return new(1, 1, 1, 2);
            yield return new(364, 1, 12, 30);
            yield return new(365, 2, 1, 1);

            // 128-year cycle.
            yield return new(46_750, 128, 12, 30);
            yield return new(93_501, 256, 12, 30);
            yield return new(140_252, 384, 12, 30);
            yield return new(187_003, 512, 12, 30);
            yield return new(233_754, 640, 12, 30);
            yield return new(280_505, 768, 12, 30);
            yield return new(327_256, 896, 12, 30);
            yield return new(374_007, 1024, 12, 30);
            yield return new(420_758, 1152, 12, 30);
            yield return new(467_509, 1280, 12, 30);
        }
    }
}

public partial class Tropicalia3031DataSet // Start and end of year
{
    public override TheoryData<Yemoda> EndOfYearPartsData => new()
    {
        new(CommonYear, 12, 30),
        new(LeapYear, 12, 31),
    };
}

public partial class Tropicalia3031DataSet // Invalid date parts
{
    public override TheoryData<int, int, int> InvalidDayFieldData => new()
    {
        // Common year.
        { CommonYear, 1, 0 },
        { CommonYear, 1, 31 },
        { CommonYear, 2, 0 },
        { CommonYear, 2, 32 },
        { CommonYear, 3, 0 },
        { CommonYear, 3, 31 },
        { CommonYear, 4, 0 },
        { CommonYear, 4, 32 },
        { CommonYear, 5, 0 },
        { CommonYear, 5, 31 },
        { CommonYear, 6, 0 },
        { CommonYear, 6, 32 },
        { CommonYear, 7, 0 },
        { CommonYear, 7, 31 },
        { CommonYear, 8, 0 },
        { CommonYear, 8, 32 },
        { CommonYear, 9, 0 },
        { CommonYear, 9, 31 },
        { CommonYear, 10, 0 },
        { CommonYear, 10, 32 },
        { CommonYear, 11, 0 },
        { CommonYear, 11, 31 },
        { CommonYear, 12, 0 },
        { CommonYear, 12, 31 },
        // Leap year.
        { LeapYear, 1, 0 },
        { LeapYear, 1, 31 },
        { LeapYear, 2, 0 },
        { LeapYear, 2, 32 },
        { LeapYear, 3, 0 },
        { LeapYear, 3, 31 },
        { LeapYear, 4, 0 },
        { LeapYear, 4, 32 },
        { LeapYear, 5, 0 },
        { LeapYear, 5, 31 },
        { LeapYear, 6, 0 },
        { LeapYear, 6, 32 },
        { LeapYear, 7, 0 },
        { LeapYear, 7, 31 },
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
