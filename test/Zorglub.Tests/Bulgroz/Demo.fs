// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Bulgroz.Demo

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Horology
open Zorglub.Time.Simple
open Zorglub.Time.Specialized

open type Zorglub.Time.Extensions.SimpleDateExtensions
open type Zorglub.Time.Extensions.Unboxing

let ``Gregorian date`` () =
    let clock = CivilClock.Local
    let today = clock.GetCurrentDate()

    printfn "CivilDate today"
    printfn "%O, %O" today.DayOfWeek today
    printfn "  IsIntercalary    = %b" today.IsIntercalary
    printfn "  DayOfYear        = %i" today.DayOfYear
    printfn "  Century          = %i" today.Century
    printfn "  YearOfCentury    = %i" today.YearOfCentury
    printfn "  DayNumber        = %O" today.DayNumber

let ``Gregorian calendar`` () =
    let clock = SimpleCalendar.Gregorian.LocalClock
    let day = clock.GetCurrentDay()
    let month = day.CalendarMonth
    let year = day.CalendarYear

    printfn "Gregorian calendar"
    printfn "  Calendar day     = %O" day
    printfn "           month   = %O" month
    printfn "           year    = %O" year
    printfn "  Ordinal date     = %O" <| day.ToOrdinalDate()

    printfn "  End of month     = %O" month.LastDay
    printfn "         year      = %O" year.LastDay

let ``Gregorian calendar w/ dates after 15/10/1582`` () =
    let sch = GregorianSchema.GetInstance()
    let q = sch.Select(fun x ->
        BoundedBelowScope.StartingAt(x, DayZero.NewStyle, new DateParts(1582, 10, 15))).Select(fun x ->
        new BoundedBelowCalendar("Genuine Gregorian", x))
    let chr = q.Unbox()

    let clock = SystemClock.Local
    let today = clock.Today()
    let parts = chr.GetDateParts(today)
    let y, m, d = parts.Deconstruct()

    printfn "Gregorian calendar"
    printfn "  Today            = %i/%i/%i (%O)" d m y chr

let ``Gregorian calendar w/ dates after 1/1/1`` () =
    let sch = GregorianSchema.GetInstance()
    let q = sch.Select(fun x ->
        MinMaxYearScope.StartingAt(x, DayZero.NewStyle, 1)).Select(fun x ->
        new MinMaxYearCalendar("Gregorian", x))
    let chr = q.Unbox()

    let clock = SystemClock.Local
    let today = clock.Today()
    let parts = chr.GetDateParts(today)
    let y, m, d = parts.Deconstruct()

    printfn "Custom Gregorian calendar"
    printfn "  Today            = %i/%i/%i (%O)" d m y chr

let ``Armenian calendar`` () =
    let clock = SimpleCalendar.Armenian.LocalClock
    let day = clock.GetCurrentDay()
    let month = day.CalendarMonth
    let year = day.CalendarYear

    printfn "Armenian calendar"
    printfn "  Calendar day     = %O" day
    printfn "           month   = %O" month
    printfn "           year    = %O" year
    printfn "  Ordinal date     = %O" <| day.ToOrdinalDate()

    let mutable epanum = 0
    printfn "  Epagomenal day?  = %b" <| day.IsEpagomenal(&epanum)
    printfn "             num   = %i" epanum

    printfn "  End of month     = %O" month.LastDay
    printfn "         year      = %O" year.LastDay
