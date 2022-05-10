// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.GregorianFormulaeTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit

// TODO(code): overflow in Release build.

// Overflow tests for 32-bit versions:
// - same as with the instance methods.
// - we cannot use Int32.Min/MaxValue for month.
// Overflow tests for 64-bit versions:
// - "unbounded" methods, use Int64.Min/MaxValue
// - other methods, use Min/MaxMonth and Min/MaxDay and for the year or
//   daysSinceEpoch params, use Int32.Min/MaxValue if Int64 is too large.
// Release builds use unchecked arithmetic: a bunch of methods do not overflow
// even if they should, but these methods are internal and normally a caller
// ensures that it only calls them in situations where there is no risk of
// overflow.

let [<Literal>] private MinDay = Yemoda.MinDay
let [<Literal>] private MaxDay = Yemoda.MaxDay

let [<Literal>] private MinMonth = Yemoda.MinMonth
let [<Literal>] private MaxMonth = Yemoda.MaxMonth

let private schema = new GregorianSchema()
let private minYear, maxYear = schema.SupportedYears.Endpoints.Deconstruct()
let private minDaysSinceEpoch = schema.GetStartOfYear(minYear)
let private maxDaysSinceEpoch = schema.GetEndOfYear(maxYear)

let private dataSet = GregorianDataSet.Instance

let daysSinceEpochInfoData = dataSet.DaysSinceEpochInfoData
let dateInfoData = dataSet.DateInfoData
let monthInfoData = dataSet.MonthInfoData
let yearInfoData = dataSet.YearInfoData
let startOfYearDaysSinceEpochData = dataSet.StartOfYearDaysSinceEpochData
let endOfYearDaysSinceEpochData = dataSet.EndOfYearDaysSinceEpochData

//
// IsIntercalaryDay()
//

[<Theory; MemberData(nameof(dateInfoData))>]
let ``IsIntercalaryDay(int32)`` (x: DateInfo) =
    let _, m, d, _ = x.Deconstruct()
    GregorianFormulae.IsIntercalaryDay(m, d) === x.IsIntercalary

//
// IsLeapYear()
//

[<Theory; MemberData(nameof(yearInfoData))>]
let ``IsLeapYear(int64)`` (x: YearInfo) =
    GregorianFormulae.IsLeapYear(int64(x.Year)) === x.IsLeap

[<Fact>]
let ``IsLeapYear(int32) does not overflow`` () =
    GregorianFormulae.IsLeapYear(Int32.MinValue) |> ignore
    GregorianFormulae.IsLeapYear(Int32.MaxValue) |> ignore

[<Fact>]
let ``IsLeapYear(int64) does not overflow`` () =
    GregorianFormulae.IsLeapYear(Int64.MinValue) |> ignore
    GregorianFormulae.IsLeapYear(Int64.MaxValue) |> ignore

//
// CountDaysInYear()
//

[<Theory; MemberData(nameof(yearInfoData))>]
let ``CountDaysInYear(int32)`` (x: YearInfo) =
    GregorianFormulae.CountDaysInYear(x.Year) === x.DaysInYear

[<Theory; MemberData(nameof(yearInfoData))>]
let ``CountDaysInYear(int64)`` (x: YearInfo) =
    GregorianFormulae.CountDaysInYear(int64(x.Year)) === x.DaysInYear

[<Fact>]
let ``CountDaysInYear(int32) does not overflow`` () =
    GregorianFormulae.CountDaysInYear(Int32.MinValue) |> ignore
    GregorianFormulae.CountDaysInYear(Int32.MaxValue) |> ignore

[<Fact>]
let ``CountDaysInYear(int64) does not overflow`` () =
    GregorianFormulae.CountDaysInYear(Int64.MinValue) |> ignore
    GregorianFormulae.CountDaysInYear(Int64.MaxValue) |> ignore

//
// CountDaysInYearBeforeMonth()
//

[<Theory; MemberData(nameof(monthInfoData))>]
let ``CountDaysInYearBeforeMonth(int32)`` (x: MonthInfo) =
    let y, m = x.Yemo.Deconstruct()
    GregorianFormulae.CountDaysInYearBeforeMonth(y, m) === x.DaysInYearBeforeMonth

