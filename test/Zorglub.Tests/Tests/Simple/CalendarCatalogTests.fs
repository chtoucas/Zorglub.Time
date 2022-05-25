// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarCatalogTests

open System
open System.Collections.Generic

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Simple

open Xunit

// NB: we also test BoxExtensions.

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

module ReadOps =
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
        let succeed, chr = CalendarCatalog.TryGetCalendar("Unknown Key")

        succeed |> nok
        chr     |> isnull

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

module WriteOps =
    let userGregorianKey = "User Gregorian"
    let userGregorian = CalendarCatalog.Add(userGregorianKey, new GregorianSchema(), DayZero.NewStyle, false)

    let private onKeyNotSet key =
        Assert.DoesNotContain(key, CalendarCatalog.Keys)
        throws<KeyNotFoundException> (fun () -> CalendarCatalog.GetCalendar(key))

    // TODO(code): test "proleptic"
    let private onKeySet key epoch (chr: Calendar) =
        chr |> isnotnull

        chr.Key   === key
        chr.Epoch === epoch

        Assert.Contains(key, CalendarCatalog.Keys)

        CalendarCatalog.GetCalendar(key) ==& chr

    //
    // GetOrAdd()
    //

    [<Fact>]
    let ``GetOrAdd() throws for null key`` () =
        nullExn "key" (fun () -> CalendarCatalog.GetOrAdd(null, new GregorianSchema(), DayNumber.Zero, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> CalendarCatalog.GetOrAdd(key, null, DayNumber.Zero, false))
        onKeyNotSet key

    [<Fact>]
    let ``GetOrAdd() when key is a system key`` () =
        let sys = GregorianCalendar.Instance
        // NB: on utilise volontairement une epoch et un schéma différents.
        let chr = CalendarCatalog.GetOrAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& sys

    [<Fact>]
    let ``GetOrAdd() when key already exists`` () =
        // NB: on utilise volontairement une epoch et un schéma différents.
        let chr = CalendarCatalog.GetOrAdd(userGregorianKey, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& userGregorian

    [<Fact>]
    let ``GetOrAdd() when key is new`` () =
        let key = "GetOrAdd"
        let epoch = DayNumber.Zero + 1234
        let chr = CalendarCatalog.GetOrAdd(key, new GregorianSchema(), epoch, false)

        onKeySet key epoch chr

    //
    // Add()
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        nullExn "key" (fun () -> CalendarCatalog.Add(null, new GregorianSchema(), DayNumber.Zero, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> CalendarCatalog.Add(key, null, DayNumber.Zero, false))
        onKeyNotSet key

    [<Fact>]
    let ``Add() when key is a system key`` () =
        let sys = GregorianCalendar.Instance
        // NB: on utilise volontairement une epoch et un schéma différents.
        argExn "key" (fun () -> CalendarCatalog.Add(sys.Key, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() when key already exists`` () =
        // NB: on utilise volontairement une epoch et un schéma différents.
        argExn "key" (fun () -> CalendarCatalog.Add(userGregorianKey, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() when key is new`` () =
        let key = "Add"
        let epoch = DayNumber.Zero + 1234
        let chr = CalendarCatalog.Add(key, new GregorianSchema(), epoch, false)

        onKeySet key epoch chr

    [<Fact>]
    let ``BoxExtensions.CreateCalendar() when key is new`` () =
        let key = "BoxExtensions.CreateCalendar"
        let epoch = DayNumber.Zero + 1234
        let chr = GregorianSchema.GetInstance().CreateCalendar(key, epoch)

        onKeySet key epoch chr

    //
    // TryAdd()
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        nullExn "key" (fun () -> CalendarCatalog.TryAdd(null, new GregorianSchema(), DayNumber.Zero, false))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> CalendarCatalog.TryAdd(key, null, DayNumber.Zero, false))
        onKeyNotSet key

    [<Fact>]
    let ``TryAdd() when key is a system key`` () =
        let sys = GregorianCalendar.Instance
        // NB: on utilise volontairement une epoch et un schéma différents.
        let succeed, chr = CalendarCatalog.TryAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd() when key already exists`` () =
        // NB: on utilise volontairement une epoch et un schéma différents.
        let succeed, chr = CalendarCatalog.TryAdd(userGregorianKey, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd() when key is new`` () =
        let key = "TryAdd"
        let epoch = DayNumber.Zero + 1234
        let succeed, chr = CalendarCatalog.TryAdd(key, new GregorianSchema(), epoch, false)

        succeed |> ok
        onKeySet key epoch chr

    [<Fact>]
    let ``TryAdd() when key is new and empty`` () =
        let key = ""
        let epoch = DayNumber.Zero + 1234
        let succeed, chr = CalendarCatalog.TryAdd(key, new GregorianSchema(), epoch, false)

        succeed |> ok
        onKeySet key epoch chr

    [<Fact>]
    let ``BoxExtensions.TryCreateCalendar() when key is new`` () =
        let key = "BoxExtensions.TryCreateCalendar"
        let epoch = DayNumber.Zero + 1234
        let succeed, chr = GregorianSchema.GetInstance().TryCreateCalendar(key, epoch)

        succeed |> ok
        onKeySet key epoch chr
