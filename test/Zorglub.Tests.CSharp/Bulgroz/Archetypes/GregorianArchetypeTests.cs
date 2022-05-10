// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Archetypes;

public sealed class GregorianArchetypeTests : ArchetypalSchemaFacts<GregorianDataSet>
{
    public GregorianArchetypeTests() : base(GregorianArchetype.Instance)
    {
        // CountDaysInMonths() and CountDaysInYearBeforeMonth() use an array lookup.
        MaxMonth = 12;
    }

    [Fact]
    public void CountDaysInMonths_CommonYear()
    {
        // Arrange
        var expected = new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        // Act
        var actual = GregorianArchetype.CountDaysInMonths(leapYear: false).ToArray();
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CountDaysInMonths_LeapYear()
    {
        // Arrange
        var expected = new byte[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        // Act
        var actual = GregorianArchetype.CountDaysInMonths(leapYear: true).ToArray();
        // Assert
        Assert.Equal(expected, actual);
    }
}
