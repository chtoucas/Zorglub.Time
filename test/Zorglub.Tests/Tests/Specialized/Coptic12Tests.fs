﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.Coptic12Tests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    let private chr = new Coptic12Calendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<Coptic12Date, Coptic12Calendar, StandardCoptic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new Coptic12Date(y, m, d);
        override __.GetDate(y, doy) = new Coptic12Date(y, doy);
        override __.GetDate(dayNumber) = new Coptic12Date(dayNumber);

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<Coptic12Date, StandardCoptic12DataSet>(chr.Domain)

        override __.MinDate = Coptic12Date.MinValue
        override __.MaxDate = Coptic12Date.MaxValue

        override __.GetDate(y, m, d) = new Coptic12Date(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Coptic12Date, StandardCoptic12DataSet>()

        override __.GetDate(y, m, d) = new Coptic12Date(y, m, d)