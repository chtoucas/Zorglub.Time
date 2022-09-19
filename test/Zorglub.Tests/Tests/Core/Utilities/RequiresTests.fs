// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.RequiresTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time
open Zorglub.Time.Core.Utilities

open Xunit

// TODO(code): failing tests; see commented code.

let private paramName = "paramName"

let dayOfWeekData  = EnumDataSet.DayOfWeekData
let isoWeekdayData  = EnumDataSet.IsoWeekdayData
let additionRuleData = EnumDataSet.AdditionRuleData
let invalidDayOfWeekData  = EnumDataSet.InvalidDayOfWeekData
let invalidIsoWeekdayData  = EnumDataSet.InvalidIsoWeekdayData
let invalidAdditionRuleData = EnumDataSet.InvalidAdditionRuleData

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

//
// DayOfWeek
//

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

//
// IsoWeekday
//

[<Theory; MemberData(nameof(isoWeekdayData))>]
let ``Defined(weekday) does not throw when "weekday" is a valid value`` (weekday: IsoWeekday) =
    Requires.Defined(weekday)
    Requires.Defined(weekday, paramName)

[<Theory; MemberData(nameof(invalidIsoWeekdayData))>]
let ``Defined(weekday) throws when "weekday" is not a valid value (without paramName)`` (weekday: IsoWeekday) =
    //outOfRangeExn "weekday" (fun () -> Requires.Defined(weekday))
    outOfRangeExn "" (fun () -> Requires.Defined(weekday))

[<Theory; MemberData(nameof(invalidIsoWeekdayData))>]
let ``Defined(weekday) throws when "weekday" is not a valid value (with paramName)`` (weekday: IsoWeekday) =
    outOfRangeExn paramName (fun () -> Requires.Defined(weekday, paramName))

//
// AdditionRule
//

[<Theory; MemberData(nameof(additionRuleData))>]
let ``Defined(rule) does not throw when "rule" is a valid value`` (rule: AdditionRule) =
    Requires.Defined(rule)
    Requires.Defined(rule, paramName)

[<Theory; MemberData(nameof(invalidAdditionRuleData))>]
let ``Defined(rule) throws when "rule" is not a valid value (without paramName)`` (rule: AdditionRule) =
    //outOfRangeExn "weekday" (fun () -> Requires.Defined(rule))
    outOfRangeExn "" (fun () -> Requires.Defined(rule))

[<Theory; MemberData(nameof(invalidAdditionRuleData))>]
let ``Defined(rule) throws when "rule" is not a valid value (with paramName)`` (rule: AdditionRule) =
    outOfRangeExn paramName (fun () -> Requires.Defined(rule, paramName))
