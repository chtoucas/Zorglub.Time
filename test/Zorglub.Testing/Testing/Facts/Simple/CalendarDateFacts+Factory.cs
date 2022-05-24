// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="CalendarDateFactory"/>.
/// </summary>
public abstract partial class CalendarDateFactoryFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarDateFactoryFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class CalendarDateFactoryFacts<TDataSet> // CalendarDate
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear﹍Date(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarDateFactory.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear﹍Date(YearInfo info)
    {
        int y = info.Year;
        var date = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act
        var endOfYear = CalendarDateFactory.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfMonth﹍Date(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CalendarUT.GetCalendarDate(y, m, d);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarDateFactory.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth﹍Date(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CalendarUT.GetCalendarDate(y, m, 1);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarDateFactory.GetEndOfMonth(date));
    }
}

public partial class CalendarDateFactoryFacts<TDataSet> // CalendarYear
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var startOfYear = CalendarUT.GetCalendarDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarDateFactory.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = CalendarUT.GetCalendarYear(y);
        var exp = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, CalendarDateFactory.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var year = CalendarUT.GetCalendarYear(y);
        var endOfYear = CalendarUT.GetOrdinalDate(y, info.DaysInYear).ToCalendarDate();
        // Act & Assert
        Assert.Equal(endOfYear, CalendarDateFactory.GetEndOfYear(year));
    }
}
