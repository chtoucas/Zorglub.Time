// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.CivilTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    // NB: notice the use of StandardGregorianDataSet.

    let private chr = new CivilCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<CivilDate, CivilCalendar, StandardGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new CivilDate(y, m, d);
        override __.GetDate(y, doy) = new CivilDate(y, doy);
        override __.GetDate(dayNumber) = new CivilDate(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<CivilDate, CivilCalendar, StandardGregorianDataSet>(chr)

        override __.MinDate = CivilDate.MinValue
        override __.MaxDate = CivilDate.MaxValue

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = CivilDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjusters_Prop() = CivilDate.Adjusters |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateAdjustersFacts() =
        inherit IDateAdjustersFacts<CivilDate, StandardGregorianDataSet>(new CivilAdjusters())

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
