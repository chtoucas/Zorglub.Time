// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// TODO(fact): do not use conversion. Idem with the two other date types.

/// <summary>
/// Provides facts about <see cref="CalendarDayFactory"/>.
/// </summary>
public abstract partial class CalendarDayFactoryFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDayFactoryFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class CalendarDayFactoryFacts<TDataSet> // CalendarYear
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1).ToCalendarDay();
        // Act & Assert
        Assert.Equal(startOfYear, CalendarDayFactory.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
        // Act & Assert
        Assert.Equal(exp, CalendarDayFactory.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDay();
        // Act & Assert
        Assert.Equal(endOfYear, CalendarDayFactory.GetEndOfYear(year));
    }
}

public partial class CalendarDayFactoryFacts<TDataSet> // CalendarMonth
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToCalendarDay();
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarDayFactory.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
        // Act & Assert
        Assert.Equal(date, CalendarDayFactory.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToCalendarDay();
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarDayFactory.GetEndOfMonth(month));
    }
}