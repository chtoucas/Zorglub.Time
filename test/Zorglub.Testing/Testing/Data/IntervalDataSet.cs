// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

using Zorglub.Time.Core.Intervals;

public sealed record RangePairInfo(
    Range<int> First,
    Range<int> Second,
    Range<int> Span,
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
        new(new(1, 1), new(1, 1), new(1, 1), RangeSet<int>.Empty, false, false, true),
        // Equal (non-degenerate)
        new(new(1, 4), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        // Strict subset (degenerate)
        new(new(1, 1), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        new(new(2, 2), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        new(new(3, 3), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        new(new(4, 4), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        // Strict subset (non-degenerate)
        new(new(1, 2), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        new(new(1, 3), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        new(new(2, 3), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        new(new(2, 4), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        new(new(3, 4), new(1, 4), new(1, 4), RangeSet<int>.Empty, false, false, true),
        // Other non-disjoint cases
        new(new(1, 4), new(4, 7), new(1, 7), RangeSet<int>.Empty, false, false, true),
        new(new(1, 4), new(3, 7), new(1, 7), RangeSet<int>.Empty, false, false, true),

        //
        // Disjoint ranges
        //

        // Disjoint and connected
        new(new(1, 4), new(5, 5), new(1, 5), RangeSet<int>.Empty, true, true, true),
        new(new(1, 4), new(5, 7), new(1, 7), RangeSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(1, 1), new(4, 4), new(1, 4), RangeSet.Create(2, 3), true, false, false),
        new(new(1, 1), new(4, 7), new(1, 7), RangeSet.Create(2, 3), true, false, false),
        new(new(1, 4), new(6, 9), new(1, 9), RangeSet.Create(5, 5), true, false, false),
    };
}
