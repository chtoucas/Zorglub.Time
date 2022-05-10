// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Intervals;

public static partial class IntervalTests { }

public partial class IntervalTests // Union, Coalesce, Span, etc.
{
    [Theory]
    [InlineData(1, 1, 1, 6)]
    [InlineData(1, 4, 2, 6)]
    public static void Range_Range_Overlapping(int min1, int max1, int min2, int max2)
    {
        var inter1 = Range.Create(min1, max1);
        var inter2 = Range.Create(min2, max2);
        var hull = Range.Create(min1, max2);
        // Act & Assert
        Assert.Equal(hull, Interval.Coalesce(inter1, inter2));
        Assert.Equal(hull, Interval.Span(inter1, inter2));
        Assert.True(Interval.Gap(inter1, inter2).IsEmpty);
        Assert.False(Interval.Disjoint(inter1, inter2));
        Assert.False(Interval.Adjacent(inter1, inter2));
        Assert.True(Interval.Connected(inter1, inter2));
        // Arg flipped
        Assert.Equal(hull, Interval.Coalesce(inter2, inter1));
        Assert.Equal(hull, Interval.Span(inter2, inter1));
        Assert.True(Interval.Gap(inter2, inter1).IsEmpty);
        Assert.False(Interval.Disjoint(inter2, inter1));
        Assert.False(Interval.Adjacent(inter2, inter1));
        Assert.True(Interval.Connected(inter2, inter1));
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(1, 4, 2)]
    public static void Range_LowerRay_Overlapping(int min1, int max1, int max2)
    {
        var inter1 = Range.Create(min1, max1);
        var inter2 = LowerRay.EndingAt(max2);
        var hull = LowerRay.EndingAt(max1);
        // Act & Assert
        Assert.Equal(hull, Interval.Coalesce(inter1, inter2));
        Assert.Equal(hull, Interval.Span(inter1, inter2));
        Assert.True(Interval.Gap(inter1, inter2).IsEmpty);
        Assert.False(Interval.Disjoint(inter1, inter2));
        Assert.False(Interval.Adjacent(inter1, inter2));
        Assert.True(Interval.Connected(inter1, inter2));
        // Arg flipped
        Assert.Equal(hull, Lavretni.Coalesce(inter2, inter1));
        Assert.Equal(hull, Lavretni.Span(inter2, inter1));
        Assert.True(Lavretni.Gap(inter2, inter1).IsEmpty);
        Assert.False(Lavretni.Disjoint(inter2, inter1));
        Assert.False(Lavretni.Adjacent(inter2, inter1));
        Assert.True(Lavretni.Connected(inter2, inter1));
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(1, 4, 2)]
    public static void Range_UpperRay_Overlapping(int min1, int max1, int min2)
    {
        var inter1 = Range.Create(min1, max1);
        var inter2 = UpperRay.StartingAt(min2);
        var hull = UpperRay.StartingAt(min1);
        // Act & Assert
        Assert.Equal(hull, Interval.Coalesce(inter1, inter2));
        Assert.Equal(hull, Interval.Span(inter1, inter2));
        Assert.True(Interval.Gap(inter1, inter2).IsEmpty);
        Assert.False(Interval.Disjoint(inter1, inter2));
        Assert.False(Interval.Adjacent(inter1, inter2));
        Assert.True(Interval.Connected(inter1, inter2));
        // Arg flipped
        Assert.Equal(hull, Lavretni.Coalesce(inter2, inter1));
        Assert.Equal(hull, Lavretni.Span(inter2, inter1));
        Assert.True(Lavretni.Gap(inter2, inter1).IsEmpty);
        Assert.False(Lavretni.Disjoint(inter2, inter1));
        Assert.False(Lavretni.Adjacent(inter2, inter1));
        Assert.True(Lavretni.Connected(inter2, inter1));
    }

