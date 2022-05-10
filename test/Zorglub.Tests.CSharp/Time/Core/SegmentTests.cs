// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Utilities;

public static partial class SegmentTests { }

public partial class SegmentTests // Factories
{
    [Fact]
    public static void Create_InvalidEnd()
    {
        Assert.ThrowsAoorexn("end", () => Segment.Create(0d, EndpointType.Open, 0d, EndpointType.Open));
        Assert.ThrowsAoorexn("end", () => Segment.Create(0d, EndpointType.Open, 0d, EndpointType.Closed));
        Assert.ThrowsAoorexn("end", () => Segment.Create(0d, EndpointType.Closed, 0d, EndpointType.Open));
    }

    [Fact]
    public static void Open_InvalidEnd() =>
        Assert.ThrowsAoorexn("end", () => Segment.Open(0d, 0d));

    [Fact]
    public static void LeftOpen_InvalidEnd() =>
        Assert.ThrowsAoorexn("end", () => Segment.LeftOpen(0d, 0d));

    [Fact]
    public static void RightOpen_InvalidEnd() =>
        Assert.ThrowsAoorexn("end", () => Segment.RightOpen(0d, 0d));

    [Fact]
    public static void Closed()
    {
        // Arrange
        var endpoints = OrderedPair.Create(1d, 5d);
        // Act
        var inter = Segment.Closed(1d, 5d);
        // Assert
        Assert.Equal(endpoints, inter.Endpoints);
        Assert.Equal(1d, inter.LowerEnd);
        Assert.Equal(5d, inter.UpperEnd);
        Assert.False(inter.IsLeftOpen);
        Assert.False(inter.IsRightOpen);

        Assert.Equal("[1, 5]", inter.ToString());

        Assert.False(inter.Contains(0d));
        Assert.True(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.True(inter.Contains(5d));
        Assert.False(inter.Contains(6d));
    }

    [Fact]
    public static void Open()
    {
        // Arrange
        var endpoints = OrderedPair.Create(1d, 5d);
        // Act
        var inter = Segment.Open(1d, 5d);
        // Assert
        Assert.Equal(endpoints, inter.Endpoints);
        Assert.Equal(1d, inter.LowerEnd);
        Assert.Equal(5d, inter.UpperEnd);
        Assert.True(inter.IsLeftOpen);
        Assert.True(inter.IsRightOpen);

        Assert.Equal("]1, 5[", inter.ToString());

        Assert.False(inter.Contains(0d));
        Assert.False(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.False(inter.Contains(5d));
        Assert.False(inter.Contains(6d));
    }

    [Fact]
    public static void LeftOpen()
    {
        // Arrange
        var endpoints = OrderedPair.Create(1d, 5d);
        // Act
        var inter = Segment.LeftOpen(1d, 5d);
        // Assert
        Assert.Equal(endpoints, inter.Endpoints);
        Assert.Equal(1d, inter.LowerEnd);
        Assert.Equal(5d, inter.UpperEnd);
        Assert.True(inter.IsLeftOpen);
        Assert.False(inter.IsRightOpen);

        Assert.Equal("]1, 5]", inter.ToString());

        Assert.False(inter.Contains(0d));
        Assert.False(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.True(inter.Contains(5d));
        Assert.False(inter.Contains(6d));
    }

    [Fact]
    public static void RightOpen()
    {
        // Arrange
        var endpoints = OrderedPair.Create(1d, 5d);
        // Act
        var inter = Segment.RightOpen(1d, 5d);
        // Assert
        Assert.Equal(endpoints, inter.Endpoints);
        Assert.Equal(1d, inter.LowerEnd);
        Assert.Equal(5d, inter.UpperEnd);
        Assert.False(inter.IsLeftOpen);
        Assert.True(inter.IsRightOpen);

        Assert.Equal("[1, 5[", inter.ToString());

        Assert.False(inter.Contains(0d));
        Assert.True(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.False(inter.Contains(5d));
        Assert.False(inter.Contains(6d));
    }

    [Fact]
    public static void Singleton()
    {
        // Arrange
        var endpoints = OrderedPair.Create(3d, 3d);
        // Act
        var inter = Segment.Singleton(3d);
        // Assert
        Assert.Equal(endpoints, inter.Endpoints);
        Assert.Equal(3d, inter.LowerEnd);
        Assert.Equal(3d, inter.UpperEnd);
        Assert.False(inter.IsLeftOpen);
        Assert.False(inter.IsRightOpen);

        Assert.Equal("[3, 3]", inter.ToString());

        Assert.False(inter.Contains(0d));
        Assert.False(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.False(inter.Contains(5d));
        Assert.False(inter.Contains(6d));
    }
}
