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

open Zorglub.Time.FSharpExtensions

// Tests setup.
// We MUST initialize the user-defined calendar very early on, otherwise the
// tests checking that we cannot create a calendar with an already taken key
// might fail.
let private userGregorian = UserCalendars.Gregorian
let private userJulian = UserCalendars.Julian

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
    let ``Property MinUserId`` () =
        CalendarCatalog.MinUserId === 64

    [<Fact>]
    let ``Property MaxId`` () =
        CalendarCatalog.MaxId === 127

    [<Fact>]
    let ``Property MaxNumberOfUserCalendars`` () =
        CalendarCatalog.MaxNumberOfUserCalendars === 64

    [<Fact>]
    let ``Property IsFull`` () =
        CalendarCatalog.IsFull |> nok

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
    let ``Property ReservedKeys is exhaustive`` () =
        let count = Enum.GetValues(typeof<CalendarId>).Length
        let keys = CalendarCatalog.ReservedKeys

        keys.Count === count

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property ReservedKeys`` (id: CalendarId) =
        let key = toCalendarKey(id)

        Assert.Contains(key, CalendarCatalog.ReservedKeys)

    [<Fact>]
    let ``Property SystemCalendars is exhaustive`` () =
        let count = Enum.GetValues(typeof<CalendarId>).Length
        let calendars = CalendarCatalog.SystemCalendars

        calendars.Count === count

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property SystemCalendars`` (ident: CalendarId) =
        let calendars = CalendarCatalog.SystemCalendars
        let chr = CalendarCatalog.GetSystemCalendar(ident)

        Assert.Contains(chr, calendars)

module Snapshots =
    let calendarIdData = EnumDataSet.CalendarIdData

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetAllCalendars() contains all system calendars`` (ident: CalendarId) =
        let calendars = CalendarCatalog.GetAllCalendars()
        let chr = CalendarCatalog.GetSystemCalendar(ident)

        Assert.Contains(chr, calendars)

    [<Fact>]
    let ``GetAllCalendars() contains the user-defined calendars`` () =
        let calendars = CalendarCatalog.GetAllCalendars()

        Assert.Contains(userGregorian, calendars)
        Assert.Contains(userJulian, calendars)

    [<Fact>]
    let ``GetUserCalendars() contains the user-defined calendars`` () =
        let calendars = CalendarCatalog.GetUserCalendars()

        Assert.Contains(userGregorian, calendars)
        Assert.Contains(userJulian, calendars)

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``TakeSnapshot() contains all system calendars`` (ident: CalendarId) =
        let key = toCalendarKey(ident)
        let dict = CalendarCatalog.TakeSnapshot()
        let chr = CalendarCatalog.GetSystemCalendar(ident)

        dict.[key] ==& chr

    [<Fact>]
    let ``TakeSnapshot() contains the user-defined calendars`` () =
        let dict = CalendarCatalog.TakeSnapshot()

        dict.[userGregorian.Key] ==& userGregorian
        dict.[userJulian.Key]    ==& userJulian

