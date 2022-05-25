// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarCatalogTests

open System
open System.Collections.Generic

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time
open Zorglub.Time.Simple

open Xunit

let private toCalendarKey (id: CalendarId)  = CalendarIdExtensions.ToCalendarKey(id)

module Prelude =
    let calendarIdData = EnumDataSet.CalendarIdData

    [<Fact>]
    let ``Property Keys, unknown key`` () =
        Assert.DoesNotContain("Unknown Key", CalendarCatalog.Keys)

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property Keys, system key`` (id: CalendarId) =
        let key = toCalendarKey(id)

        Assert.Contains(key, CalendarCatalog.Keys)

    [<Fact>]
    let ``Property SystemCalendars is exhaustive`` () =
        let count = Enum.GetValues(typeof<CalendarId>).Length
        let calendars = CalendarCatalog.SystemCalendars

        calendars.Count === count

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property SystemCalendars`` (id: CalendarId) =
        let calendars = CalendarCatalog.SystemCalendars
        let chr = CalendarCatalog.GetSystemCalendar(id)

        Assert.Contains(chr, calendars)

module Lookup =
    let calendarIdData = EnumDataSet.CalendarIdData

    //
    // GetCalendar()
    //

    [<Fact>]
    let ``GetCalendar(unknown key)`` () =
        throws<KeyNotFoundException> (fun () -> CalendarCatalog.GetCalendar("Unknown Key"))

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendar(system key)`` (id: CalendarId) =
        let key = toCalendarKey(id)

        CalendarCatalog.GetCalendar(key) |> isnotnull

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendar(system key) repeated returns the same reference`` (id: CalendarId) =
        let key = toCalendarKey(id)
        let chr1 = CalendarCatalog.GetCalendar(key)
        let chr2 = CalendarCatalog.GetCalendar(key)

        chr1 ==& chr2

    //
    // TryGetCalendar()
    //

    [<Fact>]
    let ``TryGetCalendar(unknown key)`` () =
        let succeed, _ = CalendarCatalog.TryGetCalendar("Unknown Key")

        succeed |> nok

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``TryGetCalendar(system key)`` (id: CalendarId) =
        let key = toCalendarKey(id)
        let succeed, chr = CalendarCatalog.TryGetCalendar(key)

        succeed |> ok
        chr     |> isnotnull

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``TryGetCalendar(system key) repeated returns the same reference`` (id: CalendarId) =
        let key = toCalendarKey(id)
        let succeed1, chr1 = CalendarCatalog.TryGetCalendar(key)
        let succeed2, chr2 = CalendarCatalog.TryGetCalendar(key)

        succeed1 |> ok
        succeed2 |> ok
        chr1 ==& chr2

    //
    // GetSystemCalendar()
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetSystemCalendar()`` (id: CalendarId) =
        CalendarCatalog.GetSystemCalendar(id) |> isnotnull

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetSystemCalendar() repeated returns the same reference`` (id: CalendarId) =
        let chr1 = CalendarCatalog.GetSystemCalendar(id)
        let chr2 = CalendarCatalog.GetSystemCalendar(id)

        chr1 ==& chr2

    [<Fact>]
    let ``GetSystemCalendar() for every element of SystemCalendars`` () =
        for exp in CalendarCatalog.SystemCalendars do
            let actual = CalendarCatalog.GetSystemCalendar(exp.PermanentId)

            actual ==& exp

    //
    // GetCalendarUnchecked()
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendarUnchecked(system id)`` (id: CalendarId) =
        let cuid = int(id)

        CalendarCatalog.GetCalendarUnchecked(cuid) |> isnotnull

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendarUnchecked(system id) repeated returns the same reference`` (id: CalendarId) =
        let cuid = int(id)
        let chr1 = CalendarCatalog.GetCalendarUnchecked(cuid)
        let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)

        chr1 ==& chr2
