// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

public sealed partial class GregorianCalCalFormulaeTests : CalendricalDataConsumer<GregorianDataSet>
{
    private static readonly GregorianCalCalFormulae s_Formulae = new();

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = s_Formulae.CountDaysInYearBeforeMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        int mA = s_Formulae.GetMonth(y, doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear(DaysSinceEpochInfo info)
    {
        // Act
        int actual = s_Formulae.GetYear(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Year, actual);
    }
}
