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
    let fixedCuidData = EnumDataSet.FixedCuidData
    let calendarIdData = EnumDataSet.CalendarIdData
    let calendricalFamilyData = EnumDataSet.CalendricalFamilyData
    let calendricalAdjustmentsData = EnumDataSet.CalendricalAdjustmentsData

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

    [<Fact>]
    let ``ToString() (sys) returns the key`` () =
        let chr = new FauxSystemCalendar()

        chr.ToString() === chr.Key

    [<Fact>]
    let ``ToString() (usr) returns the key`` () =
        let chr = new FauxUserCalendar()

        chr.ToString() === chr.Key

    //
    // Properties
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property Key (sys)`` (ident: CalendarId) =
        let chr = new FauxSystemCalendar(ident)
        let key = toCalendarKey(ident)

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

    [<Fact>]
    let ``Property Algorithm (sys)`` () =
        let chr = new FauxSystemCalendar()

        chr.Algorithm === CalendricalAlgorithm.Arithmetical

    [<Fact>]
    let ``Property Algorithm (usr)`` () =
        let chr = new FauxUserCalendar()

        chr.Algorithm === CalendricalAlgorithm.Arithmetical

    [<Theory; MemberData(nameof(calendricalFamilyData))>]
    let ``Property Family (sys)`` (familly: CalendricalFamily) =
        let sch = new FauxSystemSchema(familly)
        let chr = new FauxSystemCalendar(sch)

        chr.Family === familly

    [<Theory; MemberData(nameof(calendricalFamilyData))>]
    let ``Property Family (usr)`` (familly: CalendricalFamily) =
        let sch = new FauxSystemSchema(familly)
        let chr = new FauxUserCalendar(sch)

        chr.Family === familly

    [<Theory; MemberData(nameof(calendricalAdjustmentsData))>]
    let ``Property PeriodicAdjustments (sys)`` (adjustments: CalendricalAdjustments) =
        let sch = new FauxSystemSchema(adjustments)
        let chr = new FauxSystemCalendar(sch)

        chr.PeriodicAdjustments === adjustments

    [<Theory; MemberData(nameof(calendricalAdjustmentsData))>]
    let ``Property PeriodicAdjustments (usr)`` (adjustments: CalendricalAdjustments) =
        let sch = new FauxSystemSchema(adjustments)
        let chr = new FauxUserCalendar(sch)

        chr.PeriodicAdjustments === adjustments

    //
    // Internal properties
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property Id (sys)`` (ident: CalendarId) =
        let id: Cuid = LanguagePrimitives.EnumOfValue <| byte(ident)
        let chr = new FauxSystemCalendar(ident)

        chr.Id === id

    [<Fact>]
    let ``Property Id (usr)`` () =
        let id = Cuid.MinUser
        let chr = new FauxUserCalendar(id)

        chr.Id === id

    [<Fact>]
    let ``Property Schema (sys)`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxSystemCalendar(sch)

        chr.Schema ==& sch

    [<Fact>]
    let ``Property Schema (usr)`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxUserCalendar(sch)

        chr.Schema ==& sch

    //
    // Internal helpers
    //

    [<Fact>]
    let ``ValidateCuid() (sys)`` () =
        let paramName = "cuidParam"
        let chr = new FauxSystemCalendar()
        let cuid: Cuid = LanguagePrimitives.EnumOfValue <| byte(FauxSystemCalendar.DefaultIdent)

        chr.ValidateCuidDisclosed(cuid, paramName)

        argExn paramName (fun () -> chr.ValidateCuidDisclosed(Cuid.Gregorian, paramName))
        argExn paramName (fun () -> chr.ValidateCuidDisclosed(Cuid.MinUser, paramName))

    [<Fact>]
    let ``ValidateCuid() (usr)`` () =
        let paramName = "cuidParam"
        let chr = new FauxUserCalendar()

        chr.ValidateCuidDisclosed(FauxUserCalendar.DefaultCuid, paramName)

        argExn paramName (fun () -> chr.ValidateCuidDisclosed(Cuid.Gregorian, paramName))
        argExn paramName (fun () -> chr.ValidateCuidDisclosed(Cuid.MinUser, paramName))

module GregorianCase =
    let private chr = GregorianCalendar.Instance
    let private domain = chr.Domain
    let private calendarDataSet = ProlepticGregorianDataSet.Instance

    let dayOfWeekData = calendarDataSet.DayOfWeekData
    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    //
    // Factories
    //

    [<Fact>]
    let ``GetCurrentYear()`` () =
        let now = DateTime.Now
        let year = chr.GetCurrentYear()

        year.Year  === now.Year

    [<Fact>]
    let ``GetCurrentMonth()`` () =
        let now = DateTime.Now
        let month = chr.GetCurrentMonth()

        month.Year  === now.Year
        month.Month === now.Month

    [<Fact>]
    let ``GetCurrentDate()`` () =
        let now = DateTime.Now
        let date = chr.GetCurrentDate()

        date.Year  === now.Year
        date.Month === now.Month
        date.Day   === now.Day

    [<Fact>]
    let ``GetCurrentDay()`` () =
        let now = DateTime.Now
        let date = chr.GetCurrentDay()

        date.Year  === now.Year
        date.Month === now.Month
        date.Day   === now.Day

    [<Fact>]
    let ``GetCurrentOrdinal()`` () =
        let now = DateTime.Now
        let date = chr.GetCurrentOrdinal()

        date.Year  === now.Year
        date.Month === now.Month
        date.Day   === now.Day

    //
    // GetDayOfWeek()
    //

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
