// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Samples;

using System.Runtime.InteropServices;

// Value types not visible to the F# test project.
public static class ValueTypeTests
{
    [Fact]
    public static void RuntimeSize()
    {
        Assert.Equal(4, Marshal.SizeOf(typeof(MyDate)));
        Assert.Equal(4, Marshal.SizeOf(typeof(MyCivilDate)));
        Assert.Equal(4, Marshal.SizeOf(typeof(CivilTriple)));
        Assert.Equal(12, Marshal.SizeOf(typeof(CivilParts)));
    }

    [Fact]
    public static void MyGregorianTriple_DefaultValue()
    {
        // Act
        var date = default(MyDate);
        // Assert
        Assert.Equal(1, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }

    [Fact]
    public static void MyCivilDate_DefaultValue()
    {
        // Act
        var date = default(MyCivilDate);
        // Assert
        Assert.Equal(1, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }

    [Fact]
    public static void CivilParts_DefaultValue()
    {
        // Act
        var date = default(CivilParts);
        // Assert
        Assert.Equal(0, date.Year);
        Assert.Equal(0, date.Month);
        Assert.Equal(0, date.Day);
    }

    [Fact]
    public static void CivilTriple_DefaultValue()
    {
        // Act
        var date = default(CivilTriple);
        // Assert
        Assert.Equal(1, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }
}
