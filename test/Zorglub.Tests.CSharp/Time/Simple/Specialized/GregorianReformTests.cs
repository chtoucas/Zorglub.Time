// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple.Specialized;

public static class GregorianReformTests
{
    [Fact]
    public static void Official_SecularShift() =>
        Assert.Equal(10, GregorianReform.Official.SecularShift);

    [Fact]
    public static void Official_FromLastJulianDate()
    {
        // Arrange
        var reform = GregorianReform.FromLastJulianDate(1582, 10, 4);
        // Assert
        Assert.Equal(reform, GregorianReform.Official);
    }

    [Fact]
    public static void Official_FromFirstGregorianDate()
    {
        // Arrange
        var reform = GregorianReform.FromFirstGregorianDate(1582, 10, 15);
        // Assert
        Assert.Equal(reform, GregorianReform.Official);
    }
}
