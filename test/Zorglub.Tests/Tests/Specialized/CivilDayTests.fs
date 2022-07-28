﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.CivilDayTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time.Specialized

module Bundles =
    // NB: notice the use of StandardGregorianDataSet.

    let private chr = new CivilCalendar()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<CivilDay, StandardGregorianDataSet>(chr.SupportedYears.Range, chr.Domain)

        override __.MinDate = CivilDay.MinValue
        override __.MaxDate = CivilDay.MaxValue

        override __.GetDate(y, m, d) = new CivilDay(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDay, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDay(y, m, d)
