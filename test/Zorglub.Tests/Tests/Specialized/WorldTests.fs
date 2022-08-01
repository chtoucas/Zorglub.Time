// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.WorldTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    // NB: notice the use of ProlepticJulianDataSet.

    let private chr = new WorldCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<WorldDate, WorldCalendar, StandardWorldDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new WorldDate(y, m, d);
        override __.GetDate(y, doy) = new WorldDate(y, doy);
        override __.GetDate(dayNumber) = new WorldDate(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<WorldDate, StandardWorldDataSet>(chr.Domain)

        override __.MinDate = WorldDate.MinValue
        override __.MaxDate = WorldDate.MaxValue

        override __.GetDate(y, m, d) = new WorldDate(y, m, d)

