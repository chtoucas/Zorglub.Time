// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.JulianFormulaeTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit

let [<Literal>] private MinDay = Yemoda.MinDay
let [<Literal>] private MaxDay = Yemoda.MaxDay

let [<Literal>] private MinMonth = Yemoda.MinMonth
let [<Literal>] private MaxMonth = Yemoda.MaxMonth

let private sch = new JulianSchema()
let private minYear, maxYear = sch.SupportedYears.Endpoints.Deconstruct()

let private dataSet = JulianDataSet.Instance

let daysSinceEpochInfoData = dataSet.DaysSinceEpochInfoData
let dateInfoData = dataSet.DateInfoData
let monthInfoData = dataSet.MonthInfoData
let yearInfoData = dataSet.YearInfoData
let startOfYearDaysSinceEpochData = dataSet.StartOfYearDaysSinceEpochData

//
// IsLeapYear()
//

[<Theory; MemberData(nameof(yearInfoData))>]
let ``IsLeapYear(int32)`` (x: YearInfo) =
    JulianFormulae.IsLeapYear(x.Year) === x.IsLeap

[<Theory; MemberData(nameof(yearInfoData))>]
let ``IsLeapYear(int64)`` (x: YearInfo) =
    JulianFormulae.IsLeapYear(int64(x.Year)) === x.IsLeap

[<Fact>]
let ``IsLeapYear(int32) does not overflow`` () =
    JulianFormulae.IsLeapYear(Int32.MinValue) |> ignore
    JulianFormulae.IsLeapYear(Int32.MaxValue) |> ignore

[<Fact>]
let ``IsLeapYear(int64) does not overflow`` () =
    JulianFormulae.IsLeapYear(Int64.MinValue) |> ignore
    JulianFormulae.IsLeapYear(Int64.MaxValue) |> ignore

//
// CountDaysInYear()
//

[<Theory; MemberData(nameof(yearInfoData))>]
let ``CountDaysInYear(int32)`` (x: YearInfo) =
    JulianFormulae.CountDaysInYear(x.Year) === x.DaysInYear

[<Theory; MemberData(nameof(yearInfoData))>]
let ``CountDaysInYear(int64)`` (x: YearInfo) =
    JulianFormulae.CountDaysInYear(int64(x.Year)) === x.DaysInYear

[<Fact>]
let ``CountDaysInYear(int32) does not overflow`` () =
    JulianFormulae.CountDaysInYear(Int32.MinValue) |> ignore
    JulianFormulae.CountDaysInYear(Int32.MaxValue) |> ignore

[<Fact>]
let ``CountDaysInYear(int64) does not overflow`` () =
    JulianFormulae.CountDaysInYear(Int64.MinValue) |> ignore
    JulianFormulae.CountDaysInYear(Int64.MaxValue) |> ignore

//
// CountDaysInMonth()
//

[<Theory; MemberData(nameof(monthInfoData))>]
let ``CountDaysInMonth(int32)`` (x: MonthInfo) =
    let y, m = x.Yemo.Deconstruct()
    JulianFormulae.CountDaysInMonth(y, m) === x.DaysInMonth

[<Theory; MemberData(nameof(monthInfoData))>]
let ``CountDaysInMonth(int64)`` (x: MonthInfo) =
    let y, m = x.Yemo.Deconstruct()
    JulianFormulae.CountDaysInMonth(int64(y), m) === x.DaysInMonth

[<Fact>]
let ``CountDaysInMonth(int32) does not overflow`` () =
    JulianFormulae.CountDaysInMonth(Int32.MinValue, MinMonth) |> ignore
    JulianFormulae.CountDaysInMonth(Int32.MaxValue, MaxMonth) |> ignore

[<Fact>]
let ``CountDaysInMonth(int64) does not overflow`` () =
    JulianFormulae.CountDaysInMonth(Int64.MinValue, MinMonth) |> ignore
    JulianFormulae.CountDaysInMonth(Int64.MaxValue, MaxMonth) |> ignore

//
// CountDaysSinceEpoch()
//

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``CountDaysSinceEpoch(int64)`` (x: DaysSinceEpochInfo) =
    let daysSinceEpoch, y, m, d = x.Deconstruct()
    JulianFormulae.CountDaysSinceEpoch(int64(y), m, d) === daysSinceEpoch

