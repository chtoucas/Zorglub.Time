// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core;

using static Zorglub.Time.Extensions.CivilDateExtensions;

public partial class CivilDateTests
{
    [Theory, MemberData(nameof(EnumDataSet.InvalidDayOfWeekData), MemberType = typeof(EnumDataSet))]
    public static void NearestSafe_InvalidDayOfWeek(DayOfWeek dayOfWeek)
    {
        // Arrange
        var date = new CivilDate(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAoorexn("dayOfWeek", () => date.NearestSafe(dayOfWeek));
    }

    [Fact]
    public static void Nearest_NearMinValue()
    {
        // Arrange

        // fri0 (overflow)
        // sat0 (overflow)
        // sun0 (overflow)

        var mon = CivilDate.MinValue;
        var tue = CivilDate.MinValue + 1;
        var wed = CivilDate.MinValue + 2;
        var thu = CivilDate.MinValue + 3;
        var fri = CivilDate.MinValue + 4;
        var sat = CivilDate.MinValue + 5;
        var sun = CivilDate.MinValue + 6;

        var mon1 = CivilDate.MinValue + 7;
        var tue1 = CivilDate.MinValue + 8;
        var wed1 = CivilDate.MinValue + 9;

        // Act & Assert

        // Sanity checks.
        Assert.Equal(DayOfWeek.Monday, mon.DayOfWeek);
        Assert.Equal(DayOfWeek.Tuesday, tue.DayOfWeek);
        Assert.Equal(DayOfWeek.Wednesday, wed.DayOfWeek);
        Assert.Equal(DayOfWeek.Thursday, thu.DayOfWeek);
        Assert.Equal(DayOfWeek.Friday, fri.DayOfWeek);
        Assert.Equal(DayOfWeek.Saturday, sat.DayOfWeek);
        Assert.Equal(DayOfWeek.Sunday, sun.DayOfWeek);

        Assert.Equal(DayOfWeek.Monday, mon1.DayOfWeek);
        Assert.Equal(DayOfWeek.Tuesday, tue1.DayOfWeek);
        Assert.Equal(DayOfWeek.Wednesday, wed1.DayOfWeek);

        // MinValue.
        Assert.Equal(mon, mon.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, mon.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, mon.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, mon.Nearest(DayOfWeek.Thursday));
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Friday));      // fri0
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Saturday));    // sat0
        Assert.Overflows(() => mon.Nearest(DayOfWeek.Sunday));      // sun0

        // MinValue + 1.
        Assert.Equal(mon, tue.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, tue.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, tue.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, tue.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, tue.Nearest(DayOfWeek.Friday));
        Assert.Overflows(() => tue.Nearest(DayOfWeek.Saturday));    // sat0
        Assert.Overflows(() => tue.Nearest(DayOfWeek.Sunday));      // sun0

        // MinValue + 2.
        Assert.Equal(mon, wed.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, wed.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, wed.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, wed.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, wed.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, wed.Nearest(DayOfWeek.Saturday));
        Assert.Overflows(() => wed.Nearest(DayOfWeek.Sunday));      // sun0

        // MinValue + 3.
        Assert.Equal(mon, thu.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, thu.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, thu.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, thu.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, thu.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, thu.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, thu.Nearest(DayOfWeek.Sunday));

        // MinValue + 4.
        Assert.Equal(mon1, fri.Nearest(DayOfWeek.Monday));          // mon1
        Assert.Equal(tue, fri.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, fri.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, fri.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, fri.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, fri.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, fri.Nearest(DayOfWeek.Sunday));

        // MinValue + 5.
        Assert.Equal(mon1, sat.Nearest(DayOfWeek.Monday));          // mon1
        Assert.Equal(tue1, sat.Nearest(DayOfWeek.Tuesday));         // tue1
        Assert.Equal(wed, sat.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, sat.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, sat.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, sat.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, sat.Nearest(DayOfWeek.Sunday));

        // MinValue + 6.
        Assert.Equal(mon1, sun.Nearest(DayOfWeek.Monday));          // mon1
        Assert.Equal(tue1, sun.Nearest(DayOfWeek.Tuesday));         // tue1
        Assert.Equal(wed1, sun.Nearest(DayOfWeek.Wednesday));       // wed1
        Assert.Equal(thu, sun.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, sun.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, sun.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, sun.Nearest(DayOfWeek.Sunday));
    }

    [Fact]
    public static void NearestSafe_NearMinValue()
    {
        // Arrange
        var mon = CivilDate.MinValue;
        var tue = CivilDate.MinValue + 1;
        var wed = CivilDate.MinValue + 2;
        var thu = CivilDate.MinValue + 3;
        var fri = CivilDate.MinValue + 4;
        var sat = CivilDate.MinValue + 5;
        var sun = CivilDate.MinValue + 6;

        var mon1 = CivilDate.MinValue + 7;
        var tue1 = CivilDate.MinValue + 8;
        var wed1 = CivilDate.MinValue + 9;

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
    public static void Nearest_NearMaxValue()
    {
        // Arrange
        var wed0 = CivilDate.MaxValue - 9;
        var thu0 = CivilDate.MaxValue - 8;
        var fri0 = CivilDate.MaxValue - 7;

        var sat = CivilDate.MaxValue - 6;
        var sun = CivilDate.MaxValue - 5;
        var mon = CivilDate.MaxValue - 4;
        var tue = CivilDate.MaxValue - 3;
        var wed = CivilDate.MaxValue - 2;
        var thu = CivilDate.MaxValue - 1;
        var fri = CivilDate.MaxValue;

        // sat1
        // sun1
        // mon1

        // Act & Assert

        // Sanity checks.
        Assert.Equal(DayOfWeek.Wednesday, wed0.DayOfWeek);
        Assert.Equal(DayOfWeek.Thursday, thu0.DayOfWeek);
        Assert.Equal(DayOfWeek.Friday, fri0.DayOfWeek);

        Assert.Equal(DayOfWeek.Saturday, sat.DayOfWeek);
        Assert.Equal(DayOfWeek.Sunday, sun.DayOfWeek);
        Assert.Equal(DayOfWeek.Monday, mon.DayOfWeek);
        Assert.Equal(DayOfWeek.Friday, fri.DayOfWeek);
        Assert.Equal(DayOfWeek.Tuesday, tue.DayOfWeek);
        Assert.Equal(DayOfWeek.Wednesday, wed.DayOfWeek);
        Assert.Equal(DayOfWeek.Thursday, thu.DayOfWeek);

        // MinValue.
        Assert.Overflows(() => fri.Nearest(DayOfWeek.Monday));      // fri1
        Assert.Equal(tue, fri.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, fri.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, fri.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, fri.Nearest(DayOfWeek.Friday));
        Assert.Overflows(() => fri.Nearest(DayOfWeek.Saturday));    // sat1
        Assert.Overflows(() => fri.Nearest(DayOfWeek.Sunday));      // mon1

        // MinValue - 1.
        Assert.Equal(mon, thu.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, thu.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, thu.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, thu.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, thu.Nearest(DayOfWeek.Friday));
        Assert.Overflows(() => thu.Nearest(DayOfWeek.Saturday));    // sat1
        Assert.Overflows(() => thu.Nearest(DayOfWeek.Sunday));      // sun1

        // MinValue - 2.
        Assert.Equal(mon, wed.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, wed.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, wed.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, wed.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, wed.Nearest(DayOfWeek.Friday));
        Assert.Overflows(() => wed.Nearest(DayOfWeek.Saturday));    // sat1
        Assert.Equal(sun, wed.Nearest(DayOfWeek.Sunday));

        // MinValue - 3.
        Assert.Equal(mon, tue.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, tue.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, tue.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, tue.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri, tue.Nearest(DayOfWeek.Friday));
        Assert.Equal(sat, tue.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, tue.Nearest(DayOfWeek.Sunday));

        // MinValue - 4.
        Assert.Equal(mon, mon.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, mon.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, mon.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu, mon.Nearest(DayOfWeek.Thursday));
        Assert.Equal(fri0, mon.Nearest(DayOfWeek.Friday));          // fri0
        Assert.Equal(sat, mon.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, mon.Nearest(DayOfWeek.Sunday));

        // MinValue - 5.
        Assert.Equal(mon, sun.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, sun.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed, sun.Nearest(DayOfWeek.Wednesday));
        Assert.Equal(thu0, sun.Nearest(DayOfWeek.Thursday));        // thu0
        Assert.Equal(fri0, sun.Nearest(DayOfWeek.Friday));          // fri0
        Assert.Equal(sat, sun.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, sun.Nearest(DayOfWeek.Sunday));

        // MinValue - 6.
        Assert.Equal(mon, sat.Nearest(DayOfWeek.Monday));
        Assert.Equal(tue, sat.Nearest(DayOfWeek.Tuesday));
        Assert.Equal(wed0, sat.Nearest(DayOfWeek.Wednesday));       // wed0
        Assert.Equal(thu0, sat.Nearest(DayOfWeek.Thursday));        // thu0
        Assert.Equal(fri0, sat.Nearest(DayOfWeek.Friday));          // fri0
        Assert.Equal(sat, sat.Nearest(DayOfWeek.Saturday));
        Assert.Equal(sun, sat.Nearest(DayOfWeek.Sunday));
    }

    [Fact]
    public static void NearestSafe_NearMaxValue()
    {
        // Arrange
        var wed0 = CivilDate.MaxValue - 9;
        var thu0 = CivilDate.MaxValue - 8;
        var fri0 = CivilDate.MaxValue - 7;

        var sat = CivilDate.MaxValue - 6;
        var sun = CivilDate.MaxValue - 5;
        var mon = CivilDate.MaxValue - 4;
        var tue = CivilDate.MaxValue - 3;
        var wed = CivilDate.MaxValue - 2;
        var thu = CivilDate.MaxValue - 1;
        var fri = CivilDate.MaxValue;

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
    public static void NearestSafe(Yemoda xdate, Yemoda xexp, DayOfWeek dayOfWeek)
    {
        // Arrange
        var date = CreateCivilDate(xdate);
        var exp = CreateCivilDate(xexp);
        // Act
        var actual = date.NearestSafe(dayOfWeek);
        // Assert
        Assert.Equal(exp, actual);
    }
}
