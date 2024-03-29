﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws for null calendar`` () =
        nullExn "calendar" (fun () -> new FauxCalendarMath(null))

    [<Fact>]
    let ``RegularMath constructor throws for non-regular schema`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxSystemCalendar(sch)

        argExn "calendar" (fun () -> new RegularMath(chr))

    //
    // Properties
    //

    [<Fact>]
    let ``Property AdditionRuleset`` () =
        let ruleset = AdditionRuleset(
            DateRule    = AdditionRule.Overspill,
            OrdinalRule = AdditionRule.Exact,
            MonthRule   = AdditionRule.Overflow)
        let math = new FauxCalendarMath(ruleset)

        math.AdditionRuleset === ruleset

    [<Fact>]
    let ``Property Cuid`` () =
        let chr = SimpleCalendar.Gregorian
        let math = new FauxCalendarMath(chr)

        math.Cuid === chr.Id

module Factories =
    [<Fact>]
    let ``Create() & CreateDefault() -> RegularMath when Regular12`` () =
        let sch = FauxSystemSchema.Regular12
        let chr = new FauxSystemCalendar(sch)
        let ruleset = new AdditionRuleset()
        let math1 = CalendarMath.CreateDefault(chr)
        let math2 = CalendarMath.Create(chr, ruleset)

        math1 |> is<RegularMath>
        math2 |> is<RegularMath>

    [<Fact>]
    let ``Create() & CreateDefault() -> RegularMath when Regular13`` () =
        let sch = FauxSystemSchema.Regular13
        let chr = new FauxSystemCalendar(sch)
        let ruleset = new AdditionRuleset()
        let math1 = CalendarMath.CreateDefault(chr)
        let math2 = CalendarMath.Create(chr, ruleset)

        math1 |> is<RegularMath>
        math2 |> is<RegularMath>

    [<Fact>]
    let ``Create() & CreateDefault() -> RegularMath when Regular14`` () =
        let sch = FauxSystemSchema.Regular14
        let chr = new FauxSystemCalendar(sch)
        let ruleset = new AdditionRuleset()
        let math1 = CalendarMath.CreateDefault(chr)
        let math2 = CalendarMath.Create(chr, ruleset)

        math1 |> is<RegularMath>
        math2 |> is<RegularMath>

    [<Fact>]
    let ``Create() & CreateDefault() -> PlainMath when not regular`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxSystemCalendar(sch)
        let ruleset = new AdditionRuleset()
        let math1 = CalendarMath.CreateDefault(chr)
        let math2 = CalendarMath.Create(chr, ruleset)

        math1 |> is<PlainMath>
        math2 |> is<PlainMath>

    [<Fact>]
    let ``Create()`` () =
        let sch = new FauxSystemSchema()
        let chr = new FauxSystemCalendar(sch)
        let ruleset = new AdditionRuleset(
            DateRule    = AdditionRule.Overspill,
            OrdinalRule = AdditionRule.Exact,
            MonthRule   = AdditionRule.Overflow)
        let math = CalendarMath.Create(chr, ruleset)

        math |> is<PowerMath>
        math.AdditionRuleset === ruleset

module Validation =
    let private chr = SimpleCalendar.Gregorian
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
    let ``CountYearsBetween(invalid month) throws`` () =
        argExn "end" (fun () -> math.CountYearsBetween(month, invalidMonth))
        argExn "start" (fun () -> math.CountYearsBetween(invalidMonth, month))

module Core =
    let private chr = SimpleCalendar.Gregorian
    let private math = new FauxCalendarMath(chr)

    let private date = new CalendarDate(1, 1, 1, chr.Id)
    let private ordinalDate = new OrdinalDate(1, 1, chr.Id)
    let private month = new CalendarMonth(1, 1, chr.Id)

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
    let ``AddYears(ordinal date) calls AddYearsCore()`` () =
        math.AddYearsCoreOrdinalDateWasCalled |> nok
        math.AddYears(ordinalDate, 1000)      |> ignore
        math.AddYearsCoreOrdinalDateWasCalled |> ok

    [<Fact>]
    let ``AddYears(month) calls AddYearsCore()`` () =
        math.AddYearsCoreMonthWasCalled |> nok
        math.AddYears(month, 1000)      |> ignore
        math.AddYearsCoreMonthWasCalled |> ok
