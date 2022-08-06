// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing;

using Zorglub.Time.Hemerology;

// Test aux limites.
public static class DayOfWeekAdjusterTester
{
    [Pure]
    public static DayOfWeekAdjusterTester<T> NearMinValue<T>(T min) where T : IFixedDate<T> =>
        new(min, testNext: false, (x, n) => x + n);

    [Pure]
    public static DayOfWeekAdjusterTester<T> NearMaxValue<T>(T max) where T : IFixedDate<T> =>
        new(max, testNext: true, (x, n) => x + n);
}

public sealed partial class DayOfWeekAdjusterTester<T> where T : IFixedDate<T>
{
    private readonly bool _testNext;

    private readonly T day;
    private readonly T day1;
    private readonly T day2;
    private readonly T day3;
    private readonly T day4;
    private readonly T day5;
    private readonly T day6;
    private readonly T day7;
    private readonly T day8;
    private readonly T day9;

    private readonly DayOfWeek dow;
    private readonly DayOfWeek dow1;
    private readonly DayOfWeek dow2;
    private readonly DayOfWeek dow3;
    private readonly DayOfWeek dow4;
    private readonly DayOfWeek dow5;
    private readonly DayOfWeek dow6;

    public DayOfWeekAdjusterTester(T day, bool testNext, Func<T, int, T> add)
    {
        Requires.NotNull(add);

        this.day = day;
        dow = day.DayOfWeek;
        _testNext = testNext;

        if (testNext)
        {
            day1 = add(day, -1);
            day2 = add(day1, -1);
            day3 = add(day2, -1);
            day4 = add(day3, -1);
            day5 = add(day4, -1);
            day6 = add(day5, -1);
            day7 = add(day6, -1);
            day8 = add(day7, -1);
            day9 = add(day8, -1);

            dow1 = PreviousDayOfWeek(dow);
            dow2 = PreviousDayOfWeek(dow1);
            dow3 = PreviousDayOfWeek(dow2);
            dow4 = PreviousDayOfWeek(dow3);
            dow5 = PreviousDayOfWeek(dow4);
            dow6 = PreviousDayOfWeek(dow5);
        }
        else
        {
            day1 = add(day, 1);
            day2 = add(day1, 1);
            day3 = add(day2, 1);
            day4 = add(day3, 1);
            day5 = add(day4, 1);
            day6 = add(day5, 1);
            day7 = add(day6, 1);
            day8 = add(day7, 1);
            day9 = add(day8, 1);

            dow1 = NextDayOfWeek(dow);
            dow2 = NextDayOfWeek(dow1);
            dow3 = NextDayOfWeek(dow2);
            dow4 = NextDayOfWeek(dow3);
            dow5 = NextDayOfWeek(dow4);
            dow6 = NextDayOfWeek(dow5);
        }
    }

    private static DayOfWeek PreviousDayOfWeek(DayOfWeek dow) =>
        dow == DayOfWeek.Sunday ? DayOfWeek.Saturday : dow - 1;

    private static DayOfWeek NextDayOfWeek(DayOfWeek dow) =>
        dow == DayOfWeek.Saturday ? DayOfWeek.Sunday : dow + 1;
}