[<Theory; MemberData(nameof(monthInfoData))>]
let ``CountDaysInYearBeforeMonth(int64)`` (x: MonthInfo) =
    let y, m = x.Yemo.Deconstruct()
    GregorianFormulae.CountDaysInYearBeforeMonth(int64(y), m) === x.DaysInYearBeforeMonth

[<Fact>]
let ``CountDaysInYearBeforeMonth(int32) does not overflow`` () =
    GregorianFormulae.CountDaysInYearBeforeMonth(Int32.MinValue, MinMonth) |> ignore
    GregorianFormulae.CountDaysInYearBeforeMonth(Int32.MaxValue, MaxMonth) |> ignore

[<Fact>]
let ``CountDaysInYearBeforeMonth(int64) does not overflow`` () =
    GregorianFormulae.CountDaysInYearBeforeMonth(Int64.MinValue, MinMonth) |> ignore
    GregorianFormulae.CountDaysInYearBeforeMonth(Int64.MaxValue, MaxMonth) |> ignore

//
// CountDaysInMonth()
//

[<Theory; MemberData(nameof(monthInfoData))>]
let ``CountDaysInMonth(int32)`` (x: MonthInfo) =
    let y, m = x.Yemo.Deconstruct()
    GregorianFormulae.CountDaysInMonth(y, m) === x.DaysInMonth

[<Theory; MemberData(nameof(monthInfoData))>]
let ``CountDaysInMonth(int64)`` (x: MonthInfo) =
    let y, m = x.Yemo.Deconstruct()
    GregorianFormulae.CountDaysInMonth(int64(y), m) === x.DaysInMonth

[<Fact>]
let ``CountDaysInMonth(int32) does not overflow`` () =
    GregorianFormulae.CountDaysInMonth(Int32.MinValue, MinMonth) |> ignore
    GregorianFormulae.CountDaysInMonth(Int32.MaxValue, MaxMonth) |> ignore

[<Fact>]
let ``CountDaysInMonth(int64) does not overflow`` () =
    GregorianFormulae.CountDaysInMonth(Int64.MinValue, MinMonth) |> ignore
    GregorianFormulae.CountDaysInMonth(Int64.MaxValue, MaxMonth) |> ignore

//
// CountDaysSinceEpoch()
//

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``CountDaysSinceEpoch(int32)`` (x: DaysSinceEpochInfo) =
    let daysSinceEpoch, y, m, d = x.Deconstruct()
    GregorianFormulae.CountDaysSinceEpoch(y, m, d) === daysSinceEpoch

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``CountDaysSinceEpoch(int64)`` (x: DaysSinceEpochInfo) =
    let daysSinceEpoch, y, m, d = x.Deconstruct()
    GregorianFormulae.CountDaysSinceEpoch(int64(y), m, d) === daysSinceEpoch

[<Fact>]
let ``CountDaysSinceEpoch(int32) does not overflow at Min/MaxYear`` () =
    GregorianFormulae.CountDaysSinceEpoch(minYear, MinMonth, MinDay) |> ignore
    GregorianFormulae.CountDaysSinceEpoch(maxYear, MaxMonth, MaxDay) |> ignore

#if DEBUG
[<Fact>]
let ``CountDaysSinceEpoch(int32) overflows at Int32.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.CountDaysSinceEpoch(Int32.MinValue, MinMonth, MinDay)) |> overflows
    (fun () -> GregorianFormulae.CountDaysSinceEpoch(Int32.MaxValue, MaxMonth, MaxDay)) |> overflows
#else
[<Fact>]
let ``CountDaysSinceEpoch(int32) does not overflow at Int32.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.CountDaysSinceEpoch(Int32.MinValue, MinMonth, MinDay) |> ignore
    GregorianFormulae.CountDaysSinceEpoch(Int32.MaxValue, MaxMonth, MaxDay) |> ignore
#endif

[<Fact>]
let ``CountDaysSinceEpoch(int64) does not overflow at Int32.Min/MaxValue`` () =
    GregorianFormulae.CountDaysSinceEpoch(int64(Int32.MinValue), MinMonth, MinDay) |> ignore
    GregorianFormulae.CountDaysSinceEpoch(int64(Int32.MaxValue), MaxMonth, MaxDay) |> ignore

