// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.GregorianDayTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time.Specialized

module Bundles =
    // NB: notice the use of ProlepticGregorianDataSet.

    let private chr = new GregorianCalendar()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<GregorianDay, ProlepticGregorianDataSet>(chr.SupportedYears.Range, chr.Domain)

        override __.MinDate = GregorianDay.MinValue
        override __.MaxDate = GregorianDay.MaxValue

        override __.GetDate(y, m, d) = new GregorianDay(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDay(y, m, d)
