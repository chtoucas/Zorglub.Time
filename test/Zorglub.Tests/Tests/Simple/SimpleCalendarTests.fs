// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.SimpleCalendarTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Simple

open Xunit

open Zorglub.Time.FSharpExtensions

// TODO(code): Domain and related props.

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
        let key = "MyFauxKey"
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

    // Scope is tested in CalendarTestSuite.
    // Math getter is tested in CalendarTestSuite.

    [<Fact>]
    let ``Property Math setter (sys) throws for a null value`` () =
        let chr = GregorianCalendar.Instance

        nullExn "value" (fun () -> chr.Math <- null; true)

    [<Fact>]
    let ``Property Math setter (usr) throws for a null value`` () =
        let chr = new FauxUserCalendar()

        nullExn "value" (fun () -> chr.Math <- null; true)

    [<Fact>]
    let ``Property Math setter (sys) throws for an invalid value`` () =
        let chr = GregorianCalendar.Instance
        let math = new FauxCalendarMath(JulianCalendar.Instance)

        argExn "value" (fun () -> chr.Math <- math; true)

    [<Fact>]
    let ``Property Math setter (usr) throws for an invalid value`` () =
        let chr = new FauxUserCalendar()
        let math = new FauxCalendarMath(ZoroastrianCalendar.Instance)

        argExn "value" (fun () -> chr.Math <- math; true)

    // NB: we canont test the setter with a system calendar as it could disturb
    // the other tests; remember that this change is global.

    [<Fact>]
    let ``Property Math setter (usr)`` () =
        let sch = FauxSystemSchema.Regular12
        let chr = new FauxUserCalendar(sch)
        let math = new FauxCalendarMath(chr)

        chr.Math |> is<RegularMath>

        chr.Math <- math

        chr.Math ==& math
        // Just to be clear about the type, even if it is obvious.
        chr.Math |> is<FauxCalendarMath>

    //
    // Internal properties
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property Id (sys)`` (ident: CalendarId) =
        let id: Cuid = toCuid ident
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

module Misc =
    [<Fact>]
    let ``IsRegular() (sys) returns true if the schema is regular`` () =
        let sch = new GregorianSchema()
        let chr = new FauxSystemCalendar(sch)
        let isRegular, monthsInYear = chr.IsRegular()

        isRegular |> ok
        monthsInYear === 12

    [<Fact>]
    let ``IsRegular() (usr) returns true if the schema is regular`` () =
        let sch = new GregorianSchema()
        let chr = new FauxUserCalendar(sch)
        let isRegular, monthsInYear = chr.IsRegular()

        isRegular |> ok
        monthsInYear === 12

    [<Fact>]
    let ``IsRegular() (sys) returns false if the schema is not regular`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxSystemCalendar(sch)
        let isRegular, monthsInYear = chr.IsRegular()

        isRegular |> nok
        monthsInYear === 0

    [<Fact>]
    let ``IsRegular() (usr) returns false if the schema is not regular`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxUserCalendar(sch)
        let isRegular, monthsInYear = chr.IsRegular()

        isRegular |> nok
        monthsInYear === 0

    //
    // Internal helpers
    //

    [<Fact>]
    let ``ValidateCuid() (sys)`` () =
        let paramName = "cuidParam"
        let chr = new FauxSystemCalendar()
        let cuid: Cuid = toCuid FauxSystemCalendar.DefaultIdent

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
    // fauxCalendar is constructed such that today is not within the range of
    // supported days.
    let private startOfYear3000 = DayNumber.FromGregorianParts(3000, 1, 1)
    let private fauxCalendar =
        new SimpleCalendar(
            FauxUserCalendar.DefaultCuid, FauxUserCalendar.DefaultKey, new GregorianSchema(), startOfYear3000, false)

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
    let ``GetCurrentYear() is out of range`` () =
        outOfRangeExn "dayNumber" (fun () -> fauxCalendar.GetCurrentYear())

    [<Fact>]
    let ``GetCurrentMonth()`` () =
        let now = DateTime.Now
        let month = chr.GetCurrentMonth()

        month.Year  === now.Year
        month.Month === now.Month

    [<Fact>]
    let ``GetCurrentMonth() is out of range`` () =
        outOfRangeExn "dayNumber" (fun () -> fauxCalendar.GetCurrentMonth())

    [<Fact>]
    let ``GetCurrentDate()`` () =
        let now = DateTime.Now
        let date = chr.GetCurrentDate()

        date.Year  === now.Year
        date.Month === now.Month
        date.Day   === now.Day

    [<Fact>]
    let ``GetCurrentDate() is out of range`` () =
        outOfRangeExn "dayNumber" (fun () -> fauxCalendar.GetCurrentDate())

    [<Fact>]
    let ``GetCurrentDay()`` () =
        let now = DateTime.Now
        let date = chr.GetCurrentDay()

        date.Year  === now.Year
        date.Month === now.Month
        date.Day   === now.Day

    [<Fact>]
    let ``GetCurrentDay() is out of range`` () =
        outOfRangeExn "dayNumber" (fun () -> fauxCalendar.GetCurrentDay())

    [<Fact>]
    let ``GetCurrentOrdinal()`` () =
        let now = DateTime.Now
        let date = chr.GetCurrentOrdinal()

        date.Year  === now.Year
        date.Month === now.Month
        date.Day   === now.Day

    [<Fact>]
    let ``GetCurrentOrdinal() is out of range`` () =
        outOfRangeExn "dayNumber" (fun () -> fauxCalendar.GetCurrentOrdinal())

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

    [<RedundantTest>]
    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetCalendarDate(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

    [<RedundantTest>]
    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetOrdinalDate(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

module JulianCase =
    let private chr = JulianCalendar.Instance
    let private domain = chr.Domain

    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetCalendarDate(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetOrdinalDate(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

module UserJulianCase =
    let private chr = UserCalendars.Julian
    let private domain = chr.Domain

    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    // Notice that to properly test GetDayOfWeek(), negative values, we use the
    // -proleptic- Julian calendar.

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetCalendarDate(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    let ``GetDayOfWeek(OrdinalDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = chr.GetOrdinalDate(dayNumber)

        chr.GetDayOfWeek(date) === dayOfWeek