public partial class DayOfWeekAdjusterTester<T> // Previous() & PreviousOrSame()
{
    public void TestPrevious()
    {
        if (_testNext) { throw new InvalidOperationException(); }

        Assert.Overflows(() => day.Previous(dow));
        Assert.Overflows(() => day.Previous(dow1));
        Assert.Overflows(() => day.Previous(dow2));
        Assert.Overflows(() => day.Previous(dow3));
        Assert.Overflows(() => day.Previous(dow4));
        Assert.Overflows(() => day.Previous(dow5));
        Assert.Overflows(() => day.Previous(dow6));

        Assert.Equal(day, day1.Previous(dow));
        Assert.Overflows(() => day1.Previous(dow1));
        Assert.Overflows(() => day1.Previous(dow2));
        Assert.Overflows(() => day1.Previous(dow3));
        Assert.Overflows(() => day1.Previous(dow4));
        Assert.Overflows(() => day1.Previous(dow5));
        Assert.Overflows(() => day1.Previous(dow6));

        Assert.Equal(day, day2.Previous(dow));
        Assert.Equal(day1, day2.Previous(dow1));
        Assert.Overflows(() => day2.Previous(dow2));
        Assert.Overflows(() => day2.Previous(dow3));
        Assert.Overflows(() => day2.Previous(dow4));
        Assert.Overflows(() => day2.Previous(dow5));
        Assert.Overflows(() => day2.Previous(dow6));

        Assert.Equal(day, day3.Previous(dow));
        Assert.Equal(day1, day3.Previous(dow1));
        Assert.Equal(day2, day3.Previous(dow2));
        Assert.Overflows(() => day3.Previous(dow3));
        Assert.Overflows(() => day3.Previous(dow4));
        Assert.Overflows(() => day3.Previous(dow5));
        Assert.Overflows(() => day3.Previous(dow6));

        Assert.Equal(day, day4.Previous(dow));
        Assert.Equal(day1, day4.Previous(dow1));
        Assert.Equal(day2, day4.Previous(dow2));
        Assert.Equal(day3, day4.Previous(dow3));
        Assert.Overflows(() => day4.Previous(dow4));
        Assert.Overflows(() => day4.Previous(dow5));
        Assert.Overflows(() => day4.Previous(dow6));

        Assert.Equal(day, day5.Previous(dow));
        Assert.Equal(day1, day5.Previous(dow1));
        Assert.Equal(day2, day5.Previous(dow2));
        Assert.Equal(day3, day5.Previous(dow3));
        Assert.Equal(day4, day5.Previous(dow4));
        Assert.Overflows(() => day5.Previous(dow5));
        Assert.Overflows(() => day5.Previous(dow6));

        Assert.Equal(day, day6.Previous(dow));
        Assert.Equal(day1, day6.Previous(dow1));
        Assert.Equal(day2, day6.Previous(dow2));
        Assert.Equal(day3, day6.Previous(dow3));
        Assert.Equal(day4, day6.Previous(dow4));
        Assert.Equal(day5, day6.Previous(dow5));
        Assert.Overflows(() => day6.Previous(dow6));

        Assert.Equal(day, day7.Previous(dow));
        Assert.Equal(day1, day7.Previous(dow1));
        Assert.Equal(day2, day7.Previous(dow2));
        Assert.Equal(day3, day7.Previous(dow3));
        Assert.Equal(day4, day7.Previous(dow4));
        Assert.Equal(day5, day7.Previous(dow5));
        Assert.Equal(day6, day7.Previous(dow6));
    }

