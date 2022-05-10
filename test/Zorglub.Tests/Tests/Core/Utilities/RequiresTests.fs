// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.RequiresTests

open System

open Zorglub.Testing
open Zorglub.Time.Core.Utilities

open Xunit

// TODO(code): failing tests.

let private paramName = "paramName"

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

[<Fact>]
let ``Defined(dayOfWeek) does not throw when "dayOfWeek" is a valid value`` () =
    Requires.Defined(DayOfWeek.Monday)
    Requires.Defined(DayOfWeek.Monday, paramName)

[<Fact>]
let ``Defined(dayOfWeek) throws when "dayOfWeek" is not a valid value (without paramName)`` () =
    //outOfRangeExn "dayOfWeek" (fun () -> Requires.Defined(dayOfWeekBeforeSunday))
    outOfRangeExn "" (fun () -> Requires.Defined(dayOfWeekBeforeSunday))

[<Fact>]
let ``Defined(dayOfWeek) throws when "dayOfWeek" is not a valid value (with paramName)`` () =
    outOfRangeExn paramName (fun () -> Requires.Defined(dayOfWeekBeforeSunday, paramName))
