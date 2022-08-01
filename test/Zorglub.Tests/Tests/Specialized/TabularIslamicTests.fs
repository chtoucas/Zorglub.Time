// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.TabularIslamicTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    let private chr = new TabularIslamicCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<TabularIslamicDate, TabularIslamicCalendar, StandardTabularIslamicDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Lunar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override x.GetDate(y, m, d) = new TabularIslamicDate(y, m, d);
        override x.GetDate(y, doy) = new TabularIslamicDate(y, doy);
        override x.GetDate(dayNumber) = new TabularIslamicDate(dayNumber);

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<TabularIslamicDate, StandardTabularIslamicDataSet>(chr.Domain)

        override __.MinDate = TabularIslamicDate.MinValue
        override __.MaxDate = TabularIslamicDate.MaxValue

        override __.GetDate(y, m, d) = new TabularIslamicDate(y, m, d)
