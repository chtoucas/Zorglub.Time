// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDay"/>.
/// </summary>
public abstract partial class CalendarDayFacts<TDataSet> :
    SimpleDateFacts<CalendarDay, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDayFacts(SimpleCalendar calendar, SimpleCalendar otherCalendar)
        : base(calendar, otherCalendar)
    {
        Debug.Assert(calendar != null);

        (MinDate, MaxDate) = calendar.MinMaxDay;
        DomainTester = new DomainTester(calendar.Domain);
    }

    protected sealed override CalendarDay MinDate { get; }
    protected sealed override CalendarDay MaxDate { get; }

    protected DomainTester DomainTester { get; }

    protected sealed override CalendarDay GetDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();

    protected sealed override CalendarDay GetDate(int y, int doy) =>
        CalendarUT.GetOrdinalDate(y, doy).ToCalendarDay();
}

public partial class CalendarDayFacts<TDataSet> // Prelude
{
    //[Theory, MemberData(nameof(DayNumberInfoData))]
    //public void Deconstructor_DayNumber(DayNumberInfo info)
    //{
    //    var (dayNumber, y, m, d) = info;
    //    var date = CalendarUT.GetCalendarDay(dayNumber);
    //    // Act
    //    var (year, month, day) = date;
    //    // Assert
    //    Assert.Equal(y, year);
    //    Assert.Equal(m, month);
    //    Assert.Equal(d, day);
    //}

    //[Theory, MemberData(nameof(DayNumberInfoData))]
    //public void Deconstructor﹍Ordinal_DayNumber(DayNumberInfo info)
    //{
    //    int y = info.Yemoda.Year;
    //    var date = CalendarUT.GetCalendarDay(info.DayNumber);
    //    // Act
    //    var (year, dayOfYear) = date;
    //    // Assert
    //    Assert.Equal(y, year);
    //    Assert.Equal(date.DayOfYear, dayOfYear);
    //}

    [Fact]
    public void Cuid_Prop()
    {
        var date = CalendarUT.GetCalendarDay(CalendarUT.Epoch);
        // Act & Assert
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class CalendarDayFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDay(CalendarUT.Epoch);
        var other = OtherCalendar.GetCalendarDay(CalendarUT.Epoch);
        // Act & Assert
        Assert.False(date == other);
        Assert.True(date != other);

        Assert.False(date.Equals(other));
        Assert.False(date.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDay(CalendarUT.Epoch);
        var other = OtherCalendar.GetCalendarDay(CalendarUT.Epoch);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date > other);
        Assert.Throws<ArgumentException>("other", () => date >= other);
        Assert.Throws<ArgumentException>("other", () => date < other);
        Assert.Throws<ArgumentException>("other", () => date <= other);

        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }

    [Fact]
    public void CountDaysSince_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDay(CalendarUT.Epoch);
        var other = OtherCalendar.GetCalendarDay(CalendarUT.Epoch);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountDaysSince(other));
        Assert.Throws<ArgumentException>("other", () => date - other);
    }
}

public partial class CalendarDayFacts<TDataSet> // Conversions
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToCalendarDay(DayNumberInfo info)
    {
        var date = CalendarUT.GetCalendarDay(info.DayNumber);
        // Act & Assert
        Assert.Equal(date, ((ISimpleDate)date).ToCalendarDay());
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToCalendarDate(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = CalendarUT.GetCalendarDay(dayNumber);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, date.ToCalendarDate());
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToOrdinalDate(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = CalendarUT.GetCalendarDay(dayNumber);
        var exp = CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(exp, date.ToOrdinalDate());
    }

    [Fact]
    public void WithCalendar_NullCalendar()
    {
        var date = CalendarUT.GetCalendarDay(CalendarUT.Epoch);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar_Invariance(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        var date = CalendarUT.GetCalendarDay(dayNumber);
        // Act & Assert
        Assert.Equal(date, date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        if (OtherCalendar.Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetCalendarDay(dayNumber);
        var other = OtherCalendar.GetCalendarDay(dayNumber);
        // Act & Assert
        Assert.Equal(other, date.WithCalendar(OtherCalendar));
        Assert.Equal(date, other.WithCalendar(CalendarUT));
    }
}

//
// Tests for related classes.
//

public partial class CalendarDayFacts<TDataSet> // CalendarDayProvider
{
    private static readonly CalendarDayProvider s_Provider = new();

    //
    // CalendarYear
    //

    [Theory, MemberData(nameof(YearInfoData))]
    public void CalendarDayProvider_GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1).ToCalendarDay();
        // Act & Assert
        Assert.Equal(startOfYear, s_Provider.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarDayProvider_GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
        // Act & Assert
        Assert.Equal(exp, s_Provider.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CalendarDayProvider_GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDay();
        // Act & Assert
        Assert.Equal(endOfYear, s_Provider.GetEndOfYear(year));
    }

    //
    // CalendarMonth
    //

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CalendarDayProvider_GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToCalendarDay();
        // Act & Assert
        Assert.Equal(startOfMonth, s_Provider.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarDayProvider_GetDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
        // Act & Assert
        Assert.Equal(date, s_Provider.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CalendarDayProvider_GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToCalendarDay();
        // Act & Assert
        Assert.Equal(endOfMonth, s_Provider.GetEndOfMonth(month));
    }
}
