// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDay"/>.
/// </summary>
public abstract partial class CalendarDayFacts<TDataSet> : SimpleDateFacts<CalendarDay, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected CalendarDayFacts(Calendar calendar, Calendar otherCalendar)
        : base(calendar, otherCalendar)
    {
        Debug.Assert(calendar != null);

        (MinDate, MaxDate) = calendar.MinMaxDay;
    }

    protected sealed override CalendarDay MinDate { get; }
    protected sealed override CalendarDay MaxDate { get; }

    protected sealed override CalendarDay GetDate(int y, int m, int d) =>
        // Notice that to create a date we must first pass thru CalendarDate.
        CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
}

public partial class CalendarDayFacts<TDataSet> // Prelude
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Deconstructor(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDay(info.DayNumber);
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
        var date = CalendarUT.GetCalendarDay(CalendarUT.Epoch);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        // We also test the internal prop Cuid.
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void CalendarYear_Prop(DayNumberInfo info)
    {
        var date = CalendarUT.GetCalendarDay(info.DayNumber);
        var exp = CalendarUT.GetCalendarYear(info.Year);
        // Act & Assert
        Assert.Equal(exp, date.CalendarYear);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void CalendarMonth_Prop(DayNumberInfo info)
    {
        var (dayNumber, y, m, _) = info;
        var date = CalendarUT.GetCalendarDay(dayNumber);
        var exp = CalendarUT.GetCalendarMonth(y, m);
        // Act & Assert
        Assert.Equal(exp, date.CalendarMonth);
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
    public void WithCalendar_Invariant(DayNumberInfo info)
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
