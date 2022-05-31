// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

public sealed class GregorianTroeschFormulaeTests : ICalendricalFormulaeFacts<GregorianDataSet>
{
    public GregorianTroeschFormulaeTests() : base(new GregorianTroeschFormulae()) { }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysSinceEpoch400(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;

        // Act
        int actual = GregorianTroeschFormulae.CountDaysSinceEpoch400(y, m, d);
        // Assert
        Assert.Equal(daysSinceEpoch, actual);
    }
}
