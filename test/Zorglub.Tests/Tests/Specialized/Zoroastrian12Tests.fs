// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.Zoroastrian12Tests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    let private chr = new Zoroastrian12Calendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<Zoroastrian12Date, Zoroastrian12Calendar, StandardZoroastrian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new Zoroastrian12Date(y, m, d);
        override __.GetDate(y, doy) = new Zoroastrian12Date(y, doy);
        override __.GetDate(dayNumber) = new Zoroastrian12Date(dayNumber);

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<Zoroastrian12Date, StandardZoroastrian12DataSet>(chr.Domain)

        override __.MinDate = Zoroastrian12Date.MinValue
        override __.MaxDate = Zoroastrian12Date.MaxValue

        override __.GetDate(y, m, d) = new Zoroastrian12Date(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Zoroastrian12Date, StandardZoroastrian12DataSet>()

        override __.GetDate(y, m, d) = new Zoroastrian12Date(y, m, d)

