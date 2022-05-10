// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using static Zorglub.Time.Extensions.DayNumberExtensions;

public static partial class DayNumberTests
{
    [Fact]
    public static void Op_Increment_Overflows_AtMaxValue()
    {
        var copy = DayNumber.MaxValue;
        Assert.Overflows(() => copy++);
    }

    [Fact]
    public static void Op_Increment()
    {
        var dayNumber = DayNumber.Zero + 345;
        var dayNumberAfter = DayNumber.Zero + 346;
        // Act & Assert
        var copy = dayNumber;
        copy++;
        Assert.Equal(dayNumberAfter, copy);
    }

    [Fact]
    public static void Op_Decrement_Overflows_AtMinValue()
    {
        var copy = DayNumber.MinValue;
        Assert.Overflows(() => copy--);
    }

    [Fact]
    public static void Op_Decrement()
    {
        var dayNumber = DayNumber.Zero + 345;
        var dayNumberBefore = DayNumber.Zero + 344;
        // Act & Assert
        var copy = dayNumber;
        copy--;
        Assert.Equal(dayNumberBefore, copy);
    }
}

public partial class DayNumberTests // Adjust the day of the week
{
    // TODO: revoir les tests aux limites pour Before() & co.
    // Test before epoch.
    // Test avec extensions w/ (int num).
    // Basculer Nearest_NearMin/MaxValue dans DayOfWeekAdjusterTester.

