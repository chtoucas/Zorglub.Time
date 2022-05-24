// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

/// <summary>
/// Provides facts about <see cref="OrdinalDateFactory"/>.
/// </summary>
public abstract partial class OrdinalDateFactoryFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateFactoryFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class OrdinalDateFactoryFacts<TDataSet> // OrdinalDate
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear﹍Date(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        var startOfYear = CalendarUT.GetOrdinalDate(y, 1);
        // Act & Assert
        Assert.Equal(startOfYear, OrdinalDateFactory.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear﹍Date(YearInfo info)
    {
        int y = info.Year;
        var date = CalendarUT.GetOrdinalDate(y, 1);
        // Act
        var endOfYear = OrdinalDateFactory.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfMonth﹍Date(DateInfo info)
    {
        var (y, m, _, doy) = info;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(startOfMonth, OrdinalDateFactory.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth﹍Date(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CalendarUT.GetOrdinalDate(y, 1);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(endOfMonth, OrdinalDateFactory.GetEndOfMonth(date));
    }
}

public partial class OrdinalDateFactoryFacts<TDataSet> // CalendarMonth
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(startOfMonth, OrdinalDateFactory.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, OrdinalDateFactory.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(endOfMonth, OrdinalDateFactory.GetEndOfMonth(month));
    }
}