// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;

// In addition, one should test WithYear() with valid and invalid results.

public abstract partial class IDateAdjusterFacts<TAdjuster, TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TAdjuster : IDateAdjuster<TDate>
    where TDate : IDateable
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateAdjusterFacts(TAdjuster adjuster)
    {
        AdjusterUT = adjuster ?? throw new ArgumentNullException(nameof(adjuster));
        SupportedYearsTester = new SupportedYearsTester(adjuster.Scope.Segment.SupportedYears);
    }

    /// <summary>
    /// Gets the adjuster under test.
    /// </summary>
    protected TAdjuster AdjusterUT { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected abstract TDate GetDate(int y, int m, int d);
    protected abstract TDate GetDate(int y, int doy);
}

public partial class IDateAdjusterFacts<TAdjuster, TDate, TDataSet> // Special dates
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfYear = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, AdjusterUT.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = GetDate(y, 1, 1);
        // Act
        var endOfYear = AdjusterUT.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfMonth = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, AdjusterUT.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, m, 1);
        var endOfMonth = GetDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, AdjusterUT.GetEndOfMonth(date));
    }
}

public partial class IDateAdjusterFacts<TAdjuster, TDate, TDataSet> // AdjustYear()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustYear_InvalidYears(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(y => AdjusterUT.AdjustYear(date, y), "newYear");
    }

    [Fact]
    public void AdjustYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = GetDate(1, 1, 1);
            var exp = GetDate(y, 1, 1);
            // Act & Assert
            Assert.Equal(exp, AdjusterUT.AdjustYear(date, y));
        }
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustYear_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustYear(date, y));
    }

    // NB: disabled because this cannot work in case the matching day in year 1
    // is not valid. Nevertheless I keep it around just to remind me that I
    // should not try to create it again.
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void AdjustYear(DateInfo info)
    //{
    //    var (y, m, d) = info.Yemoda;
    //    var date = GetDate(1, m, d);
    //    var exp = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(exp, AdjusterUT.AdjustYear(date, y));
    //}
}

public partial class IDateAdjusterFacts<TAdjuster, TDate, TDataSet> // AdjustMonth()
{
    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void AdjustMonth_InvalidMonth(int y, int newMonth)
    {
        var date = GetDate(y, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newMonth", () => AdjusterUT.AdjustMonth(date, newMonth));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustMonth_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustMonth(date, m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AdjustMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, 1, 1);
        var exp = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustMonth(date, m));
    }
}

public partial class IDateAdjusterFacts<TAdjuster, TDate, TDataSet> // AdjustDay()
{
    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void AdjustDay_InvalidDay(int y, int m, int newDay)
    {
        var date = GetDate(y, m, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newDay", () => AdjusterUT.AdjustDay(date, newDay));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDay_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustDay(date, d));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, 1);
        var exp = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustDay(date, d));
    }
}

public partial class IDateAdjusterFacts<TAdjuster, TDate, TDataSet> // AdjustDayOfYear()
{
    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void AdjustDayOfYear_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = GetDate(y, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newDayOfYear", () => AdjusterUT.AdjustDayOfYear(date, newDayOfYear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDayOfYear_Invariance(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustDayOfYear(date, doy));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, 1);
        var exp = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustDayOfYear(date, doy));
    }
}
