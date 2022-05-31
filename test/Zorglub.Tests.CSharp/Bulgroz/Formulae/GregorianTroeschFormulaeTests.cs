// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

public sealed class GregorianTroeschFormulaeTests : ICalendricalFormulaeFacts<GregorianDataSet>
{
    private static readonly GregorianTroeschFormulae s_Formulae = new();

    public GregorianTroeschFormulaeTests() : base(s_Formulae) { }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysSinceEpoch400(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Act
        int actual = s_Formulae.CountDaysSinceEpoch400(y, m, d);
        // Assert
        Assert.Equal(daysSinceEpoch, actual);
    }
}
