// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.CalendricalSegmentTests

open System

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
        let seg = CalendricalSegment.Create(sch, Range.Create(1, 4)) :> ISchemaBound

        seg.Schema ==& sch

    [<Fact>]
    let ``ToString()`` () =
        let min = new DateParts(5, 7, 11)
        let max = new DateParts(13, 5, 3)
        let pair = OrderedPair.Create(min, max)
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())
        builder.SetMinDateParts(min)
        builder.SetMaxDateParts(max)
        let seg = builder.BuildSegment()

        seg.ToString() === pair.ToString()

module Factories =
    [<Fact>]
    let ``CreateMaximal() throws for null schema`` () =
        nullExn "schema" (fun () -> CalendricalSegment.CreateMaximal(null))
        nullExn "schema" (fun () -> CalendricalSegment.CreateMaximal(null, true))

    [<Fact>]
    let ``CreateMaximal() does not throw when the schema only supports years <= 0`` () =
        let range = Range.Create(-10, 0)
        let seg = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range

    [<Fact>]
    let ``CreateMaximal() may throw when the schema only supports years <= 0 and onOrAfterEpoch = true`` () =
        let range = Range.Create(-10, 0)

        argExn "onOrAfterEpoch" (fun () -> CalendricalSegment.CreateMaximal(new FauxSystemSchema(range), true))

    [<Fact>]
    let ``CreateMaximal()`` () =
        let range = Range.Create(-10, 10)
        let seg = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range

    [<Fact>]
    let ``CreateMaximal() when onOrAfterEpoch = true`` () =
        let range = Range.Create(-10, 10)
        let seg = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range), true)

        seg.IsComplete |> ok
        seg.SupportedYears === Range.Create(1, 10)

    [<Fact>]
    let ``CreateMaximal() when the schema only supports years > 0`` () =
        let range = Range.Create(5, 10)
        let seg = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range

    [<Fact>]
    let ``CreateMaximal() when the schema only supports years > 0 when onOrAfterEpoch = true`` () =
        let range = Range.Create(5, 10)
        let seg = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range), true)

        seg.IsComplete |> ok
        seg.SupportedYears === range

    [<Fact>]
    let ``CreateMaximal() when the schema only supports years <= 0`` () =
        let range = Range.Create(-10, 0)
        let seg = CalendricalSegment.CreateMaximal(new FauxSystemSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range

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

    [<Fact>]
    let ``Create(range)`` () =
        let sch = new GregorianSchema()
        let range = Range.Create(1, 2)
        let seg = CalendricalSegment.Create(sch, range)

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

    [<Fact>]
    let ``Create() throws for null schema`` () =
        nullExn "schema" (fun () -> CalendricalSegment.Create(null, new DateParts(1, 1, 1), 2))

    [<Fact>]
    let ``Create() throws for an invalid date`` () =
        let sch = new GregorianSchema()

        outOfRangeExn "parts" (fun () -> CalendricalSegment.Create(sch, new DateParts(GregorianSchema.MinYear - 1, 12, 1), GregorianSchema.MaxYear))
        outOfRangeExn "parts" (fun () -> CalendricalSegment.Create(sch, new DateParts(GregorianSchema.MaxYear + 1, 12, 1), GregorianSchema.MaxYear))
        outOfRangeExn "parts" (fun () -> CalendricalSegment.Create(sch, new DateParts(1, 0, 1), 2))
        outOfRangeExn "parts" (fun () -> CalendricalSegment.Create(sch, new DateParts(1, 13, 1), 2))
        outOfRangeExn "parts" (fun () -> CalendricalSegment.Create(sch, new DateParts(1, 1, 0), 2))
        outOfRangeExn "parts" (fun () -> CalendricalSegment.Create(sch, new DateParts(1, 1, 32), 2))

    [<Fact>]
    let ``Create() throws for an invalid max year`` () =
        outOfRangeExn "year" (fun () -> CalendricalSegment.Create(new GregorianSchema(), new DateParts(1, 12, 1), GregorianSchema.MaxYear + 1))

    [<Fact>]
    let ``Create() when max year is null`` () =
        let seg = CalendricalSegment.Create(new GregorianSchema(), new DateParts(1, 12, 1), Nullable())

        seg.IsComplete |> nok

        seg.SupportedYears.Max === GregorianSchema.MaxYear

    [<Fact>]
    let ``Create()`` () =
        let parts = new DateParts(5, 3, 13)
        let maxYear = 15
        let seg = CalendricalSegment.Create(new GregorianSchema(), parts, maxYear)

        seg.IsComplete |> nok

        seg.MinMaxDateParts.LowerValue === parts
        seg.MinMaxDateParts.UpperValue === new DateParts(maxYear, 12, 31)
        seg.SupportedYears.Max === maxYear

    [<Fact>]
    let ``Create() when the min date is the start of a year`` () =
        let parts = new DateParts(5, 1, 1)
        let maxYear = 15
        let seg = CalendricalSegment.Create(new GregorianSchema(), parts, maxYear)

        seg.IsComplete |> ok

        seg.MinMaxDateParts.LowerValue === parts
        seg.MinMaxDateParts.UpperValue === new DateParts(maxYear, 12, 31)
        seg.SupportedYears.Max === maxYear
