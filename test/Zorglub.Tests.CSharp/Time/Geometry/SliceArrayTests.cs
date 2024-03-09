// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Discrete;

public static partial class SliceArrayTests { }

public partial class SliceArrayTests
{
    // Palier initial négligé.
    [Fact]
    public static void RemoveMinorExternals1()
    {
        var list = new SliceArray([2, 3, 4], true);
        var exp = new CodeArray([3, 4]);
        // Act
        var actual = list.RemoveMinorExternals(out int g);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(2, g);
    }

    // Palier initial négligé.
    // Palier final complet.
    [Fact]
    public static void RemoveMinorExternals2()
    {
        var list = new SliceArray([2, 3, 5], false);
        var exp = new CodeArray([3, 5]);
        // Act
        var actual = list.RemoveMinorExternals(out int g);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(2, g);
    }

    // Palier initial gardé.
    [Fact]
    public static void RemoveMinorExternals3()
    {
        var list = new SliceArray([3, 2, 4], true);
        var exp = new CodeArray([3, 2, 4]);
        // Act
        var actual = list.RemoveMinorExternals(out int g);
        // Assert
        Assert.Equal(exp, actual);
        Assert.Equal(0, g);
    }
}