    public void TestPreviousOrSame()
    {
        if (_testNext) { throw new InvalidOperationException(); }

        Assert.Equal(day, day.PreviousOrSame(dow));
        Assert.Overflows(() => day.PreviousOrSame(dow1));
        Assert.Overflows(() => day.PreviousOrSame(dow2));
        Assert.Overflows(() => day.PreviousOrSame(dow3));
        Assert.Overflows(() => day.PreviousOrSame(dow4));
        Assert.Overflows(() => day.PreviousOrSame(dow5));
        Assert.Overflows(() => day.PreviousOrSame(dow6));

        Assert.Equal(day, day1.PreviousOrSame(dow));
        Assert.Equal(day1, day1.PreviousOrSame(dow1));
        Assert.Overflows(() => day1.PreviousOrSame(dow2));
        Assert.Overflows(() => day1.PreviousOrSame(dow3));
        Assert.Overflows(() => day1.PreviousOrSame(dow4));
        Assert.Overflows(() => day1.PreviousOrSame(dow5));
        Assert.Overflows(() => day1.PreviousOrSame(dow6));

        Assert.Equal(day, day2.PreviousOrSame(dow));
        Assert.Equal(day1, day2.PreviousOrSame(dow1));
        Assert.Equal(day2, day2.PreviousOrSame(dow2));
        Assert.Overflows(() => day2.PreviousOrSame(dow3));
        Assert.Overflows(() => day2.PreviousOrSame(dow4));
        Assert.Overflows(() => day2.PreviousOrSame(dow5));
        Assert.Overflows(() => day2.PreviousOrSame(dow6));

        Assert.Equal(day, day3.PreviousOrSame(dow));
        Assert.Equal(day1, day3.PreviousOrSame(dow1));
        Assert.Equal(day2, day3.PreviousOrSame(dow2));
        Assert.Equal(day3, day3.PreviousOrSame(dow3));
        Assert.Overflows(() => day3.PreviousOrSame(dow4));
        Assert.Overflows(() => day3.PreviousOrSame(dow5));
        Assert.Overflows(() => day3.PreviousOrSame(dow6));

        Assert.Equal(day, day4.PreviousOrSame(dow));
        Assert.Equal(day1, day4.PreviousOrSame(dow1));
        Assert.Equal(day2, day4.PreviousOrSame(dow2));
        Assert.Equal(day3, day4.PreviousOrSame(dow3));
        Assert.Equal(day4, day4.PreviousOrSame(dow4));
        Assert.Overflows(() => day4.PreviousOrSame(dow5));
        Assert.Overflows(() => day4.PreviousOrSame(dow6));

        Assert.Equal(day, day5.PreviousOrSame(dow));
        Assert.Equal(day1, day5.PreviousOrSame(dow1));
        Assert.Equal(day2, day5.PreviousOrSame(dow2));
        Assert.Equal(day3, day5.PreviousOrSame(dow3));
        Assert.Equal(day4, day5.PreviousOrSame(dow4));
        Assert.Equal(day5, day5.PreviousOrSame(dow5));
        Assert.Overflows(() => day5.PreviousOrSame(dow6));

        Assert.Equal(day, day6.PreviousOrSame(dow));
        Assert.Equal(day1, day6.PreviousOrSame(dow1));
        Assert.Equal(day2, day6.PreviousOrSame(dow2));
        Assert.Equal(day3, day6.PreviousOrSame(dow3));
        Assert.Equal(day4, day6.PreviousOrSame(dow4));
        Assert.Equal(day5, day6.PreviousOrSame(dow5));
        Assert.Equal(day6, day6.PreviousOrSame(dow6));
    }
}

public partial class DayOfWeekAdjusterTester<T> // Nearest()
{
    [Fact]
    public void TestNearest()
    {
        // MinValue.
        Assert.Equal(day, day.Nearest(dow));
        Assert.Equal(day1, day.Nearest(dow1));
        Assert.Equal(day2, day.Nearest(dow2));
        Assert.Equal(day3, day.Nearest(dow3));
        Assert.Overflows(() => day.Nearest(dow4));  // day - 3
        Assert.Overflows(() => day.Nearest(dow5));  // day - 2
        Assert.Overflows(() => day.Nearest(dow6));  // day - 1

        // MinValue + 1.
        Assert.Equal(day, day1.Nearest(dow));
        Assert.Equal(day1, day1.Nearest(dow1));
        Assert.Equal(day2, day1.Nearest(dow2));
        Assert.Equal(day3, day1.Nearest(dow3));
        Assert.Equal(day4, day1.Nearest(dow4));
        Assert.Overflows(() => day1.Nearest(dow5)); // day - 2
        Assert.Overflows(() => day1.Nearest(dow6)); // day - 1

        // MinValue + 2.
        Assert.Equal(day, day2.Nearest(dow));
        Assert.Equal(day1, day2.Nearest(dow1));
        Assert.Equal(day2, day2.Nearest(dow2));
        Assert.Equal(day3, day2.Nearest(dow3));
        Assert.Equal(day4, day2.Nearest(dow4));
        Assert.Equal(day5, day2.Nearest(dow5));
        Assert.Overflows(() => day2.Nearest(dow6)); // day - 1

        // MinValue + 3.
        Assert.Equal(day, day3.Nearest(dow));
        Assert.Equal(day1, day3.Nearest(dow1));
        Assert.Equal(day2, day3.Nearest(dow2));
        Assert.Equal(day3, day3.Nearest(dow3));
        Assert.Equal(day4, day3.Nearest(dow4));
        Assert.Equal(day5, day3.Nearest(dow5));
        Assert.Equal(day6, day3.Nearest(dow6));

        // MinValue + 4.
        Assert.Equal(day7, day4.Nearest(dow));      // day + 7
        Assert.Equal(day1, day4.Nearest(dow1));
        Assert.Equal(day2, day4.Nearest(dow2));
        Assert.Equal(day3, day4.Nearest(dow3));
        Assert.Equal(day4, day4.Nearest(dow4));
        Assert.Equal(day5, day4.Nearest(dow5));
        Assert.Equal(day6, day4.Nearest(dow6));

        // MinValue + 5.
        Assert.Equal(day7, day5.Nearest(dow));      // day + 7
        Assert.Equal(day8, day5.Nearest(dow1));     // day + 8
        Assert.Equal(day2, day5.Nearest(dow2));
        Assert.Equal(day3, day5.Nearest(dow3));
        Assert.Equal(day4, day5.Nearest(dow4));
        Assert.Equal(day5, day5.Nearest(dow5));
        Assert.Equal(day6, day5.Nearest(dow6));

        // MinValue + 6.
        Assert.Equal(day7, day6.Nearest(dow));      // day + 7
        Assert.Equal(day8, day6.Nearest(dow1));     // day + 8
        Assert.Equal(day9, day6.Nearest(dow2));     // day + 9
        Assert.Equal(day3, day6.Nearest(dow3));
        Assert.Equal(day4, day6.Nearest(dow4));
        Assert.Equal(day5, day6.Nearest(dow5));
        Assert.Equal(day6, day6.Nearest(dow6));
    }
}

