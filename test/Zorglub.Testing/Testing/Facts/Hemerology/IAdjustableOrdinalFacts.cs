// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

public abstract partial class IAdjustableOrdinalFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : IAdjustableOrdinal<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IAdjustableOrdinalFacts(Range<int> supportedYears)
    {
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected abstract TDate GetDate(int y, int doy);
}

public partial class IAdjustableOrdinalFacts<TDate, TDataSet> // Adjust()
{
    //[Fact]
    //public void Adjust_InvalidAdjuster()
    //{
    //    var date = GetDate(1, 1);
    //    // Act & Assert
    //    Assert.ThrowsAnexn("adjuster", () => date.Adjust(null!));
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust_InvalidYears(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = GetDate(y, doy);
    //    foreach (var invalidYear in SupportedYearsTester.InvalidYears)
    //    {
    //        var adjuster = (OrdinalParts parts) =>
    //        {
    //            var (y, doy) = parts;
    //            return new OrdinalParts(invalidYear, doy);
    //        };
    //        // Act & Assert
    //        Assert.ThrowsAoorexn("year", () => date.Adjust(adjuster));
    //    }
    //}

    //[Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    //public void Adjust_InvalidDayOfYear(int y, int newDayOfYear)
    //{
    //    var date = GetDate(y, 1);
    //    var adjuster = (OrdinalParts parts) =>
    //    {
    //        var (y, doy) = parts;
    //        return new OrdinalParts(y, newDayOfYear);
    //    };
    //    // Act & Assert
    //    Assert.ThrowsAoorexn("dayOfYear", () => date.Adjust(adjuster));
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

public partial class IAdjustableOrdinalFacts<TDate, TDataSet> // WithYear()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_InvalidYears(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(date.WithYear, "newYear");
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = GetDate(1, 1);
            var exp = GetDate(y, 1);
            // Act & Assert
            Assert.Equal(exp, date.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_Invariance(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(date, date.WithYear(y));
    }

    // NB: disabled because this cannot work in case the matching day in year 1
    // is not valid (leap vs common year). Nevertheless I keep it around just to
    // remind me that I should not try to create it again.
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void WithYear(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = GetDate(1, doy);
    //    var exp = GetDate(y, doy);
    //    // Act & Assert
    //    Assert.Equal(exp, date.WithYear(y));
    //}
}

public partial class IAdjustableOrdinalFacts<TDate, TDataSet> // WithDayOfYear()
{
    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void WithDayOfYear_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = GetDate(y, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newDayOfYear", () => date.WithDayOfYear(newDayOfYear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDayOfYear_Invariance(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(date, date.WithDayOfYear(doy));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, 1);
        var exp = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, date.WithDayOfYear(doy));
    }
}