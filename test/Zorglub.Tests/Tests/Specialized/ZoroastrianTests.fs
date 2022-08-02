﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.ZoroastrianTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    let private chr = new ZoroastrianCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<ZoroastrianDate, ZoroastrianCalendar, StandardZoroastrian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d);
        override __.GetDate(y, doy) = new ZoroastrianDate(y, doy);
        override __.GetDate(dayNumber) = new ZoroastrianDate(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<ZoroastrianDate, StandardZoroastrian12DataSet>(chr.Domain)

        override __.MinDate = ZoroastrianDate.MinValue
        override __.MaxDate = ZoroastrianDate.MaxValue

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)

module Bundles13 =
    let private chr = new Zoroastrian13Calendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<Zoroastrian13Date, Zoroastrian13Calendar, StandardZoroastrian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d);
        override __.GetDate(y, doy) = new Zoroastrian13Date(y, doy);
        override __.GetDate(dayNumber) = new Zoroastrian13Date(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 13

        [<Fact>]
        member x.VirtualMonth_Prop() = x.CalendarUT.VirtualMonth === 13

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>(chr.Domain)

        override __.MinDate = Zoroastrian13Date.MinValue
        override __.MaxDate = Zoroastrian13Date.MaxValue

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)