    [Theory]
    [InlineData(1, 1, 2, 6)]
    [InlineData(1, 4, 5, 6)]
    public static void Range_Range_Adjacent(int min1, int max1, int min2, int max2)
    {
        var inter1 = Range.Create(min1, max1);
        var inter2 = Range.Create(min2, max2);
        var hull = Range.Create(min1, max2);
        // Act & Assert
        Assert.Equal(hull, Interval.Coalesce(inter1, inter2));
        Assert.Equal(hull, Interval.Span(inter1, inter2));
        Assert.True(Interval.Gap(inter1, inter2).IsEmpty);
        Assert.True(Interval.Disjoint(inter1, inter2));
        Assert.True(Interval.Adjacent(inter1, inter2));
        Assert.True(Interval.Connected(inter1, inter2));
        // Arg flipped
        Assert.Equal(hull, Interval.Coalesce(inter2, inter1));
        Assert.Equal(hull, Interval.Span(inter2, inter1));
        Assert.True(Interval.Gap(inter2, inter1).IsEmpty);
        Assert.True(Interval.Disjoint(inter2, inter1));
        Assert.True(Interval.Adjacent(inter2, inter1));
        Assert.True(Interval.Connected(inter2, inter1));
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(1, 4, 5)]
    public static void Range_UpperRay_Adjacent(int min1, int max1, int min2)
    {
        var inter1 = Range.Create(min1, max1);
        var inter2 = UpperRay.StartingAt(min2);
        var hull = UpperRay.StartingAt(min1);
        // Act & Assert
        Assert.Equal(hull, Interval.Coalesce(inter1, inter2));
        Assert.Equal(hull, Interval.Span(inter1, inter2));
        Assert.True(Interval.Gap(inter1, inter2).IsEmpty);
        Assert.True(Interval.Disjoint(inter1, inter2));
        Assert.True(Interval.Adjacent(inter1, inter2));
        Assert.True(Interval.Connected(inter1, inter2));
        // Arg flipped
        Assert.Equal(hull, Lavretni.Coalesce(inter2, inter1));
        Assert.Equal(hull, Lavretni.Span(inter2, inter1));
        Assert.True(Lavretni.Gap(inter2, inter1).IsEmpty);
        Assert.True(Lavretni.Disjoint(inter2, inter1));
        Assert.True(Lavretni.Adjacent(inter2, inter1));
        Assert.True(Lavretni.Connected(inter2, inter1));
    }

    [Theory]
    [InlineData(1, 1, 5, 6, 2, 4)]
    [InlineData(1, 4, 6, 6, 5, 5)]
    public static void Range_Range_Disjoint_NotAdjacent(
        int min1, int max1, int min2, int max2, int gmin, int gmax)
    {
        var inter1 = Range.Create(min1, max1);
        var inter2 = Range.Create(min2, max2);
        var hull = Range.Create(min1, max2);
        var gap = RangeSet.Create(gmin, gmax);
        // Act & Assert
        Assert.Null(Interval.Coalesce(inter1, inter2));
        Assert.Equal(hull, Interval.Span(inter1, inter2));
        Assert.Equal(gap, Interval.Gap(inter1, inter2));
        Assert.True(Interval.Disjoint(inter1, inter2));
        Assert.False(Interval.Adjacent(inter1, inter2));
        Assert.False(Interval.Connected(inter1, inter2));
        // Arg flipped
        Assert.Null(Interval.Coalesce(inter2, inter1));
        Assert.Equal(hull, Interval.Span(inter2, inter1));
        Assert.Equal(gap, Interval.Gap(inter2, inter1));
        Assert.True(Interval.Disjoint(inter2, inter1));
        Assert.False(Interval.Adjacent(inter2, inter1));
        Assert.False(Interval.Connected(inter2, inter1));
    }

    [Theory]
    [InlineData(1, 1, 5, 2, 4)]
    [InlineData(1, 4, 6, 5, 5)]
    public static void Range_UpperRay_Disjoint_NotAdjacent(
        int min1, int max1, int min2, int gmin, int gmax)
    {
        var inter1 = Range.Create(min1, max1);
        var inter2 = UpperRay.StartingAt(min2);
        var hull = UpperRay.StartingAt(min1);
        var gap = RangeSet.Create(gmin, gmax);
        // Act & Assert
        Assert.Null(Interval.Coalesce(inter1, inter2));
        Assert.Equal(hull, Interval.Span(inter1, inter2));
        Assert.Equal(gap, Interval.Gap(inter1, inter2));
        Assert.True(Interval.Disjoint(inter1, inter2));
        Assert.False(Interval.Adjacent(inter1, inter2));
        Assert.False(Interval.Connected(inter1, inter2));
        // Arg flipped
        Assert.Null(Lavretni.Coalesce(inter2, inter1));
        Assert.Equal(hull, Lavretni.Span(inter2, inter1));
        Assert.Equal(gap, Lavretni.Gap(inter2, inter1));
        Assert.True(Lavretni.Disjoint(inter2, inter1));
        Assert.False(Lavretni.Adjacent(inter2, inter1));
        Assert.False(Lavretni.Connected(inter2, inter1));
    }
}
