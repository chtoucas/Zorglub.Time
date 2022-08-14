// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.IsoWeekayTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time

open Xunit

let invalidDayOfWeekData  = EnumDataSet.InvalidDayOfWeekData
let invalidIsoWeekdayData  = EnumDataSet.InvalidIsoWeekdayData

[<Theory; MemberData(nameof(invalidIsoWeekdayData))>]
let ``ToDayOfWeek() throws when the value of IsoWeekday is out of range`` (weekday: IsoWeekday) =
     outOfRangeExn "isoWeekday" (fun () -> weekday.ToDayOfWeek())

[<Theory>]
[<InlineData(IsoWeekday.Monday,    DayOfWeek.Monday)>]
[<InlineData(IsoWeekday.Tuesday,   DayOfWeek.Tuesday)>]
[<InlineData(IsoWeekday.Wednesday, DayOfWeek.Wednesday)>]
[<InlineData(IsoWeekday.Thursday,  DayOfWeek.Thursday)>]
[<InlineData(IsoWeekday.Friday,    DayOfWeek.Friday)>]
[<InlineData(IsoWeekday.Saturday,  DayOfWeek.Saturday)>]
[<InlineData(IsoWeekday.Sunday,    DayOfWeek.Sunday)>]
let ``ToDayOfWeek()`` (weekday: IsoWeekday) dayOfWeek =
    weekday.ToDayOfWeek() === dayOfWeek

[<Theory; MemberData(nameof(invalidDayOfWeekData))>]
let ``ToIsoWeekday() throws when the value of DayOfWeek is out of range`` (dayOfWeek: DayOfWeek) =
     outOfRangeExn "dayOfWeek" (fun () -> dayOfWeek.ToIsoWeekday())

[<Theory>]
[<InlineData(DayOfWeek.Monday,    IsoWeekday.Monday)>]
[<InlineData(DayOfWeek.Tuesday,   IsoWeekday.Tuesday)>]
[<InlineData(DayOfWeek.Wednesday, IsoWeekday.Wednesday)>]
[<InlineData(DayOfWeek.Thursday,  IsoWeekday.Thursday)>]
[<InlineData(DayOfWeek.Friday,    IsoWeekday.Friday)>]
[<InlineData(DayOfWeek.Saturday,  IsoWeekday.Saturday)>]
[<InlineData(DayOfWeek.Sunday,    IsoWeekday.Sunday)>]
let ``ToIsoWeekday()`` (dayOfWeek: DayOfWeek) weekday =
    dayOfWeek.ToIsoWeekday() === weekday

