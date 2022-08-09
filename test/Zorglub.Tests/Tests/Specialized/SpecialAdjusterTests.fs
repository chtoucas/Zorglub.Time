﻿// SPDX-License-Identifier: BSD-3-Clause
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
        let chr: ICalendar<ArmenianDate> = null

        nullExn "calendar" (fun () -> new FauxSpecialAdjuster<ArmenianDate>(chr))

    [<Fact>]
    let ``Constructor throws when the scope is null`` () =
        let scope: CalendarScope = null

        nullExn "scope" (fun () -> new FauxSpecialAdjuster<ArmenianDate>(scope))

    [<Fact>]
    let ``Constructor throws when the scope is not complete`` () =
        let min = new DateParts(1, 1, 2)
        let scope = BoundedBelowScope.Create(new Egyptian12Schema(), CalendarEpoch.Armenian, min, 2)
        let chr = new FauxCalendar<ArmenianDate>("Name", scope)

        argExn "scope" (fun () -> new FauxSpecialAdjuster<ArmenianDate>(scope))
        argExn "scope" (fun () -> new FauxSpecialAdjuster<ArmenianDate>(chr))

