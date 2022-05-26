// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

// Here, we test calendar-specific methods.
// For methods related to IEpagomenalCalendar, this is done in FeaturetteTestSuite.
// We also test Gregorian/JulianCalendar.GetDayOfWeek() with
// CalCalDataSet.DayNumberToDayOfWeekData in CalendarTestSuite via CalendarFacts,
// but Gregorian and Julian are not part of the "regular" test plan, the one
// used for code coverage, because they are marked as redundant test groups.

module Prelude =
    [<Fact>]
    let ``Constructor (sys) throws for null schema`` () =
        nullExn "schema" (fun () -> new FauxSystemCalendar(null))

    [<Fact>]
    let ``Constructor (usr) throws for null schema`` () =
        nullExn "schema" (fun () -> new FauxUserCalendar(null :> SystemSchema))

    [<Fact>]
    let ``Constructor (usr) throws for null key`` () =
        let key: string = null

        nullExn "key" (fun () -> new FauxUserCalendar(key))

module GregorianCase =
    let private chr = GregorianCalendar.Instance
    let private domain = chr.Domain
    let private calendarDataSet = ProlepticGregorianDataSet.Instance

    let dayOfWeekData = calendarDataSet.DayOfWeekData
    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate)`` (info: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = info.Deconstruct()
        let date = chr.GetCalendarDate(y, m, d)

        chr.GetDayOfWeek(date) === dayOfWeek

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate)`` (info: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = info.Deconstruct()
        let date = chr.GetCalendarDate(y, m, d).ToOrdinalDate()

        chr.GetDayOfWeek(date) === dayOfWeek

    [<RedundantTestUnit>]
    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetCalendarDateOn(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

    [<RedundantTestUnit>]
    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetOrdinalDateOn(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

module JulianCase =
    let private chr = JulianCalendar.Instance
    let private domain = chr.Domain

    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetCalendarDateOn(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetOrdinalDateOn(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek
