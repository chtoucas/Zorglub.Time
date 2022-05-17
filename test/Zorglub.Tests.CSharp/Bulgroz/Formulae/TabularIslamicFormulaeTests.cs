// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

public sealed class TabularIslamicFormulaeTests
    : IInterconversionFormulaeFacts<TabularIslamicFormulae, TabularIslamicDataSet>
{
    public TabularIslamicFormulaeTests() : base(new TabularIslamicFormulae()) { }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYear(YearDaysSinceEpoch info)
    {
        // Act
        var actual = TabularIslamicFormulae.GetStartOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (_, m) = info.Yemo;
        // Act
        int actual = TabularIslamicFormulae.CountDaysInYearBeforeMonth(m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear(DaysSinceEpochInfo info)
    {
        // Act
        int actual = TabularIslamicFormulae.GetYear(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Yemoda.Year, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (_, m, _, doy) = info;
        // Act
        int actual = TabularIslamicFormulae.GetMonth(doy);
        // Assert
        Assert.Equal(m, actual);
    }
}
