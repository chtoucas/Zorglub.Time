// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

using static Zorglub.Time.Extensions.SimpleInterconversions;

/// <summary>
/// Provides facts about <see cref="CalendarDate"/>.
/// </summary>
public abstract partial class CalendarDateFacts<TDataSet> :
    SimpleDateFacts<CalendarDate, TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateFacts(SimpleCalendar calendar, SimpleCalendar otherCalendar)
        : base(calendar, otherCalendar)
    {
        Debug.Assert(calendar != null);

        (MinDate, MaxDate) = calendar.MinMaxDate;
    }

    protected sealed override CalendarDate MinDate { get; }
    protected sealed override CalendarDate MaxDate { get; }

    protected sealed override CalendarDate GetDate(int y, int m, int d) =>
        CalendarUT.GetDate(y, m, d);

    protected sealed override CalendarDate GetDate(int y, int doy) =>
        CalendarUT.GetDate(y, doy).ToCalendarDate();
}

public partial class CalendarDateFacts<TDataSet> // Prelude
{
    [Fact]
    public void Cuid_Prop()
    {
        var date = CalendarUT.GetDate(1, 1, 1);
        // Act & Assert
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class CalendarDateFacts<TDataSet> // Calendar mismatch
{
    [Fact]
    public void Equality_OtherCalendar()
    {
        var date = CalendarUT.GetDate(1, 1, 1);
        var other = OtherCalendar.GetDate(1, 1, 1);
        // Act & Assert
        Assert.False(date == other);
        Assert.True(date != other);

        Assert.False(date.Equals(other));
        Assert.False(date.Equals((object)other));
    }

    [Fact]
    public void Comparison_OtherCalendar()
    {
        var date = CalendarUT.GetDate(1, 1, 1);
        var other = OtherCalendar.GetDate(1, 1, 1);
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
        var date = CalendarUT.GetDate(1, 1, 1);
        var other = OtherCalendar.GetDate(1, 1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountDaysSince(other));
        Assert.Throws<ArgumentException>("other", () => date - other);
    }

    [Fact]
    public void CountMonthsSince_OtherCalendar()
    {
        var date = CalendarUT.GetDate(1, 1, 1);
        var other = OtherCalendar.GetDate(1, 1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountMonthsSince(other));
    }

    [Fact]
    public void CountYearsSince_OtherCalendar()
    {
        var date = CalendarUT.GetDate(1, 1, 1);
        var other = OtherCalendar.GetDate(1, 1, 1);
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
        var date = CalendarUT.GetDate(y, m, d);
        var exp = CalendarUT.GetDate(dayNumber);
        // Act & Assert
        Assert.Equal(exp, date.ToCalendarDay());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToCalendarDate(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, ((ISimpleDate)date).ToCalendarDate());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ToOrdinalDate(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var date = CalendarUT.GetDate(y, m, d);
        var exp = CalendarUT.GetDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, date.ToOrdinalDate());
    }

    [Fact]
    public void WithCalendar_NullCalendar()
    {
        var date = CalendarUT.GetDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar_Invariance(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        var date = CalendarUT.GetDate(dayNumber);
        // Act & Assert
        Assert.Equal(date, date.WithCalendar(CalendarUT));
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void WithCalendar(DayNumberInfo info)
    {
        var dayNumber = info.DayNumber;
        if (OtherCalendar.Domain.Contains(dayNumber) == false) { return; }
        var date = CalendarUT.GetDate(dayNumber);
        var other = OtherCalendar.GetDate(dayNumber);
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
        var left = CalendarUT.GetDate(y, m, d);
        var right = CalendarUT.GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(0, left.CompareFast(right));
    }

    [Theory]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    public void CompareFast_WhenNotEqual(int y, int m, int d)
    {
        var left = CalendarUT.GetDate(1, 1, 1);
        var right = CalendarUT.GetDate(y, m, d);
        // Act & Assert
        Assert.True(left.CompareFast(right) < 0);
    }
}
