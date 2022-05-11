// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

#if false

namespace Zorglub.Time.Core.Intervals
{
    public static partial class IntervalTests { }

#region Overlaps()

    [Fact]
    public static void Overlaps_Disjoint()
    {
        // [1, 2] and [3, 4]
        Assert.False(new Interval(1, 2).Overlaps(new Interval(3, 4)));
        Assert.False(new Interval(3, 4).Overlaps(new Interval(1, 2)));
    }

    [Fact]
    public static void Overlaps_Equal()
    {
        // {1} and {1}
        Assert.True(Interval.Of(1).Overlaps(Interval.Of(1)));
        // [1, 3] and [1, 3]
        Assert.True(new Interval(1, 3).Overlaps(new Interval(1, 3)));
    }

    [Fact]
    public static void Overlaps_Adjacent()
    {
        // [1, 2] and [2, 3]
        Assert.True(new Interval(1, 2).Overlaps(new Interval(2, 3)));
        Assert.True(new Interval(2, 3).Overlaps(new Interval(1, 2)));
    }

    [Fact]
    public static void Overlaps_ProperSubset()
    {
        // [1, 3] and {1}
        Assert.True(new Interval(1, 3).Overlaps(Interval.Of(1)));
        Assert.True(Interval.Of(1).Overlaps(new Interval(1, 3)));
        // [1, 3] and {2}
        Assert.True(new Interval(1, 3).Overlaps(Interval.Of(2)));
        Assert.True(Interval.Of(2).Overlaps(new Interval(1, 3)));
        // [1, 3] and {3}
        Assert.True(new Interval(1, 3).Overlaps(Interval.Of(3)));
        Assert.True(Interval.Of(3).Overlaps(new Interval(1, 3)));

        // [1, 3] and [1, 2]
        Assert.True(new Interval(1, 3).Overlaps(new Interval(1, 2)));
        Assert.True(new Interval(1, 2).Overlaps(new Interval(1, 3)));
        // [1, 3] and [2, 3]
        Assert.True(new Interval(1, 3).Overlaps(new Interval(2, 3)));
        Assert.True(new Interval(2, 3).Overlaps(new Interval(1, 3)));
    }

#endregion

#if false
#region Hull()

    [Fact]
    public static void Hull()
    {
        // {1} ⋃ {2}
        Assert.Equal(Interval0.Create(1, 2), Interval0.Create(1).Hull(Interval0.Create(2)));
        // {1} ⋃ {3}
        Assert.Equal(Interval0.Create(1, 3), Interval0.Create(1).Hull(Interval0.Create(3)));
        // {1} ⋃ [2, 3]
        Assert.Equal(Interval0.Create(1, 3), Interval0.Create(1).Hull(Interval0.Create(2, 3)));
        // {1} ⋃ [2, 4]
        Assert.Equal(Interval0.Create(1, 4), Interval0.Create(1).Hull(Interval0.Create(2, 4)));
        // [0, 1] ⋃ [2, 3]
        Assert.Equal(Interval0.Create(0, 3), Interval0.Create(0, 1).Hull(Interval0.Create(2, 3)));
        // [0, 1] ⋃ [2, 4]
        Assert.Equal(Interval0.Create(0, 4), Interval0.Create(0, 1).Hull(Interval0.Create(2, 4)));

        // [0, 1] ⋃ [3, 5]
        Assert.Equal(Interval0.Create(0, 5), Interval0.Create(0, 1).Hull(Interval0.Create(2, 5)));

        // [1, 5] ⋃ [2, 4]
        Assert.Equal(Interval0.Create(1, 5), Interval0.Create(1, 5).Hull(Interval0.Create(2, 4)));
        // [1, 5] ⋃ [2, 6]
        Assert.Equal(Interval0.Create(1, 6), Interval0.Create(1, 5).Hull(Interval0.Create(2, 6)));
    }

#endregion
#region Intersect()

    [Fact]
    public static void Intersect_Disjoint()
    {
        // [1, 2] ⋂ [3, 4]
        Assert.Null(Interval0.Create(1, 2).Intersect(Interval0.Create(3, 4)));
        Assert.Null(Interval0.Create(3, 4).Intersect(Interval0.Create(1, 2)));
    }

    [Fact]
    public static void Intersect_Equal()
    {
        // {1} ⋂ {1}
        Assert.Equal(Interval0.Create(1), Interval0.Create(1).Intersect(Interval0.Create(1)));
        // [1, 3] ⋂ [1, 3]
        Assert.Equal(Interval0.Create(1, 3), Interval0.Create(1, 3).Intersect(Interval0.Create(1, 3)));
    }

