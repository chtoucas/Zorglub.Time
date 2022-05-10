// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;
//using Zorglub.Time.Core.Schemas;

//public static partial class CurrentMathTests
//{
//    private static readonly CurrentArithmetic<GregorianSchema> s_Arithmetic =
//        new(GregorianCalendar.Instance);

//    private static readonly CalendarDate s_Date = new(1, 1, 1, Cuid.Gregorian);
//    private static readonly CalendarMonth s_Month = new(1, 1, Cuid.Gregorian);

//    private static readonly CalendarDate s_InvalidDate = new(1, 1, 1, Cuid.Julian);
//    private static readonly CalendarMonth s_InvalidMonth = new(1, 1, Cuid.Julian);
//}

//public partial class CurrentMathTests
//{
//    [Fact]
//    public static void AddAdjustment_Prop() =>
//        Assert.Equal(AddAdjustment.EndOfMonth, s_Arithmetic.AddAdjustment);

//    [Fact]
//    public static void AddYears_WithInvalidCalendar()
//    {
//        Assert.Throws<ArgumentException>("month", () => s_Arithmetic.AddYears(s_InvalidMonth, 1));
//        Assert.Throws<ArgumentException>("date", () => s_Arithmetic.AddYears(s_InvalidDate, 1));
//    }

//    [Fact]
//    public static void AddMonths_WithInvalidCalendar()
//    {
//        Assert.Throws<ArgumentException>("month", () => s_Arithmetic.AddMonths(s_InvalidMonth, 1));
//        Assert.Throws<ArgumentException>("date", () => s_Arithmetic.AddMonths(s_InvalidDate, 1));
//    }

//    [Fact]
//    public static void CountYearsBetween_WithInvalidCalendar()
//    {
//        Assert.Throws<ArgumentException>("start",
//            () => s_Arithmetic.CountYearsBetween(s_InvalidDate, s_Date));
//        Assert.Throws<ArgumentException>("end",
//            () => s_Arithmetic.CountYearsBetween(s_Date, s_InvalidDate));
//    }

//    [Fact]
//    public static void CountMonthsBetween_WithInvalidCalendar()
//    {
//        Assert.Throws<ArgumentException>("start",
//            () => s_Arithmetic.CountMonthsBetween(s_InvalidMonth, s_Month));
//        Assert.Throws<ArgumentException>("end",
//            () => s_Arithmetic.CountMonthsBetween(s_Month, s_InvalidMonth));
//        Assert.Throws<ArgumentException>("start",
//            () => s_Arithmetic.CountMonthsBetween(s_InvalidDate, s_Date));
//        Assert.Throws<ArgumentException>("end",
//            () => s_Arithmetic.CountMonthsBetween(s_Date, s_InvalidDate));
//    }
//}
