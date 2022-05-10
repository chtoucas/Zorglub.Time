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
        IDaysAfterDataSet,
        IAdvancedMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected CalendarDayFacts(Calendar calendar, Calendar otherCalendar!!)
        : base(calendar, otherCalendar)
    {
        (MinDate, MaxDate) = calendar.MinMaxDay;
    }

    protected sealed override CalendarDay MinDate { get; }
    protected sealed override CalendarDay MaxDate { get; }

    protected sealed override CalendarDay CreateDate(int y, int m, int d) =>
        // Base tests are indirect.
        CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
}

public partial class CalendarDayFacts<TDataSet>
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
}

public partial class CalendarDayFacts<TDataSet> // Properties
{
    // We also test the internal prop Cuid.
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Calendar_Prop(DayNumberInfo info)
    {
        var date = CalendarUT.GetCalendarDay(info.DayNumber);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        Assert.Equal(CalendarUT.Id, date.Cuid);
    }
}

public partial class CalendarDayFacts<TDataSet> // Conversions
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToCalendarDay(DayNumberInfo info)
    {
        var day = CalendarUT.GetCalendarDay(info.DayNumber);
        // Act & Assert
        Assert.Equal(day, ((ISimpleDate)day).ToCalendarDay());
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToCalendarDate(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var day = CalendarUT.GetCalendarDay(dayNumber);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, day.ToCalendarDate());
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToOrdinalDate(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        var day = CalendarUT.GetCalendarDay(dayNumber);
        var exp = CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(exp, day.ToOrdinalDate());
    }

    //public void WithCalendar_NullCalendar()
}
