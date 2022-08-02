// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.BasicCalendarTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "name" is null`` () =
        let name: string = null
        let scope = new StandardScope(new GregorianSchema(), DayZero.NewStyle)

        nullExn "name" (fun () -> new FauxBasicCalendar(name, scope))

    [<Fact>]
    let ``Constructor throws when "scope" is null`` () =
        let scope: CalendarScope = null

        nullExn "scope" (fun () -> new FauxBasicCalendar("Name", scope))

    [<Fact>]
    let ``Properties from constructor`` () =
        let name = "My Name"
        let scope = new StandardScope(new GregorianSchema(), DayZero.NewStyle)
        let chr = new FauxBasicCalendar(name, scope)

        chr.Name  === name
        chr.Scope ==& scope
