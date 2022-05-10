// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Utilities;

public static class UnitTests
{
    [Fact]
    public static void Constructor() =>
        Assert.Equal(default, new Unit());

    [Fact]
    public static void ToString_InvariantCulture() =>
        Assert.Equal("()", Unit.Value.ToString());

    [Fact]
    public static void DefaultValue() =>
        Assert.Equal(default, Unit.Value);

    [Fact]
    public static void Equals_WhenSame()
    {
        var unit = new Unit();
        var same = new Unit();

        // Act & Assert
        Assert.True(unit == same);
        Assert.False(unit != same);
        Assert.True(unit.Equals(same));
        Assert.True(unit.Equals((object)same));
    }

    [Fact]
    public static void Equals_ValueTuple()
    {
        var unit = new Unit();
        var tupl = new ValueTuple();
        // Act & Assert
        Assert.True(unit == tupl);
        Assert.True(tupl == unit);
        Assert.False(unit != tupl);
        Assert.False(tupl != unit);
        Assert.True(unit.Equals(tupl));
        Assert.True(unit.Equals((object)tupl));
    }

    [Fact]
    public static void Equals_NullOrOtherType()
    {
        var unit = new Unit();
        // Act & Assert
        Assert.False(unit.Equals(1));
        Assert.False(unit.Equals(null));
        Assert.False(unit.Equals(new object()));
    }

    [Fact]
    public static void HashCode() =>
        Assert.Equal(0, Unit.Value.GetHashCode());
}