#if DEBUG
[<Fact>]
let ``CountDaysSinceEpoch(int64) overflows at Int64.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.CountDaysSinceEpoch(Int64.MinValue, MinMonth, MinDay)) |> overflows
    (fun () -> GregorianFormulae.CountDaysSinceEpoch(Int64.MaxValue, MaxMonth, MaxDay)) |> overflows
#else
[<Fact>]
let ``CountDaysSinceEpoch(int64) does not overflow at Int64.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.CountDaysSinceEpoch(Int64.MinValue, MinMonth, MinDay) |> ignore
    GregorianFormulae.CountDaysSinceEpoch(Int64.MaxValue, MaxMonth, MaxDay) |> ignore
#endif

//
// GetDateParts()
//

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``GetDateParts(int32)`` (x: DaysSinceEpochInfo) =
    let daysSinceEpoch, ymd = x.Deconstruct()
    GregorianFormulae.GetDateParts(daysSinceEpoch) === ymd

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``GetDateParts(int64)`` (x: DaysSinceEpochInfo) =
    let daysSinceEpoch, y, m, d = x.Deconstruct()
    GregorianFormulae.GetDateParts(int64(daysSinceEpoch)) === (y, m, d)

[<Fact>]
let ``GetDateParts(int32) does not overflow at Min/MaxDaysSinceEpoch`` () =
    GregorianFormulae.GetDateParts(minDaysSinceEpoch) |> ignore
    GregorianFormulae.GetDateParts(maxDaysSinceEpoch) |> ignore

