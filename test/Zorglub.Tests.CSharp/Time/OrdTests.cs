﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

public static class OrdTests
{
    [Fact]
    public static void Increment_Overflows_AtMaxValue()
    {
        var copy = Ord.MaxValue;
        Assert.Overflows(() => copy++);
    }

    [Fact]
    public static void Increment()
    {
        // Arrange
        var ord = Ord.FromRank(345);
        var ordAfter = Ord.FromRank(346);
        // Act & Assert
        var copy = ord;
        copy++;
        Assert.Equal(ordAfter, copy);
    }

    [Fact]
    public static void Decrement_Overflows_AtMinValue()
    {
        var copy = Ord.MinValue;
        Assert.Overflows(() => copy--);
    }

    [Fact]
    public static void Decrement()
    {
        // Arrange
        var ord = Ord.FromRank(345);
        var ordBefore = Ord.FromRank(344);
        // Act & Assert
        var copy = ord;
        copy--;
        Assert.Equal(ordBefore, copy);
    }
}
