// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.DateAdjustersTests

open Zorglub.Testing
open Zorglub.Testing.Data.Unbounded
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor for DateAdjusters throws when the calendar is null`` () =
        nullExn "calendar" (fun () -> new DateAdjusters<ZDate>(null))

    [<Fact>]
    let ``Constructor for MinMaxYearDateAdjusters throws when the calendar scope is not complete`` () =
        let min = new DateParts(1, 1, 2)
        let scope = BoundedBelowScope.Create(new GregorianSchema(), DayZero.NewStyle, min, 2)
        let chr = new FauxCalendar<ZDate>("Name", scope)

        argExn "calendar" (fun () -> new FauxMinMaxYearDateAdjusters<ZDate>(chr))

module Bundles =
    let private chr = ZCalendar.Gregorian
    let private adjusters = new DateAdjusters<ZDate>(chr)

    [<Sealed>]
    type DateAdjustersFacts() =
        inherit IDateAdjustersFacts<ZDate, UnboundedGregorianDataSet>(adjusters)

        override __.GetDate(y, m, d) = new ZDate(y, m, d)