#if DEBUG
[<Fact>]
let ``GetDateParts(int32) overflows at Int32.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetDateParts(Int32.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetDateParts(Int32.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetDateParts(int32) does not overflow at Int32.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.GetDateParts(Int32.MinValue) |> ignore
    GregorianFormulae.GetDateParts(Int32.MaxValue) |> ignore
#endif

[<Fact>]
let ``GetDateParts(int64) does not overflow at Int32.Min/MaxValue`` () =
    GregorianFormulae.GetDateParts(int64(Int32.MinValue)) |> ignore
    GregorianFormulae.GetDateParts(int64(Int32.MaxValue)) |> ignore

#if DEBUG
[<Fact>]
let ``GetDateParts(int64) overflows at Int64.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetDateParts(Int64.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetDateParts(Int64.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetDateParts(int64) does not overflow at Int64.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.GetDateParts(Int64.MinValue) |> ignore
    GregorianFormulae.GetDateParts(Int64.MaxValue) |> ignore
#endif

//
// GetOrdinalParts()
//

[<Theory; MemberData(nameof(dateInfoData))>]
let ``GetOrdinalParts(int32)`` (x: DateInfo) =
    let y, m, d, doy = x.Deconstruct()
    let daysSinceEpoch = GregorianFormulae.CountDaysSinceEpoch(y, m, d)
    GregorianFormulae.GetOrdinalParts(daysSinceEpoch) === new Yedoy(y, doy)

[<Fact>]
let ``GetOrdinalParts(int32) does not overflow at Min/MaxDaysSinceEpoch`` () =
    GregorianFormulae.GetOrdinalParts(minDaysSinceEpoch) |> ignore
    GregorianFormulae.GetOrdinalParts(maxDaysSinceEpoch) |> ignore

#if DEBUG
[<Fact>]
let ``GetOrdinalParts(int32) overflows at Int32.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetOrdinalParts(Int32.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetOrdinalParts(Int32.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetOrdinalParts(int32) does not overflow at Int32.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.GetOrdinalParts(Int32.MinValue) |> ignore
    GregorianFormulae.GetOrdinalParts(Int32.MaxValue) |> ignore
#endif

//
// GetYear()
//

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``GetYear(int32)`` (x: DaysSinceEpochInfo) =
    GregorianFormulae.GetYear(x.DaysSinceEpoch) === x.Year

[<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
let ``GetYear(int64)`` (x: DaysSinceEpochInfo) =
    GregorianFormulae.GetYear(int64(x.DaysSinceEpoch)) === x.Year

[<Fact>]
let ``GetYear(int32) does not overflow at Min/MaxDaysSinceEpoch`` () =
    GregorianFormulae.GetYear(minDaysSinceEpoch) |> ignore
    GregorianFormulae.GetYear(maxDaysSinceEpoch) |> ignore

#if DEBUG
[<Fact>]
let ``GetYear(int32) overflows at Int32.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetYear(Int32.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetYear(Int32.MaxValue)) |> overflows
 #else
[<Fact>]
let ``GetYear(int32) does not overflow at Int32.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.GetYear(Int32.MinValue) |> ignore
    GregorianFormulae.GetYear(Int32.MaxValue) |> ignore
 #endif

[<Fact>]
let ``GetYear(int64) does not overflow at Int32.Min/MaxValue`` () =
    GregorianFormulae.GetYear(int64(Int32.MinValue)) |> ignore
    GregorianFormulae.GetYear(int64(Int32.MaxValue)) |> ignore

#if DEBUG
[<Fact>]
let ``GetYear(int64) overflows at Int64.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetYear(Int64.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetYear(Int64.MaxValue)) |> overflows
 #else
[<Fact>]
let ``GetYear(int64) does not overflow at Int64.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.GetYear(Int64.MinValue) |> ignore
    GregorianFormulae.GetYear(Int64.MaxValue) |> ignore
 #endif

//
// GetStartOfYear()
//

[<Theory; MemberData(nameof(startOfYearDaysSinceEpochData))>]
let ``GetStartOfYear(int32)`` (x: YearDaysSinceEpoch) =
    GregorianFormulae.GetStartOfYear(x.Year) === x.DaysSinceEpoch

[<Theory; MemberData(nameof(startOfYearDaysSinceEpochData))>]
let ``GetStartOfYear(int64)`` (x: YearDaysSinceEpoch) =
    GregorianFormulae.GetStartOfYear(int64(x.Year)) === x.DaysSinceEpoch

[<Fact>]
let ``GetStartOfYear(int32) does not overflow`` () =
    GregorianFormulae.GetStartOfYear(minYear) |> ignore
    GregorianFormulae.GetStartOfYear(maxYear) |> ignore

#if DEBUG
[<Fact>]
let ``GetStartOfYear(int32) overflows at Int32.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetStartOfYear(Int32.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetStartOfYear(Int32.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetStartOfYear(int32) does not overflow at Int32.Min/MaxValue (unchecked)`` () =
     GregorianFormulae.GetStartOfYear(Int32.MinValue) |> ignore
     GregorianFormulae.GetStartOfYear(Int32.MaxValue) |> ignore
#endif

[<Fact>]
let ``GetStartOfYear(int64) does not overflow at Int32.Min/MaxValue`` () =
    GregorianFormulae.GetStartOfYear(int64(Int32.MinValue)) |> ignore
    GregorianFormulae.GetStartOfYear(int64(Int32.MaxValue)) |> ignore

#if DEBUG
[<Fact>]
let ``GetStartOfYear(int64) overflows at Int64.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetStartOfYear(Int64.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetStartOfYear(Int64.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetStartOfYear(int64) does not overflow at Int64.Min/MaxValue (unchecked)`` () =
    GregorianFormulae.GetStartOfYear(Int64.MinValue) |> ignore
    GregorianFormulae.GetStartOfYear(Int64.MaxValue) |> ignore
#endif

//
// GetEndOfYear()
//

[<Theory; MemberData(nameof(endOfYearDaysSinceEpochData))>]
let ``GetEndOfYear(int32)`` (x: YearDaysSinceEpoch) =
    GregorianFormulae.GetEndOfYear(x.Year) === x.DaysSinceEpoch

[<Fact>]
let ``GetEndOfYear(int32) does not overflow`` () =
    GregorianFormulae.GetEndOfYear(minYear) |> ignore
    GregorianFormulae.GetEndOfYear(maxYear) |> ignore

#if DEBUG
[<Fact>]
let ``GetEndOfYear(int32) overflows at Int32.Min/MaxValue`` () =
    (fun () -> GregorianFormulae.GetEndOfYear(Int32.MinValue)) |> overflows
    (fun () -> GregorianFormulae.GetEndOfYear(Int32.MaxValue)) |> overflows
#else
[<Fact>]
let ``GetEndOfYear(int32) does not overflow at Int32.Min/MaxValue (unchecked)`` () =
     GregorianFormulae.GetEndOfYear(Int32.MinValue) |> ignore
     GregorianFormulae.GetEndOfYear(Int32.MaxValue) |> ignore
#endif
