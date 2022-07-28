// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.GregorianDateTests

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
        inherit IDateFacts<GregorianDate, ProlepticGregorianDataSet>(chr.SupportedYears.Range, chr.Domain)

        override __.MinDate = GregorianDate.MinValue
        override __.MaxDate = GregorianDate.MaxValue

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
