// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Discrete;

public static partial class BoolArrayTests
{
    private static BoolArray CreateArray(int[] arr)
    {
        Debug.Assert(Array.TrueForAll(arr, n => n == 0 || n == 1));

        var newArr = new bool[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            newArr[i] = arr[i] == 1;
        }
        return new BoolArray(newArr);
    }
}

public partial class BoolArrayTests
{
    [Fact]
    public static void IsTrueIsolated()
    {
        Assert.True(CreateArray([0]).IsTrueIsolated());
        Assert.True(CreateArray([1]).IsTrueIsolated());

        Assert.True(CreateArray([0, 0]).IsTrueIsolated());
        Assert.True(CreateArray([1, 0]).IsTrueIsolated());
        Assert.True(CreateArray([0, 1]).IsTrueIsolated());
        Assert.False(CreateArray([1, 1]).IsTrueIsolated());

        Assert.True(CreateArray([0, 0, 0]).IsTrueIsolated());
        Assert.True(CreateArray([1, 0, 0]).IsTrueIsolated());
        Assert.True(CreateArray([0, 1, 0]).IsTrueIsolated());
        Assert.True(CreateArray([0, 0, 1]).IsTrueIsolated());
        Assert.True(CreateArray([1, 0, 1]).IsTrueIsolated());
        Assert.False(CreateArray([1, 1, 0]).IsTrueIsolated());
        Assert.False(CreateArray([0, 1, 1]).IsTrueIsolated());
        Assert.False(CreateArray([1, 1, 1]).IsTrueIsolated());

        Assert.True(CreateArray([0, 0, 0, 0]).IsTrueIsolated());
        Assert.True(CreateArray([1, 0, 0, 0]).IsTrueIsolated());
        Assert.True(CreateArray([0, 1, 0, 0]).IsTrueIsolated());
        Assert.True(CreateArray([0, 0, 1, 0]).IsTrueIsolated());
        Assert.True(CreateArray([0, 0, 0, 1]).IsTrueIsolated());
        Assert.True(CreateArray([1, 0, 1, 0]).IsTrueIsolated());
        Assert.True(CreateArray([1, 0, 0, 1]).IsTrueIsolated());
        Assert.True(CreateArray([0, 1, 0, 1]).IsTrueIsolated());
        Assert.False(CreateArray([1, 0, 1, 1]).IsTrueIsolated());
        Assert.False(CreateArray([1, 1, 0, 0]).IsTrueIsolated());
        Assert.False(CreateArray([1, 1, 1, 0]).IsTrueIsolated());
        Assert.False(CreateArray([1, 1, 0, 1]).IsTrueIsolated());
        Assert.False(CreateArray([1, 1, 1, 1]).IsTrueIsolated());
        Assert.False(CreateArray([0, 1, 1, 0]).IsTrueIsolated());
        Assert.False(CreateArray([0, 1, 1, 1]).IsTrueIsolated());
        Assert.False(CreateArray([0, 0, 1, 1]).IsTrueIsolated());
    }

    [Fact]
    public static void Negate()
    {
        var code = CreateArray([0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1]);
        var exp = CreateArray([1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0]);
        // Act & Assert
        Assert.Equal(exp, code.Negate());
    }
}

// Slice().
public partial class BoolArrayTests
{
    [Fact]
    public static void Slice_0()
    {
        var code = CreateArray([0]);
        var exp = new SliceArray([2], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_00()
    {
        var code = CreateArray([0, 0]);
        var exp = new SliceArray([3], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_001()
    {
        var code = CreateArray([0, 0, 1]);
        var exp = new SliceArray([3], true);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_0010()
    {
        var code = CreateArray([0, 0, 1, 0]);
        var exp = new SliceArray([3, 2], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_0011()
    {
        var code = CreateArray([0, 0, 1, 1]);
        var exp = new SliceArray([3, 1], true);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_00100()
    {
        var code = CreateArray([0, 0, 1, 0, 0]);
        var exp = new SliceArray([3, 3], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_00101()
    {
        var code = CreateArray([0, 0, 1, 0, 1]);
        var exp = new SliceArray([3, 2], true);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_1()
    {
        var code = CreateArray([1]);
        var exp = new SliceArray([1], true);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_10()
    {
        var code = CreateArray([1, 0]);
        var exp = new SliceArray([1, 2], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_11()
    {
        var code = CreateArray([1, 1]);
        var exp = new SliceArray([1, 1], true);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_100()
    {
        var code = CreateArray([1, 0, 0]);
        var exp = new SliceArray([1, 3], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_A()
    {
        var code = CreateArray([1, 0, 1, 1]);
        var exp = new SliceArray([1, 2, 1], true);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_B()
    {
        var code = CreateArray([0, 0, 0, 1, 0, 1, 1]);
        var exp = new SliceArray([4, 2, 1], true);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_C()
    {
        var code = CreateArray([1, 0, 1, 0]);
        var exp = new SliceArray([1, 2, 2], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }

    [Fact]
    public static void Slice_D()
    {
        var code = CreateArray([1, 0, 1, 0, 0, 0, 0]);
        var exp = new SliceArray([1, 2, 5], false);
        // Act & Assert
        Assert.Equal(exp, code.Slice());
    }
}
