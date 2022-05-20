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
}

public partial class CalendarDateFacts<TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Deconstructor(DateInfo info)
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

    //
    // Properties
    //

    [Fact]
    public void Calendar_Prop()
    {
        var date = CalendarUT.GetCalendarDate(1, 1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarYear_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var exp = CalendarUT.GetCalendarYear(y);
        // Act & Assert
        Assert.Equal(exp, date.CalendarYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CalendarMonth_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(exp, date.CalendarMonth);
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
    public void WithCalendar_Invariant(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        var date = CalendarUT.GetCalendarDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(date, date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        if (OtherCalendar.Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetCalendarDateOn(dayNumber);
        var other = OtherCalendar.GetCalendarDateOn(dayNumber);
        // Act & Assert
        Assert.Equal(other, date.WithCalendar(OtherCalendar));
        Assert.Equal(date, other.WithCalendar(CalendarUT));
    }
}
