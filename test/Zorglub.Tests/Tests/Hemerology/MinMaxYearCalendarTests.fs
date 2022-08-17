// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.MinMaxYearCalendarTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "name" is null`` () =
        let scope = new StandardScope(new GregorianSchema(), DayZero.NewStyle)

        nullExn "name" (fun () -> new MinMaxYearBasicCalendar(null, scope))
        nullExn "name" (fun () -> new MinMaxYearCalendar(null, scope))

    [<Fact>]
    let ``Constructor throws when "scope" is null`` () =
        nullExn "scope" (fun () -> new MinMaxYearBasicCalendar("Name", (null: CalendarScope)))
        nullExn "scope" (fun () -> new MinMaxYearBasicCalendar("Name", (null: MinMaxYearScope)))
        nullExn "scope" (fun () -> new MinMaxYearCalendar("Name", (null: CalendarScope)))
        nullExn "scope" (fun () -> new MinMaxYearCalendar("Name", (null: MinMaxYearScope)))

    [<Fact>]
    let ``Constructor throws when "scope" is not complete`` () =
        let min = new DateParts(1, 1, 2)
        let scope = BoundedBelowScope.StartingAt(new GregorianSchema(), DayZero.NewStyle, min)

        scope.IsComplete |> nok

        argExn "scope" (fun () -> new MinMaxYearBasicCalendar("Name", scope))
        argExn "scope" (fun () -> new MinMaxYearCalendar("Name", scope))
