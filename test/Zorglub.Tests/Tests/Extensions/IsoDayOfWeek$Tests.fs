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

[<Theory; MemberData(nameof(invalidIsoDayOfWeekData))>]
let ``ToIsoWeekday() throws when the value of IsoDayOfWeek is out of range`` (dayOfWeek: IsoDayOfWeek) =
     outOfRangeExn "this" (fun () -> dayOfWeek.ToIsoWeekday())

[<Theory>]
[<InlineData(IsoDayOfWeek.Monday,    1)>]
[<InlineData(IsoDayOfWeek.Tuesday,   2)>]
[<InlineData(IsoDayOfWeek.Wednesday, 3)>]
[<InlineData(IsoDayOfWeek.Thursday,  4)>]
[<InlineData(IsoDayOfWeek.Friday,    5)>]
[<InlineData(IsoDayOfWeek.Saturday,  6)>]
[<InlineData(IsoDayOfWeek.Sunday,    7)>]
let ``ToIsoWeekday()`` (iso: IsoDayOfWeek) dayOfWeek =
    iso.ToIsoWeekday() === dayOfWeek

[<Theory; MemberData(nameof(invalidIsoDayOfWeekData))>]
let ``ToDayOfWeek() throws when the value of IsoDayOfWeek is out of range`` (dayOfWeek: IsoDayOfWeek) =
     outOfRangeExn "this" (fun () -> dayOfWeek.ToDayOfWeek())

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
