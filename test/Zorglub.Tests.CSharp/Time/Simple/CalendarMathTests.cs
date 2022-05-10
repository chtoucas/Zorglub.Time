// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public static partial class CalendarMathTests
{
    private static readonly FauxCalendarMath s_Math = new(GregorianCalendar.Instance);

    private static readonly CalendarDate s_Date = new(1, 1, 1, Cuid.Gregorian);
    private static readonly CalendarMonth s_Month = new(1, 1, Cuid.Gregorian);

    private static readonly CalendarDate s_InvalidDate = new(1, 1, 1, Cuid.Julian);
    private static readonly CalendarMonth s_InvalidMonth = new(1, 1, Cuid.Julian);

    private static readonly CalendarDate s_OtherDate = new(2000, 1, 1, Cuid.Gregorian);
    private static readonly CalendarMonth s_OtherMonth = new(2000, 1, Cuid.Gregorian);
}

public partial class CalendarMathTests
{
    [Fact]
    public static void Constructor_WithInvalidCalendar() =>
        Assert.ThrowsAnexn("calendar", () => new FauxCalendarMath(null!));

    [Fact]
    public static void Constructor_WithCalendar()
    {
        var chr = GregorianCalendar.Instance;
        // Act
        var arithmetic = new FauxCalendarMath(GregorianCalendar.Instance);
        // Assert
        Assert.Equal(chr.Id, arithmetic.Cuid);
    }

    [Fact]
    public static void AddAdjustment_Prop() =>
        Assert.Equal(AddAdjustment.EndOfMonth, s_Math.AddAdjustment);

    [Fact]
    public static void AddYears_WithInvalidCalendar()
    {
        Assert.Throws<ArgumentException>("month", () => s_Math.AddYears(s_InvalidMonth, 1));
        Assert.Throws<ArgumentException>("date", () => s_Math.AddYears(s_InvalidDate, 1));
    }

    [Fact]
    public static void AddYears()
    {
        Assert.False(s_Math.AddYearsCoreWasCalled);
        _ = s_Math.AddYears(s_Date, 1000);
        Assert.True(s_Math.AddYearsCoreWasCalled);

        Assert.False(s_Math.AddYearsCore1WasCalled);
        _ = s_Math.AddYears(s_Month, 1000);
        Assert.True(s_Math.AddYearsCore1WasCalled);

    }

    [Fact]
    public static void AddMonths_WithInvalidCalendar()
    {
        Assert.Throws<ArgumentException>("month", () => s_Math.AddMonths(s_InvalidMonth, 1));
        Assert.Throws<ArgumentException>("date", () => s_Math.AddMonths(s_InvalidDate, 1));
    }

    [Fact]
    public static void AddMonths()
    {
        Assert.False(s_Math.AddMonthsCoreWasCalled);
        _ = s_Math.AddMonths(s_Date, 1000);
        Assert.True(s_Math.AddMonthsCoreWasCalled);

        Assert.False(s_Math.AddMonthsCore1WasCalled);
        _ = s_Math.AddMonths(s_Month, 1000);
        Assert.True(s_Math.AddMonthsCore1WasCalled);
    }

    [Fact]
    public static void CountYearsBetween_WithInvalidCalendar()
    {
        Assert.Throws<ArgumentException>("start",
            () => s_Math.CountYearsBetween(s_InvalidDate, s_Date));
        Assert.Throws<ArgumentException>("end",
            () => s_Math.CountYearsBetween(s_Date, s_InvalidDate));
    }

    [Fact]
    public static void CountYearsBetween()
    {
        Assert.False(s_Math.CountYearsBetweenCoreWasCalled);
        _ = s_Math.CountYearsBetween(s_Date, s_OtherDate);
        Assert.True(s_Math.CountYearsBetweenCoreWasCalled);
    }

    [Fact]
    public static void CountMonthsBetween_WithInvalidCalendar()
    {
        Assert.Throws<ArgumentException>("start",
            () => s_Math.CountMonthsBetween(s_InvalidMonth, s_Month));
        Assert.Throws<ArgumentException>("end",
            () => s_Math.CountMonthsBetween(s_Month, s_InvalidMonth));
        Assert.Throws<ArgumentException>("start",
            () => s_Math.CountMonthsBetween(s_InvalidDate, s_Date));
        Assert.Throws<ArgumentException>("end",
            () => s_Math.CountMonthsBetween(s_Date, s_InvalidDate));
    }

    [Fact]
    public static void CountMonthsBetween()
    {
        Assert.False(s_Math.CountMonthsBetweenCoreWasCalled);
        _ = s_Math.CountMonthsBetween(s_Date, s_OtherDate);
        Assert.True(s_Math.CountMonthsBetweenCoreWasCalled);

        Assert.False(s_Math.CountMonthsBetweenCore1WasCalled);
        _ = s_Math.CountMonthsBetween(s_Month, s_OtherMonth);
        Assert.True(s_Math.CountMonthsBetweenCore1WasCalled);
    }
}
