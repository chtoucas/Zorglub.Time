// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Core;
using Zorglub.Time.Simple;

[Obsolete("DateRange is obsolete.")]
public sealed class GregorianDateRangeTests : DateRangeFacts<ProlepticGregorianDataSet>
{
    public GregorianDateRangeTests()
        : base(SimpleGregorian.Instance, SimpleJulian.Instance) { }

    [Fact]
    public void WithCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1970, 1, 1);
        var range = DateRangeV0.Create(date, 2);
        // Act
        var actual = range.WithCalendar(OtherCalendar);
        // Assert
        Assert.Equal(OtherCalendar.GetCalendarDate(1969, 12, 19), actual.Start);
        Assert.Equal(OtherCalendar.GetCalendarDate(1969, 12, 20), actual.End);
        Assert.Equal(OtherCalendar.Id, actual.Calendar.Id);
    }
}

[Obsolete("DateRange is obsolete.")]
public sealed class JulianDateRangeTests : DateRangeFacts<ProlepticJulianDataSet>
{
    public JulianDateRangeTests()
        : base(SimpleJulian.Instance, SimpleGregorian.Instance) { }

    [Fact]
    public void WithCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1969, 12, 19);
        var range = DateRangeV0.Create(date, 2);
        // Act
        var actual = range.WithCalendar(OtherCalendar);
        // Assert
        Assert.Equal(OtherCalendar.GetCalendarDate(1970, 1, 1), actual.Start);
        Assert.Equal(OtherCalendar.GetCalendarDate(1970, 1, 2), actual.End);
        Assert.Equal(OtherCalendar.Id, actual.Calendar.Id);
    }
}

[Obsolete("DateRange is obsolete.")]
public sealed class DateRangeYearTests : CalendarDataConsumer<ProlepticGregorianDataSet>
{
    private static readonly SimpleJulian s_Julian = SimpleJulian.Instance;

    private static SimpleGregorian CalendarUT => SimpleGregorian.Instance;

    [Fact]
    public void WithCalendar()
    {
        var year = CalendarUT.GetCalendarYear(1970);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRangeV0.Create(date, 365);
        // Act
        var actual = DateRangeV0.FromYear(year).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
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
        var range = DateRangeV0.FromYear(CalendarUT.GetCalendarYear(3));
        var other = DateRangeV0.FromMonth(CalendarUT.GetCalendarMonth(y, m));
        // Act & Assert
        Assert.Equal(inRange, range.IsSupersetOf(other));
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void IsSupersetOf_YearRange(int y, bool inRange)
    {
        var range = DateRangeV0.FromYear(CalendarUT.GetCalendarYear(3));
        var other = DateRangeV0.FromYear(CalendarUT.GetCalendarYear(y));
        // Act & Assert
        Assert.Equal(inRange, range.IsSupersetOf(other));
    }

    [Theory]
    [InlineData(2, 12, 31, false)]
    [InlineData(3, 1, 1, true)]
    [InlineData(3, 5, 10, true)]
    [InlineData(3, 12, 31, true)]
    [InlineData(4, 1, 1, false)]
    public void Contains_Date(int y, int m, int d, bool inRange)
    {
        var range = DateRangeV0.FromYear(CalendarUT.GetCalendarYear(3));
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
        var range = DateRangeV0.FromYear(CalendarUT.GetCalendarYear(3));
        var month = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(month));
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void Contains_Year(int y, bool inRange)
    {
        var range = DateRangeV0.FromYear(CalendarUT.GetCalendarYear(3));
        var year1 = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(year1));
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
}

[Obsolete("DateRange is obsolete.")]
public sealed class DateRangeMonthTests : CalendarDataConsumer<ProlepticGregorianDataSet>
{
    private static readonly SimpleJulian s_Julian = SimpleJulian.Instance;

    private static SimpleGregorian CalendarUT => SimpleGregorian.Instance;

    [Fact]
    public void WithCalendar()
    {
        var month = CalendarUT.GetCalendarMonth(1970, 1);
        var date = s_Julian.GetCalendarDate(1969, 12, 19);
        var range = DateRangeV0.Create(date, 31);
        // Act
        var actual = DateRangeV0.FromMonth(month).WithCalendar(s_Julian);
        // Assert
        Assert.Equal(range, actual);
    }

    [Fact]
    public void IsSupersetOf_RangeA()
    {
        var range1 = DateRangeV0.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRangeV0.FromMonth(CalendarUT.GetCalendarMonth(3, 5));
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
    }

    [Fact]
    public void IsSupersetOf_RangeB()
    {
        var range1 = DateRangeV0.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRangeV0.FromMonth(CalendarUT.GetCalendarMonth(4, 4));
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
    }

    [Fact]
    public void IsSupersetOf_RangeC()
    {
        var range1 = DateRangeV0.FromMonth(CalendarUT.GetCalendarMonth(3, 4));
        var range2 = DateRangeV0.Create(CalendarUT.GetCalendarDate(3, 4, 2), 2);
        // Act & Assert
        Assert.True(range1.IsSupersetOf(range2));
    }

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
        var range = DateRangeV0.FromMonth(month);
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
        var range = DateRangeV0.FromMonth(month);
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
        var range = DateRangeV0.FromMonth(month);
        var year = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.False(range.Contains(year));
    }
}
