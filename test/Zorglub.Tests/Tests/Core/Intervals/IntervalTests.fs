// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Intervals.IntervalTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time
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
// We also test Lavretni.

let private maprange (range: Range<int>) =
    let endpoints = range.Endpoints.Select(fun i -> DayNumber.Zero + i)
    Range.FromEndpoints(endpoints)
let private maprangeset (set: RangeSet<int>) =
    if set.IsEmpty then
        RangeSet<DayNumber>.Empty
    else
        let endpoints = set.Range.Value.Endpoints.Select(fun i -> DayNumber.Zero + i)
        RangeSet.FromEndpoints(endpoints)
let private maplowerray (ray: LowerRay<int>) =
    LowerRay.EndingAt(DayNumber.Zero + ray.Max)
let private mapupperray (ray: UpperRay<int>) =
    UpperRay.StartingAt(DayNumber.Zero + ray.Min)

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

// We test:
// - Span
// - Intersect
// - Gap
// - Disjoint
// - Adjacent
// - Connected
// - Coalesce
module SetOps =
    let rangePairInfoData = IntervalDataSet.RangePairInfoData
    let lowerRayRangeInfoData = IntervalDataSet.LowerRayRangeInfoData
    let upperRayRangeInfoData = IntervalDataSet.UpperRayRangeInfoData
    let lowerRayUpperRayInfoData = IntervalDataSet.LowerRayUpperRayInfoData

    //
    // Range and Range
    //

    [<Theory; MemberData(nameof(rangePairInfoData))>]
    let ``(Range, Range)`` (data: RangePairInfo) =
        let v = data.First
        let w = data.Second
        let span  = data.Span
        let inter = data.Intersection
        let gap   = data.Gap

        Interval.Span(v, w) === span
        Interval.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Interval.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Interval.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Interval.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Interval.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Interval.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> isnull
            Interval.Coalesce(w, v) |> isnull

    [<Theory; MemberData(nameof(rangePairInfoData))>]
    let ``(Range, Range) for DayNumber's`` (data: RangePairInfo) =
        let v = maprange data.First
        let w = maprange data.Second
        let span  = maprange data.Span
        let inter = maprangeset data.Intersection
        let gap   = maprangeset data.Gap

        Interval.Span(v, w) === span
        Interval.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Interval.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Interval.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Interval.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Interval.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Interval.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> isnull
            Interval.Coalesce(w, v) |> isnull

    //
    // Range and LowerRay
    //

    [<Theory; MemberData(nameof(lowerRayRangeInfoData))>]
    let ``(Range, LowerRay)`` (data: LowerRayRangeInfo) =
        let w = data.First
        let v = data.Second
        let span  = data.Span
        let inter = data.Intersection
        let gap   = data.Gap

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> isnull
            Lavretni.Coalesce(w, v) |> isnull

    [<Theory; MemberData(nameof(lowerRayRangeInfoData))>]
    let ``(Range, LowerRay) for DayNumber's`` (data: LowerRayRangeInfo) =
        let w = maplowerray data.First
        let v = maprange data.Second
        let span  = maplowerray data.Span
        let inter = maprangeset data.Intersection
        let gap   = maprangeset data.Gap

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> isnull
            Lavretni.Coalesce(w, v) |> isnull

    //
    // Range and UpperRay
    //

    [<Theory; MemberData(nameof(upperRayRangeInfoData))>]
    let ``(Range, UpperRay)`` (data: UpperRayRangeInfo) =
        let w = data.First
        let v = data.Second
        let span  = data.Span
        let inter = data.Intersection
        let gap   = data.Gap

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> isnull
            Lavretni.Coalesce(w, v) |> isnull

    [<Theory; MemberData(nameof(upperRayRangeInfoData))>]
    let ``(Range, UpperRay) for DayNumber's`` (data: UpperRayRangeInfo) =
        let w = mapupperray data.First
        let v = maprange data.Second
        let span  = mapupperray data.Span
        let inter = maprangeset data.Intersection
        let gap   = maprangeset data.Gap

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> isnull
            Lavretni.Coalesce(w, v) |> isnull

    //
    // LowerRay and UpperRay
    //

    [<Theory; MemberData(nameof(lowerRayUpperRayInfoData))>]
    let ``(LowerRay, UpperRay)`` (data: LowerRayUpperRayInfo) =
        let v = data.First
        let w = data.Second
        let inter = data.Intersection
        let gap   = data.Gap

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

    [<Theory; MemberData(nameof(lowerRayUpperRayInfoData))>]
    let ``(LowerRay, UpperRay) for DayNumber's`` (data: LowerRayUpperRayInfo) =
        let v = maplowerray data.First
        let w = mapupperray data.Second
        let inter = maprangeset data.Intersection
        let gap   = maprangeset data.Gap

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

    //
    // LowerRay and LowerRay
    //

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
    let ``Union(LowerRay, LowerRay)`` i j m =
        let v = LowerRay.EndingAt(i)
        let w = LowerRay.EndingAt(j)
        let union = LowerRay.EndingAt(m)

        Interval.Union(v, w) === union
        Interval.Union(w, v) === union

    //
    // UpperRay and UpperRay
    //

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
    [<InlineData(1, 1, 1)>]
    [<InlineData(1, 4, 1)>]
    let ``Union(UpperRay, UpperRay)`` i j m =
        let v = UpperRay.StartingAt(i)
        let w = UpperRay.StartingAt(j)
        let union = UpperRay.StartingAt(m)

        Interval.Union(v, w) === union
        Interval.Union(w, v) === union
