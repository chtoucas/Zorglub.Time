// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//namespace Zorglub.Time.Extensions;

//using Zorglub.Time.Core.Schemas;
//using Zorglub.Time.Hemerology;

//public static partial class MinMaxYearCalendarExtensionsTests
//{
//    public static readonly TheoryData<int> InvalidPaxYear = new()
//    {
//        0,
//        1 + SystemSchema.DefaultMaxYear,
//    };
//    public static readonly TheoryData<int> InvalidWorldYear = new()
//    {
//        0,
//        1 + SystemSchema.DefaultMaxYear,
//    };

//    private static volatile MinMaxYearCalendar<PaxSchema>? s_Pax;
//    public static MinMaxYearCalendar<PaxSchema> Pax =>
//        s_Pax ??= MinMaxYearCalendar.CreateRetroCalendar(
//            "Pax", CalendricalEpochs.SundayBeforeGregorian, new PaxSchema());

//    private static volatile MinMaxYearCalendar<WorldSchema>? s_World;
//    public static MinMaxYearCalendar<WorldSchema> World =>
//        s_World ??= MinMaxYearCalendar.CreateRetroCalendar(
//            "World", CalendricalEpochs.SundayBeforeGregorian, new WorldSchema());
//}

//// Abstract class LeapWeekSchema.
//public partial class MinMaxYearCalendarExtensionsTests
//{
//    #region GetDayNumber(y, woy, dow)

//    [Fact]
//    public static void GetDayNumber_InvalidCalendar()
//    {
//        MinMaxYearCalendar<PaxSchema> calendar = null!;
//        // Act
//        Assert.ThrowsAnexn("this", () => calendar.GetDayNumber(1, 1, DayOfWeek.Sunday));
//    }

//    [Theory, MemberData(nameof(InvalidPaxYear))]
//    public static void GetDayNumber_InvalidYear(int y) =>
//        Assert.ThrowsAoorexn("year",
//            () => Pax.GetDayNumber(y, 1, DayOfWeek.Monday));

//    [Theory, MemberData(nameof(PaxData.InvalidWeekOfYear), MemberType = typeof(PaxData))]
//    public static void GetDayNumber_InvalidWeekOfYear(int y, int woy) =>
//        Assert.ThrowsAoorexn("weekOfYear",
//            () => Pax.GetDayNumber(y, woy, DayOfWeek.Monday));

//    [Theory, MemberData(nameof(CalCalData.InvalidDayOfWeekData), MemberType = typeof(CalCalData))]
//    public static void GetDayNumber_InvalidDayOfWeek(DayOfWeek dayOfWeek) =>
//        Assert.ThrowsAoorexn("dayOfWeek",
//            () => Pax.GetDayNumber(1, 1, dayOfWeek));

//    [Theory, MemberData(nameof(PaxData.MoreSampleDayNumbers), MemberType = typeof(PaxData))]
//    public static void GetDayNumber(DayNumber dayNumber, int y, int woy, DayOfWeek dow)
//    {
//        // Act
//        var actual = Pax.GetDayNumber(y, woy, dow);
//        // Assert
//        Assert.Equal(dayNumber, actual);
//    }

//    #endregion

//    #region IsIntercalaryWeek()

//    [Fact]
//    public static void IsIntercalaryWeek_InvalidCalendar()
//    {
//        MinMaxYearCalendar<PaxSchema> calendar = null!;
//        // Act
//        Assert.ThrowsAnexn("this", () => calendar.IsIntercalaryWeek(1, 1));
//    }

//    [Theory, MemberData(nameof(InvalidPaxYear))]
//    public static void IsIntercalaryWeek_InvalidYear(int y) =>
//        Assert.ThrowsAoorexn("year",
//            () => Pax.IsIntercalaryWeek(y, 1));

//    [Theory, MemberData(nameof(PaxData.InvalidWeekOfYear), MemberType = typeof(PaxData))]
//    public static void IsIntercalaryWeek_InvalidWeekOfYear(int y, int woy) =>
//        Assert.ThrowsAoorexn("weekOfYear",
//            () => Pax.IsIntercalaryWeek(y, woy));

//    [Theory, MemberData(nameof(PaxData.SampleWeeks), MemberType = typeof(PaxData))]
//    public static void IsIntercalaryWeek(int y, int woy, bool isIntercalary)
//    {
//        // Act
//        bool actual = Pax.IsIntercalaryWeek(y, woy);
//        // Assert
//        Assert.Equal(isIntercalary, actual);
//    }

//    #endregion

//    #region CountWeeksInYear()

//    [Fact]
//    public static void CountWeeksInYear_InvalidCalendar()
//    {
//        MinMaxYearCalendar<PaxSchema> calendar = null!;
//        // Act
//        Assert.ThrowsAnexn("this", () => calendar.CountWeeksInYear(1));
//    }

//    [Theory, MemberData(nameof(InvalidPaxYear))]
//    public static void CountWeeksInYear_InvalidYear(int y) =>
//        Assert.ThrowsAoorexn("year",
//            () => Pax.CountWeeksInYear(y));

//    [Theory, MemberData(nameof(PaxData.MoreSampleYears), MemberType = typeof(PaxData))]
//    public static void CountWeeksInYear(int y, int weeksInYear)
//    {
//        // Act
//        int actual = Pax.CountWeeksInYear(y);
//        // Assert
//        Assert.Equal(weeksInYear, actual);
//    }

//    #endregion
//}

//// Abstract class WorldSchema.
//public partial class MinMaxYearCalendarExtensionsTests
//{
//    public static TheoryData<int, int> InvalidWorldMonthOfYear =>
//        new RevisedWorldData().InvalidMonthFieldData;

//    #region CountDaysInWorldMonth()

//    [Fact]
//    public static void CountDaysInWorldMonth_InvalidCalendar()
//    {
//        MinMaxYearCalendar<WorldSchema> calendar = null!;
//        // Act
//        Assert.ThrowsAnexn("this", () => calendar.CountDaysInWorldMonth(1, 1));
//    }

//    [Theory, MemberData(nameof(InvalidWorldYear))]
//    public static void CountDaysInWorldMonth_InvalidYear(int y) =>
//        Assert.ThrowsAoorexn("year",
//            () => World.CountDaysInWorldMonth(y, 1));

//    [Theory, MemberData(nameof(InvalidWorldMonthOfYear))]
//    public static void CountDaysInWorldMonth_InvalidWeekOfYear(int y, int m) =>
//        Assert.ThrowsAoorexn("month",
//            () => World.CountDaysInWorldMonth(y, m));

//    [Theory, MemberData(nameof(RevisedWorldData.MoreSampleMonths), MemberType = typeof(RevisedWorldData))]
//    public static void CountDaysInWorldMonth(int y, int m, int daysInMonth) =>
//        Assert.Equal(daysInMonth, World.CountDaysInWorldMonth(y, m));

//    #endregion
//}
