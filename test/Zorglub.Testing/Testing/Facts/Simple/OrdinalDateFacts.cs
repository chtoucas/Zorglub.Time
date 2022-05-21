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
    protected OrdinalDateFacts(Calendar calendar, Calendar otherCalendar)
        : base(calendar, otherCalendar)
    {
        Debug.Assert(calendar != null);

        (MinDate, MaxDate) = calendar.MinMaxOrdinal;
    }

    protected sealed override OrdinalDate MinDate { get; }
    protected sealed override OrdinalDate MaxDate { get; }

    protected sealed override OrdinalDate GetDate(int y, int m, int d) =>
        // Notice that to create a date we must first pass thru CalendarDate.
        CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
}

public partial class OrdinalDateFacts<TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = CalendarUT.GetOrdinalDate(y, doy);
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
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
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
    public void Calendar_Prop()
    {
        var date = CalendarUT.GetOrdinalDate(1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarYear_Prop(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        var exp = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(exp, date.CalendarYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarMonth_Prop(DateInfo info)
    {
        var (y, m, _, doy) = info;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(exp, date.CalendarMonth);
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

public partial class OrdinalDateFacts<TDataSet> // Factories
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void AtStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetOrdinalDate(y, 1);
        // Act & Assert
        Assert.Equal(startOfYear, OrdinalDate.AtStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, OrdinalDate.AtDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void AtEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear);
        // Act & Assert
        Assert.Equal(endOfYear, OrdinalDate.AtEndOfYear(year));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToCalendarDay();
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarDay.AtStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, OrdinalDate.AtDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(endOfMonth, OrdinalDate.AtEndOfMonth(month));
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
    public void WithCalendar_Invariant(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        var date = CalendarUT.GetOrdinalDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(date, date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        if (OtherCalendar.Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetOrdinalDateOn(dayNumber);
        var other = OtherCalendar.GetOrdinalDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(other, date.WithCalendar(OtherCalendar));
        Assert.Equal(date, other.WithCalendar(CalendarUT));
    }
}

public partial class OrdinalDateFacts<TDataSet> // Adjustments
{
    [Fact]
    public void WithYear_InvalidYears()
    {
        var date = CalendarUT.GetOrdinalDate(1, 1);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(y => date.WithYear(y), "newYear");
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_Invariant(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, date.WithYear(y));
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = CalendarUT.GetOrdinalDate(1, 1);
            var exp = CalendarUT.GetOrdinalDate(y, 1);
            // Act & Assert
            Assert.Equal(exp, date.WithYear(y));
        }
    }
}
