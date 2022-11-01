// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.IsoWeekdayTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Time

open Xunit

[<Sealed>]
type IsoWeekdayToDayOfWeekData() as self =
    inherit TheoryData<IsoWeekday, DayOfWeek>()
    do
        self.Add(IsoWeekday.Monday,     DayOfWeek.Monday)
        self.Add(IsoWeekday.Tuesday,    DayOfWeek.Tuesday)
        self.Add(IsoWeekday.Wednesday,  DayOfWeek.Wednesday)
        self.Add(IsoWeekday.Thursday,   DayOfWeek.Thursday)
        self.Add(IsoWeekday.Friday,     DayOfWeek.Friday)
        self.Add(IsoWeekday.Saturday,   DayOfWeek.Saturday)
        self.Add(IsoWeekday.Sunday,     DayOfWeek.Sunday)

let badDayOfWeekData = EnumDataSet.InvalidDayOfWeekData
let badIsoWeekdayData = EnumDataSet.InvalidIsoWeekdayData

[<Fact>]
let ``Default value of IsoWeekday is None`` () =
    Unchecked.defaultof<IsoWeekday> === IsoWeekday.None

//
// Extension methods
//

[<Theory; MemberData(nameof(badIsoWeekdayData))>]
let ``ToDayOfWeek() throws when the IsoWeekday value is out of range`` (weekday: IsoWeekday) =
    outOfRangeExn "isoWeekday" (fun () -> weekday.ToDayOfWeek())

[<Theory; ClassData(typeof<IsoWeekdayToDayOfWeekData>)>]
let ``ToDayOfWeek()`` (weekday: IsoWeekday) dayOfWeek =
    weekday.ToDayOfWeek() === dayOfWeek

[<Theory; MemberData(nameof(badDayOfWeekData))>]
let ``ToIsoWeekday() throws when the DayOfWeek value is out of range`` (dayOfWeek: DayOfWeek) =
    outOfRangeExn "dayOfWeek" (fun () -> dayOfWeek.ToIsoWeekday())

[<Theory; ClassData(typeof<IsoWeekdayToDayOfWeekData>)>]
let ``ToIsoWeekday()`` weekday (dayOfWeek: DayOfWeek) =
    dayOfWeek.ToIsoWeekday() === weekday
