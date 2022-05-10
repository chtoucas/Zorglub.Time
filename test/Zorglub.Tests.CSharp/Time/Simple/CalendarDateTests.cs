// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Time.Core;

public sealed class CalendarDateTests
{
    public static readonly GregorianCalendar CalendarUT = GregorianCalendar.Instance;
    public static readonly JulianCalendar OtherCalendar = JulianCalendar.Instance;

    [Theory]
    [InlineData(-1, 1, 1, "01/01/-0001 (Gregorian)")]
    [InlineData(0, 1, 1, "01/01/0000 (Gregorian)")]
    [InlineData(1, 1, 1, "01/01/0001 (Gregorian)")]
    [InlineData(1, 2, 3, "03/02/0001 (Gregorian)")]
    [InlineData(11, 12, 13, "13/12/0011 (Gregorian)")]
    [InlineData(111, 3, 6, "06/03/0111 (Gregorian)")]
    [InlineData(2019, 1, 3, "03/01/2019 (Gregorian)")]
    [InlineData(9999, 12, 31, "31/12/9999 (Gregorian)")]
    public void ToString_InvariantCulture(int y, int m, int d, string asString)
    {
        // Arrange
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(asString, date.ToString());
    }

    [Fact]
    public void WithCalendar_NotSupported()
    {
        // Arrange
        // Julian MinDayNumber is not in the Gregorian range.
        var minDayNumber = OtherCalendar.Domain.Min;
        var date = OtherCalendar.GetCalendarDateOn(minDayNumber);
        // Act & Assert
        Assert.ThrowsAoorexn("dayNumber", () => date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianJulianData), MemberType = typeof(CalCalDataSet))]
    public void WithCalendar_GregorianToJulian(Yemoda gregorian, Yemoda julian)
    {
        // Arrange
        var source = CalendarUT.GetCalendarDate(gregorian.Year, gregorian.Month, gregorian.Day);
        var result = OtherCalendar.GetCalendarDate(julian.Year, julian.Month, julian.Day);
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(OtherCalendar));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianJulianData), MemberType = typeof(CalCalDataSet))]
    public void WithCalendar_JulianToGregorian(Yemoda gregorian, Yemoda julian)
    {
        // Arrange
        var source = OtherCalendar.GetCalendarDate(julian.Year, julian.Month, julian.Day);
        var result = CalendarUT.GetCalendarDate(gregorian.Year, gregorian.Month, gregorian.Day);
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(CalendarUT));
    }
}
