// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class PaxDataSet // Supplementary data
{
    /// <summary>DaysSinceEpoch, year, week of the year, day of the week.</summary>
    public static TheoryData<int, int, int, DayOfWeek> MoreDaySinceEpochInfoData { get; } =
        MapToTheoryData(MoreDaySinceEpochInfos);

    /// <summary>Year, weeks in year.</summary>
    public static TheoryData<int, int> MoreYearInfoData { get; } = new()
    {
        { CommonYear, 52 },
        { LeapYear, 53 },
    };

    /// <summary>Year, month of the year (Yemo), isPaxMonth, isLastMonthOfYear.</summary>
    public static DataGroup<YemoAnd<bool, bool>> MoreMonthInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, false, false),
        new(CommonYear, 2, false, false),
        new(CommonYear, 3, false, false),
        new(CommonYear, 4, false, false),
        new(CommonYear, 5, false, false),
        new(CommonYear, 6, false, false),
        new(CommonYear, 7, false, false),
        new(CommonYear, 8, false, false),
        new(CommonYear, 9, false, false),
        new(CommonYear, 10, false, false),
        new(CommonYear, 11, false, false),
        new(CommonYear, 12, false, false),
        new(CommonYear, 13, false, true),
        // Leap year.
        new(LeapYear, 1, false, false),
        new(LeapYear, 2, false, false),
        new(LeapYear, 3, false, false),
        new(LeapYear, 4, false, false),
        new(LeapYear, 5, false, false),
        new(LeapYear, 6, false, false),
        new(LeapYear, 7, false, false),
        new(LeapYear, 8, false, false),
        new(LeapYear, 9, false, false),
        new(LeapYear, 10, false, false),
        new(LeapYear, 11, false, false),
        new(LeapYear, 12, false, false),
        new(LeapYear, 13, true, false),
        new(LeapYear, 14, false, true),
    };

    /// <summary>Year, week of the year (Yewe), isIntercalary.</summary>
    public static DataGroup<YeweAnd<bool>> WeekInfoData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, false),
        new(CommonYear, 48, false),
        new(CommonYear, 49, false),
        new(CommonYear, 50, false),
        new(CommonYear, 51, false),
        new(CommonYear, 52, false),
        // Leap year.
        new(LeapYear, 1, false),
        new(LeapYear, 48, false),
        new(LeapYear, 49, true),
        new(LeapYear, 50, false),
        new(LeapYear, 51, false),
        new(LeapYear, 52, false),
        new(LeapYear, 53, false),
    };

    /// <summary>Year, invalid week of the year.</summary>
    public static TheoryData<int, int> InvalidWeekOfYearData { get; } = new()
    {
        { CommonYear, 0 },
        { CommonYear, 53 },
        { LeapYear, 0 },
        { LeapYear, 54 },
    };

    /// <summary>Day number, year, week of the year, day of the week.</summary>
    internal static IEnumerable<(int DaysSinceEpoch, int Year, int WeekOfYear, DayOfWeek DayOfWeek)> MoreDaySinceEpochInfos
    {
        get
        {
            // First week.
            yield return (0, 1, 1, DayOfWeek.Sunday);
            yield return (1, 1, 1, DayOfWeek.Monday);
            yield return (2, 1, 1, DayOfWeek.Tuesday);
            yield return (3, 1, 1, DayOfWeek.Wednesday);
            yield return (4, 1, 1, DayOfWeek.Thursday);
            yield return (5, 1, 1, DayOfWeek.Friday);
            yield return (6, 1, 1, DayOfWeek.Saturday);
            // Second week.
            yield return (7, 1, 2, DayOfWeek.Sunday);
            yield return (13, 1, 2, DayOfWeek.Saturday);
            // Last week of first year.
            yield return (357, 1, 52, DayOfWeek.Sunday);
            yield return (363, 1, 52, DayOfWeek.Saturday);
            // First week of second year.
            yield return (364, 2, 1, DayOfWeek.Sunday);
            yield return (370, 2, 1, DayOfWeek.Saturday);
        }
    }

    [Pure]
    private static TheoryData<int, int, int, DayOfWeek> MapToTheoryData(
        IEnumerable<(int DaysSinceEpoch, int Year, int WeekOfYear, DayOfWeek DayOfWeek)> source)
    {
        var data = new TheoryData<int, int, int, DayOfWeek>();
        foreach (var (daysSinceEpoch, y, woy, dayOfWeek) in source)
        {
            data.Add(daysSinceEpoch, y, woy, dayOfWeek);
        }
        return data;
    }
}
