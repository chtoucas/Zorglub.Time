// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Samples;

using System.Runtime.InteropServices;

using global::Samples;

// Value types not visible to the F# test project.
public static class ValueTypeTests
{
    [Fact]
    public static void RuntimeSize()
    {
        Assert.Equal(4, Marshal.SizeOf(typeof(DateTriple)));
        Assert.Equal(4, Marshal.SizeOf(typeof(DateTemplate)));
        Assert.Equal(4, Marshal.SizeOf(typeof(CivilTriple)));
        Assert.Equal(12, Marshal.SizeOf(typeof(CivilParts)));
    }

    [Fact]
    public static void DateTriple_DefaultValue()
    {
        // Act
        var date = default(DateTriple);
        // Assert
        Assert.Equal(1, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }

    [Fact]
    public static void DateTemplate_DefaultValue()
    {
        // Act
        var date = default(DateTemplate);
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