    [Fact]
    public static void Intersect_Adjacent()
    {
        // [1, 2] ⋂ [2, 3]
        Assert.Equal(Interval0.Create(2), Interval0.Create(1, 2).Intersect(Interval0.Create(2, 3)));
        Assert.Equal(Interval0.Create(2), Interval0.Create(2, 3).Intersect(Interval0.Create(1, 2)));
    }

    [Fact]
    public static void Intersect_ProperSubset()
    {
        // [1, 3] ⋂ {1}
        Assert.Equal(Interval0.Create(1), Interval0.Create(1, 3).Intersect(Interval0.Create(1)));
        Assert.Equal(Interval0.Create(1), Interval0.Create(1).Intersect(Interval0.Create(1, 3)));
        // [1, 3] ⋂ {2}
        Assert.Equal(Interval0.Create(2), Interval0.Create(1, 3).Intersect(Interval0.Create(2)));
        Assert.Equal(Interval0.Create(2), Interval0.Create(2).Intersect(Interval0.Create(1, 3)));
        // [1, 3] ⋂ {3}
        Assert.Equal(Interval0.Create(3), Interval0.Create(1, 3).Intersect(Interval0.Create(3)));
        Assert.Equal(Interval0.Create(3), Interval0.Create(3).Intersect(Interval0.Create(1, 3)));

        // [1, 3] ⋂ [1, 2]
        Assert.Equal(Interval0.Create(1, 2), Interval0.Create(1, 3).Intersect(Interval0.Create(1, 2)));
        Assert.Equal(Interval0.Create(1, 2), Interval0.Create(1, 2).Intersect(Interval0.Create(1, 3)));
        // [1, 3] ⋂ [2, 3]
        Assert.Equal(Interval0.Create(2, 3), Interval0.Create(1, 3).Intersect(Interval0.Create(2, 3)));
        Assert.Equal(Interval0.Create(2, 3), Interval0.Create(2, 3).Intersect(Interval0.Create(1, 3)));
    }

#endregion
#region Except()

    [Fact]
    public static void Except_Disjoint()
    {
        var inter = Interval0.Create(-1, 1);
        // Assert
        // [-1, 1] \ {2}
        Assert.Equal(inter, inter.Except(Interval0.Create(2)));
        // [-1, 1] \ {-2}
        Assert.Equal(inter, inter.Except(Interval0.Create(-2)));
        // [-1, 1] \ [2, 4]
        Assert.Equal(inter, inter.Except(Interval0.Create(2, 4)));
        // [-1, 1] \ [-4, -2]
        Assert.Equal(inter, inter.Except(Interval0.Create(-4, -2)));
    }

    [Fact]
    public static void Except_Equal()
    {
        // {1} \ {1}
        Assert.Null(Interval0.Create(1).Except(Interval0.Create(1)));
        // [-1, 1] \ [-1, 1]
        Assert.Null(Interval0.Create(-1, 1).Except(Interval0.Create(-1, 1)));
    }

    [Fact]
    public static void Except_Adjacent()
    {
        // {1} \ [1, 3]    = ∅
        Assert.Null(Interval0.Create(1).Except(Interval0.Create(1, 3)));
        // {-1} \ [-3, -1] = ∅
        Assert.Null(Interval0.Create(-1).Except(Interval0.Create(-3, -1)));

        // [-1, 1] \ [1, 3] = [-1, 0]
        Assert.Equal(Interval0.Create(-1, 0), Interval0.Create(-1, 1).Except(Interval0.Create(1, 3)));
        // [0, 1] \ [1, 3]  = {0}
        Assert.Equal(Interval0.Create(0), Interval0.Create(0, 1).Except(Interval0.Create(1, 3)));

        // [-1, 1] \ [-3, -1] = [0, 1]
        Assert.Equal(Interval0.Create(0, 1), Interval0.Create(-1, 1).Except(Interval0.Create(-3, -1)));
        // [-1, 0] \ [-3, -1] = {0}
        Assert.Equal(Interval0.Create(0), Interval0.Create(-1, 0).Except(Interval0.Create(-3, -1)));
    }