[<Fact>]
let ``CountDaysSinceEpoch(int64) does not overflow at Int32.Min/MaxValue`` () =
    JulianFormulae.CountDaysSinceEpoch(int64(Int32.MinValue), MinMonth, MinDay) |> ignore
    JulianFormulae.CountDaysSinceEpoch(int64(Int32.MaxValue), MaxMonth, MaxDay) |> ignore

#if DEBUG
[<Fact>]
let ``CountDaysSinceEpoch(int64) overflows at Int64.Min/MaxValue`` () =
    (fun () -> JulianFormulae.CountDaysSinceEpoch(Int64.MinValue, MinMonth, MinDay)) |> overflows
    (fun () -> JulianFormulae.CountDaysSinceEpoch(Int64.MaxValue, MaxMonth, MaxDay)) |> overflows
#else
[<Fact>]
let ``CountDaysSinceEpoch(int64) does not overflow at Int64.Min/MaxValue (unchecked)`` () =
    JulianFormulae.CountDaysSinceEpoch(Int64.MinValue, MinMonth, MinDay) |> ignore
    JulianFormulae.CountDaysSinceEpoch(Int64.MaxValue, MaxMonth, MaxDay) |> ignore
#endif

//
// GetDateParts()
//

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``GetDateParts(int64)`` (x: DaysSinceEpochInfo) =
    let daysSinceEpoch, y, m, d = x.Deconstruct()
    JulianFormulae.GetDateParts(int64(daysSinceEpoch)) === (y, m, d)

[<Fact>]
let ``GetDateParts(int64) does not overflow at Int32.Min/MaxValue`` () =
    JulianFormulae.GetDateParts(int64(Int32.MinValue)) |> ignore
    JulianFormulae.GetDateParts(int64(Int32.MaxValue)) |> ignore

#if DEBUG
[<Fact>]
let ``GetDateParts(int64) overflows at Int64.Min/MaxValue`` () =
    (fun () -> JulianFormulae.GetDateParts(Int64.MinValue)) |> overflows
    (fun () -> JulianFormulae.GetDateParts(Int64.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetDateParts(int64) does not overflowsat Int64.Min/MaxValue (unchecked)`` () =
    JulianFormulae.GetDateParts(Int64.MinValue) |> ignore
    JulianFormulae.GetDateParts(Int64.MaxValue) |> ignore
#endif

//
// GetYear()
//

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``GetYear(int64)`` (x: DaysSinceEpochInfo) =
    JulianFormulae.GetYear(int64(x.DaysSinceEpoch)) === x.Year

[<Fact>]
let ``GetYear(int64) does not overflow`` () =
    JulianFormulae.GetYear(Int64.MinValue) |> ignore
    JulianFormulae.GetYear(Int64.MaxValue) |> ignore

//
// GetStartOfYear()
//

[<Theory; MemberData(nameof(startOfYearDaysSinceEpochData))>]
let ``GetStartOfYear(int32)`` (x: YearDaysSinceEpoch) =
    JulianFormulae.GetStartOfYear(x.Year) === x.DaysSinceEpoch

[<Theory; MemberData(nameof(startOfYearDaysSinceEpochData))>]
let ``GetStartOfYear(int64)`` (x: YearDaysSinceEpoch) =
    JulianFormulae.GetStartOfYear(int64(x.Year)) === x.DaysSinceEpoch

[<Fact>]
let ``GetStartOfYear(int32) does not overflow at MinYear/MaxYear`` () =
    JulianFormulae.GetStartOfYear(minYear) |> ignore
    JulianFormulae.GetStartOfYear(maxYear) |> ignore

[<Fact>]
let ``GetStartOfYear(int64) does not overflow at Int32.Min/MaxValue`` () =
    JulianFormulae.GetStartOfYear(int64(Int32.MinValue)) |> ignore
    JulianFormulae.GetStartOfYear(int64(Int32.MaxValue)) |> ignore

#if DEBUG
[<Fact>]
let ``GetStartOfYear(int64) overflows at Int64.Min/MaxValue`` () =
    (fun () -> JulianFormulae.GetStartOfYear(Int64.MinValue)) |> overflows
    (fun () -> JulianFormulae.GetStartOfYear(Int64.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetStartOfYear(int64) does not overflow at Int64.Min/MaxValue (unchecked)`` () =
    JulianFormulae.GetStartOfYear(Int64.MinValue) |> ignore
    JulianFormulae.GetStartOfYear(Int64.MaxValue) |> ignore
#endif
