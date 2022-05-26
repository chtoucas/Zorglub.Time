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

open Zorglub.Time.FSharpExtensions

// Here, we test calendar-specific methods.
// For methods related to IEpagomenalCalendar, this is done in FeaturetteTestSuite.
// We also test Gregorian/JulianCalendar.GetDayOfWeek() with
// CalCalDataSet.DayNumberToDayOfWeekData in CalendarTestSuite via CalendarFacts,
// but Gregorian and Julian are not part of the "regular" test plan, the one
// used for code coverage, because they are marked as redundant test groups.

module Prelude =
    let calendarIdData = EnumDataSet.CalendarIdData

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

    //
    // Properties
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property Key (sys)`` (id: CalendarId) =
        let chr = new FauxSystemCalendar(id)
        let key = toCalendarKey(id)

        chr.Key === key

    [<Fact>]
    let ``Property Key (usr)`` () =
        let key = "FauxKey"
        let chr = new FauxUserCalendar(key)

        chr.Key === key

    [<Fact>]
    let ``Property IsUserDefined (sys)`` () =
        let chr = new FauxSystemCalendar()

        chr.IsUserDefined |> nok

    [<Fact>]
    let ``Property IsUserDefined (usr)`` () =
        let chr = new FauxUserCalendar()

        chr.IsUserDefined |> ok

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property PermanentId (sys)`` (ident: CalendarId) =
        let chr = new FauxSystemCalendar(ident)

        chr.PermanentId === ident

    [<Fact>]
    let ``Property PermanentId (usr)`` () =
        let chr = new FauxUserCalendar()

        throws<NotSupportedException> (fun () -> chr.PermanentId)

    [<Fact>]
    let ``Property Epoch (sys)`` () =
        let epoch = DayNumber.Zero + 1234
        let chr = new FauxSystemCalendar(epoch)

        chr.Epoch === epoch

    [<Fact>]
    let ``Property Epoch (usr)`` () =
        let epoch = DayNumber.Zero + 1234
        let chr = new FauxUserCalendar(epoch)

        chr.Epoch === epoch

    [<Theory>]
    [<InlineData true>]
    [<InlineData false>]
    let ``Property IsProleptic (sys)`` (proleptic: bool) =
        let chr = new FauxSystemCalendar(proleptic)

        chr.IsProleptic === proleptic

    [<Theory>]
    [<InlineData true>]
    [<InlineData false>]
    let ``Property IsProleptic (usr)`` (proleptic: bool) =
        let chr = new FauxUserCalendar(proleptic)

        chr.IsProleptic === proleptic

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
