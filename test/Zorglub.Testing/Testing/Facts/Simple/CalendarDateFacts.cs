// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDate"/>.
/// </summary>
public abstract partial class CalendarDateFacts<TDataSet> : SimpleDateFacts<CalendarDate, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IAdvancedMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected CalendarDateFacts(Calendar calendar, Calendar otherCalendar!!)
        : base(calendar, otherCalendar)
    {
        (MinDate, MaxDate) = calendar.MinMaxDate;
    }

    protected sealed override CalendarDate MinDate { get; }
    protected sealed override CalendarDate MaxDate { get; }

    protected sealed override CalendarDate CreateDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d);
}

public partial class CalendarDateFacts<TDataSet>
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
}

public partial class CalendarDateFacts<TDataSet> // Properties
{
    // We also test the internal prop Cuid.
    [Theory, MemberData(nameof(DateInfoData))]
    public void Calendar_Prop(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        Assert.Equal(CalendarUT.Id, date.Cuid);
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
        var date = CalendarUT.GetCalendarDate(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAnexn("newCalendar", () => date.WithCalendar(null!));
    }
}

public partial class CalendarDateFacts<TDataSet> // Math ops
{
    [Fact]
    public void CountDaysSince_InvalidDate()
    {
        var date = CalendarUT.GetCalendarDate(3, 4, 5);
        var other = OtherCalendar.GetCalendarDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountDaysSince(other));
        Assert.Throws<ArgumentException>("other", () => date - other);
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(Yemoda xstart, Yemoda xend, int months)
    {
        var start = new CalendarDate(xstart, CalendarUT.Id);
        var end = new CalendarDate(xend, CalendarUT.Id);
        // Act & Assert
        Assert.Equal(end, start.PlusMonths(months));
    }

    [Fact]
    public void CountMonthsSince_InvalidDate()
    {
        var date = CalendarUT.GetCalendarDate(3, 4, 5);
        var other = OtherCalendar.GetCalendarDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountMonthsSince(other));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void CountMonthsSince(Yemoda xstart, Yemoda xend, int months)
    {
        var start = new CalendarDate(xstart, CalendarUT.Id);
        var end = new CalendarDate(xend, CalendarUT.Id);
        // Act & Assert
        Assert.Equal(months, end.CountMonthsSince(start));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(Yemoda xstart, Yemoda xend, int years)
    {
        var start = new CalendarDate(xstart, CalendarUT.Id);
        var end = new CalendarDate(xend, CalendarUT.Id);
        // Act & Assert
        Assert.Equal(end, start.PlusYears(years));
    }

    [Fact]
    public void CountYearsSince_InvalidDate()
    {
        var date = CalendarUT.GetCalendarDate(3, 4, 5);
        var other = OtherCalendar.GetCalendarDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date.CountYearsSince(other));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void CountYearsSince(Yemoda xstart, Yemoda xend, int years)
    {
        var start = new CalendarDate(xstart, CalendarUT.Id);
        var end = new CalendarDate(xend, CalendarUT.Id);
        // Act & Assert
        Assert.Equal(years, end.CountYearsSince(start));
    }
}

public partial class CalendarDateFacts<TDataSet> // IEquatable
{
    [Fact]
    public void Equals_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDate(3, 4, 5);
        var other = OtherCalendar.GetCalendarDate(3, 4, 5);
        // Act & Assert
        Assert.False(date == other);
        Assert.True(date != other);

        Assert.False(date.Equals(other));
        Assert.False(date.Equals((object)other));
    }
}

public partial class CalendarDateFacts<TDataSet> // IComparable
{
    [Fact]
    public void CompareTo_OtherCalendar()
    {
        var date = CalendarUT.GetCalendarDate(3, 4, 5);
        var other = OtherCalendar.GetCalendarDate(3, 4, 5);
        // Act & Assert
        Assert.Throws<ArgumentException>("other", () => date > other);
        Assert.Throws<ArgumentException>("other", () => date >= other);
        Assert.Throws<ArgumentException>("other", () => date < other);
        Assert.Throws<ArgumentException>("other", () => date <= other);

        Assert.Throws<ArgumentException>("other", () => date.CompareTo(other));
    }
}