public partial class DayOfWeekAdjusterTester<T> // Next() & NextOrSame()
{
    public void TestNextOrSame()
    {
        if (_testNext == false) { throw new InvalidOperationException(); }

        Assert.Equal(day, day.NextOrSame(dow));
        Assert.Overflows(() => day.NextOrSame(dow1));
        Assert.Overflows(() => day.NextOrSame(dow2));
        Assert.Overflows(() => day.NextOrSame(dow3));
        Assert.Overflows(() => day.NextOrSame(dow4));
        Assert.Overflows(() => day.NextOrSame(dow5));
        Assert.Overflows(() => day.NextOrSame(dow6));

        Assert.Equal(day, day1.NextOrSame(dow));
        Assert.Equal(day1, day1.NextOrSame(dow1));
        Assert.Overflows(() => day1.NextOrSame(dow2));
        Assert.Overflows(() => day1.NextOrSame(dow3));
        Assert.Overflows(() => day1.NextOrSame(dow4));
        Assert.Overflows(() => day1.NextOrSame(dow5));
        Assert.Overflows(() => day1.NextOrSame(dow6));

        Assert.Equal(day, day2.NextOrSame(dow));
        Assert.Equal(day1, day2.NextOrSame(dow1));
        Assert.Equal(day2, day2.NextOrSame(dow2));
        Assert.Overflows(() => day2.NextOrSame(dow3));
        Assert.Overflows(() => day2.NextOrSame(dow4));
        Assert.Overflows(() => day2.NextOrSame(dow5));
        Assert.Overflows(() => day2.NextOrSame(dow6));

        Assert.Equal(day, day3.NextOrSame(dow));
        Assert.Equal(day1, day3.NextOrSame(dow1));
        Assert.Equal(day2, day3.NextOrSame(dow2));
        Assert.Equal(day3, day3.NextOrSame(dow3));
        Assert.Overflows(() => day3.NextOrSame(dow4));
        Assert.Overflows(() => day3.NextOrSame(dow5));
        Assert.Overflows(() => day3.NextOrSame(dow6));

        Assert.Equal(day, day4.NextOrSame(dow));
        Assert.Equal(day1, day4.NextOrSame(dow1));
        Assert.Equal(day2, day4.NextOrSame(dow2));
        Assert.Equal(day3, day4.NextOrSame(dow3));
        Assert.Equal(day4, day4.NextOrSame(dow4));
        Assert.Overflows(() => day4.NextOrSame(dow5));
        Assert.Overflows(() => day4.NextOrSame(dow6));

        Assert.Equal(day, day5.NextOrSame(dow));
        Assert.Equal(day1, day5.NextOrSame(dow1));
        Assert.Equal(day2, day5.NextOrSame(dow2));
        Assert.Equal(day3, day5.NextOrSame(dow3));
        Assert.Equal(day4, day5.NextOrSame(dow4));
        Assert.Equal(day5, day5.NextOrSame(dow5));
        Assert.Overflows(() => day5.NextOrSame(dow6));

        Assert.Equal(day, day6.NextOrSame(dow));
        Assert.Equal(day1, day6.NextOrSame(dow1));
        Assert.Equal(day2, day6.NextOrSame(dow2));
        Assert.Equal(day3, day6.NextOrSame(dow3));
        Assert.Equal(day4, day6.NextOrSame(dow4));
        Assert.Equal(day5, day6.NextOrSame(dow5));
        Assert.Equal(day6, day6.NextOrSame(dow6));
    }

