// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Extensions.DayOfWeekExtensionsTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time.Extensions

open Xunit

let invalidDayOfWeekData  = EnumDataSet.InvalidDayOfWeekData

[<Theory; MemberData(nameof(invalidDayOfWeekData))>]
let ``ToIsoWeekday() throws when the value of DayOfWeek is out of range`` (dayOfWeek: DayOfWeek) =
     outOfRangeExn "@this" (fun () -> dayOfWeek.ToIsoWeekday())

[<Theory>]
[<InlineData(DayOfWeek.Monday,    1)>]
[<InlineData(DayOfWeek.Tuesday,   2)>]
[<InlineData(DayOfWeek.Wednesday, 3)>]
[<InlineData(DayOfWeek.Thursday,  4)>]
[<InlineData(DayOfWeek.Friday,    5)>]
[<InlineData(DayOfWeek.Saturday,  6)>]
[<InlineData(DayOfWeek.Sunday,    7)>]
let ``ToIsoWeekday()`` (dayOfWeek: DayOfWeek) iso =
    dayOfWeek.ToIsoWeekday() === iso
