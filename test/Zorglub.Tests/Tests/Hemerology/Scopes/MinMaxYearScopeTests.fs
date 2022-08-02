// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.MinMaxYearScopeTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        let range = Range.Create(1, 2)

        nullExn "schema" (fun () -> new MinMaxYearScope(null, DayZero.NewStyle, range))
