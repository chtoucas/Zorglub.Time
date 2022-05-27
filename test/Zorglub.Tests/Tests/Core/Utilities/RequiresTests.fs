// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.RequiresTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time.Core.Utilities

open Xunit

// TODO(code): failing tests; see commented code.

let private paramName = "paramName"

let dayOfWeekData  = EnumDataSet.DayOfWeekData
let invalidDayOfWeekData  = EnumDataSet.InvalidDayOfWeekData

[<Fact>]
let ``NotNull(obj) does not throw when "obj" is not null`` () =
    Requires.NotNull(new obj())
    Requires.NotNull(new obj(), paramName)

[<Fact>]
let ``NotNull(obj) throws when "obj" is null (without paramName)`` () =
    //let v = null

    //nullExn "obj" (fun () -> Requires.NotNull(v))
    nullExn "" (fun () -> Requires.NotNull(null))

[<Fact>]
let ``NotNull(obj) throws when "obj" is null (with paramName)`` () =
    let v = null

    nullExn paramName (fun () -> Requires.NotNull(v, paramName))
    nullExn paramName (fun () -> Requires.NotNull(null, paramName))

[<Theory; MemberData(nameof(dayOfWeekData))>]
let ``Defined(dayOfWeek) does not throw when "dayOfWeek" is a valid value`` (dayOfWeek: DayOfWeek) =
    Requires.Defined(dayOfWeek)
    Requires.Defined(dayOfWeek, paramName)

[<Theory; MemberData(nameof(invalidDayOfWeekData))>]
let ``Defined(dayOfWeek) throws when "dayOfWeek" is not a valid value (without paramName)`` (dayOfWeek: DayOfWeek) =
    //outOfRangeExn "dayOfWeek" (fun () -> Requires.Defined(dayOfWeek))
    outOfRangeExn "" (fun () -> Requires.Defined(dayOfWeek))

[<Theory; MemberData(nameof(invalidDayOfWeekData))>]
let ``Defined(dayOfWeek) throws when "dayOfWeek" is not a valid value (with paramName)`` (dayOfWeek: DayOfWeek) =
    outOfRangeExn paramName (fun () -> Requires.Defined(dayOfWeek, paramName))
