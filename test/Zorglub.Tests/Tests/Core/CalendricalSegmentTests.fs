// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.CalendricalSegmentTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Utilities

open Xunit

module Prelude =
    [<Fact>]
    let ``Property IsComplete`` () =
        // See also CalendricalSegmentBuilderTests.
        let seg = CalendricalSegment.Create(new GregorianSchema(), Range.Create(1, 4))

        seg.IsComplete |> ok

    [<Fact>]
    let ``Property Schema`` () =
        let sch = new GregorianSchema()
        let seg = CalendricalSegment.Create(sch, Range.Create(1, 4)) :> ISchemaBound

        seg.Schema ==& sch

    [<Fact>]
    let ``ToString()`` () =
        let range = Range.Create(1, 4)
        let minMaxDateParts = OrderedPair.Create(
            new DateParts(range.Min, 1, 1),
            new DateParts(range.Max, 12, 31))
        let seg = CalendricalSegment.Create(new GregorianSchema(), range)

        seg.ToString() === minMaxDateParts.ToString()

module Factories =
    [<Fact>]
    let ``CreateMaximal() throws for null schema`` () =
        nullExn "schema" (fun () -> CalendricalSegment.CreateMaximal(null))

    [<Fact>]
    let ``CreateMaximal() may throw when the schema only supports years <= 0`` () =
        let range = Range.Create(-10, 0)
        let seg = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range))

        seg.SupportedYears === range
        argExn "onOrAfterEpoch" (fun () -> CalendricalSegment.CreateMaximal(new FauxSystemSchema(range), true))

    [<Fact>]
    let ``CreateMaximal()`` () =
        let range = Range.Create(-10, 10)
        let seg1 = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range))
        let seg2 = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range), true)

        seg1.SupportedYears === range
        seg2.SupportedYears === Range.Create(1, 10)

    [<Fact>]
    let ``CreateMaximal() when the schema only supports years > 0`` () =
        let range = Range.Create(5, 10)
        let seg1 = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range))
        let seg2 = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range), true)

        seg1.SupportedYears === range
        seg2.SupportedYears === range

    [<Fact>]
    let ``Create(range) throws for null schema`` () =
        nullExn "schema" (fun () -> CalendricalSegment.Create(null, Range.Create(1, 4)))

    [<Fact>]
    let ``Create(range) throws when range is not a subinterval of schema.SupportedYears`` () =
        let sch = new FauxSystemSchema(Range.Create(5, 10))

        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Range.Create(3, 4)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Range.Create(3, 5)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Range.Create(3, 6)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Range.Create(9, 12)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Range.Create(10, 12)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Range.Create(11, 12)))
