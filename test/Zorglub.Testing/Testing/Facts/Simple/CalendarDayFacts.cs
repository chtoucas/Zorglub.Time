// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// Tests indirects : OrdinalDate utilise la repr DayNumber mais ici on en
// passe systématiquement par CreateDate(y, m, d).

/// <summary>
/// Provides facts about <see cref="CalendarDay"/>.
/// </summary>
[TestExcludeFrom(TestExcludeFrom.Smoke)] // Indirect tests
public abstract partial class CalendarDayFacts<TDataSet> : IDateFacts<CalendarDay, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected CalendarDayFacts(Calendar calendar) : this(calendar, CreateCtorArgs(calendar)) { }

    private CalendarDayFacts(Calendar calendar, CtorArgs args) : base(args)
    {
        CalendarUT = calendar;

        (MinDate, MaxDate) = calendar.MinMaxDay;
    }

    protected Calendar CalendarUT { get; }

    protected sealed override CalendarDay MinDate { get; }
    protected sealed override CalendarDay MaxDate { get; }

    protected sealed override CalendarDay CreateDate(int y, int m, int d) =>
        CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
}

public partial class CalendarDayFacts<TDataSet>
{
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Deconstructor(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Arrange
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
        // Arrange
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
        // Arrange
        var day = CalendarUT.GetCalendarDay(info.DayNumber);
        // Act & Assert
        Assert.Equal(day, ((ISimpleDate)day).ToCalendarDay());
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void ToCalendarDate(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        // Arrange
        var day = CalendarUT.GetCalendarDay(dayNumber);
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, day.ToCalendarDate());
    }

    //public void ToOrdinalDate(DateInfo info)

    //public void WithCalendar_NullCalendar()
}
