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
    let ``CreateMaximal() throws for null schema`` () =
        nullExn "schema" (fun () -> MinMaxYearScope.CreateMaximal(null, DayZero.NewStyle))
        nullExn "schema" (fun () -> MinMaxYearScope.CreateMaximal(null, DayZero.NewStyle))

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1() throws for null schema`` () =
        nullExn "schema" (fun () -> MinMaxYearScope.CreateMaximalOnOrAfterYear1(null, DayZero.NewStyle))
        nullExn "schema" (fun () -> MinMaxYearScope.CreateMaximalOnOrAfterYear1(null, DayZero.NewStyle))
