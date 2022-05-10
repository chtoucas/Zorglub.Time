// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Intervals.IntervalTests

open Zorglub.Testing
open Zorglub.Time.Core.Intervals

open Xunit
open FsCheck.Xunit

// 1/ Intersect, Union, Coalesce
// 2/ Span, Gap
// 3/ Disjoint, Adjacent, Connected
//
// Interval
// - Intersection
// - Union
// - Convex hull (Span)
// - Disjoint
// Interval (specialized)
// - Coalesce
// - Gap
// - Adjacency
// - Connectedness
// IntervalExtra
// - Convex hull
// - Disjoint
// - Coalesce
// - Gap
// - Adjacency
// - Connectedness

module Prelude =
    [<Property>]
    let ``Range with itself`` (x: Pair<int>) =
        let v = Range.Create(x.Min, x.Max)

        Interval.Intersect(v, v) === RangeSet.Create(x.Min, x.Max)

        let coalesce = Interval.Coalesce(v, v)
        coalesce.HasValue |> ok
        coalesce.Value === v

        Interval.Span(v, v) === v
        Interval.Gap(v, v).IsEmpty  |> ok

        Interval.Disjoint(v, v)     |> nok
        Interval.Adjacent(v, v)     |> nok
        Interval.Connected(v, v)    |> ok

    [<Property>]
    let ``Range with itself when singleton`` (i: int) =
        let v = Range.Singleton(i)

        Interval.Intersect(v, v) === RangeSet.Create(i, i)

        let coalesce = Interval.Coalesce(v, v)
        coalesce.HasValue |> ok
        coalesce.Value === v

        Interval.Span(v, v) === v
        Interval.Gap(v, v).IsEmpty  |> ok

        Interval.Disjoint(v, v)     |> nok
        Interval.Adjacent(v, v)     |> nok
        Interval.Connected(v, v)    |> ok

    [<Property>]
    let ``LowerRay with itself`` (i: int) =
        let v = LowerRay.EndingAt(i)

        Interval.Intersect(v, v) === v
        Interval.Union(v, v) === v

        IntervalExtra.Coalesce(v, v) === v
        IntervalExtra.Span(v, v) === v
        IntervalExtra.Gap(v, v).IsEmpty  |> ok

        IntervalExtra.Disjoint(v, v)    |> nok
        IntervalExtra.Adjacent(v, v)    |> nok
        IntervalExtra.Connected(v, v)   |> ok

    [<Property>]
    let ``UpperRay with itself`` (i: int) =
        let v = UpperRay.StartingAt(i)

        Interval.Intersect(v, v) === v
        Interval.Union(v, v) === v

        IntervalExtra.Coalesce(v, v) === v
        IntervalExtra.Span(v, v) === v
        IntervalExtra.Gap(v, v).IsEmpty  |> ok

        IntervalExtra.Disjoint(v, v)    |> nok
        IntervalExtra.Adjacent(v, v)    |> nok
        IntervalExtra.Connected(v, v)   |> ok

module DisjointMethod =
    [<Theory>]
    // Equal
    [<InlineData(1, 1, 1, 1, false)>]
    [<InlineData(1, 4, 1, 4, false)>]
    // Strict subset (degenerate)
    [<InlineData(1, 1, 1, 4, false)>]
    [<InlineData(2, 2, 1, 4, false)>]
    [<InlineData(3, 3, 1, 4, false)>]
    [<InlineData(4, 4, 1, 4, false)>]
    // Strict subset (non-degenerate)
    [<InlineData(1, 2, 1, 4, false)>]
    [<InlineData(1, 3, 1, 4, false)>]
    [<InlineData(2, 3, 1, 4, false)>]
    [<InlineData(2, 4, 1, 4, false)>]
    [<InlineData(3, 4, 1, 4, false)>]
    // Other non-disjoint cases
    [<InlineData(1, 4, 4, 7, false)>]
    [<InlineData(1, 4, 3, 7, false)>]
    // Disjoint
    [<InlineData(1, 1, 4, 4, true)>]
    [<InlineData(1, 1, 4, 7, true)>]
    [<InlineData(1, 4, 6, 9, true)>]
    let ``Disjoint(Range, Range)`` i j m n disjoint =
        let v = Range.Create(i, j)
        let w = Range.Create(m, n)

        Interval.Disjoint(v, w) === disjoint
        Interval.Disjoint(w, v) === disjoint
