// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.SystemSegmentTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Utilities

open Xunit

module Prelude =
    [<Fact>]
    let ``Property Schema`` () =
        let sch = new GregorianSchema()
        let seg = SystemSegment.Create(sch, Range.Create(1, 4)) :> ISchemaBound

        seg.Schema ==& sch

    [<Fact>]
    let ``ToString()`` () =
        let range = Range.Create(1, 4)
        let seg = SystemSegment.Create(new GregorianSchema(), range)

        seg.ToString() === range.ToString()

module Factories =
    [<Fact>]
    let ``Create() throws for null schema`` () =
        nullExn "schema" (fun () -> SystemSegment.Create(null, Range.Create(1, 4)))

    [<Fact>]
    let ``Create() throws when range is not a subinterval of schema.SupportedYears`` () =
        let sch = new FauxSystemSchema(Range.Create(5, 10))

        argExn "supportedYears" (fun () -> SystemSegment.Create(sch, Range.Create(3, 4)))
        argExn "supportedYears" (fun () -> SystemSegment.Create(sch, Range.Create(3, 5)))
        argExn "supportedYears" (fun () -> SystemSegment.Create(sch, Range.Create(3, 6)))
        argExn "supportedYears" (fun () -> SystemSegment.Create(sch, Range.Create(9, 12)))
        argExn "supportedYears" (fun () -> SystemSegment.Create(sch, Range.Create(10, 12)))
        argExn "supportedYears" (fun () -> SystemSegment.Create(sch, Range.Create(11, 12)))

    [<Fact>]
    let ``Create()`` () =
        let range = Range.Create(1, 2)
        let seg = SystemSegment.Create(new GregorianSchema(), range)

        let minMaxDateParts = OrderedPair.Create(
            new Yemoda(range.Min, 1, 1),
            new Yemoda(range.Max, 12, 31))

        let minMaxOrdinalParts = OrderedPair.Create(
            new Yedoy(range.Min, 1),
            new Yedoy(range.Max, 365))

        let minMaxMonthParts = OrderedPair.Create(
            new Yemo(range.Min, 1),
            new Yemo(range.Max, 12))

        seg.SupportedDays   === Range.Create(0, 729)
        seg.SupportedMonths === Range.Create(0, 23)
        seg.SupportedYears  === range

        seg.MinMaxDateParts    === minMaxDateParts
        seg.MinMaxOrdinalParts === minMaxOrdinalParts
        seg.MinMaxMonthParts   === minMaxMonthParts

module Conversions =
    [<Fact>]
    let ``FromCalendricalSegment() throws when the schema is not a system schema`` () =
        let seg = CalendricalSegment.Create(new FauxCalendricalSchema(), Range.Create(1, 4))

        argExn "segment" (fun () -> SystemSegment.FromCalendricalSegment(seg))

    [<Fact>]
    let ``FromCalendricalSegment()`` () =
        let sch = new GregorianSchema()
        let range = Range.Create(1, 2)
        let sys = SystemSegment.Create(sch, range)
        let seg = SystemSegment.FromCalendricalSegment(CalendricalSegment.Create(sch, range))

        seg.SupportedDays   === sys.SupportedDays
        seg.SupportedMonths === sys.SupportedMonths
        seg.SupportedYears  === sys.SupportedYears

        seg.MinMaxDateParts    === sys.MinMaxDateParts
        seg.MinMaxOrdinalParts === sys.MinMaxOrdinalParts
        seg.MinMaxMonthParts   === sys.MinMaxMonthParts

    [<Fact>]
    let ``ToCalendricalSegment()`` () =
        let sch = new GregorianSchema()
        let range = Range.Create(1, 2)
        let seg = SystemSegment.Create(sch, range).ToCalendricalSegment()

        let minMaxDateParts = OrderedPair.Create(
            new DateParts(range.Min, 1, 1),
            new DateParts(range.Max, 12, 31))

        let minMaxOrdinalParts = OrderedPair.Create(
            new OrdinalParts(range.Min, 1),
            new OrdinalParts(range.Max, 365))

        let minMaxMonthParts = OrderedPair.Create(
            new MonthParts(range.Min, 1),
            new MonthParts(range.Max, 12))

        seg.IsComplete |> ok

        seg.SupportedDays   === Range.Create(0, 729)
        seg.SupportedMonths === Range.Create(0, 23)
        seg.SupportedYears  === range

        seg.MinMaxDateParts    === minMaxDateParts
        seg.MinMaxOrdinalParts === minMaxOrdinalParts
        seg.MinMaxMonthParts   === minMaxMonthParts
