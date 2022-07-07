// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.CalendarScopeTests

open Zorglub.Testing

open Zorglub.Time.Hemerology

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new FauxCalendarScope(null, 1, 2))
