// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Testing.Data.Unbounded;
using Zorglub.Time.Core;

public sealed class WideDateDayOfWeekTests : IDateDayOfWeekFacts<WideDate, UnboundedGregorianDataSet>
{
    protected sealed override WideDate GetDate(int y, int m, int d) => WideCalendar.Gregorian.GetWideDate(y, m, d);
}

public sealed class WideDateMathTests : IDateMathFacts<WideDate, UnboundedGregorianDataSet>
{
    protected sealed override WideDate GetDate(int y, int m, int d) => WideCalendar.Gregorian.GetWideDate(y, m, d);
}

public sealed class WideDateTests : WideDateFacts<UnboundedGregorianDataSet>
{
    public WideDateTests() : base(WideCalendar.Gregorian, WideCalendar.Julian) { }

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
    public void ToString_InvariantCulture(int y, int m, int d, string str)
    {
        var date = CalendarUT.GetWideDate(y, m, d);
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }

    [Fact]
    public void WithCalendar_NotSupported()
    {
        // Julian MinDayNumber is not within the Gregorian range.
        var minDayNumber = OtherCalendar.Domain.Min;
        var date = OtherCalendar.GetWideDateOn(minDayNumber);
        // Act & Assert
        Assert.ThrowsAoorexn("dayNumber", () => date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianToJulianData), MemberType = typeof(CalCalDataSet))]
    public void WithCalendar_GregorianToJulian(YemodaPair pair)
    {
        var (g, j) = pair;
        var source = CalendarUT.GetWideDate(g.Year, g.Month, g.Day);
        var result = OtherCalendar.GetWideDate(j.Year, j.Month, j.Day);
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(OtherCalendar));
    }

    [Theory, MemberData(nameof(CalCalDataSet.GregorianToJulianData), MemberType = typeof(CalCalDataSet))]
    public void WithCalendar_JulianToGregorian(YemodaPair pair)
    {
        var (g, j) = pair;
        var source = OtherCalendar.GetWideDate(j.Year, j.Month, j.Day);
        var result = CalendarUT.GetWideDate(g.Year, g.Month, g.Day);
        // Act & Assert
        Assert.Equal(result, source.WithCalendar(CalendarUT));
    }
}
