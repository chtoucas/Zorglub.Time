// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.IsoDayOfWeekTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time

open Xunit

open type Zorglub.Time.Extensions.IsoDayOfWeekExtensions

let invalidIsoDayOfWeekData  = EnumDataSet.InvalidIsoDayOfWeekData

[<TestExtrasAssembly>]
[<Theory; MemberData(nameof(invalidIsoDayOfWeekData))>]
let ``ToDayOfWeek() throws when the value of IsoDayOfWeek is out of range`` (dayOfWeek: IsoDayOfWeek) =
     outOfRangeExn "this" (fun () -> dayOfWeek.ToDayOfWeek())

[<TestExtrasAssembly>]
[<Theory>]
[<InlineData(IsoDayOfWeek.Monday,    DayOfWeek.Monday)>]
[<InlineData(IsoDayOfWeek.Tuesday,   DayOfWeek.Tuesday)>]
[<InlineData(IsoDayOfWeek.Wednesday, DayOfWeek.Wednesday)>]
[<InlineData(IsoDayOfWeek.Thursday,  DayOfWeek.Thursday)>]
[<InlineData(IsoDayOfWeek.Friday,    DayOfWeek.Friday)>]
[<InlineData(IsoDayOfWeek.Saturday,  DayOfWeek.Saturday)>]
[<InlineData(IsoDayOfWeek.Sunday,    DayOfWeek.Sunday)>]
let ``ToDayOfWeek()`` (iso: IsoDayOfWeek) dayOfWeek =
    iso.ToDayOfWeek() === dayOfWeek
