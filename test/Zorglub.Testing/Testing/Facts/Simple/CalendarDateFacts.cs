// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDate"/>.
/// </summary>
public abstract partial class CalendarDateFacts<TDataSet> :
    SimpleDateFacts<CalendarDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateFacts(Calendar calendar, Calendar otherCalendar)
        : base(calendar, otherCalendar)
    {
        Debug.Assert(calendar != null);

        (MinDate, MaxDate) = calendar.MinMaxDate;
    }

    protected sealed override CalendarDate MinDate { get; }
    protected sealed override CalendarDate MaxDate { get; }

    protected sealed override CalendarDate GetDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d);

    protected sealed override CalendarDate GetDate(int y, int doy) =>
        CalendarUT.GetOrdinalDate(y, doy).ToCalendarDate();
}

public partial class CalendarDateFacts<TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor﹍Date(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor﹍Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act
        var (year, dayOfYear) = date;
        // Assert
        Assert.Equal(y, year);
        Assert.Equal(doy, dayOfYear);
    }

    //
    // Properties
    //

    [Fact]
    public void Cuid_Prop()
    {
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class CalendarDateFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        var other = OtherCalendar.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.False(date == other);
        Assert.True(date != other);

        Assert.False(date.Equals(other));
        Assert.False(date.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        var other = OtherCalendar.GetCalendarDate(1, 1, 1);
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
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        var other = OtherCalendar.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountDaysSince(other));
        Assert.Throws<ArgumentException>("other", () => date - other);
    }

    [Fact]
    public void CountMonthsSince_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        var other = OtherCalendar.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountMonthsSince(other));
    }

    [Fact]
    public void CountYearsSince_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        var other = OtherCalendar.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountYearsSince(other));
    }
}

public partial class CalendarDateFacts<TDataSet> // Conversions
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToCalendarDay(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var exp = CalendarUT.GetCalendarDay(dayNumber);
        // Act & Assert
        Assert.Equal(exp, date.ToCalendarDay());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToCalendarDate(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, ((ISimpleDate)date).ToCalendarDate());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToOrdinalDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var exp = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, date.ToOrdinalDate());
    }

    [Fact]
    public void WithCalendar_NullCalendar()
    {
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar_Invariance(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        var date = CalendarUT.GetCalendarDate(dayNumber);
        // Act & Assert
        Assert.Equal(date, date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        if (OtherCalendar.Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetCalendarDate(dayNumber);
        var other = OtherCalendar.GetCalendarDate(dayNumber);
        // Act & Assert
        Assert.Equal(other, date.WithCalendar(OtherCalendar));
        Assert.Equal(date, other.WithCalendar(CalendarUT));
    }
}

public partial class CalendarDateFacts<TDataSet> // Comparison
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void CompareFast_WhenEqual(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var left = CalendarUT.GetCalendarDate(y, m, d);
        var right = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, left.CompareFast(right));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void CompareFast_WhenNotEqual(int y, int m, int d)
    {
        var left = CalendarUT.GetCalendarDate(1, 1, 1);
        var right = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.True(left.CompareFast(right) < 0);
    }
}

//
// Tests for related classes
//

public partial class CalendarDateFacts<TDataSet> // DateAdjusters
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void DateAdjusters_GetStartOfYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, DateAdjusters.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void DateAdjusters_GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act
        var endOfYear = DateAdjusters.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void DateAdjusters_GetStartOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, DateAdjusters.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void DateAdjusters_GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CalendarUT.GetCalendarDate(y, m, 1);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, DateAdjusters.GetEndOfMonth(date));
    }
}

public partial class CalendarDateFacts<TDataSet> // CalendarDateProvider
{
    private static readonly IDateProvider<CalendarDate> s_Provider = new CalendarDateProvider();

    //
    // CalendarYear
    //

    [Theory, MemberData(nameof(YearInfoData))]
    public void CalendarDateProvider_GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, s_Provider.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarDateProvider_GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, s_Provider.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CalendarDateProvider_GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDate();
        // Act & Assert
        Assert.Equal(endOfYear, s_Provider.GetEndOfYear(year));
    }
}
