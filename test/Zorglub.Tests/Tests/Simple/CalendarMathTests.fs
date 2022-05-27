// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

module Prelude =
    let addAdjustmentData = EnumDataSet.AddAdjustmentData
    let invalidAddAdjustmentData = EnumDataSet.InvalidAddAdjustmentData

    [<Fact>]
    let ``Constructor throws for null calendar`` () =
        nullExn "calendar" (fun () -> new FauxCalendarMath(null))

    [<Theory; MemberData(nameof(invalidAddAdjustmentData))>]
    let ``Constructor throws for invalid AddAdjustment`` (adjustment: AddAdjustment) =
        outOfRangeExn "adjustment" (fun () -> new FauxCalendarMath(adjustment))

    //
    // Properties
    //

    [<Theory; MemberData(nameof(addAdjustmentData))>]
    let ``Property AddAdjustment`` (adjustment: AddAdjustment) =
        let math = new FauxCalendarMath(adjustment)

        math.AddAdjustment === adjustment

    [<Fact>]
    let ``Property SupportedYears`` () =
        let chr = GregorianCalendar.Instance
        let math = new FauxCalendarMath(chr)

        math.SupportedYearsDisclosed === chr.SupportedYears

    [<Fact>]
    let ``Property Cuid`` () =
        let chr = GregorianCalendar.Instance
        let math = new FauxCalendarMath(chr)

        math.Cuid === chr.Id

