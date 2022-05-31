// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

public sealed class Tropicalia3031FormulaeTests : CalendricalDataConsumer<Tropicalia3031DataSet>
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        // Act
        int actual = Tropicalia3031Formulae.CountDaysInYearBeforeMonth(info.Yemo.Month);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = Tropicalia3031Formulae.CountDaysInMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (_, m, d, doy) = info;
        // Act
        int mA = Tropicalia3031Formulae.GetMonth(doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }
}
