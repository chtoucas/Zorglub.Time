// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Testing.EnumDataSetTests

open System
open System.Linq

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Time

open Xunit

[<Fact>]
let ``DayOfWeekData is exhaustive`` () =
    let count = Enum.GetValues(typeof<DayOfWeek>).Length

    EnumDataSet.DayOfWeekData.Count === count

[<Fact>]
let ``AdditionRuleData is exhaustive`` () =
    let count = Enum.GetValues(typeof<AdditionRule>).Length

    EnumDataSet.AdditionRuleData.Count === count

[<Fact>]
let ``CalendricalAlgorithmData is exhaustive`` () =
    let count = Enum.GetValues(typeof<CalendricalAlgorithm>).Length

    EnumDataSet.CalendricalAlgorithmData.Count === count

[<Fact>]
let ``CalendricalFamilyData is exhaustive`` () =
    let count = Enum.GetValues(typeof<CalendricalFamily>).Length

    EnumDataSet.CalendricalFamilyData.Count === count

[<Fact>]
let ``CalendarIdData is exhaustive`` () =
    let count = Enum.GetValues(typeof<CalendarId>).Length

    EnumDataSet.CalendarIdData.Count === count
