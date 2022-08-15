// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.CivilTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Hemerology
open Zorglub.Testing.Facts.Specialized

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

// NB: notice the use of StandardGregorianDataSet.

module Prelude =
    let private calendarDataSet = StandardGregorianDataSet.Instance

    let daysSinceEpochInfoData = calendarDataSet.DaysSinceEpochInfoData

    [<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
    let ``Property DaysSinceZero`` (info: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = info.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

module Bundles =
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
        static member Adjuster_Prop() = CivilDate.Adjuster |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<CivilDate, StandardGregorianDataSet>(new CivilAdjuster())

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
        override __.GetDate(y, doy) = new CivilDate(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
