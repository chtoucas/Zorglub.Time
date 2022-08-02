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

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        let range = Range.Create(1, 2)

        nullExn "schema" (fun () -> new MinMaxYearScope(null, DayZero.NewStyle, range))

    [<Fact>]
    let ``Constructor throws when "range" is not a subinterval of schema.SupportedYears`` () =
        let sch = new FauxSystemSchema(Range.Create(5, 10))

        argExn "supportedYears" (fun () -> new MinMaxYearScope(sch, DayZero.NewStyle, Range.Create(3, 4)))
        argExn "supportedYears" (fun () -> new MinMaxYearScope(sch, DayZero.NewStyle, Range.Create(3, 5)))
        argExn "supportedYears" (fun () -> new MinMaxYearScope(sch, DayZero.NewStyle, Range.Create(3, 6)))
        argExn "supportedYears" (fun () -> new MinMaxYearScope(sch, DayZero.NewStyle, Range.Create(9, 12)))
        argExn "supportedYears" (fun () -> new MinMaxYearScope(sch, DayZero.NewStyle, Range.Create(10, 12)))
        argExn "supportedYears" (fun () -> new MinMaxYearScope(sch, DayZero.NewStyle, Range.Create(11, 12)))


module Factories =
    [<Fact>]
    let ``WithMaximalRange() throws for null schema`` () =
        nullExn "schema" (fun () -> MinMaxYearScope.WithMaximalRange(null, DayZero.NewStyle, false))
        nullExn "schema" (fun () -> MinMaxYearScope.WithMaximalRange(null, DayZero.NewStyle, true))
