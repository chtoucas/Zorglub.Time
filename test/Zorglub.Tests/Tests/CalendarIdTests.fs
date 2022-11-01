// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.CalendarIdTests

open System
open System.Linq

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Time

open Xunit

[<Sealed>]
type IdToStringData() as self =
    inherit TheoryData<CalendarId, string>()
    do
        self.Add(CalendarId.Armenian,       "Armenian")
        self.Add(CalendarId.Civil,          "Civil")
        self.Add(CalendarId.Coptic,         "Coptic")
        self.Add(CalendarId.Ethiopic,       "Ethiopic")
        self.Add(CalendarId.Gregorian,      "Gregorian")
        self.Add(CalendarId.Julian,         "Julian")
        self.Add(CalendarId.TabularIslamic, "TabularIslamic")
        self.Add(CalendarId.Zoroastrian,    "Zoroastrian")

let calendarIdData = EnumDataSet.CalendarIdData
let badCalendarIdData = EnumDataSet.InvalidCalendarIdData

[<Fact>]
let ``Sanity checks`` () =
    let data = new IdToStringData()
    let count = Enum.GetValues(typeof<CalendarId>).Length

    data.Count() === count

[<Fact>]
let ``Default value of CalendarId is Gregorian`` () =
    Unchecked.defaultof<CalendarId> === CalendarId.Gregorian

[<Theory; ClassData(typeof<IdToStringData>)>]
let ``ToString() does not use any alias`` (id: CalendarId) str =
    id.ToString() === str

//
// Extension methods
//

[<Theory; MemberData(nameof(badCalendarIdData))>]
let ``IsDefined() returns false when the CalendarId value is out of range`` (id: CalendarId) =
     CalendarIdExtensions.IsDefined(id) |> nok

[<Theory; MemberData(nameof(calendarIdData))>]
let ``IsDefined() returns true when the CalendarId value is valid`` (id: CalendarId) =
     CalendarIdExtensions.IsDefined(id) |> ok

[<Theory; MemberData(nameof(badCalendarIdData))>]
let ``ToCalendarKey() throws when the CalendarId value is out of range`` (id: CalendarId) =
     outOfRangeExn "this" (fun () -> CalendarIdExtensions.ToCalendarKey(id))

[<Theory; MemberData(nameof(calendarIdData))>]
let ``ToCalendarKey() does not throw when the CalendarId value is valid`` (id: CalendarId) =
     CalendarIdExtensions.ToCalendarKey(id) |> ignore
