// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

public abstract partial class OrdinalDateHelperFacts<TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected OrdinalDateHelperFacts(Calendar calendar)
    {
        CalendarUT = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected Calendar CalendarUT { get; }
}

public partial class OrdinalDateHelperFacts<TDataSet> //
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        var helper = new OrdinalDateHelper(date);
        var startOfYear = CalendarUT.GetOrdinalDate(y, 1);
        // Act & Assert
        Assert.Equal(startOfYear, helper.GetStartOfYear());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = CalendarUT.GetOrdinalDate(y, 1);
        var helper = new OrdinalDateHelper(date);
        // Act
        var endOfYear = helper.GetEndOfYear();
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfMonth(DateInfo info)
    {
        var (y, m, _, doy) = info;
        var date = CalendarUT.GetOrdinalDate(y, doy);
        var helper = new OrdinalDateHelper(date);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(startOfMonth, helper.GetStartOfMonth());
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CalendarUT.GetOrdinalDate(y, 1);
        var helper = new OrdinalDateHelper(date);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, info.DaysInMonth).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(endOfMonth, helper.GetEndOfMonth());
    }
}

public partial class OrdinalDateHelperFacts<TDataSet>
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var startOfMonth = CalendarUT.GetCalendarDate(y, m, 1).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(startOfMonth, OrdinalDateHelper.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AtDayOfMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var date = CalendarUT.GetOrdinalDate(y, doy);
        // Act & Assert
        Assert.Equal(date, OrdinalDateHelper.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = CalendarUT.GetCalendarMonth(y, m);
        var endOfMonth = CalendarUT.GetCalendarDate(y, m, daysInMonth).ToOrdinalDate();
        // Act & Assert
        Assert.Equal(endOfMonth, OrdinalDateHelper.GetEndOfMonth(month));
    }
}