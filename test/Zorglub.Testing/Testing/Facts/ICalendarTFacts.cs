// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="ICalendar{TDate}"/>.
/// </summary>
public abstract partial class ICalendarTFacts<TDate, TCalendar, TDataSet> :
    ICalendarFacts<TCalendar, TDataSet>
    where TDate : struct, IDate<TDate>
    where TCalendar : ICalendar<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected ICalendarTFacts(TCalendar calendar) : base(calendar) { }

    protected abstract TDate CreateDate(int y, int m, int d);
    protected abstract TDate CreateDate(int y, int doy);
    protected abstract TDate CreateDate(DayNumber dayNumber);
}

public partial class ICalendarTFacts<TDate, TCalendar, TDataSet> // Factories
{
    #region Factory(y, m, d)

    [Fact]
    public void Factory_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CreateDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Factory_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CreateDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Factory_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => CreateDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Factory(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        TDate date = CreateDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Factory_ViaDayNumber(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        TDate date = CreateDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
    #region Factory(y, doy)

    [Fact]
    public void Factory﹍Ordinal_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CreateDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void Factory﹍Ordinal_InvalidDayOfYear(int y, int dayOfYear) =>
        Assert.ThrowsAoorexn("dayOfYear", () => CreateDate(y, dayOfYear));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Factory﹍Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        TDate date = CreateDate(y, doy);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
    }

    #endregion
    #region Factory(dayNumber)

    [Fact]
    public void Factory﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(CreateDate);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Factory﹍DayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        // Act
        TDate date = CreateDate(dayNumber);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
}

// Dates in a given year or month.
public partial class ICalendarTFacts<TDate, TCalendar, TDataSet> // IDayProvider
{
    #region GetDaysInYear(y)

    [Fact]
    public void GetDaysInYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetDaysInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDaysInYear(YearInfo info)
    {
        int y = info.Year;
        TDate startOfYear = CalendarUT.GetStartOfYear(y);
        TDate endOfYear = CalendarUT.GetEndOfYear(y);
        IEnumerable<TDate> exp =
            from i in Enumerable.Range(1, info.DaysInYear)
            select CreateDate(y, i);
        // Act
        IEnumerable<TDate> actual = CalendarUT.GetDaysInYear(y);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInYear, actual.Count());
        Assert.Equal(startOfYear, actual.First());
        Assert.Equal(endOfYear, actual.Last());
    }

    #endregion
    #region GetDaysInMonth(y, m)

    [Fact]
    public void GetDaysInMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetDaysInMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetDaysInMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetDaysInMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        TDate startofMonth = CalendarUT.GetStartOfMonth(y, m);
        TDate endOfMonth = CalendarUT.GetEndOfMonth(y, m);
        IEnumerable<TDate> exp =
            from i in Enumerable.Range(1, info.DaysInMonth)
            select CreateDate(y, m, i);
        // Act
        IEnumerable<TDate> actual = CalendarUT.GetDaysInMonth(y, m);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInMonth, actual.Count());
        Assert.Equal(startofMonth, actual.First());
        Assert.Equal(endOfMonth, actual.Last());
    }

    #endregion

    #region GetStartOfYear(y)

    [Fact]
    public void GetStartOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetStartOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetStartOfYear(YearInfo info)
    {
        int y = info.Year;
        TDate startOfYear = CreateDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarUT.GetStartOfYear(y));
    }

    #endregion
    #region GetEndOfYear(y)

    [Fact]
    public void GetEndOfYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.GetEndOfYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        TDate endOfYear = CreateDate(y, info.DaysInYear);
        // Act & Assert
        Assert.Equal(endOfYear, CalendarUT.GetEndOfYear(y));
    }

    #endregion
    #region GetStartOfMonth(y, m)

    [Fact]
    public void GetStartOfMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetStartOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetStartOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetStartOfMonth(y, m)!);

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        TDate startOfMonth = CreateDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarUT.GetStartOfMonth(y, m));
    }

    #endregion
    #region GetEndOfMonth(y, m)

    [Fact]
    public void GetEndOfMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.GetEndOfMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void GetEndOfMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => CalendarUT.GetEndOfMonth(y, m)!);

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        TDate endOfMonth = CreateDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarUT.GetEndOfMonth(y, m));
    }

    #endregion
}
