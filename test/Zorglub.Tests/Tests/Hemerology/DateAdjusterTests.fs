// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.DateAdjusterTests

open Zorglub.Testing
open Zorglub.Testing.Data.Unbounded
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Hemerology

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor for DateAdjuster throws when the calendar is null`` () =
        nullExn "calendar" (fun () -> new DateAdjusterV0<ZDate>(null))

//module Bundles =
//    let private chr = ZCalendar.Gregorian
//    let private adjusters = new DateAdjusterV0<ZDate>(chr)

//    [<Sealed>]
//    type DateAdjusterFacts() =
//        inherit IDateAdjusterFacts<ZDate, UnboundedGregorianDataSet>(adjusters)

//        override __.GetDate(y, m, d) = new ZDate(y, m, d)

