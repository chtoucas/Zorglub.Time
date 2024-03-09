// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

using Zorglub.Testing.Facts.Bulgroz;

public sealed class Tropicalia3031TroeschFormulaeTests : ICalendricalFormulaeFacts<Tropicalia3031DataSet>
{
    public Tropicalia3031TroeschFormulaeTests() : base(new Tropicalia3031TroeschFormulae()) { }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYear(YearInfo info)
    {
        // Act
        int actual = Tropicalia3031TroeschFormulae.CountDaysInYear(info.Year);
        // Assert
        Assert.Equal(info.DaysInYear, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        // Act
        int actual = Tropicalia3031TroeschFormulae.CountDaysInYearBeforeMonth(info.Yemo.Month);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = Tropicalia3031TroeschFormulae.CountDaysInMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (_, m, d, doy) = info;
        // Act
        int mA = Tropicalia3031TroeschFormulae.GetMonth(doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear(DaysSinceEpochInfo info)
    {
        // Act
        int actual = Tropicalia3031TroeschFormulae.GetYear(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Yemoda.Year, actual);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYear(YearDaysSinceEpoch info)
    {
        // Act
        var actual = Tropicalia3031TroeschFormulae.GetStartOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }
}
