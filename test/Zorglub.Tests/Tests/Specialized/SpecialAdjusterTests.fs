// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Specialized.SpecialAdjusterTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Specialized

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when the scope is null`` () =
        let scope: MinMaxYearScope = null

        nullExn "scope" (fun () -> new FauxSpecialAdjuster<ArmenianDate>(scope))

    [<Fact>]
    let ``Property scope`` () =
        let range = Range.Create(1, 2)
        let scope = MinMaxYearScope.Create(new Egyptian12Schema(), DayZero.Armenian, range)
        let adjuster = new FauxSpecialAdjuster<ArmenianDate>(scope)

        adjuster.Scope ==& scope