    public void TestNext()
    {
        if (_testNext == false) { throw new InvalidOperationException(); }

        Assert.Overflows(() => day.Next(dow));
        Assert.Overflows(() => day.Next(dow1));
        Assert.Overflows(() => day.Next(dow2));
        Assert.Overflows(() => day.Next(dow3));
        Assert.Overflows(() => day.Next(dow4));
        Assert.Overflows(() => day.Next(dow5));
        Assert.Overflows(() => day.Next(dow6));

        Assert.Equal(day, day1.Next(dow));
        Assert.Overflows(() => day1.Next(dow1));
        Assert.Overflows(() => day1.Next(dow2));
        Assert.Overflows(() => day1.Next(dow3));
        Assert.Overflows(() => day1.Next(dow4));
        Assert.Overflows(() => day1.Next(dow5));
        Assert.Overflows(() => day1.Next(dow6));

        Assert.Equal(day, day2.Next(dow));
        Assert.Equal(day1, day2.Next(dow1));
        Assert.Overflows(() => day2.Next(dow2));
        Assert.Overflows(() => day2.Next(dow3));
        Assert.Overflows(() => day2.Next(dow4));
        Assert.Overflows(() => day2.Next(dow5));
        Assert.Overflows(() => day2.Next(dow6));

        Assert.Equal(day, day3.Next(dow));
        Assert.Equal(day1, day3.Next(dow1));
        Assert.Equal(day2, day3.Next(dow2));
        Assert.Overflows(() => day3.Next(dow3));
        Assert.Overflows(() => day3.Next(dow4));
        Assert.Overflows(() => day3.Next(dow5));
        Assert.Overflows(() => day3.Next(dow6));

        Assert.Equal(day, day4.Next(dow));
        Assert.Equal(day1, day4.Next(dow1));
        Assert.Equal(day2, day4.Next(dow2));
        Assert.Equal(day3, day4.Next(dow3));
        Assert.Overflows(() => day4.Next(dow4));
        Assert.Overflows(() => day4.Next(dow5));
        Assert.Overflows(() => day4.Next(dow6));

        Assert.Equal(day, day5.Next(dow));
        Assert.Equal(day1, day5.Next(dow1));
        Assert.Equal(day2, day5.Next(dow2));
        Assert.Equal(day3, day5.Next(dow3));
        Assert.Equal(day4, day5.Next(dow4));
        Assert.Overflows(() => day5.Next(dow5));
        Assert.Overflows(() => day5.Next(dow6));

        Assert.Equal(day, day6.Next(dow));
        Assert.Equal(day1, day6.Next(dow1));
        Assert.Equal(day2, day6.Next(dow2));
        Assert.Equal(day3, day6.Next(dow3));
        Assert.Equal(day4, day6.Next(dow4));
        Assert.Equal(day5, day6.Next(dow5));
        Assert.Overflows(() => day6.Next(dow6));

        Assert.Equal(day, day7.Next(dow));
        Assert.Equal(day1, day7.Next(dow1));
        Assert.Equal(day2, day7.Next(dow2));
        Assert.Equal(day3, day7.Next(dow3));
        Assert.Equal(day4, day7.Next(dow4));
        Assert.Equal(day5, day7.Next(dow5));
        Assert.Equal(day6, day7.Next(dow6));
    }
}
