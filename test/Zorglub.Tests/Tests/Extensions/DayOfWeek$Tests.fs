// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Extensions.DayOfWeekExtensionsTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time

open Xunit

open type Zorglub.Time.Extensions.DayOfWeekExtensions

let invalidDayOfWeekData  = EnumDataSet.InvalidDayOfWeekData

[<TestExtrasAssembly>]
[<Theory; MemberData(nameof(invalidDayOfWeekData))>]
let ``ToIsoDayOfWeek() throws when the value of DayOfWeek is out of range`` (dayOfWeek: DayOfWeek) =
     outOfRangeExn "@this" (fun () -> dayOfWeek.ToIsoDayOfWeek())

[<TestExtrasAssembly>]
[<Theory>]
[<InlineData(DayOfWeek.Monday,    IsoDayOfWeek.Monday)>]
[<InlineData(DayOfWeek.Tuesday,   IsoDayOfWeek.Tuesday)>]
[<InlineData(DayOfWeek.Wednesday, IsoDayOfWeek.Wednesday)>]
[<InlineData(DayOfWeek.Thursday,  IsoDayOfWeek.Thursday)>]
[<InlineData(DayOfWeek.Friday,    IsoDayOfWeek.Friday)>]
[<InlineData(DayOfWeek.Saturday,  IsoDayOfWeek.Saturday)>]
[<InlineData(DayOfWeek.Sunday,    IsoDayOfWeek.Sunday)>]
let ``ToIsoDayOfWeek()`` (dayOfWeek: DayOfWeek) iso =
    dayOfWeek.ToIsoDayOfWeek() === iso