    [Fact]
    public static void Except_ProperSubset()
    {
        // [1, 3] \ {1} = [2, 3]
        Assert.Equal(Interval0.Create(2, 3), Interval0.Create(1, 3).Except(Interval0.Create(1)));
        // [1, 3] \ {2} = ∅
        Assert.Null(Interval0.Create(1, 3).Except(Interval0.Create(2)));
        // [1, 3] \ {3} = [1, 2]
        Assert.Equal(Interval0.Create(1, 2), Interval0.Create(1, 3).Except(Interval0.Create(3)));

        // [1, 3] \ [1, 2] = {3}
        Assert.Equal(Interval0.Create(3), Interval0.Create(1, 3).Except(Interval0.Create(1, 2)));
        // [1, 3] \ [2, 3] = {1}
        Assert.Equal(Interval0.Create(1), Interval0.Create(1, 3).Except(Interval0.Create(2, 3)));

        // [1, 10] \ [4, 5]  = ∅
        Assert.Null(Interval0.Create(1, 10).Except(Interval0.Create(4, 5)));
        // [1, 10] \ [1, 4]  = [5, 10]
        Assert.Equal(Interval0.Create(5, 10), Interval0.Create(1, 10).Except(Interval0.Create(1, 4)));
        // [1, 10] \ [9, 10] = [1, 8]
        Assert.Equal(Interval0.Create(1, 8), Interval0.Create(1, 10).Except(Interval0.Create(9, 10)));

        // [1, 10] \ [10, 15] = [1, 9]
        Assert.Equal(Interval0.Create(1, 9), Interval0.Create(1, 10).Except(Interval0.Create(10, 15)));
        // [1, 10] \ [9, 15]  = [1, 8]
        Assert.Equal(Interval0.Create(1, 8), Interval0.Create(1, 10).Except(Interval0.Create(9, 15)));
        // [1, 10] \ [3, 15]  = [1, 2]
        Assert.Equal(Interval0.Create(1, 2), Interval0.Create(1, 10).Except(Interval0.Create(3, 15)));
        // [1, 10] \ [2, 15]  = {1}
        Assert.Equal(Interval0.Create(1), Interval0.Create(1, 10).Except(Interval0.Create(2, 15)));
        // [1, 10] \ [1, 15]  = ∅
        Assert.Null(Interval0.Create(1, 10).Except(Interval0.Create(1, 15)));
        // [1, 10] \ [-1, 15] = ∅
        Assert.Null(Interval0.Create(1, 10).Except(Interval0.Create(-1, 15)));

        // [1, 10] \ [-10, 1] = [2, 10]
        Assert.Equal(Interval0.Create(2, 10), Interval0.Create(1, 10).Except(Interval0.Create(-10, 1)));
        // [1, 10] \ [-10, 2] = [3, 10]
        Assert.Equal(Interval0.Create(3, 10), Interval0.Create(1, 10).Except(Interval0.Create(-10, 2)));
        // [1, 10] \ [-10, 9] = {10}
        Assert.Equal(Interval0.Create(10), Interval0.Create(1, 10).Except(Interval0.Create(-10, 9)));
        // [1, 10] \ [-10, 10] = ∅
        Assert.Null(Interval0.Create(1, 10).Except(Interval0.Create(-10, 10)));
        // [1, 10] \ [-10, 11] = ∅
        Assert.Null(Interval0.Create(1, 10).Except(Interval0.Create(-10, 11)));
    }

#endregion
#endif
}

