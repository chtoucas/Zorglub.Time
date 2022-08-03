// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.MinMaxYearScopeTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Hemerology.Scopes

open Xunit

// See also CalendricalSegmentTests.

module Factories =
    [<Fact>]
    let ``Create() throws when "schema" is null`` () =
        let range = Range.Create(1, 2)

        nullExn "schema" (fun () -> MinMaxYearScope.Create(null, DayZero.NewStyle, range))

    [<Fact>]
    let ``Create() throws when "range" is not a subinterval of schema.SupportedYears`` () =
        let sch = new FauxSystemSchema(Range.Create(5, 10))

        argExn "supportedYears" (fun () -> MinMaxYearScope.Create(sch, DayZero.NewStyle, Range.Create(3, 4)))
        argExn "supportedYears" (fun () -> MinMaxYearScope.Create(sch, DayZero.NewStyle, Range.Create(3, 5)))
        argExn "supportedYears" (fun () -> MinMaxYearScope.Create(sch, DayZero.NewStyle, Range.Create(3, 6)))
        argExn "supportedYears" (fun () -> MinMaxYearScope.Create(sch, DayZero.NewStyle, Range.Create(9, 12)))
        argExn "supportedYears" (fun () -> MinMaxYearScope.Create(sch, DayZero.NewStyle, Range.Create(10, 12)))
        argExn "supportedYears" (fun () -> MinMaxYearScope.Create(sch, DayZero.NewStyle, Range.Create(11, 12)))

    [<Fact>]
    let ``Create()`` () =
        let range = Range.Create(-2, 9)
        let sch = new FauxSystemSchema(Range.Create(-9, 10))
        let scope = MinMaxYearScope.Create(sch, DayZero.NewStyle, range)

        scope.Segment.SupportedYears === range

    [<Fact>]
    let ``CreateMaximal() throws for null schema`` () =
        nullExn "schema" (fun () -> MinMaxYearScope.CreateMaximal(null, DayZero.NewStyle))

    [<Fact>]
    let ``CreateMaximal()`` () =
        let range = Range.Create(-10, 10)
        let sch = new FauxSystemSchema(range)
        let scope = MinMaxYearScope.CreateMaximal(sch, DayZero.NewStyle)

        scope.Segment.SupportedYears === range

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1() throws for null schema`` () =
        nullExn "schema" (fun () -> MinMaxYearScope.CreateMaximalOnOrAfterYear1(null, DayZero.NewStyle))

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1() throws when the schema only supports years <= 0`` () =
        let range = Range.Create(-10, 0)
        let sch = new FauxSystemSchema(range)

        argExn "schema" (fun () -> MinMaxYearScope.CreateMaximalOnOrAfterYear1(sch, DayZero.NewStyle))

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1()`` () =
        let range = Range.Create(-10, 10)
        let sch = new FauxSystemSchema(range)
        let scope = MinMaxYearScope.CreateMaximalOnOrAfterYear1(sch, DayZero.NewStyle)

        scope.Segment.SupportedYears === Range.Create(1, 10)

    [<Fact>]
    let ``StartingAt() throws for null schema`` () =
        nullExn "schema" (fun () -> MinMaxYearScope.StartingAt(null, DayZero.NewStyle, 1))

    [<Fact>]
    let ``StartingAt() throws when the year is out of range`` () =
        let range = Range.Create(2, 10)
        let sch = new FauxSystemSchema(range)

        outOfRangeExn "year" (fun () -> MinMaxYearScope.StartingAt(sch, DayZero.NewStyle, 1))

    [<Fact>]
    let ``StartingAt()`` () =
        let range = Range.Create(2, 10)
        let sch = new FauxSystemSchema(range)
        let scope = MinMaxYearScope.StartingAt(sch, DayZero.NewStyle, 3)

        scope.Segment.SupportedYears === Range.Create(3, 10)

    [<Fact>]
    let ``EndingAt() throws for null schema`` () =
        nullExn "schema" (fun () -> MinMaxYearScope.EndingAt(null, DayZero.NewStyle, 1))

    [<Fact>]
    let ``EndingAt() throws when the year is out of range`` () =
        let range = Range.Create(2, 10)
        let sch = new FauxSystemSchema(range)

        outOfRangeExn "year" (fun () -> MinMaxYearScope.EndingAt(sch, DayZero.NewStyle, 1))

    [<Fact>]
    let ``EndingAt()`` () =
        let range = Range.Create(2, 10)
        let sch = new FauxSystemSchema(range)
        let scope = MinMaxYearScope.EndingAt(sch, DayZero.NewStyle, 5)

        scope.Segment.SupportedYears === Range.Create(2, 5)
