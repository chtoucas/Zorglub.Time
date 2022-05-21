// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Linq;

using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;

public sealed partial class CalendarMonthTests : GregorianOnlyTesting
{
    private static readonly JulianCalendar s_Julian = JulianCalendar.Instance;

    public CalendarMonthTests() : base(GregorianCalendar.Instance) { }

    public static DataGroup<YemodaPairAnd<int>> AddYearsData => DataSet.AddYearsData;
    public static DataGroup<YemodaPairAnd<int>> AddMonthsData => DataSet.AddMonthsData;

    //[Theory, MemberData(nameof(SampleDates))]
    //public void Constructor_WithDay(int y, int m, int d, int _4, bool _5, bool _6)
    //{
    //    var date = CalendarUT.NewCalendarDate(y, m, d);
    //    var cmonth = CalendarUT.NewCalendarMonth(y, m);
    //    // Act
    //    var actual = new CalendarMonth(date);
    //    // Assert
    //    Assert.Equal(cmonth, actual);
    //}
}

public partial class CalendarMonthTests // Range stuff
{
    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var cmonth = CalendarUT.GetCalendarMonth(3, 4);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => DateRange.FromMonth(cmonth).WithCalendar(null!));
    }

    [Fact]
    public void WithCalendar()
    {
        var cmonth = CalendarUT.GetCalendarMonth(1970, 1);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 31);
        // Act
        var actual = DateRange.FromMonth(cmonth).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void ToDateRange(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var start = CalendarUT.GetCalendarDate(y, m, 1);
        var end = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth);
        // Act
        var range = DateRange.FromMonth(month);
        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Length(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act
        var range = DateRange.FromMonth(month);
        // Assert
        Assert.Equal(info.DaysInMonth, range.Length);
    }

    #region IsSupersetOf()

    [Fact]
    public void IsSupersetOf_RangeA()
    {
        var range1 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 5));
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
    }

    [Fact]
    public void IsSupersetOf_RangeB()
    {
        var range1 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(4, 4));
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
    }

    [Fact]
    public void IsSupersetOf_RangeC()
    {
        var range1 = DateRange.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRange.Create(CalendarUT.GetCalendarDate(3, 4, 2), 2);
        // Act & Assert
        Assert.True(range1.IsSupersetOf(range2));
    }

    #endregion
    #region Contains()

    [Theory]
    [InlineData(2, 12, 31, false)]
    [InlineData(3, 2, 28, false)]
    [InlineData(3, 3, 1, true)]
    [InlineData(3, 3, 31, true)]
    [InlineData(3, 4, 1, false)]
    [InlineData(4, 1, 1, false)]
    public void Contains_Date(int y, int m, int d, bool inRange)
    {
        var month = CalendarUT.GetCalendarMonth(3, 3);
        var range = DateRange.FromMonth(month);
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(date));
    }

    [Theory]
    [InlineData(2, 12, false)]
    [InlineData(3, 2, false)]
    [InlineData(3, 3, true)]
    [InlineData(3, 4, false)]
    [InlineData(4, 1, false)]
    public void Contains_Month(int y, int m, bool inRange)
    {
        var month = CalendarUT.GetCalendarMonth(3, 3);
        var range = DateRange.FromMonth(month);
        var other = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(other));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void Contains_Year(int y)
    {
        var month = CalendarUT.GetCalendarMonth(3, 3);
        var range = DateRange.FromMonth(month);
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.False(range.Contains(year));
    }

    #endregion

    #region IEnumerable

    [Fact]
    public void Enumerate()
    {
        IEnumerable<CalendarDate> listE =
            from i in Enumerable.Range(1, 30)
            select CalendarUT.GetCalendarDate(3, 4, i);
        // Act
        var month = CalendarUT.GetCalendarMonth(3, 4);
        var actual = DateRange.FromMonth(month);
        // Assert
        Assert.Equal(listE, actual);
    }

    #endregion
}

public partial class CalendarMonthTests // Math ops
{
    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        var (xstart, xend, months) = info;
        var start = new CalendarDate(xstart, CalendarUT.Id).CalendarMonth;
        var end = new CalendarDate(xend, CalendarUT.Id).CalendarMonth;
        // Act & Assert
        Assert.Equal(end, start.PlusMonths(months));
        Assert.Equal(end, start + months);
        Assert.Equal(end, start - (-months));

        Assert.Equal(months, end.CountMonthsSince(start));
        Assert.Equal(months, end - start);
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        var (xstart, xend, years) = info;
        var start = new CalendarDate(xstart, CalendarUT.Id).CalendarMonth;
        var end = new CalendarDate(xend, CalendarUT.Id).CalendarMonth;
        // Act & Assert
        Assert.Equal(end, start.PlusYears(years));
        Assert.Equal(years, end.CountYearsSince(start));
    }
}
