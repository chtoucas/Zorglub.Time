// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Extras.ZDateTests

open Zorglub.Testing
open Zorglub.Testing.Data.Unbounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time.Extras

module Bundles =
    // NB: notice the use of UnboundedGregorianDataSet.

    let private other = ZCalendar.Julian

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit ZDateFacts<UnboundedGregorianDataSet>(ZCalendar.Gregorian, other)

    // TODO(test): to be removed.
    //[<Sealed>]
    //[<TestExcludeFrom(TestExcludeFrom.Regular)>]
    //type AdjustableDateFacts() =
    //    inherit IAdjustableDateFacts<ZDate, UnboundedGregorianDataSet>(ZCalendar.Gregorian.SupportedYears)

    //    override __.GetDate(y, m, d) = ZCalendar.Gregorian.GetDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<ZDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = ZCalendar.Gregorian.GetDate(y, m, d)
