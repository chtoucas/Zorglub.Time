// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.TabularIslamicTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    let private chr = new TabularIslamicCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<TabularIslamicDate, TabularIslamicCalendar, StandardTabularIslamicDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Lunar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new TabularIslamicDate(y, m, d);
        override __.GetDate(y, doy) = new TabularIslamicDate(y, doy);
        override __.GetDate(dayNumber) = new TabularIslamicDate(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<TabularIslamicDate, StandardTabularIslamicDataSet>(chr.Domain)

        override __.MinDate = TabularIslamicDate.MinValue
        override __.MaxDate = TabularIslamicDate.MaxValue

        override __.GetDate(y, m, d) = new TabularIslamicDate(y, m, d)