    [Fact]
    public static void Nearest_NearMinValue()
    {
        //  thu0 (overflow)
        //  fri0 (overflow)
        //  sat0 (overflow)

        var sun = DayNumber.MinValue;
        var mon = DayNumber.MinValue + 1;
        var tue = DayNumber.MinValue + 2;
        var wed = DayNumber.MinValue + 3;
        var thu = DayNumber.MinValue + 4;
        var fri = DayNumber.MinValue + 5;
        var sat = DayNumber.MinValue + 6;

        var sun1 = DayNumber.MinValue + 7;
        var mon1 = DayNumber.MinValue + 8;
        var tue1 = DayNumber.MinValue + 9;

        // Act & Assert

        // Sanity checks.
        Assert.Equal(DayOfWeek.Sunday, sun.DayOfWeek);
        Assert.Equal(DayOfWeek.Monday, mon.DayOfWeek);
        Assert.Equal(DayOfWeek.Tuesday, tue.DayOfWeek);
        Assert.Equal(DayOfWeek.Wednesday, wed.DayOfWeek);
        Assert.Equal(DayOfWeek.Thursday, thu.DayOfWeek);
        Assert.Equal(DayOfWeek.Friday, fri.DayOfWeek);
        Assert.Equal(DayOfWeek.Saturday, sat.DayOfWeek);

        Assert.Equal(DayOfWeek.Sunday, sun1.DayOfWeek);
        Assert.Equal(DayOfWeek.Monday, mon1.DayOfWeek);
        Assert.Equal(DayOfWeek.Tuesday, tue1.DayOfWeek);

        // MinValue.
        Assert.Equal(mon, sun.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, sun.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, sun.Nearest(DayOfWeek.Wednesday));
        Assert.Overflows(() => sun.Nearest(DayOfWeek.Thursday));    // thu0
        Assert.Overflows(() => sun.Nearest(DayOfWeek.Friday));      // fri0
        Assert.Overflows(() => sun.Nearest(DayOfWeek.Saturday));    // sat0
        Assert.Equal(sun, sun.Nearest(DayOfWeek.Sunday));

        // MinValue + 1.
        Assert.Equal(mon, mon.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, mon.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, mon.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, mon.Nearest(DayOfWeek.Thursday));
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Friday));      // fri0
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Saturday));    // sat0
        Assert.Equal(sun, mon.Nearest(DayOfWeek.Sunday));

        // MinValue + 2.
        Assert.Equal(mon, tue.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, tue.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, tue.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, tue.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, tue.Nearest(DayOfWeek.Friday));
        Assert.Overflows(() => tue.Nearest(DayOfWeek.Saturday));    // sat0
        Assert.Equal(sun, tue.Nearest(DayOfWeek.Sunday));

        // MinValue + 3.
        Assert.Equal(mon, wed.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, wed.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, wed.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, wed.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, wed.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, wed.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, wed.Nearest(DayOfWeek.Sunday));

        // MinValue + 4.
        Assert.Equal(mon, thu.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, thu.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, thu.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, thu.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, thu.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, thu.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun1, thu.Nearest(DayOfWeek.Sunday));          // sun1

        // MinValue + 5.
        Assert.Equal(mon1, fri.Nearest(DayOfWeek.Monday));          // mon1
        Assert.Equal(tue, fri.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, fri.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, fri.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, fri.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, fri.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun1, fri.Nearest(DayOfWeek.Sunday));          // sun1

        // MinValue + 6.
        Assert.Equal(mon1, sat.Nearest(DayOfWeek.Monday));          // mon1
        Assert.Equal(tue1, sat.Nearest(DayOfWeek.Tuesday));         // tue1
        Assert.Equal(wed, sat.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, sat.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, sat.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, sat.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun1, sat.Nearest(DayOfWeek.Sunday));          // sun1
    }

    [Fact]
    public static void Nearest_NearMaxValue()
    {
        var sat0 = DayNumber.MaxValue - 9;
        var sun0 = DayNumber.MaxValue - 8;
        var mon0 = DayNumber.MaxValue - 7;

        var tue = DayNumber.MaxValue - 6;
        var wed = DayNumber.MaxValue - 5;
        var thu = DayNumber.MaxValue - 4;
        var fri = DayNumber.MaxValue - 3;
        var sat = DayNumber.MaxValue - 2;
        var sun = DayNumber.MaxValue - 1;
        var mon = DayNumber.MaxValue;

        //  tue1 (overflow)
        //  wed1 (overflow)
        //  thu1 (overflow)

        // Act & Assert

        // Sanity checks.
        Assert.Equal(DayOfWeek.Saturday, sat0.DayOfWeek);
        Assert.Equal(DayOfWeek.Sunday, sun0.DayOfWeek);
        Assert.Equal(DayOfWeek.Monday, mon0.DayOfWeek);

        Assert.Equal(DayOfWeek.Tuesday, tue.DayOfWeek);
        Assert.Equal(DayOfWeek.Wednesday, wed.DayOfWeek);
        Assert.Equal(DayOfWeek.Thursday, thu.DayOfWeek);
        Assert.Equal(DayOfWeek.Friday, fri.DayOfWeek);
        Assert.Equal(DayOfWeek.Saturday, sat.DayOfWeek);
        Assert.Equal(DayOfWeek.Sunday, sun.DayOfWeek);
        Assert.Equal(DayOfWeek.Monday, mon.DayOfWeek);

        // MaxValue.
        Assert.Equal(mon, mon.Nearest(DayOfWeek.Monday));
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Tuesday));     // tue1
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Wednesday));   // wed1
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Thursday));    // thu1
        Assert.Equal(fri, mon.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, mon.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, mon.Nearest(DayOfWeek.Sunday));

        // MaxValue - 1.
        Assert.Equal(mon, sun.Nearest(DayOfWeek.Monday));
        Assert.Overflows(() => sun.Nearest(DayOfWeek.Tuesday));     // tue1
        Assert.Overflows(() => sun.Nearest(DayOfWeek.Wednesday));   // wed1
        Assert.Equal(thu, sun.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, sun.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, sun.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, sun.Nearest(DayOfWeek.Sunday));

        // MaxValue - 2.
        Assert.Equal(mon, sat.Nearest(DayOfWeek.Monday));
        Assert.Overflows(() => sat.Nearest(DayOfWeek.Tuesday));     // tue1
        Assert.Equal(wed, sat.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, sat.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, sat.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, sat.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, sat.Nearest(DayOfWeek.Sunday));

        // MaxValue - 3.
        Assert.Equal(mon, fri.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, fri.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, fri.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, fri.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, fri.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, fri.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, fri.Nearest(DayOfWeek.Sunday));

        // MaxValue - 4.
        Assert.Equal(mon0, thu.Nearest(DayOfWeek.Monday));          // mon0
        Assert.Equal(tue, thu.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, thu.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, thu.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, thu.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, thu.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, thu.Nearest(DayOfWeek.Sunday));

        // MaxValue - 5.
        Assert.Equal(mon0, wed.Nearest(DayOfWeek.Monday));          // mon0
        Assert.Equal(tue, wed.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, wed.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, wed.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, wed.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, wed.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun0, wed.Nearest(DayOfWeek.Sunday));          // sun0

        // MaxValue - 6.
        Assert.Equal(mon0, tue.Nearest(DayOfWeek.Monday));          // mon0
        Assert.Equal(tue, tue.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, tue.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, tue.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, tue.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat0, tue.Nearest(DayOfWeek.Saturday));        // sat0
        Assert.Equal(sun0, tue.Nearest(DayOfWeek.Sunday));          // sun0
    }

    [Fact]
    public static void Before_Underflows() =>
        Assert.Overflows(() => DayNumber.MinValue.Before(DayOfWeek.Monday));

    [Fact]
    public static void OnOrBefore_Underflows() =>
        Assert.Overflows(() => DayNumber.MinValue.OnOrBefore(DayOfWeek.Friday));

    [Fact]
    public static void OnOrAfter_Overflows() =>
        Assert.Overflows(() => DayNumber.MaxValue.OnOrAfter(DayOfWeek.Monday));

    [Fact]
    public static void After_Overflows() =>
        Assert.Overflows(() => DayNumber.MaxValue.After(DayOfWeek.Monday));
}