module Lookup =
    let calendarIdData = EnumDataSet.CalendarIdData
    let invalidCalendarIdData = EnumDataSet.InvalidCalendarIdData

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
        chr1.Key === key
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendar(user key) is not null and always returns the same reference`` () =
        let key = userGregorian.Key
        let chr1 = CalendarCatalog.GetCalendar(key)
        let chr2 = CalendarCatalog.GetCalendar(key)

        chr1 |> isnotnull
        chr1.Key === key
        chr2 ==& chr1

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
        chr1.Key === key
        chr2 ==& chr1

    [<Fact>]
    let ``TryGetCalendar(user key) succeeds and always returns the same reference`` () =
        let key = userGregorian.Key
        let succeed1, chr1 = CalendarCatalog.TryGetCalendar(key)
        let succeed2, chr2 = CalendarCatalog.TryGetCalendar(key)

        succeed1 |> ok
        succeed2 |> ok
        chr1     |> isnotnull
        chr1.Key === key
        chr2 ==& chr1

    //
    // GetSystemCalendar()
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetSystemCalendar() is not null and always returns the same reference`` (ident: CalendarId) =
        let chr1 = CalendarCatalog.GetSystemCalendar(ident)
        let chr2 = CalendarCatalog.GetSystemCalendar(ident)

        chr1 |> isnotnull
        chr1.PermanentId === ident
        chr2 ==& chr1

    [<Theory; MemberData(nameof(invalidCalendarIdData))>]
    let ``GetSystemCalendar() throws for invalid id`` (ident: CalendarId) =
        outOfRangeExn "ident" (fun () -> CalendarCatalog.GetSystemCalendar(ident))

    [<Fact>]
    let ``GetSystemCalendar(user id) throws`` () =
        let ident: CalendarId = enum <| int(userGregorian.Id)

        outOfRangeExn "ident" (fun () -> CalendarCatalog.GetSystemCalendar(ident))

    //
    // GetCalendarUnchecked()
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendarUnchecked(system id) is not null and always returns the same reference`` (id: CalendarId) =
        let cuid = int(id)
        let chr1 = CalendarCatalog.GetCalendarUnchecked(cuid)
        let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)

        chr1 |> isnotnull
        chr1.PermanentId === id
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendarUnchecked(user id) is not null and always returns the same reference`` () =
        let cuid = int(userGregorian.Id)
        let chr1 = CalendarCatalog.GetCalendarUnchecked(cuid)
        let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)

        chr1 |> isnotnull
        chr1.Id === userGregorian.Id
        chr2 ==& chr1

    //
    // GetCalendarUnsafe()
    //
    // https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/byrefs#interop-with-c

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendarUnsafe(system id) is not null and always returns the same reference`` (id: CalendarId) =
        let cuid = int(id)
        let chr1 = CalendarCatalog.GetCalendarUnsafe(cuid)
        let chr2 = CalendarCatalog.GetCalendarUnsafe(cuid)

        chr1 |> isnotnull
        chr1.PermanentId === id
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendarUnsafe(user id) is not null and always returns the same reference`` () =
        let cuid = int(userGregorian.Id)
        let chr1 = CalendarCatalog.GetCalendarUnsafe(cuid)
        let chr2 = CalendarCatalog.GetCalendarUnsafe(cuid)

        chr1 |> isnotnull
        chr1.Id === userGregorian.Id
        chr2 ==& chr1

    //
    //
    //

    [<Fact>]
    let ``All lookup methods return the same reference for system calendars`` () =
        for sys in CalendarCatalog.SystemCalendars do
            let key = sys.Key
            let ident = sys.PermanentId
            let cuid = int(ident)

            let chr = CalendarCatalog.GetSystemCalendar(ident)
            let chr1 = CalendarCatalog.GetCalendar(key)
            let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)
            let chr3 = CalendarCatalog.GetCalendarUnsafe(cuid)
            let succeed, chr4 = CalendarCatalog.TryGetCalendar(key)

            succeed |> ok
            chr  ==& sys
            chr1 ==& sys
            chr2 ==& sys
            chr3 ==& sys
            chr4 ==& sys

    [<Fact>]
    let ``All lookup methods return the same reference for user-defined calendars`` () =
        let key = userGregorian.Key
        let cuid = int(userGregorian.Id)

        let chr1 = CalendarCatalog.GetCalendar(key)
        let chr2 = CalendarCatalog.GetCalendarUnchecked(cuid)
        let chr3 = CalendarCatalog.GetCalendarUnsafe(cuid)
        let succeed, chr4 = CalendarCatalog.TryGetCalendar(key)

        succeed |> ok
        chr1 ==& userGregorian
        chr2 ==& userGregorian
        chr3 ==& userGregorian
        chr4 ==& userGregorian

module AddOps =
    open TestCommon

    // NB: quand on essaie d'ajouter un calendrier avec une clé qui est déjà
    // prise, on utilise volontairement une epoch et un schéma différents.

    //
    // GetOrAdd() --- failure
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
    let ``GetOrAdd() when the key is a system key`` () =
        let sys = GregorianCalendar.Instance
        let chr = CalendarCatalog.GetOrAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& sys

    [<Fact>]
    let ``GetOrAdd() when the key is already taken`` () =
        let chr = CalendarCatalog.GetOrAdd(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& userGregorian

    //
    // Add() --- failure
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
    let ``Add() throws when the key is a system key`` () =
        let sys = GregorianCalendar.Instance

        argExn "key" (fun () -> CalendarCatalog.Add(sys.Key, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws when the key is already taken`` () =
        argExn "key" (fun () -> CalendarCatalog.Add(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false))

    //
    // TryAdd() --- failure
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
    let ``TryAdd() when the key is a system key`` () =
        let sys = GregorianCalendar.Instance
        let succeed, chr = CalendarCatalog.TryAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd() when the key is already taken`` () =
        let succeed, chr = CalendarCatalog.TryAdd(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    //
    // Successful ops
    //

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
