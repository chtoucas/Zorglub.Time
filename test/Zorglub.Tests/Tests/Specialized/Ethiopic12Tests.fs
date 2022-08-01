// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.Ethiopic12Tests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    let private chr = new Ethiopic12Calendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<Ethiopic12Date, Ethiopic12Calendar, StandardEthiopic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override x.GetDate(y, m, d) = new Ethiopic12Date(y, m, d);
        override x.GetDate(y, doy) = new Ethiopic12Date(y, doy);
        override x.GetDate(dayNumber) = new Ethiopic12Date(dayNumber);

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Ethiopic12Date, StandardEthiopic12DataSet>(chr.Domain)

        override __.MinDate = Ethiopic12Date.MinValue
        override __.MaxDate = Ethiopic12Date.MaxValue

        override __.GetDate(y, m, d) = new Ethiopic12Date(y, m, d)
