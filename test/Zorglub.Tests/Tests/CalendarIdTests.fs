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
        self.Add(CalendarId.Armenian, "Armenian")
        self.Add(CalendarId.Civil, "Civil")
        self.Add(CalendarId.Coptic, "Coptic")
        self.Add(CalendarId.Ethiopic, "Ethiopic")
        self.Add(CalendarId.Gregorian, "Gregorian")
        self.Add(CalendarId.Julian, "Julian")
        self.Add(CalendarId.TabularIslamic, "TabularIslamic")
        self.Add(CalendarId.Zoroastrian, "Zoroastrian")

let validData = EnumDataSet.CalendarIdData
let invalidData = EnumDataSet.InvalidCalendarIdData

[<Fact>]
let ``Sanity checks`` () =
    let data = new IdToStringData()
    let count = Enum.GetValues(typeof<CalendarId>).Length

    data.Count() === count

[<Theory; ClassData(typeof<IdToStringData>)>]
let ``ToString() does not use any alias`` (cuid: CalendarId) str =
    cuid.ToString() === str

//
// Extension methods
//

[<Theory; MemberData(nameof(invalidData))>]
let ``IsDefined() returns false when the CalendarId value is out of range`` (cuid: CalendarId) =
     CalendarIdExtensions.IsDefined(cuid) |> nok

[<Theory; MemberData(nameof(validData))>]
let ``IsDefined() returns true when the CalendarId value is valid`` (cuid: CalendarId) =
     CalendarIdExtensions.IsDefined(cuid) |> ok

[<Theory; MemberData(nameof(invalidData))>]
let ``ToCalendarKey() throws when the CalendarId value is out of range`` (cuid: CalendarId) =
     outOfRangeExn "this" (fun () -> CalendarIdExtensions.ToCalendarKey(cuid))

[<Theory; MemberData(nameof(validData))>]
let ``ToCalendarKey() does not throw when the CalendarId value is valid`` (cuid: CalendarId) =
     CalendarIdExtensions.ToCalendarKey(cuid) |> ignore
