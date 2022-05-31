// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using Zorglub.Time.Core.Intervals;

public sealed record RangePairInfo(
    Range<int> First,
    Range<int> Second,
    Range<int> Span,
    RangeSet<int> Intersection,
    RangeSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public sealed record LowerRayRangeInfo(
    LowerRay<int> First,
    Range<int> Second,
    LowerRay<int> Span,
    RangeSet<int> Intersection,
    RangeSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public sealed record UpperRayRangeInfo(
    UpperRay<int> First,
    Range<int> Second,
    UpperRay<int> Span,
    RangeSet<int> Intersection,
    RangeSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public sealed record LowerRayUpperRayInfo(
    LowerRay<int> First,
    UpperRay<int> Second,
    RangeSet<int> Intersection,
    RangeSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public static class IntervalDataSet
{
    public static DataGroup<RangePairInfo> RangePairInfoData { get; } = new()
    {
        //
        // Overlapping ranges
        //

        // Equal (degenerate)
        new(new(1, 1), new(1, 1), new(1, 1), new(1, 1), RangeSet<int>.Empty, false, false, true),
        // Equal (non-degenerate)
        new(new(1, 4), new(1, 4), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        // Strict subset (degenerate)
        new(new(1, 1), new(1, 4), new(1, 4), new(1, 1), RangeSet<int>.Empty, false, false, true),
        new(new(2, 2), new(1, 4), new(1, 4), new(2, 2), RangeSet<int>.Empty, false, false, true),
        new(new(3, 3), new(1, 4), new(1, 4), new(3, 3), RangeSet<int>.Empty, false, false, true),
        new(new(4, 4), new(1, 4), new(1, 4), new(4, 4), RangeSet<int>.Empty, false, false, true),
        // Strict subset (non-degenerate)
        new(new(1, 2), new(1, 4), new(1, 4), new(1, 2), RangeSet<int>.Empty, false, false, true),
        new(new(1, 3), new(1, 4), new(1, 4), new(1, 3), RangeSet<int>.Empty, false, false, true),
        new(new(2, 3), new(1, 4), new(1, 4), new(2, 3), RangeSet<int>.Empty, false, false, true),
        new(new(2, 4), new(1, 4), new(1, 4), new(2, 4), RangeSet<int>.Empty, false, false, true),
        new(new(3, 4), new(1, 4), new(1, 4), new(3, 4), RangeSet<int>.Empty, false, false, true),
        // Other non-disjoint cases
        new(new(1, 4), new(4, 7), new(1, 7), new(4, 4), RangeSet<int>.Empty, false, false, true),
        new(new(1, 4), new(3, 7), new(1, 7), new(3, 4), RangeSet<int>.Empty, false, false, true),

        //
        // Disjoint ranges
        //

        // Disjoint and connected
        new(new(1, 4), new(5, 5), new(1, 5), RangeSet<int>.Empty, RangeSet<int>.Empty, true, true, true),
        new(new(1, 4), new(5, 7), new(1, 7), RangeSet<int>.Empty, RangeSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(1, 1), new(4, 4), new(1, 4), RangeSet<int>.Empty, new(2, 3), true, false, false),
        new(new(1, 1), new(4, 7), new(1, 7), RangeSet<int>.Empty, new(2, 3), true, false, false),
        new(new(1, 4), new(6, 9), new(1, 9), RangeSet<int>.Empty, new(5, 5), true, false, false),
    };

    public static DataGroup<LowerRayRangeInfo> LowerRayRangeInfoData { get; } = new()
    {
        //
        // Overlapping intervals
        //

        // Intersection is a singleton
        new(new(5), new(5, 8), new(8), new(5, 5), RangeSet<int>.Empty, false, false, true),
        new(new(5), new(5, 5), new(5), new(5, 5), RangeSet<int>.Empty, false, false, true),
        new(new(5), new(4, 4), new(5), new(4, 4), RangeSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is not a subset
        new(new(5), new(2, 7), new(7), new(2, 5), RangeSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is a subset
        new(new(5), new(2, 5), new(5), new(2, 5), RangeSet<int>.Empty, false, false, true),
        new(new(5), new(1, 4), new(5), new(1, 4), RangeSet<int>.Empty, false, false, true),

        //
        // Disjoint intervals
        //

        // Disjoint and connected
        new(new(5), new(6, 6), new(6), RangeSet<int>.Empty, RangeSet<int>.Empty, true, true, true),
        new(new(5), new(6, 9), new(9), RangeSet<int>.Empty, RangeSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(5), new(7, 7), new(7), RangeSet<int>.Empty, new(6, 6), true, false, false),
        new(new(5), new(9, 9), new(9), RangeSet<int>.Empty, new(6, 8), true, false, false),
        new(new(5), new(7, 9), new(9), RangeSet<int>.Empty, new(6, 6), true, false, false),
        new(new(5), new(8, 9), new(9), RangeSet<int>.Empty, new(6, 7), true, false, false),
    };

    public static DataGroup<UpperRayRangeInfo> UpperRayRangeInfoData { get; } = new()
    {
        //
        // Overlapping intervals
        //

        // Intersection is a singleton
        new(new(5), new(1, 5), new(1), new(5, 5), RangeSet<int>.Empty, false, false, true),
        new(new(5), new(5, 5), new(5), new(5, 5), RangeSet<int>.Empty, false, false, true),
        new(new(5), new(6, 6), new(5), new(6, 6), RangeSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is not a subset
        new(new(5), new(2, 7), new(2), new(5, 7), RangeSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is a subset
        new(new(5), new(5, 7), new(5), new(5, 7), RangeSet<int>.Empty, false, false, true),
        new(new(5), new(7, 9), new(5), new(7, 9), RangeSet<int>.Empty, false, false, true),

        //
        // Disjoint intervals
        //

        // Disjoint and connected
        new(new(5), new(4, 4), new(4), RangeSet<int>.Empty, RangeSet<int>.Empty, true, true, true),
        new(new(5), new(1, 4), new(1), RangeSet<int>.Empty, RangeSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(5), new(3, 3), new(3), RangeSet<int>.Empty, new(4, 4), true, false, false),
        new(new(5), new(1, 1), new(1), RangeSet<int>.Empty, new(2, 4), true, false, false),
        new(new(5), new(1, 3), new(1), RangeSet<int>.Empty, new(4, 4), true, false, false),
        new(new(5), new(1, 2), new(1), RangeSet<int>.Empty, new(3, 4), true, false, false),
    };

    public static DataGroup<LowerRayUpperRayInfo> LowerRayUpperRayInfoData { get; } = new()
    {
        //
        // Overlapping rays
        //

        new(new(5), new(5), new(5, 5), RangeSet<int>.Empty, false, false, true),
        new(new(5), new(4), new(4, 5), RangeSet<int>.Empty, false, false, true),

        //
        // Disjoint rays
        //

        // Disjoint and connected
        new(new(5), new(6), RangeSet<int>.Empty, RangeSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(5), new(7), RangeSet<int>.Empty, new(6, 6), true, false, false),
        new(new(5), new(8), RangeSet<int>.Empty, new(6, 7), true, false, false),
    };
}
