// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz;

using static Zorglub.Bulgroz.CivilDateExtensions;

public partial class CivilPrototypeTests
{
    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public static void NearestSafe_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        var date = new CivilPrototype(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.NearestSafe(dayOfWeek));
    }

    [Fact]
    public static void NearestSafe_NearMinValue()
    {
        var mon = CivilPrototype.MinValue;
        var tue = CivilPrototype.MinValue + 1;
        var wed = CivilPrototype.MinValue + 2;
        var thu = CivilPrototype.MinValue + 3;
        var fri = CivilPrototype.MinValue + 4;
        var sat = CivilPrototype.MinValue + 5;
        var sun = CivilPrototype.MinValue + 6;

        var mon1 = CivilPrototype.MinValue + 7;
        var tue1 = CivilPrototype.MinValue + 8;
        var wed1 = CivilPrototype.MinValue + 9;

        // Act & Assert

        // MinValue.
        Assert.Equal(mon, mon.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, mon.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, mon.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, mon.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, mon.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, mon.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, mon.NearestSafe(DayOfWeek.Sunday));

        // MinValue + 1.
        Assert.Equal(mon, tue.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, tue.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, tue.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, tue.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, tue.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, tue.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, tue.NearestSafe(DayOfWeek.Sunday));

        // MinValue + 2.
        Assert.Equal(mon, wed.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, wed.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, wed.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, wed.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, wed.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, wed.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, wed.NearestSafe(DayOfWeek.Sunday));

        // MinValue + 3.
        Assert.Equal(mon, thu.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, thu.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, thu.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, thu.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, thu.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, thu.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, thu.NearestSafe(DayOfWeek.Sunday));

        // MinValue + 4.
        Assert.Equal(mon1, fri.NearestSafe(DayOfWeek.Monday));      // mon1
        Assert.Equal(tue, fri.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, fri.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, fri.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, fri.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, fri.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, fri.NearestSafe(DayOfWeek.Sunday));

        // MinValue + 5.
        Assert.Equal(mon1, sat.NearestSafe(DayOfWeek.Monday));      // mon1
        Assert.Equal(tue1, sat.NearestSafe(DayOfWeek.Tuesday));     // tue1
        Assert.Equal(wed, sat.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, sat.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, sat.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, sat.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, sat.NearestSafe(DayOfWeek.Sunday));

        // MinValue + 6.
        Assert.Equal(mon1, sun.NearestSafe(DayOfWeek.Monday));      // mon1
        Assert.Equal(tue1, sun.NearestSafe(DayOfWeek.Tuesday));     // tue1
        Assert.Equal(wed1, sun.NearestSafe(DayOfWeek.Wednesday));   // wed1
        Assert.Equal(thu, sun.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, sun.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, sun.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, sun.NearestSafe(DayOfWeek.Sunday));
    }

    [Fact]
    public static void NearestSafe_NearMaxValue()
    {
        var wed0 = CivilPrototype.MaxValue - 9;
        var thu0 = CivilPrototype.MaxValue - 8;
        var fri0 = CivilPrototype.MaxValue - 7;

        var sat = CivilPrototype.MaxValue - 6;
        var sun = CivilPrototype.MaxValue - 5;
        var mon = CivilPrototype.MaxValue - 4;
        var tue = CivilPrototype.MaxValue - 3;
        var wed = CivilPrototype.MaxValue - 2;
        var thu = CivilPrototype.MaxValue - 1;
        var fri = CivilPrototype.MaxValue;

        // Act & Assert

        // MinValue.
        Assert.Equal(mon, fri.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, fri.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, fri.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, fri.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, fri.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, fri.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, fri.NearestSafe(DayOfWeek.Sunday));

        // MinValue - 1.
        Assert.Equal(mon, thu.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, thu.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, thu.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, thu.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, thu.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, thu.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, thu.NearestSafe(DayOfWeek.Sunday));

        // MinValue - 2.
        Assert.Equal(mon, wed.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, wed.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, wed.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, wed.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, wed.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, wed.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, wed.NearestSafe(DayOfWeek.Sunday));

        // MinValue - 3.
        Assert.Equal(mon, tue.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, tue.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, tue.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, tue.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri, tue.NearestSafe(DayOfWeek.Friday));
        Assert.Equal(sat, tue.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, tue.NearestSafe(DayOfWeek.Sunday));

        // MinValue - 4.
        Assert.Equal(mon, mon.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, mon.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, mon.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu, mon.NearestSafe(DayOfWeek.Thursday));
        Assert.Equal(fri0, mon.NearestSafe(DayOfWeek.Friday));      // fri0
        Assert.Equal(sat, mon.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, mon.NearestSafe(DayOfWeek.Sunday));

        // MinValue - 5.
        Assert.Equal(mon, sun.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, sun.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed, sun.NearestSafe(DayOfWeek.Wednesday));
        Assert.Equal(thu0, sun.NearestSafe(DayOfWeek.Thursday));    // thu0
        Assert.Equal(fri0, sun.NearestSafe(DayOfWeek.Friday));      // fri0
        Assert.Equal(sat, sun.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, sun.NearestSafe(DayOfWeek.Sunday));

        // MinValue - 6.
        Assert.Equal(mon, sat.NearestSafe(DayOfWeek.Monday));
        Assert.Equal(tue, sat.NearestSafe(DayOfWeek.Tuesday));
        Assert.Equal(wed0, sat.NearestSafe(DayOfWeek.Wednesday));   // wed0
        Assert.Equal(thu0, sat.NearestSafe(DayOfWeek.Thursday));    // thu0
        Assert.Equal(fri0, sat.NearestSafe(DayOfWeek.Friday));      // fri0
        Assert.Equal(sat, sat.NearestSafe(DayOfWeek.Saturday));
        Assert.Equal(sun, sat.NearestSafe(DayOfWeek.Sunday));
    }

    [Theory, MemberData(nameof(DayOfWeek_Nearest_Data))]
    public static void NearestSafe(YemodaPairAnd<DayOfWeek> info)
    {
        var (xdate, xexp, dayOfWeek) = info;
        var date = CreateDate(xdate);
        var exp = CreateDate(xexp);
        // Act
        var actual = date.NearestSafe(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }
}
