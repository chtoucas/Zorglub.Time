// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Intervals;

public static partial class IntervalDoubleTests
{
    [Fact]
    public static void Empty_Prop()
    {
        // Act
        var inter = IntervalDouble.Empty;
        // Assert
        Assert.True(inter.Boundary.IsEmpty);
        Assert.Equal(0d, inter.Width);
        Assert.True(inter.IsEmpty);
        Assert.False(inter.IsSingleton);
        Assert.False(inter.IsProper);
        Assert.True(inter.IsLeftOpen);
        Assert.True(inter.IsRightOpen);
        Assert.True(inter.IsLeftBounded);
        Assert.True(inter.IsRightBounded);

        Assert.Equal(IntervalFormat.Empty, inter.ToString());

        Assert.False(inter.Contains(0d));
        Assert.False(inter.Contains(1d));
        Assert.False(inter.Contains(3d));
        Assert.False(inter.Contains(5d));
        Assert.False(inter.Contains(6d));
    }

    [Fact]
    public static void Maximal_Prop()
    {
        // Arrange
        var boundary = IntervalBoundary.Create(Double.MinValue, Double.MaxValue);
        double width = Double.MaxValue - Double.MinValue + 1;
        // Act
        var inter = IntervalDouble.Maximal;
        // Assert
        Assert.Equal(boundary, inter.Boundary);
        Assert.Equal(width, inter.Width);
        Assert.False(inter.IsEmpty);
        Assert.False(inter.IsSingleton);
        Assert.True(inter.IsProper);
        Assert.False(inter.IsLeftOpen);
        Assert.False(inter.IsRightOpen);
        Assert.True(inter.IsLeftBounded);
        Assert.True(inter.IsRightBounded);

        Assert.True(inter.Contains(0d));
        Assert.True(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.True(inter.Contains(5d));
        Assert.True(inter.Contains(6d));
        Assert.False(inter.Contains(Double.NegativeInfinity));
        Assert.False(inter.Contains(Double.PositiveInfinity));
    }

    [Fact]
    public static void Unbounded_Prop()
    {
        // Act
        var inter = IntervalDouble.Unbounded;
        // Assert
        Assert.True(inter.Boundary.IsEmpty);
        Assert.Equal(Double.PositiveInfinity, inter.Width);
        Assert.False(inter.IsEmpty);
        Assert.False(inter.IsSingleton);
        Assert.True(inter.IsProper);
        Assert.True(inter.IsLeftOpen);
        Assert.True(inter.IsRightOpen);
        Assert.False(inter.IsLeftBounded);
        Assert.False(inter.IsRightBounded);

        Assert.True(inter.Contains(0d));
        Assert.True(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.True(inter.Contains(5d));
        Assert.True(inter.Contains(6d));
        Assert.False(inter.Contains(Double.NegativeInfinity));
        Assert.False(inter.Contains(Double.PositiveInfinity));
    }
}

public partial class IntervalDoubleTests // Factories
{
    [Fact]
    public static void Create_Empty()
    {
        Assert.True(IntervalDouble.Create(0d, EndpointType.Open, 0d, EndpointType.Open).IsEmpty);
        Assert.True(IntervalDouble.Create(0d, EndpointType.Open, 0d, EndpointType.Closed).IsEmpty);
        Assert.True(IntervalDouble.Create(0d, EndpointType.Closed, 0d, EndpointType.Open).IsEmpty);
    }

    [Fact]
    public static void Open_Empty() =>
        Assert.True(IntervalDouble.Open(0d, 0d).IsEmpty);

    [Fact]
    public static void LeftOpen_Empty() =>
        Assert.True(IntervalDouble.LeftOpen(0d, 0d).IsEmpty);

    [Fact]
    public static void RightOpen_Empty() =>
        Assert.True(IntervalDouble.RightOpen(0d, 0d).IsEmpty);

    [Fact]
    public static void Closed()
    {
        // Arrange
        var boundary = IntervalBoundary.Create(1d, 5d);
        // Act
        var inter = IntervalDouble.Closed(1d, 5d);
        // Assert
        Assert.Equal(boundary, inter.Boundary);
        Assert.Equal(4d, inter.Width);
        Assert.False(inter.IsEmpty);
        Assert.False(inter.IsSingleton);
        Assert.True(inter.IsProper);
        Assert.False(inter.IsLeftOpen);
        Assert.False(inter.IsRightOpen);
        Assert.True(inter.IsLeftBounded);
        Assert.True(inter.IsRightBounded);

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
        var boundary = IntervalBoundary.Create(1d, 5d);
        // Act
        var inter = IntervalDouble.Open(1d, 5d);
        // Assert
        Assert.Equal(boundary, inter.Boundary);
        Assert.Equal(4d, inter.Width);
        Assert.False(inter.IsEmpty);
        Assert.False(inter.IsSingleton);
        Assert.True(inter.IsProper);
        Assert.True(inter.IsLeftOpen);
        Assert.True(inter.IsRightOpen);
        Assert.True(inter.IsLeftBounded);
        Assert.True(inter.IsRightBounded);

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
        var boundary = IntervalBoundary.Create(1d, 5d);
        // Act
        var inter = IntervalDouble.LeftOpen(1d, 5d);
        // Assert
        Assert.Equal(boundary, inter.Boundary);
        Assert.Equal(4d, inter.Width);
        Assert.False(inter.IsEmpty);
        Assert.False(inter.IsSingleton);
        Assert.True(inter.IsProper);
        Assert.True(inter.IsLeftOpen);
        Assert.False(inter.IsRightOpen);
        Assert.True(inter.IsLeftBounded);
        Assert.True(inter.IsRightBounded);

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
        var boundary = IntervalBoundary.Create(1d, 5d);
        // Act
        var inter = IntervalDouble.RightOpen(1d, 5d);
        // Assert
        Assert.Equal(boundary, inter.Boundary);
        Assert.Equal(4d, inter.Width);
        Assert.False(inter.IsEmpty);
        Assert.False(inter.IsSingleton);
        Assert.True(inter.IsProper);
        Assert.False(inter.IsLeftOpen);
        Assert.True(inter.IsRightOpen);
        Assert.True(inter.IsLeftBounded);
        Assert.True(inter.IsRightBounded);

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
        var boundary = IntervalBoundary.Create(3d, 3d);
        // Act
        var inter = IntervalDouble.Singleton(3d);
        // Assert
        Assert.Equal(boundary, inter.Boundary);
        Assert.Equal(0d, inter.Width);
        Assert.False(inter.IsEmpty);
        Assert.True(inter.IsSingleton);
        Assert.False(inter.IsProper);
        Assert.False(inter.IsLeftOpen);
        Assert.False(inter.IsRightOpen);
        Assert.True(inter.IsLeftBounded);
        Assert.True(inter.IsRightBounded);

        Assert.Equal("[3, 3]", inter.ToString());

        Assert.False(inter.Contains(0d));
        Assert.False(inter.Contains(1d));
        Assert.True(inter.Contains(3d));
        Assert.False(inter.Contains(5d));
        Assert.False(inter.Contains(6d));
    }
}

public partial class IntervalDoubleTests //
{
    [Fact]
    public static void Span_Closed()
    {
        // Act
        var inter = IntervalDouble.Closed(1d, 5d);

        // Assert
        // Closed
        Assert.SetEqual(IntervalDouble.Closed(0d, 5d), inter.Span(IntervalDouble.Closed(0d, .5d)));
        Assert.SetEqual(IntervalDouble.Closed(0d, 5d), inter.Span(IntervalDouble.Closed(0d, 1d)));
        Assert.SetEqual(IntervalDouble.Closed(0d, 5d), inter.Span(IntervalDouble.Closed(0d, 2d)));
        Assert.SetEqual(inter, inter.Span(IntervalDouble.Closed(1d, 2d)));
        Assert.SetEqual(inter, inter.Span(IntervalDouble.Closed(4d, 5d)));
        Assert.SetEqual(IntervalDouble.Closed(1d, 6d), inter.Span(IntervalDouble.Closed(4d, 6d)));
        Assert.SetEqual(IntervalDouble.Closed(1d, 6d), inter.Span(IntervalDouble.Closed(5d, 6d)));
        Assert.SetEqual(IntervalDouble.Closed(1d, 6d), inter.Span(IntervalDouble.Closed(5.5d, 6d)));

        // Open
        Assert.SetEqual(IntervalDouble.LeftOpen(0d, 5d), inter.Span(IntervalDouble.Open(0d, .5d)));
        Assert.SetEqual(IntervalDouble.LeftOpen(0d, 5d), inter.Span(IntervalDouble.Open(0d, 1d)));
        Assert.SetEqual(IntervalDouble.LeftOpen(0d, 5d), inter.Span(IntervalDouble.Open(0d, 2d)));
        Assert.SetEqual(inter, inter.Span(IntervalDouble.Open(1d, 2d)));
        Assert.SetEqual(inter, inter.Span(IntervalDouble.Open(4d, 5d)));
        Assert.SetEqual(IntervalDouble.RightOpen(1d, 6d), inter.Span(IntervalDouble.Open(4d, 6d)));
        Assert.SetEqual(IntervalDouble.RightOpen(1d, 6d), inter.Span(IntervalDouble.Open(5d, 6d)));
        Assert.SetEqual(IntervalDouble.RightOpen(1d, 6d), inter.Span(IntervalDouble.Open(5.5d, 6d)));
    }

    [Fact]
    public static void Intersect_Closed()
    {
        // Act
        var inter = IntervalDouble.Closed(1d, 5d);

        // Assert
        // Closed
        Assert.True(inter.Intersect(IntervalDouble.Closed(0d, .5d)).IsEmpty);
        Assert.SetEqual(IntervalDouble.Closed(1d, 1d), inter.Intersect(IntervalDouble.Closed(0d, 1d)));
        Assert.SetEqual(IntervalDouble.Closed(1d, 2d), inter.Intersect(IntervalDouble.Closed(0d, 2d)));
        Assert.SetEqual(IntervalDouble.Closed(1d, 2d), inter.Intersect(IntervalDouble.Closed(1d, 2d)));
        Assert.SetEqual(IntervalDouble.Closed(4d, 5d), inter.Intersect(IntervalDouble.Closed(4d, 5d)));
        Assert.SetEqual(IntervalDouble.Closed(4d, 5d), inter.Intersect(IntervalDouble.Closed(4d, 6d)));
        Assert.SetEqual(IntervalDouble.Closed(5d, 5d), inter.Intersect(IntervalDouble.Closed(5d, 6d)));
        Assert.True(inter.Intersect(IntervalDouble.Closed(5.5d, 6d)).IsEmpty);

        // Open
        Assert.True(inter.Intersect(IntervalDouble.Open(0d, .5d)).IsEmpty);
        Assert.True(inter.Intersect(IntervalDouble.Open(0d, 1d)).IsEmpty);
        Assert.SetEqual(IntervalDouble.RightOpen(1d, 2d), inter.Intersect(IntervalDouble.Open(0d, 2d)));
        Assert.SetEqual(IntervalDouble.Open(1d, 2d), inter.Intersect(IntervalDouble.Open(1d, 2d)));
        Assert.SetEqual(IntervalDouble.Open(4d, 5d), inter.Intersect(IntervalDouble.Open(4d, 5d)));
        Assert.SetEqual(IntervalDouble.LeftOpen(4d, 5d), inter.Intersect(IntervalDouble.Open(4d, 6d)));
        Assert.True(inter.Intersect(IntervalDouble.Open(5d, 6d)).IsEmpty);
        Assert.True(inter.Intersect(IntervalDouble.Open(5.5d, 6d)).IsEmpty);
    }
}
