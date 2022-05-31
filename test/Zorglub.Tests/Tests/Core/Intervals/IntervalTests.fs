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

module IntersectionCase =
    // The intersection is the first range.
    [<Theory>]
    // Equal
    [<InlineData(1, 1, 1, 1)>]
    [<InlineData(1, 4, 1, 4)>]
    // Strict subset (degenerate)
    [<InlineData(1, 1, 1, 4)>]
    [<InlineData(2, 2, 1, 4)>]
    [<InlineData(3, 3, 1, 4)>]
    [<InlineData(4, 4, 1, 4)>]
    // Strict subset (non-degenerate)
    [<InlineData(1, 2, 1, 4)>]
    [<InlineData(1, 3, 1, 4)>]
    [<InlineData(2, 3, 1, 4)>]
    [<InlineData(2, 4, 1, 4)>]
    [<InlineData(3, 4, 1, 4)>]
    let ``Intersect(Range, Range) when subset`` i j m n =
        let v = Range.Create(i, j)
        let w = Range.Create(m, n)
        let inter = RangeSet.Create(i, j)

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

    [<Theory>]
    [<InlineData(1, 4, 4, 7, 4, 4)>]
    [<InlineData(1, 4, 3, 7, 3, 4)>]
    let ``Intersect(Range, Range) when overlapping and not subset`` i j k l m n =
        let v = Range.Create(i, j)
        let w = Range.Create(k, l)
        let inter = RangeSet.Create(m, n)

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

    [<Theory>]
    [<InlineData(1, 1, 4, 4)>]
    [<InlineData(1, 1, 4, 7)>]
    [<InlineData(1, 4, 6, 9)>]
    let ``Intersect(Range, Range) when disjoint`` i j m n =
        let v = Range.Create(i, j)
        let w = Range.Create(m, n)

        Interval.Intersect(v, w).IsEmpty |> ok
        Interval.Intersect(w, v).IsEmpty |> ok

    [<Theory>]
    [<InlineData(1, 1, 1)>]
    [<InlineData(1, 4, 1)>]
    let ``Intersect(LowerRay, LowerRay)`` i j m =
        let v = LowerRay.EndingAt(i)
        let w = LowerRay.EndingAt(j)
        let inter = LowerRay.EndingAt(m)

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

    [<Theory>]
    [<InlineData(1, 1, 1)>]
    [<InlineData(1, 4, 4)>]
    let ``Intersect(UpperRay, UpperRay)`` i j m =
        let v = UpperRay.StartingAt(i)
        let w = UpperRay.StartingAt(j)
        let inter = UpperRay.StartingAt(m)

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

    [<Theory>]
    // Intersection is a singleton
    [<InlineData(5, 5, 8, 5, 5)>]
    [<InlineData(5, 5, 5, 5, 5)>]
    [<InlineData(5, 4, 4, 4, 4)>]
    // Overlapping but range is not a subset
    [<InlineData(5, 4, 8, 4, 5)>]
    // Overlapping and range is a subset
    [<InlineData(5, 2, 5, 2, 5)>]
    [<InlineData(5, 1, 4, 1, 4)>]
    let ``Intersect(Range, LowerRay) overlapping`` i k l m n=
        let w = LowerRay.EndingAt(i)
        let v = Range.Create(k, l)
        let inter = RangeSet.Create(m, n)

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

    [<Theory>]
    [<InlineData(5, 6, 6)>]
    [<InlineData(5, 6, 9)>]
    let ``Intersect(Range, LowerRay) disjoint`` i m n =
        let w = LowerRay.EndingAt(i)
        let v = Range.Create(m, n)

        Interval.Intersect(v, w).IsEmpty |> ok
        Lavretni.Intersect(w, v).IsEmpty |> ok

    [<Theory>]
    // Overlapping
    [<InlineData(5, 1, 5, 5, 5)>]
    [<InlineData(5, 5, 5, 5, 5)>]
    [<InlineData(5, 6, 6, 6, 6)>]
    // Overlapping but range is not a subset
    [<InlineData(5, 4, 8, 5, 8)>]
    // Overlapping and range is a subset
    [<InlineData(5, 5, 6, 5, 6)>]
    [<InlineData(5, 6, 9, 6, 9)>]
    let ``Intersect(Range, UpperRay) overlapping`` i k l m n =
        let w = UpperRay.StartingAt(i)
        let v = Range.Create(k, l)
        let inter = RangeSet.Create(m, n)

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

    [<Theory>]
    [<InlineData(5, 1, 1)>]
    [<InlineData(5, 1, 4)>]
    let ``Intersect(Range, UpperRay) disjoint`` i m n =
        let w = UpperRay.StartingAt(i)
        let v = Range.Create(m, n)

        Interval.Intersect(v, w).IsEmpty |> ok
        Lavretni.Intersect(w, v).IsEmpty |> ok

    [<Theory>]
    [<InlineData(5, 5, 5, 5)>]
    [<InlineData(5, 4, 4, 5)>]
    let ``Intersect(LowerRay, UpperRay) overlapping`` i j m n =
        let v = LowerRay.EndingAt(i)
        let w = UpperRay.StartingAt(j)
        let inter = RangeSet.Create(m, n)

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

    [<Theory>]
    [<InlineData(5, 6)>]
    let ``Intersect(LowerRay, UpperRay) disjoint`` i j =
        let v = LowerRay.EndingAt(i)
        let w = UpperRay.StartingAt(j)

        Interval.Intersect(v, w).IsEmpty |> ok
        Lavretni.Intersect(w, v).IsEmpty |> ok

