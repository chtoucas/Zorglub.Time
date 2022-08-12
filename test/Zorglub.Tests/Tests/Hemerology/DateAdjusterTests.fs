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
    let ``Constructor for DateAdjuster throws when the scope is null`` () =
        nullExn "scope" (fun () -> new DateAdjuster<ZDate, ZCalendar>(null))

module Bundles =
    let private chr = ZCalendar.Gregorian
    let private adjuster = new DateAdjuster<ZDate, ZCalendar>(chr)

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit IDateAdjusterFacts<ZDate, UnboundedGregorianDataSet>(adjuster)

        override __.GetDate(y, m, d) = chr.GetDate(y, m, d)
        override __.GetDate(y, doy) = chr.GetDate(y, doy)

