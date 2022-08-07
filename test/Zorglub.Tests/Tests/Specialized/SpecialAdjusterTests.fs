// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.SpecialAdjusterTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Specialized

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when the calendar is null`` () =
        nullExn "calendar" (fun () -> new FauxSpecialAdjuster<ArmenianDate>(null))

    [<Fact>]
    let ``Constructor throws when the calendar scope is not complete`` () =
        let min = new DateParts(1, 1, 2)
        let scope = BoundedBelowScope.Create(new Egyptian12Schema(), CalendarEpoch.Armenian, min, 2)
        let chr = new FauxCalendar<ArmenianDate>("Name", scope)

        argExn "calendar" (fun () -> new FauxSpecialAdjuster<ArmenianDate>(chr))