module Factories =
    [<Fact>]
    let ``Create() -> Regular12Math`` () =
        let sch = FauxSystemSchema.Regular12
        let chr = new FauxSystemCalendar(sch)
        let math = CalendarMath.Create(chr)

        math |> is<Regular12Math>

    [<Fact>]
    let ``Create() -> Regular13Math`` () =
        let sch = FauxSystemSchema.Regular13
        let chr = new FauxSystemCalendar(sch)
        let math = CalendarMath.Create(chr)

        math |> is<Regular13Math>

    [<Fact>]
    let ``Create() -> RegularMath`` () =
        let sch = FauxSystemSchema.Regular14
        let chr = new FauxSystemCalendar(sch)
        let math = CalendarMath.Create(chr)

        math |> is<RegularMath>

    [<Fact>]
    let ``Create() -> DefaultMath`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxSystemCalendar(sch)
        let math = CalendarMath.Create(chr)

        math |> is<DefaultMath>

module Validation =
    let private chr = GregorianCalendar.Instance
    let private math = new FauxCalendarMath(chr)

    let private date = new CalendarDate(1, 1, 1, chr.Id)
    let private ordinalDate = new OrdinalDate(1, 1, chr.Id)
    let private month = new CalendarMonth(1, 1, chr.Id)

    let private invalidDate = new CalendarDate(1, 1, 1, Cuid.Julian)
    let private invalidOrdinalDate = new OrdinalDate(1, 1, Cuid.Julian)
    let private invalidMonth = new CalendarMonth(1, 1, Cuid.Julian)

    //
    // CalendarDate
    //

    [<Fact>]
    let ``AddYears(invalid date) throws`` () =
        argExn "date" (fun () -> math.AddYears(invalidDate, 1))

    [<Fact>]
    let ``AddMonths(invalid date) throws`` () =
        argExn "date" (fun () -> math.AddMonths(invalidDate, 1))

    [<Fact>]
    let ``CountYearsBetween(invalid dates) throws`` () =
        argExn "end" (fun () -> math.CountYearsBetween(date, invalidDate))
        argExn "start" (fun () -> math.CountYearsBetween(invalidDate, date))

    [<Fact>]
    let ``CountMonthsBetween(invalid dates) throws`` () =
        argExn "end" (fun () -> math.CountMonthsBetween(date, invalidDate))
        argExn "start" (fun () -> math.CountMonthsBetween(invalidDate, date))

    //
    // OrdinalDate
    //

    [<Fact>]
    let ``AddYears(invalid ordinal date) throws`` () =
        argExn "date" (fun () -> math.AddYears(invalidOrdinalDate, 1))

    [<Fact>]
    let ``CountYearsBetween(invalid ordinal dates) throws`` () =
        argExn "end" (fun () -> math.CountYearsBetween(ordinalDate, invalidOrdinalDate))
        argExn "start" (fun () -> math.CountYearsBetween(invalidOrdinalDate, ordinalDate))

    //
    // CalendarMonth
    //

    [<Fact>]
    let ``AddYears(invalid month) throws`` () =
        argExn "month" (fun () -> math.AddYears(invalidMonth, 1))

    [<Fact>]
    let ``AddMonths(invalid month) throws`` () =
        argExn "month" (fun () -> math.AddMonths(invalidMonth, 1))

    [<Fact>]
    let ``CountYearsBetween(invalid month) throws`` () =
        argExn "end" (fun () -> math.CountYearsBetween(month, invalidMonth))
        argExn "start" (fun () -> math.CountYearsBetween(invalidMonth, month))

    [<Fact>]
    let ``CountMonthsBetween(invalid month) throws`` () =
        argExn "end" (fun () -> math.CountMonthsBetween(month, invalidMonth))
        argExn "start" (fun () -> math.CountMonthsBetween(invalidMonth, month))

module Core =
    let private chr = GregorianCalendar.Instance
    let private math = new FauxCalendarMath(chr)

    let private date = new CalendarDate(1, 1, 1, chr.Id)
    let private ordinalDate = new OrdinalDate(1, 1, chr.Id)
    let private month = new CalendarMonth(1, 1, chr.Id)

    let private otherDate = new CalendarDate(2000, 1, 1, chr.Id)
    let private otherOrdinalDate = new OrdinalDate(2000, 1, chr.Id)
    let private otherMonth = new CalendarMonth(2000, 1, chr.Id)

    //
    // CalendarDate
    //

    [<Fact>]
    let ``AddYears(date) calls AddYearsCore()`` () =
        math.AddYearsCoreDateWasCalled |> nok
        math.AddYears(date, 1000)      |> ignore
        math.AddYearsCoreDateWasCalled |> ok

    [<Fact>]
    let ``AddMonths(date) calls AddMonthsCore()`` () =
        math.AddMonthsCoreDateWasCalled |> nok
        math.AddMonths(date, 1000)      |> ignore
        math.AddMonthsCoreDateWasCalled |> ok

    [<Fact>]
    let ``CountYearsBetween(date) calls CountYearsBetweenCore()`` () =
        math.CountYearsBetweenCoreDateWasCalled |> nok
        math.CountYearsBetween(date, otherDate) |> ignore
        math.CountYearsBetweenCoreDateWasCalled |> ok

    [<Fact>]
    let ``CountMonthsBetween(date) calls CountMonthsBetweenCore()`` () =
        math.CountMonthsBetweenCoreDateWasCalled |> nok
        math.CountMonthsBetween(date, otherDate) |> ignore
        math.CountMonthsBetweenCoreDateWasCalled |> ok

    //
    // OrdinalDate
    //

    [<Fact>]
    let ``AddYears(ordinal date) calls AddYearsCore()`` () =
        math.AddYearsCoreOrdinalDateWasCalled |> nok
        math.AddYears(ordinalDate, 1000)      |> ignore
        math.AddYearsCoreOrdinalDateWasCalled |> ok

    [<Fact>]
    let ``CountYearsBetween(ordinal date) calls CountYearsBetweenCore()`` () =
        math.CountYearsBetweenCoreOrdinalDateWasCalled        |> nok
        math.CountYearsBetween(ordinalDate, otherOrdinalDate) |> ignore
        math.CountYearsBetweenCoreOrdinalDateWasCalled        |> ok

    //
    // CalendarMonth
    //

    [<Fact>]
    let ``AddYears(month) calls AddYearsCore()`` () =
        math.AddYearsCoreMonthWasCalled |> nok
        math.AddYears(month, 1000)      |> ignore
        math.AddYearsCoreMonthWasCalled |> ok

    [<Fact>]
    let ``AddMonths(month) calls AddMonthsCore()`` () =
        math.AddMonthsCoreMonthWasCalled |> nok
        math.AddMonths(month, 1000)      |> ignore
        math.AddMonthsCoreMonthWasCalled |> ok

    [<Fact>]
    let ``CountYearsBetween(month) calls CountYearsBetweenCore()`` () =
        math.CountYearsBetweenCoreMonthWasCalled  |> nok
        math.CountYearsBetween(month, otherMonth) |> ignore
        math.CountYearsBetweenCoreMonthWasCalled  |> ok

    [<Fact>]
    let ``CountMonthsBetween(month) calls CountMonthsBetweenCore()`` () =
        math.CountMonthsBetweenCoreMonthWasCalled  |> nok
        math.CountMonthsBetween(month, otherMonth) |> ignore
        math.CountMonthsBetweenCoreMonthWasCalled  |> ok
