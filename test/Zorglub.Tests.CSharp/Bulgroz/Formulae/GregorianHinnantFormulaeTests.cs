// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

using Zorglub.Testing.Facts.Bulgroz;

public sealed class GregorianHinnantFormulaeTests : ICalendricalFormulaeFacts<GregorianDataSet>
{
    private static readonly GregorianHinnantFormulae s_Formulae = new();

    public GregorianHinnantFormulaeTests() : base(s_Formulae) { }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = s_Formulae.CountDaysInYearBeforeMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }
}