public static partial class IntervalTests
{
    // Intervalles égaux.
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 4)]
    public static void SetOperations_Equal(int min, int max)
    {
        var inter = new Interval(min, max);

        // Act & Assert

        Assert.False(inter.IsAdjacent(inter));
        Assert.False(inter.IsNearby(inter));
        Assert.True(inter.IsConnected(inter));
        Assert.Equal(0, Interval.Distance(inter, inter));

        // Intersection
        Assert.True(inter.Overlaps(inter));
        Assert.Equal(inter, inter.Intersect(inter));

        //// Union
        //Assert.Equal(inter, inter.Union(inter));

        // Hull
        Assert.Equal(inter, inter.Hull(inter));

        // Gap
        Assert.Null(inter.Gap(inter));

        //// Difference
        //Assert.Null(inter.Except(inter));

        //// Symmetric difference
        //Assert.Throws<NotImplementedException>(() => inter.SymmetricExcept(inter));
        ////Assert.Null(inter.SymmetricExcept(inter));
    }

    // Intervalle disjoint "éloigné".
    [Theory]
    [InlineData(9, 9)]
    [InlineData(6, 9)]
    public static void SetOperations_Distant(int min, int max)
    {
        var inter = new Interval(1, 4);
        var other = new Interval(min, max);

        // Act & Assert

        Assert.True(inter < other);
        Assert.True(other > inter);
        Assert.False(inter > other); // asymmetry
        Assert.False(other < inter); // asymmetry

        Assert.False(inter.IsAdjacent(other));
        Assert.False(inter.IsNearby(other));
        Assert.False(inter.IsConnected(other));
        Assert.Equal(min - 4, Interval.Distance(inter, other));
        // Flipped
        Assert.False(other.IsAdjacent(inter));
        Assert.False(other.IsNearby(inter));
        Assert.False(other.IsConnected(inter));
        Assert.Equal(min - 4, Interval.Distance(other, inter));

        // Intersection
        Assert.False(inter.Overlaps(other));
        Assert.Null(inter.Intersect(other));
        // Flipped
        Assert.False(other.Overlaps(inter));
        Assert.Null(other.Intersect(inter));

        //// Union = [1, 4] ∪ {9} or [1, 4] ∪ [6, 9]
        //Assert.Null(inter.Union(other));
        //// Flipped
        //Assert.Null(other.Union(inter));

        // Hull = [1, 9]
        Assert.Equal(new Interval(1, 9), inter.Hull(other));
        // Flipped
        Assert.Equal(new Interval(1, 9), other.Hull(inter));

        // Gap
        Assert.Equal(new Interval(5, min - 1), inter.Gap(other));

        //// Difference
        //Assert.Equal(inter, inter.Except(other));

        //// Symmetric difference = [1, 4] ∪ {9} or [1, 4] ∪ [6, 9]
        //Assert.Null(inter.SymmetricExcept(other));
    }

    // Intervalle disjoint dans le voisinage immédiat de droite.
    [Theory]
    [InlineData(5, 5)]
    [InlineData(5, 9)]
    public static void SetOperations_NearbyFromRight(int min, int max)
    {
        var inter = new Interval(1, 4);
        var other = new Interval(min, max);

        // Act & Assert

        Assert.True(inter < other);
        Assert.True(other > inter);
        Assert.False(inter > other); // asymmetry
        Assert.False(other < inter); // asymmetry

        Assert.False(inter.IsAdjacent(other));
        Assert.True(inter.IsNearby(other));
        Assert.True(inter.IsConnected(other));
        Assert.Equal(1, Interval.Distance(inter, other));
        // Flipped
        Assert.False(other.IsAdjacent(inter));
        Assert.True(other.IsNearby(inter));
        Assert.True(other.IsConnected(inter));
        Assert.Equal(1, Interval.Distance(other, inter));

        // Intersection
        Assert.False(inter.Overlaps(other));
        Assert.Null(inter.Intersect(other));
        // Flipped
        Assert.False(other.Overlaps(inter));
        Assert.Null(other.Intersect(inter));

        //// Union = [1, max]
        //Assert.Equal(Interval0.Create(1, max), inter.Union(other));
        //// Flipped
        //Assert.Equal(Interval0.Create(1, max), other.Union(inter));

        // Hull = [1, max]
        Assert.Equal(new Interval(1, max), inter.Hull(other));
        // Flipped
        Assert.Equal(new Interval(1, max), other.Hull(inter));

        // Gap
        Assert.Null(inter.Gap(other));

        //// Difference
        //Assert.Equal(inter, inter.Except(other));

        //// Symmetric difference = [1, max]
        //Assert.Equal(Interval0.Create(1, max), inter.SymmetricExcept(other));
    }

    // Intervalle adjacent à droite.
    [Theory]
    [InlineData(4, 4)]
    [InlineData(4, 9)]
    public static void SetOperations_AdjacentFromRight(int min, int max)
    {
        var inter = new Interval(1, 4);
        var other = new Interval(min, max);
        var intersection = Interval.Of(4);

        // Act & Assert

        Assert.False(inter < other);
        Assert.False(other > inter);
        Assert.False(inter > other);
        Assert.False(other < inter);

        Assert.True(inter.IsAdjacent(other));
        Assert.False(inter.IsNearby(other));
        Assert.True(inter.IsConnected(other));
        Assert.Equal(0, Interval.Distance(inter, other));
        // Flipped
        Assert.True(other.IsAdjacent(inter));
        Assert.False(other.IsNearby(inter));
        Assert.True(other.IsConnected(inter));
        Assert.Equal(0, Interval.Distance(other, inter));

        // Intersection = {4}
        Assert.True(inter.Overlaps(other));
        Assert.Equal(intersection, inter.Intersect(other));
        // Flipped
        Assert.True(other.Overlaps(inter));
        Assert.Equal(intersection, other.Intersect(inter));

        //// Union = [1, max]
        //Assert.Equal(Interval0.Create(1, max), inter.Union(other));
        //// Flipped
        //Assert.Equal(Interval0.Create(1, max), other.Union(inter));

        // Hull = [1, max]
        Assert.Equal(new Interval(1, max), inter.Hull(other));
        // Flipped
        Assert.Equal(new Interval(1, max), other.Hull(inter));

        // Gap
        Assert.Null(inter.Gap(other));

        //// Difference = [1, 3]
        //Assert.Equal(Interval0.Create(1, 3), inter.Except(other));

        //// Symmetric difference = [1, 3] or [1, 3] ∪ [5, 9]
        //Assert.Throws<NotImplementedException>(() => inter.SymmetricExcept(other));
        //if (max == 4)
        //{
        //    Assert.Equal(Interval.Of(1, 3), inter.SymmetricExcept(other));
        //}
        //else
        //{
        //    Assert.Null(inter.SymmetricExcept(other));
        //}
    }
}
}

#endif