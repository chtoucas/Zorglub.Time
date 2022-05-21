// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

public static class OrdTests
{
    [Fact]
    public static void Increment_Overflows_AtMaxValue()
    {
        var max = Ord.MaxValue;
        Assert.Overflows(() => max++);
        Assert.Overflows(() => ++max);
    }

    [Fact]
    public static void Increment()
    {
        var ord = Ord.FromRank(345);
        var ordAfter = Ord.FromRank(346);
        // Act & Assert
        var copy = ord;
        copy++;
        Assert.Equal(ordAfter, copy);
        Assert.Equal(ordAfter, ++ord);
    }

    [Fact]
    public static void Decrement_Overflows_AtMinValue()
    {
        var min = Ord.MinValue;
        Assert.Overflows(() => min--);
        Assert.Overflows(() => --min);
    }

    [Fact]
    public static void Decrement()
    {
        var ord = Ord.FromRank(345);
        var ordBefore = Ord.FromRank(344);
        // Act & Assert
        var copy = ord;
        copy--;
        Assert.Equal(ordBefore, copy);
        Assert.Equal(ordBefore, --ord);
    }
}
