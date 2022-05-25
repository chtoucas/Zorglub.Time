﻿// SPDX-License-Identifier: BSD-3-Clause
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

let private toCalendarKey (id: CalendarId)  = CalendarIdExtensions.ToCalendarKey(id)

let private userGregorian = UserCalendar.Gregorian

module TestCommon =
    let onKeyNotSet key =
        Assert.DoesNotContain(key, CalendarCatalog.Keys)
        throws<KeyNotFoundException> (fun () -> CalendarCatalog.GetCalendar(key))

    let onKeySet key epoch (chr: Calendar) proleptic =
        chr |> isnotnull

        chr.Key         === key
        chr.Epoch       === epoch
        chr.IsProleptic === proleptic

        Assert.Contains(key, CalendarCatalog.Keys)

        CalendarCatalog.GetCalendar(key) ==& chr

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
    let ``Property Keys, user key`` () =
        let key = userGregorian.Key

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
    let ``GetCalendar(system key) is not null and always returns the same reference`` (id: CalendarId) =
        let key = toCalendarKey(id)
        let chr1 = CalendarCatalog.GetCalendar(key)
        let chr2 = CalendarCatalog.GetCalendar(key)

        chr1 |> isnotnull
        chr1 ==& chr2

    [<Fact>]
    let ``GetCalendar(user key) is not null and always returns the same reference`` () =
        let key = UserCalendar.Gregorian.Key
        let chr1 = CalendarCatalog.GetCalendar(key)
        let chr2 = CalendarCatalog.GetCalendar(key)

        chr1 |> isnotnull
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
    let ``TryGetCalendar(system key) succeeds and always returns the same reference`` (id: CalendarId) =
        let key = toCalendarKey(id)
        let succeed1, chr1 = CalendarCatalog.TryGetCalendar(key)
        let succeed2, chr2 = CalendarCatalog.TryGetCalendar(key)

        succeed1 |> ok
        succeed2 |> ok
        chr1     |> isnotnull
        chr1 ==& chr2

    [<Fact>]
    let ``TryGetCalendar(user key) succeeds and always returns the same reference`` () =
        let key = UserCalendar.Gregorian.Key
        let succeed1, chr1 = CalendarCatalog.TryGetCalendar(key)
        let succeed2, chr2 = CalendarCatalog.TryGetCalendar(key)

        succeed1 |> ok
        succeed2 |> ok
        chr1     |> isnotnull
        chr1 ==& chr2

    //
    // GetSystemCalendar()
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetSystemCalendar() is not null and always returns the same reference`` (id: CalendarId) =
        let chr1 = CalendarCatalog.GetSystemCalendar(id)
        let chr2 = CalendarCatalog.GetSystemCalendar(id)

        chr1 |> isnotnull
        chr1 ==& chr2

    [<Fact>]
    let ``GetSystemCalendar(user id) throws`` () =
        let id: CalendarId = enum <| int(userGregorian.Id)

        outOfRangeExn "id" (fun () -> CalendarCatalog.GetSystemCalendar(id))

    //
    // GetCalendarUnchecked()
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendarUnchecked(system id) is not null and always returns the same reference`` (id: CalendarId) =
        let cuid = int(id)
        let chr1 = CalendarCatalog.GetCalendarUnchecked(cuid)
        let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)

        chr1 |> isnotnull
        chr1 ==& chr2

    [<Fact>]
    let ``GetCalendarUnchecked(user id) is not null and always returns the same reference`` () =
        let cuid = int(userGregorian.Id)
        let chr1 = CalendarCatalog.GetCalendarUnchecked(cuid)
        let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)

        chr1 |> isnotnull
        chr1 ==& chr2

    //
    // System calendars
    //

    [<Fact>]
    let ``All lookup methods return the same reference for system calendars`` () =
        for sys in CalendarCatalog.SystemCalendars do
            let key = sys.Key
            let id = sys.PermanentId
            let cuid = int(id)

            let chr = CalendarCatalog.GetSystemCalendar(id)
            let chr1 = CalendarCatalog.GetCalendar(key)
            let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)
            let succeed, chr3 = CalendarCatalog.TryGetCalendar(key)

            succeed |> ok
            chr  ==& sys
            chr1 ==& sys
            chr2 ==& sys
            chr3 ==& sys

    [<Fact>]
    let ``All lookup methods return the same reference for user-defined calendars`` () =
        let key = userGregorian.Key
        let cuid = int(userGregorian.Id)

        let chr1 = CalendarCatalog.GetCalendar(key)
        let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)
        let succeed, chr3 = CalendarCatalog.TryGetCalendar(key)

        succeed |> ok
        chr1 ==& userGregorian
        chr2 ==& userGregorian
        chr3 ==& userGregorian

module NoWrite =
    open TestCommon

    // NB: quand on essaie d'ajouter un calendrier avec une clé qui est déjà
    // prise, on utilise volontairement une epoch et un schéma différents.

    //
    // GetOrAdd()
    //

    [<Fact>]
    let ``GetOrAdd() throws for null key`` () =
        nullExn "key" (fun () -> CalendarCatalog.GetOrAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> CalendarCatalog.GetOrAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``GetOrAdd() when key is a system key`` () =
        let sys = GregorianCalendar.Instance
        let chr = CalendarCatalog.GetOrAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& sys

    [<Fact>]
    let ``GetOrAdd() when key already exists`` () =
        let chr = CalendarCatalog.GetOrAdd(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& userGregorian

    //
    // Add()
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        nullExn "key" (fun () -> CalendarCatalog.Add(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> CalendarCatalog.Add(key, null, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``Add() when key is a system key`` () =
        let sys = GregorianCalendar.Instance

        argExn "key" (fun () -> CalendarCatalog.Add(sys.Key, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() when key already exists`` () =
        argExn "key" (fun () -> CalendarCatalog.Add(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false))

    //
    // TryAdd()
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        nullExn "key" (fun () -> CalendarCatalog.TryAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> CalendarCatalog.TryAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``TryAdd() when key is a system key`` () =
        let sys = GregorianCalendar.Instance
        let succeed, chr = CalendarCatalog.TryAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd() when key already exists`` () =
        let succeed, chr = CalendarCatalog.TryAdd(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

module Write =
    open TestCommon

    // NB: we keep together all the tests that actually modify the state of
    // the catalog to keep track of all used keys.

    [<Fact>]
    let ``GetOrAdd()`` () =
        let key = "CalendarCatalogTests.GetOrAdd"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let chr = CalendarCatalog.GetOrAdd(key, new GregorianSchema(), epoch, proleptic)

        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``Add()`` () =
        let key = "CalendarCatalogTests.Add"
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let chr = CalendarCatalog.Add(key, new GregorianSchema(), epoch, proleptic)

        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``TryAdd()`` () =
        let key = "CalendarCatalogTests.TryAdd"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let succeed, chr = CalendarCatalog.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``TryAdd() when the key is empty`` () =
        let key = ""
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let succeed, chr = CalendarCatalog.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet key epoch chr proleptic
