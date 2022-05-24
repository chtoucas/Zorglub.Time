// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

// TODO(fact): do not use conversion. Idem with the two other date types.

public abstract partial class CalendarDayHelperFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDayHelperFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class CalendarDayHelperFacts<TDataSet>
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void AtStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1).ToCalendarDay();
        // Act & Assert
        Assert.Equal(startOfYear, CalendarDayHelper.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
        // Act & Assert
        Assert.Equal(exp, CalendarDayHelper.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void AtEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDay();
        // Act & Assert
        Assert.Equal(endOfYear, CalendarDayHelper.GetEndOfYear(year));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToCalendarDay();
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarDayHelper.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetCalendarDate(y, m, d).ToCalendarDay();
        // Act & Assert
        Assert.Equal(date, CalendarDayHelper.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToCalendarDay();
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarDayHelper.GetEndOfMonth(month));
    }
}