// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Hemerology.Scopes;

public sealed partial class GregorianCalendarTests :
    CalendarFacts<GregorianCalendar, ProlepticGregorianDataSet>
{
    public GregorianCalendarTests() : base(GregorianCalendar.Instance) { }

    public static TheoryData<YemodaAnd<DayOfWeek>> DayOfWeekData => DataSet.DayOfWeekData;

    protected override GregorianCalendar GetSingleton() => GregorianCalendar.Instance;

    [Fact]
    public override void Id() => Assert.Equal(Cuid.Gregorian, CalendarUT.Id);

    [Fact]
    public override void Math() =>
        Assert.IsType<Regular12Math>(CalendarUT.Math);

    [Fact]
    public override void Scope() => Assert.IsType<GregorianProlepticShortScope>(CalendarUT.Scope);
}

public partial class GregorianCalendarTests
{
    [Fact]
    public void Today()
    {
        // Arrange
        var exp = DateTime.Now;
        // Act
        var today = CalendarUT.GetCurrentDate();
        // Assert
        Assert.Equal(exp.Year, today.Year);
        Assert.Equal(exp.Month, today.Month);
        Assert.Equal(exp.Day, today.Day);
    }

    [Theory, MemberData(nameof(DayOfWeekData))]
    public void GetDayOfWeek_UsingDates(YemodaAnd<DayOfWeek> info)
    {
        var (y, m, d, dayOfWeek) = info;
        // Arrange
        var date = CalendarUT.GetCalendarDate(y, m, d);
        // Act & Assert
        Assert.Equal(dayOfWeek, date.DayOfWeek);
    }
}

public partial class GregorianCalendarTests
{
    [Fact]
    public void NewGregorianDate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new CalendarDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void NewGregorianDate_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => new CalendarDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public static void NewGregorianDate_InvalidDay(int y, int m, int d) =>
        Assert.ThrowsAoorexn("day", () => new CalendarDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public static void NewGregorianDate_UsingDates(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = new CalendarDate(y, m, d);
        var (year, month, day) = date;

        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);

        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);

        Assert.Equal(doy, date.DayOfYear);
        Assert.Equal(info.IsIntercalary, date.IsIntercalary);
        Assert.False(date.IsSupplementary);
    }

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public static void NewGregorianDate_UsingDayNumbers(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;

        // Act
        var date = new CalendarDate(y, m, d);
        var (year, month, day) = date;
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);

        Assert.Equal(y, year);
        Assert.Equal(m, month);
        Assert.Equal(d, day);
    }

    [Fact]
    public void NewGregorianMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new CalendarMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public static void NewGregorianMonth_InvalidMonth(int y, int m) =>
        Assert.ThrowsAoorexn("month", () => new CalendarMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public static void NewGregorianMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var cmonth = new CalendarMonth(y, m);
        var (year, month) = cmonth;

        // Assert
        Assert.Equal(y, cmonth.Year);
        Assert.Equal(m, cmonth.MonthOfYear);

        Assert.Equal(y, year);
        Assert.Equal(m, month);

        Assert.Equal(info.IsIntercalary, cmonth.IsIntercalary);
    }

    [Fact]
    public void NewGregorianYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => new CalendarYear(y));

    [Theory, MemberData(nameof(YearInfoData))]
    public static void NewGregorianYear(YearInfo info)
    {
        int y = info.Year;

        // Act
        var cyear = new CalendarYear(y);
        // Assert
        Assert.Equal(y, cyear.Year);
        Assert.Equal(info.IsLeap, cyear.IsLeap);
    }
}
