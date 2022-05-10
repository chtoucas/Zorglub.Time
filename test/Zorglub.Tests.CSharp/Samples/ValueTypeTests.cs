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
        Assert.Equal(4, Marshal.SizeOf(typeof(DateTemplate)));
        Assert.Equal(4, Marshal.SizeOf(typeof(DayTemplate)));
        Assert.Equal(4, Marshal.SizeOf(typeof(GregorianTriple)));
        Assert.Equal(4, Marshal.SizeOf(typeof(MyDate)));
        Assert.Equal(12, Marshal.SizeOf(typeof(GregorianRecord)));
    }

    [Fact]
    public static void DateTemplate_DefaultValue()
    {
        // Act
        var date = default(DateTemplate);
        // Assert
        Assert.Equal(0, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }

    [Fact]
    public static void DayTemplate_DefaultValue()
    {
        // Act
        var date = default(DayTemplate);
        // Assert
        Assert.Equal(1, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }

    [Fact]
    public static void GregorianRecord_DefaultValue()
    {
        // Act
        var date = default(GregorianRecord);
        // Assert
        Assert.Equal(0, date.Year);
        Assert.Equal(0, date.Month);
        Assert.Equal(0, date.Day);
    }

    [Fact]
    public static void GregorianTriple_DefaultValue()
    {
        // Act
        var date = default(GregorianTriple);
        // Assert
        Assert.Equal(0, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }

    [Fact]
    public static void MyDate_DefaultValue()
    {
        // Act
        var date = default(MyDate);
        // Assert
        Assert.Equal(0, date.Year);
        Assert.Equal(1, date.Month);
        Assert.Equal(1, date.Day);
    }
}
