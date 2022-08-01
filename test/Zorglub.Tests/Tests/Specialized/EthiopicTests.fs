// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.Ethiopic12Tests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    let private chr = new EthiopicCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<EthiopicDate, EthiopicCalendar, StandardEthiopic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d);
        override __.GetDate(y, doy) = new EthiopicDate(y, doy);
        override __.GetDate(dayNumber) = new EthiopicDate(dayNumber);

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<EthiopicDate, StandardEthiopic12DataSet>(chr.Domain)

        override __.MinDate = EthiopicDate.MinValue
        override __.MaxDate = EthiopicDate.MaxValue

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<EthiopicDate, StandardEthiopic12DataSet>()

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)
