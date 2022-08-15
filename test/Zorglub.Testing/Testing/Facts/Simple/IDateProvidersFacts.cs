// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Simple;

public abstract partial class IDateProvidersFacts<TProvider, TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TProvider : IDateProviders<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateProvidersFacts(SimpleCalendar calendar)
    {
        Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    protected SimpleCalendar Calendar { get; }

    protected abstract TDate GetDate(int y, int m, int d);
    protected abstract TDate GetDate(int y, int doy);
}

public partial class IDateProvidersFacts<TProvider, TDate, TDataSet> // CalendarYear
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDaysInYear(YearInfo info)
    {
        int y = info.Year;
        var year = Calendar.GetCalendarYear(y);
        var exp = from doy in Enumerable.Range(1, info.DaysInYear)
                  select GetDate(y, doy);
        // Act
        var actual = TProvider.GetDaysInYear(year);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear﹍CalendarYear(YearInfo info)
    {
        int y = info.Year;
        var year = Calendar.GetCalendarYear(y);
        var startOfYear = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, TProvider.GetStartOfYear(year));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var year = Calendar.GetCalendarYear(y);
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, TProvider.GetDayOfYear(year, doy));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear﹍CalendarYear(YearInfo info)
    {
        int y = info.Year;
        var year = Calendar.GetCalendarYear(y);
        var endOfYear = GetDate(y, info.DaysInYear);
        // Act & Assert
        Assert.Equal(endOfYear, TProvider.GetEndOfYear(year));
    }
}

public partial class IDateProvidersFacts<TProvider, TDate, TDataSet> // CalendarMonth
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        var exp = from d in Enumerable.Range(1, info.DaysInMonth)
                  select GetDate(y, m, d);
        // Act
        var actual = TProvider.GetDaysInMonth(month);
        // Assert
        Assert.Equal(exp, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfYear﹍CalendarMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        var startOfYear = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, TProvider.GetStartOfYear(month));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear﹍CalendarMonth(YearInfo info)
    {
        int y = info.Year;
        for (int m = 1; m <= info.MonthsInYear; m++)
        {
            var month = Calendar.GetCalendarMonth(y, m);
            var endOfYear = GetDate(y, info.DaysInYear);
            // Act & Assert
            Assert.Equal(endOfYear, TProvider.GetEndOfYear(month));
        }
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = Calendar.GetCalendarMonth(y, m);
        var startOfMonth = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, TProvider.GetStartOfMonth(month));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var month = Calendar.GetCalendarMonth(y, m);
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, TProvider.GetDayOfMonth(month, d));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var daysInMonth = info.DaysInMonth;
        var month = Calendar.GetCalendarMonth(y, m);
        var endOfMonth = GetDate(y, m, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, TProvider.GetEndOfMonth(month));
    }
}
