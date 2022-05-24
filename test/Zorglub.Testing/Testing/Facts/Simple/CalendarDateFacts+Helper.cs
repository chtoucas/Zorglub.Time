// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

public abstract partial class CalendarDateHelperFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateHelperFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class CalendarDateHelperFacts<TDataSet> //
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var helper = new CalendarDateHelper(date);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, helper.GetStartOfYear());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = CalendarUT.GetCalendarDate(y, 1, 1);
        var helper = new CalendarDateHelper(date);
        // Act
        var endOfYear = helper.GetEndOfYear();
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var helper = new CalendarDateHelper(date);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, helper.GetStartOfMonth());
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CalendarUT.GetCalendarDate(y, m, 1);
        var helper = new CalendarDateHelper(date);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, helper.GetEndOfMonth());
    }
}

public partial class CalendarDateHelperFacts<TDataSet> //
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void AtStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarDateHelper.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, CalendarDateHelper.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void AtEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDate();
        // Act & Assert
        Assert.Equal(endOfYear, CalendarDateHelper.GetEndOfYear(year));
    }
}
