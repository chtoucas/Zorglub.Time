// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Prototypes;

public sealed class GregorianPrototypeTests : PrototypalSchemaFacts<GregorianDataSet>
{
    public GregorianPrototypeTests() : base(GregorianPrototype.Instance)
    {
        // CountDaysInMonths() and CountDaysInYearBeforeMonth() use an array lookup.
        MaxMonth = 12;
    }

    [Fact]
    public void CountDaysInMonths_CommonYear()
    {
        var expected = new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        // Act
        var actual = GregorianPrototype.CountDaysInMonths(leapYear: false).ToArray();
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CountDaysInMonths_LeapYear()
    {
        var expected = new byte[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        // Act
        var actual = GregorianPrototype.CountDaysInMonths(leapYear: true).ToArray();
        // Assert
        Assert.Equal(expected, actual);
    }
}
