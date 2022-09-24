// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Specialized;

using Zorglub.Testing.Data;
using Zorglub.Testing.Facts.Hemerology;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Specialized;

public abstract partial class SpecialAdjusterFacts<TDate, TDataSet> :
    IDateAdjusterFacts<SpecialAdjuster<TDate>, TDate, TDataSet>
    where TDate : IAdjustable<TDate>, IDateable
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected SpecialAdjusterFacts(SpecialAdjuster<TDate> adjuster) : base(adjuster) { }
}

public partial class SpecialAdjusterFacts<TDate, TDataSet> // Adjust()
{
    [Fact]
    public void Adjust_InvalidAdjuster()
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        Assert.ThrowsAnexn("adjuster", () => date.Adjust(null!));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_InvalidYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        foreach (var invalidYear in SupportedYearsTester.InvalidYears)
        {
            var adjuster = AdjusterUT.WithYear(invalidYear);
            // Act & Assert
            Assert.ThrowsAoorexn("newYear", () => date.Adjust(adjuster));
        }
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Adjust_InvalidMonth(int y, int newMonth)
    {
        var date = GetDate(y, 1, 1);
        var adjuster = AdjusterUT.WithMonth(newMonth);
        // Act & Assert
        Assert.ThrowsAoorexn("newMonth", () => date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Adjust_InvalidDay(int y, int m, int newDay)
    {
        var date = GetDate(y, m, 1);
        var adjuster = AdjusterUT.WithDay(newDay);
        // Act & Assert
        Assert.ThrowsAoorexn("newDay", () => date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void Adjust_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = GetDate(y, 1);
        var adjuster = AdjusterUT.WithDayOfYear(newDayOfYear);
        // Act & Assert
        Assert.ThrowsAoorexn("newDayOfYear", () => date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.Adjust(x => x));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Adjust_WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, 1, 1);
        var adjuster = AdjusterUT.WithMonth(m);
        var exp = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_WithDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, 1);
        var exp = GetDate(y, m, d);
        var adjuster = AdjusterUT.WithDay(d);
        // Act & Assert
        Assert.Equal(exp, date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_WithDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, 1);
        var exp = GetDate(y, doy);
        var adjuster = AdjusterUT.WithDayOfYear(doy);
        // Act & Assert
        Assert.Equal(exp, date.Adjust(adjuster));
    }

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust(DateInfo info)
    //{
    //    var (y, m, d) = info.Yemoda;
    //    var date = GetDate(1, 1, 1);
    //    var adjuster = (DateParts _) => new DateParts(y, m, d);
    //    var exp = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(exp, date.Adjust(adjuster));
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust_Invariance(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = GetDate(y, doy);
    //    // Act & Assert
    //    Assert.Equal(date, date.Adjust(x => x));
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = GetDate(1, 1);
    //    var adjuster = (OrdinalParts _) => new OrdinalParts(y, doy);
    //    var exp = GetDate(y, doy);
    //    // Act & Assert
    //    Assert.Equal(exp, date.Adjust(adjuster));
    //}
}
