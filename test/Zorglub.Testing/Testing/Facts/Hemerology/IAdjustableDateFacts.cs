// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Hemerology;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

// Hypothesis: years and months are complete.

public abstract partial class IAdjustableDateFacts<TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDate : IAdjustableDate<TDate>
    where TDataSet : ICalendricalDataSet, IYearAdjustmentDataSet, ISingleton<TDataSet>
{
    protected IAdjustableDateFacts(Range<int> supportedYears)
    {
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected abstract TDate CreateDate(int y, int m, int d);

    public static DataGroup<YemodaAnd<int>> InvalidYearAdjustementData => DataSet.InvalidYearAdjustementData;
    public static DataGroup<YemodaAnd<int>> YearAdjustementData => DataSet.YearAdjustementData;
}

public partial class IAdjustableDateFacts<TDate, TDataSet> // WithYear()
{
    [Fact]
    public void WithYear_InvalidYears()
    {
        var date = CreateDate(1, 1, 1);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(y => date.WithYear(y), "newYear");
    }

    [Theory, MemberData(nameof(InvalidYearAdjustementData))]
    public void WithYear_InvalidResult(YemodaAnd<int> info)
    {
        var (y, m, d, newYear) = info;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.ThrowsAoorexn("newYear", () => date.WithYear(newYear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_Invariant(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.WithYear(y));
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = CreateDate(1, 1, 1);
            var exp = CreateDate(y, 1, 1);
            // Act & Assert
            Assert.Equal(exp, date.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(YearAdjustementData))]
    public void WithYear(YemodaAnd<int> info)
    {
        var (y, m, d, newYear) = info;
        var date = CreateDate(y, m, d);
        var exp = CreateDate(newYear, m, d);
        // Act & Assert
        Assert.Equal(exp, date.WithYear(newYear));
    }
}

public partial class IAdjustableDateFacts<TDate, TDataSet> // WithMonth()
{
    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void WithMonth_InvalidMonth(int y, int newMonth)
    {
        var date = CreateDate(y, 1, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newMonth", () => date.WithMonth(newMonth));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithMonth_Invariant(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.WithMonth(m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = CreateDate(y, 1, 1);
        var exp = CreateDate(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, date.WithMonth(m));
    }
}

public partial class IAdjustableDateFacts<TDate, TDataSet> // WithDay()
{
    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void WithDay_InvalidDay(int y, int m, int newDay)
    {
        var date = CreateDate(y, m, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newDay", () => date.WithDay(newDay));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDay_Invariant(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.WithDay(d));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = CreateDate(y, m, 1);
        var exp = CreateDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, date.WithDay(d));
    }
}
