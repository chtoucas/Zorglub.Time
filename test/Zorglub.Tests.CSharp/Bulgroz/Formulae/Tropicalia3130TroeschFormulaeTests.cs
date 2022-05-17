// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

public sealed class Tropicalia3130TroeschFormulaeTests
    : IInterconversionFormulaeFacts<Tropicalia3130TroeschFormulae, Tropicalia3130DataSet>
{
    public Tropicalia3130TroeschFormulaeTests() : base(new Tropicalia3130TroeschFormulae()) { }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (_, m) = info.Yemo;
        // Act
        int actual = Tropicalia3130TroeschFormulae.CountDaysInYearBeforeMonth(m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (_, m, d, doy) = info;
        // Act
        int mA = Tropicalia3130TroeschFormulae.GetMonth(doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear(DaysSinceEpochInfo info)
    {
        // Act
        int actual = Tropicalia3130TroeschFormulae.GetYear(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Yemoda.Year, actual);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYear(YearDaysSinceEpoch info)
    {
        // Act
        var actual = Tropicalia3130TroeschFormulae.GetStartOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }
}
