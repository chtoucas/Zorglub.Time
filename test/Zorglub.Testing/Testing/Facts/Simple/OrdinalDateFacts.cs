// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="OrdinalDate"/>.
/// </summary>
public abstract partial class OrdinalDateFacts<TDataSet> : SimpleDateFacts<OrdinalDate, TDataSet>
    where TDataSet :
        ICalendarDataSet,
        IDaysAfterDataSet,
        IAdvancedMathDataSet,
        IDayOfWeekDataSet,
        ISingleton<TDataSet>
{
    protected OrdinalDateFacts(Calendar calendar, Calendar otherCalendar!!)
        : base(calendar, otherCalendar)
    {
        (MinDate, MaxDate) = calendar.MinMaxOrdinal;
    }

    protected sealed override OrdinalDate MinDate { get; }
    protected sealed override OrdinalDate MaxDate { get; }

    protected sealed override OrdinalDate CreateDate(int y, int m, int d) =>
        // Base tests are indirect.
        CalendarUT.GetCalendarDate(y, m, d).ToOrdinalDate();
}

public partial class OrdinalDateFacts<TDataSet>
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
}

public partial class OrdinalDateFacts<TDataSet> // Properties
{
    // We also test the internal prop Cuid.
    [Theory, MemberData(nameof(DateInfoData))]
    public void Calendar_Prop(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(CalendarUT, date.Calendar);
        Assert.Equal(CalendarUT.Id, date.Cuid);
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

    //public void WithCalendar_NullCalendar()
}
