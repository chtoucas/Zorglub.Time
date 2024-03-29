﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides facts about <see cref="ICalendar{TDate}"/>.
/// </summary>
public abstract partial class ICalendarTFacts<TDate, TCalendar, TDataSet> :
    ICalendarFacts<TCalendar, TDataSet>
    where TDate : IDateable
    where TCalendar : ICalendar<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected ICalendarTFacts(TCalendar calendar) : base(calendar) { }

    /// <summary>Creates a new instance of <typeparamref name="TDate"/> from the specified components.</summary>
    protected abstract TDate GetDate(int y, int m, int d);
    /// <summary>Creates a new instance of <typeparamref name="TDate"/> from the specified ordinal components.</summary>
    protected abstract TDate GetDate(int y, int doy);
    /// <summary>Creates a new instance of <typeparamref name="TDate"/> from the specified <see cref="DayNumber"/>.</summary>
    protected abstract TDate GetDate(DayNumber dayNumber);
}

public partial class ICalendarTFacts<TDate, TCalendar, TDataSet> // Factories
{
    #region Factory(y, m, d)

    [Fact]
    public void Factory_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => GetDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Factory_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => GetDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Factory_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => GetDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Factory(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        TDate date = GetDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [RedundantTest]
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Factory_ViaDayNumber(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        TDate date = GetDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
    #region Factory(y, doy)

    [Fact]
    public void Factory﹍Ordinal_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => GetDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void Factory﹍Ordinal_InvalidDayOfYear(int y, int doy) =>
        Assert.ThrowsAoorexn("dayOfYear", () => GetDate(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Factory﹍Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        TDate date = GetDate(y, doy);
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
        DomainTester.TestInvalidDayNumber(GetDate);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Factory﹍DayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        // Act
        TDate date = GetDate(dayNumber);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
}

public partial class ICalendarTFacts<TDate, TCalendar, TDataSet> // IDateProvider<TDate>
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
            select GetDate(y, i);
        // Act
        IEnumerable<TDate> actual = CalendarUT.GetDaysInYear(y);
        var arr = actual.ToArray();
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInYear, arr.Length);
        Assert.Equal(startOfYear, arr.First());
        Assert.Equal(endOfYear, arr.Last());
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
            select GetDate(y, m, i);
        // Act
        IEnumerable<TDate> actual = CalendarUT.GetDaysInMonth(y, m);
        var arr = actual.ToArray();
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(info.DaysInMonth, arr.Length);
        Assert.Equal(startofMonth, arr.First());
        Assert.Equal(endOfMonth, arr.Last());
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
        TDate startOfYear = GetDate(y, 1, 1);
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
        TDate endOfYear = GetDate(y, info.DaysInYear);
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
        TDate startOfMonth = GetDate(y, m, 1);
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
        TDate endOfMonth = GetDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarUT.GetEndOfMonth(y, m));
    }

    #endregion
}