module DisjointCase =
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

    [<Theory>]
    // Intersection is a singleton
    [<InlineData(5, 5, 8, false)>]
    [<InlineData(5, 5, 5, false)>]
    [<InlineData(5, 4, 4, false)>]
    // Overlapping but range is not a subset
    [<InlineData(5, 4, 8, false)>]
    // Overlapping and range is a subset
    [<InlineData(5, 2, 5, false)>]
    [<InlineData(5, 1, 4, false)>]
    // Disjoint
    [<InlineData(5, 6, 6, true)>]
    [<InlineData(5, 6, 9, true)>]
    let ``Disjoint(Range, LowerRay)`` i m n disjoint =
        let w = LowerRay.EndingAt(i)
        let v = Range.Create(m, n)

        Interval.Disjoint(v, w) === disjoint
        Lavretni.Disjoint(w, v) === disjoint

    [<Theory>]
    // Overlapping
    [<InlineData(5, 1, 5, false)>]
    [<InlineData(5, 5, 5, false)>]
    [<InlineData(5, 6, 6, false)>]
    // Overlapping but range is not a subset
    [<InlineData(5, 4, 8, false)>]
    // Overlapping and range is a subset
    [<InlineData(5, 5, 6, false)>]
    [<InlineData(5, 6, 9, false)>]
    // Disjoint
    [<InlineData(5, 1, 1, true)>]
    [<InlineData(5, 1, 4, true)>]
    let ``Disjoint(Range, UpperRay)`` i m n disjoint =
        let w = UpperRay.StartingAt(i)
        let v = Range.Create(m, n)

        Interval.Disjoint(v, w) === disjoint
        Lavretni.Disjoint(w, v) === disjoint

    [<Theory>]
    // Overlapping
    [<InlineData(5, 5, false)>]
    [<InlineData(5, 4, false)>]
    // Disjoint
    [<InlineData(5, 6, true)>]
    let ``Disjoint(LowerRay, UpperRay)`` i j disjoint =
        let v = LowerRay.EndingAt(i)
        let w = UpperRay.StartingAt(j)

        Interval.Disjoint(v, w) === disjoint
        Lavretni.Disjoint(w, v) === disjoint
