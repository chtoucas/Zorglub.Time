// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="OrdinalDate"/>.
/// </summary>
public abstract partial class OrdinalDateFacts<TDataSet> :
    SimpleDateFacts<OrdinalDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateFacts(SimpleCalendar calendar, SimpleCalendar otherCalendar)
        : base(calendar, otherCalendar)
    {
        Debug.Assert(calendar != null);

        (MinDate, MaxDate) = calendar.MinMaxOrdinal;
    }

    protected sealed override OrdinalDate MinDate { get; }
    protected sealed override OrdinalDate MaxDate { get; }

    protected sealed override OrdinalDate GetDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();

    protected sealed override OrdinalDate GetDate(int y, int doy) =>
        CalendarUT.GetOrdinalDate(y, doy);
}

public partial class OrdinalDateFacts<TDataSet> // Prelude
{
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Deconstructor_Ordinal(DateInfo info)
    //{
    //    var (y, m, d, doy) = info;
    //    var date = CalendarUT.GetOrdinalDate(y, doy);
    //    // Act
    //    var (year, month, day) = date;
    //    // Assert
    //    Assert.Equal(y, year);
    //    Assert.Equal(m, month);
    //    Assert.Equal(d, day);
    //}

    [Fact]
    public void Cuid_Prop()
    {
        var date = CalendarUT.GetOrdinalDate(1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class OrdinalDateFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var date = CalendarUT.GetOrdinalDate(1, 1);
        var other = OtherCalendar.GetOrdinalDate(1, 1);
        // Act & Assert
        Assert.False(date == other);
        Assert.True(date != other);

        Assert.False(date.Equals(other));
        Assert.False(date.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var date = CalendarUT.GetOrdinalDate(1, 1);
        var other = OtherCalendar.GetOrdinalDate(1, 1);
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
        var date = CalendarUT.GetOrdinalDate(1, 1);
        var other = OtherCalendar.GetOrdinalDate(1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountDaysSince(other));
        Assert.Throws<ArgumentException>("other", () => date - other);
    }

    [Fact]
    public void CountYearsSince_OtherCalendar()
    {
        var date = CalendarUT.GetOrdinalDate(1, 1);
        var other = OtherCalendar.GetOrdinalDate(1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountYearsSince(other));
    }
}

public partial class OrdinalDateFacts<TDataSet> // Conversions
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToCalendarDay(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var date = CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
        var exp = CalendarUT.GetCalendarDay(dayNumber);
        // Act & Assert
        Assert.Equal(exp, date.ToCalendarDay());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToCalendarDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, date.ToCalendarDate());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToOrdinalDate(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, ((ISimpleDate)date).ToOrdinalDate());
    }

    [Fact]
    public void WithCalendar_NullCalendar()
    {
        var date = CalendarUT.GetOrdinalDate(1, 1);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar_Invariance(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        var date = CalendarUT.GetOrdinalDate(dayNumber);
        // Act & Assert
        Assert.Equal(date, date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        if (OtherCalendar.Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetOrdinalDate(dayNumber);
        var other = OtherCalendar.GetOrdinalDate(dayNumber);
        // Act & Assert
        Assert.Equal(other, date.WithCalendar(OtherCalendar));
        Assert.Equal(date, other.WithCalendar(CalendarUT));
    }
}

public partial class OrdinalDateFacts<TDataSet> // Comparison
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void CompareFast_WhenEqual(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var left = CalendarUT.GetOrdinalDate(y, doy);
        var right = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(0, left.CompareFast(right));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void CompareFast_WhenNotEqual(int y, int doy)
    {
        var left = CalendarUT.GetOrdinalDate(1, 1);
        var right = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.True(left.CompareFast(right) < 0);
    }
}

//
// Tests for related classes
//

public partial class OrdinalDateFacts<TDataSet> // OrdinalDateAdjusters
{
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void DateAdjusters_GetStartOfYear(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = CalendarUT.GetOrdinalDate(y, doy);
    //    var startOfYear = CalendarUT.GetOrdinalDate(y, 1);
    //    // Act & Assert
    //    Assert.Equal(startOfYear, OrdinalDateAdjusters.GetStartOfYear(date));
    //}

    //[Theory, MemberData(nameof(YearInfoData))]
    //public void DateAdjusters_GetEndOfYear(YearInfo info)
    //{
    //    int y = info.Year;
    //    var date = CalendarUT.GetOrdinalDate(y, 1);
    //    // Act
    //    var endOfYear = OrdinalDateAdjusters.GetEndOfYear(date);
    //    // Assert
    //    Assert.Equal(y, endOfYear.Year);
    //    Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void DateAdjusters_GetStartOfMonth(DateInfo info)
    //{
    //    var (y, m, _, doy) = info;
    //    var date = CalendarUT.GetOrdinalDate(y, doy);
    //    var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
    //    // Act & Assert
    //    Assert.Equal(startOfMonth, OrdinalDateAdjusters.GetStartOfMonth(date));
    //}

    //[Theory, MemberData(nameof(MonthInfoData))]
    //public void DateAdjusters_GetEndOfMonth(MonthInfo info)
    //{
    //    var (y, m) = info.Yemo;
    //    var date = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
    //    var endOfMonth = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth).ToOrdinalDate();
    //    // Act & Assert
    //    Assert.Equal(endOfMonth, OrdinalDateAdjusters.GetEndOfMonth(date));
    //}
}

public partial class OrdinalDateFacts<TDataSet> // OrdinalDateProvider
{
    private static readonly IDateProvider<OrdinalDate> s_Provider = new OrdinalDateProvider();

    //
    // CalendarMonth
    //

    [Theory, MemberData(nameof(MonthInfoData))]
    public void OrdinalDateProvider_GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(startOfMonth, s_Provider.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void OrdinalDateProvider_GetDayOfMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, s_Provider.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void OrdinalDateProvider_GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(endOfMonth, s_Provider.GetEndOfMonth(month));
    }
}