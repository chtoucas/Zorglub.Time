// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

public static class ZDateTests
{
    [Theory]
    [InlineData(-1, 1, 1, "01/01/-0001 (Gregorian)")]
    [InlineData(0, 1, 1, "01/01/0000 (Gregorian)")]
    [InlineData(1, 1, 1, "01/01/0001 (Gregorian)")]
    [InlineData(1, 2, 3, "03/02/0001 (Gregorian)")]
    [InlineData(11, 12, 13, "13/12/0011 (Gregorian)")]
    [InlineData(111, 3, 6, "06/03/0111 (Gregorian)")]
    [InlineData(2019, 1, 3, "03/01/2019 (Gregorian)")]
    [InlineData(9999, 12, 31, "31/12/9999 (Gregorian)")]
    [InlineData(10_000, 12, 31, "31/12/10000 (Gregorian)")]
    public static void ToString_InvariantCulture(int y, int m, int d, string str)
    {
        var date = ZCalendar.Gregorian.GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }

    [Fact]
    public static void WithCalendar_NotSupported()
    {
        // Julian MinDayNumber is not within the Gregorian range.
        var minDayNumber = ZCalendar.Julian.Domain.Min;
        var date = ZCalendar.Julian.GetDate(minDayNumber);
        // Act & Assert
        Assert.ThrowsAoorexn("dayNumber", () => date.WithCalendar(ZCalendar.Gregorian));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianToJulianData), MemberType = typeof(CalCalDataSet))]
    public static void GregorianToJulian(YemodaPair pair)
    {
        var (g, j) = pair;
        var source = ZCalendar.Gregorian.GetDate(g.Year, g.Month, g.Day);
        var result = ZCalendar.Julian.GetDate(j.Year, j.Month, j.Day);
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(ZCalendar.Julian));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianToJulianData), MemberType = typeof(CalCalDataSet))]
    public static void JulianToGregorian(YemodaPair pair)
    {
        var (g, j) = pair;
        var source = ZCalendar.Julian.GetDate(j.Year, j.Month, j.Day);
        var result = ZCalendar.Gregorian.GetDate(g.Year, g.Month, g.Day);
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(ZCalendar.Gregorian));
    }
}
