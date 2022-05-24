// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Linq;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;

[Obsolete("TO BE REMOVED")]
public sealed class GregorianDateRangeTests : DateRangeFacts<ProlepticGregorianDataSet>
{
    public GregorianDateRangeTests()
        : base(GregorianCalendar.Instance, JulianCalendar.Instance) { }

    [Fact]
    public void WithCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1970, 1, 1);
        var range = DateRange.Create(date, 2);
        // Act
        var actual = range.WithCalendar(OtherCalendar);
        // Assert
        Assert.Equal(OtherCalendar.GetCalendarDate(1969, 12, 19), actual.Start);
        Assert.Equal(OtherCalendar.GetCalendarDate(1969, 12, 20), actual.End);
        Assert.Equal(OtherCalendar.Id, actual.Calendar.Id);
    }
}

[Obsolete("TO BE REMOVED")]
public sealed class JulianDateRangeTests : DateRangeFacts<ProlepticJulianDataSet>
{
    public JulianDateRangeTests()
        : base(JulianCalendar.Instance, GregorianCalendar.Instance) { }

    [Fact]
    public void WithCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 2);
        // Act
        var actual = range.WithCalendar(OtherCalendar);
        // Assert
        Assert.Equal(OtherCalendar.GetCalendarDate(1970, 1, 1), actual.Start);
        Assert.Equal(OtherCalendar.GetCalendarDate(1970, 1, 2), actual.End);
        Assert.Equal(OtherCalendar.Id, actual.Calendar.Id);
    }
}

public sealed class DateRangeYearTests : CalendarDataConsumer<ProlepticGregorianDataSet>
{
    private static readonly JulianCalendar s_Julian = JulianCalendar.Instance;

    private static GregorianCalendar CalendarUT => GregorianCalendar.Instance;

    [Theory, MemberData(nameof(YearInfoData))]
    public void ToRange(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var start = CalendarUT.GetOrdinalDate(y, 1);
        var end = CalendarUT.GetOrdinalDate(y, info.IsLeap ? 366 : 365);
        // Act
        var range = year.ToRange();
        // Assert
        Assert.Equal(start, range.Min);
        Assert.Equal(end, range.Max);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void Count(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        // Act
        var range = year.ToRange();
        // Assert
        Assert.Equal(info.DaysInYear, range.Count());
    }

    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var year = CalendarUT.GetCalendarYear(3);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => year.ToRange().WithCalendar(null!));
    }

    [Fact]
    public void WithCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1970);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 365);
        // Act
        var actual = DateRange.FromYear(year).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
    }

    [Fact]
    public void GetDaysInYear()
    {
        IEnumerable<OrdinalDate> exp =
            from i in Enumerable.Range(1, 365)
            select CalendarUT.GetOrdinalDate(3, i);
        // Act
        var actual = CalendarUT.GetCalendarYear(3).GetDaysInYear();
        // Assert
        Assert.Equal(exp, actual);
    }

    #region Contains()

    [Theory]
    [InlineData(2, 12, 31, false)]
    [InlineData(3, 1, 1, true)]
    [InlineData(3, 5, 10, true)]
    [InlineData(3, 12, 31, true)]
    [InlineData(4, 1, 1, false)]
    public void Contains_Date(int y, int m, int d, bool inRange)
    {
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(date));
    }

    [Theory]
    [InlineData(2, 12, false)]
    [InlineData(3, 1, true)]
    [InlineData(3, 2, true)]
    [InlineData(3, 3, true)]
    [InlineData(3, 4, true)]
    [InlineData(3, 5, true)]
    [InlineData(3, 6, true)]
    [InlineData(3, 7, true)]
    [InlineData(3, 8, true)]
    [InlineData(3, 9, true)]
    [InlineData(3, 10, true)]
    [InlineData(3, 11, true)]
    [InlineData(3, 12, true)]
    [InlineData(4, 1, false)]
    public void Contains_Month(int y, int m, bool inRange)
    {
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(month));
    }

    [Theory]
    [InlineData(2, 12, false)]
    [InlineData(3, 1, true)]
    [InlineData(3, 2, true)]
    [InlineData(3, 3, true)]
    [InlineData(3, 4, true)]
    [InlineData(3, 5, true)]
    [InlineData(3, 6, true)]
    [InlineData(3, 7, true)]
    [InlineData(3, 8, true)]
    [InlineData(3, 9, true)]
    [InlineData(3, 10, true)]
    [InlineData(3, 11, true)]
    [InlineData(3, 12, true)]
    [InlineData(4, 1, false)]
    public void IsSupersetOf_MonthRange(int y, int m, bool inRange)
    {
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var other = DateRange.FromMonth(CalendarUT.GetCalendarMonth(y, m));
        // Act & Assert
        Assert.Equal(inRange, range.IsSupersetOf(other));
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void Contains_Year(int y, bool inRange)
    {
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var year1 = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(year1));
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void IsSupersetOf_YearRange(int y, bool inRange)
    {
        var range = DateRange.FromYear(CalendarUT.GetCalendarYear(3));
        var other = DateRange.FromYear(CalendarUT.GetCalendarYear(y));
        // Act & Assert
        Assert.Equal(inRange, range.IsSupersetOf(other));
    }

    // start, end, inRange
    public static TheoryData<Yemoda, Yemoda, bool> Start_End_InRange { get; } = new()
    {
        { new(1, 1, 1), new(2, 12, 31), false },    // Avant
        { new(2, 12, 31), new(3, 1, 1), false },    // À cheval
        { new(2, 12, 31), new(3, 5, 1), false },    // À cheval
        { new(3, 1, 1), new(3, 1, 1), true },       // OK
        { new(3, 2, 1), new(3, 2, 25), true },      // OK
        { new(3, 2, 1), new(3, 12, 31), true },     // OK
        { new(3, 2, 1), new(4, 1, 1), false },      // À cheval
        { new(3, 12, 31), new(4, 1, 1), false },    // À cheval
        { new(4, 1, 1), new(4, 1, 1), false },      // Après
        { new(4, 5, 2), new(4, 6, 1), false },      // Après
    };

    #endregion
}

public sealed class DateRangeMonthTests : CalendarDataConsumer<ProlepticGregorianDataSet>
{
    private static readonly JulianCalendar s_Julian = JulianCalendar.Instance;

    private static GregorianCalendar CalendarUT => GregorianCalendar.Instance;

    [Theory, MemberData(nameof(MonthInfoData))]
    public void FromMonth(MonthInfo info)
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

    [Fact]
    public void WithCalendar_InvalidCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(3, 4);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => DateRange.FromMonth(month).WithCalendar(null!));
    }

    [Fact]
    public void WithCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(1970, 1);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRange.Create(date, 31);
        // Act
        var actual = DateRange.FromMonth(month).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
    }

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
}
