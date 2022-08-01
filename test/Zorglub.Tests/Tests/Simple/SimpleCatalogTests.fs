// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.SimpleCatalogTests

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
        Assert.DoesNotContain(key, SimpleCatalog.Keys)
        throws<KeyNotFoundException> (fun () -> SimpleCatalog.GetCalendar(key))

    let onKeySet key epoch (chr: SimpleCalendar) proleptic =
        chr |> isnotnull

        chr.Key         === key
        chr.Epoch       === epoch
        chr.IsProleptic === proleptic

        Assert.Contains(key, SimpleCatalog.Keys)

        SimpleCatalog.GetCalendar(key) ==& chr

module Prelude =
    let calendarIdData = EnumDataSet.CalendarIdData

    [<Fact>]
    let ``Property MinUserId`` () =
        SimpleCatalog.MinUserId === 64

    [<Fact>]
    let ``Property MaxId`` () =
        SimpleCatalog.MaxId === 127

    [<Fact>]
    let ``Property MaxNumberOfUserCalendars`` () =
        SimpleCatalog.MaxNumberOfUserCalendars === 64

    [<Fact>]
    let ``Property IsFull`` () =
        SimpleCatalog.IsFull |> nok

    [<Fact>]
    let ``Property Keys, unknown key`` () =
        Assert.DoesNotContain("Unknown Key", SimpleCatalog.Keys)

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property Keys, system key`` (id: CalendarId) =
        let key = toCalendarKey(id)

        Assert.Contains(key, SimpleCatalog.Keys)

    [<Fact>]
    let ``Property Keys, user key`` () =
        let key = userGregorian.Key

        Assert.Contains(key, SimpleCatalog.Keys)

    [<Fact>]
    let ``Property ReservedKeys is exhaustive`` () =
        let count = Enum.GetValues(typeof<CalendarId>).Length
        let keys = SimpleCatalog.ReservedKeys

        keys.Count === count

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property ReservedKeys`` (id: CalendarId) =
        let key = toCalendarKey(id)

        Assert.Contains(key, SimpleCatalog.ReservedKeys)

    [<Fact>]
    let ``Property SystemCalendars is exhaustive`` () =
        let count = Enum.GetValues(typeof<CalendarId>).Length
        let calendars = SimpleCatalog.SystemCalendars

        calendars.Count === count

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``Property SystemCalendars`` (ident: CalendarId) =
        let calendars = SimpleCatalog.SystemCalendars
        let chr = SimpleCatalog.GetSystemCalendar(ident)

        Assert.Contains(chr, calendars)

module Snapshots =
    let calendarIdData = EnumDataSet.CalendarIdData

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetAllCalendars() contains all system calendars`` (ident: CalendarId) =
        let calendars = SimpleCatalog.GetAllCalendars()
        let chr = SimpleCatalog.GetSystemCalendar(ident)

        Assert.Contains(chr, calendars)

    // REVIEW(test): this one can fail, idem with the next tests.
    [<Fact>]
    let ``GetAllCalendars() contains the user-defined calendars`` () =
        let calendars = SimpleCatalog.GetAllCalendars()

        Assert.Contains(userGregorian, calendars)
        Assert.Contains(userJulian, calendars)

    [<Fact>]
    let ``GetUserCalendars() contains the user-defined calendars`` () =
        let calendars = SimpleCatalog.GetUserCalendars()

        Assert.Contains(userGregorian, calendars)
        Assert.Contains(userJulian, calendars)

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``TakeSnapshot() contains all system calendars`` (ident: CalendarId) =
        let key = toCalendarKey(ident)
        let dict = SimpleCatalog.TakeSnapshot()
        let chr = SimpleCatalog.GetSystemCalendar(ident)

        dict.[key] ==& chr

    [<Fact>]
    let ``TakeSnapshot() contains the user-defined calendars`` () =
        let dict = SimpleCatalog.TakeSnapshot()

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
        throws<KeyNotFoundException> (fun () -> SimpleCatalog.GetCalendar("Unknown Key"))

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendar(system key) is not null and always returns the same reference`` (id: CalendarId) =
        let key = toCalendarKey(id)
        let chr1 = SimpleCatalog.GetCalendar(key)
        let chr2 = SimpleCatalog.GetCalendar(key)

        chr1 |> isnotnull
        chr1.Key === key
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendar(user key) is not null and always returns the same reference`` () =
        let key = userGregorian.Key
        let chr1 = SimpleCatalog.GetCalendar(key)
        let chr2 = SimpleCatalog.GetCalendar(key)

        chr1 |> isnotnull
        chr1.Key === key
        chr2 ==& chr1

    //
    // TryGetCalendar()
    //

    [<Fact>]
    let ``TryGetCalendar(unknown key)`` () =
        let succeed, chr = SimpleCatalog.TryGetCalendar("Unknown Key")

        succeed |> nok
        chr     |> isnull

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``TryGetCalendar(system key) succeeds and always returns the same reference`` (id: CalendarId) =
        let key = toCalendarKey(id)
        let succeed1, chr1 = SimpleCatalog.TryGetCalendar(key)
        let succeed2, chr2 = SimpleCatalog.TryGetCalendar(key)

        succeed1 |> ok
        succeed2 |> ok
        chr1     |> isnotnull
        chr1.Key === key
        chr2 ==& chr1

    [<Fact>]
    let ``TryGetCalendar(user key) succeeds and always returns the same reference`` () =
        let key = userGregorian.Key
        let succeed1, chr1 = SimpleCatalog.TryGetCalendar(key)
        let succeed2, chr2 = SimpleCatalog.TryGetCalendar(key)

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
        let chr1 = SimpleCatalog.GetSystemCalendar(ident)
        let chr2 = SimpleCatalog.GetSystemCalendar(ident)

        chr1 |> isnotnull
        chr1.PermanentId === ident
        chr2 ==& chr1

    [<Theory; MemberData(nameof(invalidCalendarIdData))>]
    let ``GetSystemCalendar() throws for invalid id`` (ident: CalendarId) =
        outOfRangeExn "ident" (fun () -> SimpleCatalog.GetSystemCalendar(ident))

    [<Fact>]
    let ``GetSystemCalendar(user id) throws`` () =
        let ident: CalendarId = enum <| int(userGregorian.Id)

        outOfRangeExn "ident" (fun () -> SimpleCatalog.GetSystemCalendar(ident))

    //
    // GetCalendarUnchecked()
    //

    [<Theory; MemberData(nameof(calendarIdData))>]
    let ``GetCalendarUnchecked(system id) is not null and always returns the same reference`` (id: CalendarId) =
        let cuid = int(id)
        let chr1 = SimpleCatalog.GetCalendarUnchecked(cuid)
        let chr2 = SimpleCatalog.GetCalendarUnchecked(cuid)

        chr1 |> isnotnull
        chr1.PermanentId === id
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendarUnchecked(user id) is not null and always returns the same reference`` () =
        let cuid = int(userGregorian.Id)
        let chr1 = SimpleCatalog.GetCalendarUnchecked(cuid)
        let chr2 = SimpleCatalog.GetCalendarUnchecked(cuid)

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
        let chr1 = SimpleCatalog.GetCalendarUnsafe(cuid)
        let chr2 = SimpleCatalog.GetCalendarUnsafe(cuid)

        chr1 |> isnotnull
        chr1.PermanentId === id
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendarUnsafe(user id) is not null and always returns the same reference`` () =
        let cuid = int(userGregorian.Id)
        let chr1 = SimpleCatalog.GetCalendarUnsafe(cuid)
        let chr2 = SimpleCatalog.GetCalendarUnsafe(cuid)

        chr1 |> isnotnull
        chr1.Id === userGregorian.Id
        chr2 ==& chr1

    //
    //
    //

    [<Fact>]
    let ``All lookup methods return the same reference for system calendars`` () =
        for sys in SimpleCatalog.SystemCalendars do
            let key = sys.Key
            let ident = sys.PermanentId
            let cuid = int(ident)

            let chr = SimpleCatalog.GetSystemCalendar(ident)
            let chr1 = SimpleCatalog.GetCalendar(key)
            let chr2 = SimpleCatalog.GetCalendarUnchecked(cuid)
            let chr3 = SimpleCatalog.GetCalendarUnsafe(cuid)
            let succeed, chr4 = SimpleCatalog.TryGetCalendar(key)

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

        let chr1 = SimpleCatalog.GetCalendar(key)
        let chr2 = SimpleCatalog.GetCalendarUnchecked(cuid)
        let chr3 = SimpleCatalog.GetCalendarUnsafe(cuid)
        let succeed, chr4 = SimpleCatalog.TryGetCalendar(key)

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
        nullExn "key" (fun () -> SimpleCatalog.GetOrAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> SimpleCatalog.GetOrAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``GetOrAdd() when the key is a system key`` () =
        let sys = SimpleCalendar.Gregorian
        let chr = SimpleCatalog.GetOrAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& sys

    [<Fact>]
    let ``GetOrAdd() when the key is already taken`` () =
        let chr = SimpleCatalog.GetOrAdd(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& userGregorian

    //
    // Add() --- failure
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        nullExn "key" (fun () -> SimpleCatalog.Add(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> SimpleCatalog.Add(key, null, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``Add() throws when the key is a system key`` () =
        let sys = SimpleCalendar.Gregorian

        argExn "key" (fun () -> SimpleCatalog.Add(sys.Key, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws when the key is already taken`` () =
        argExn "key" (fun () -> SimpleCatalog.Add(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false))

    //
    // TryAdd() --- failure
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        nullExn "key" (fun () -> SimpleCatalog.TryAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let key = "key"

        nullExn "schema" (fun () -> SimpleCatalog.TryAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``TryAdd() when the key is a system key`` () =
        let sys = SimpleCalendar.Gregorian
        let succeed, chr = SimpleCatalog.TryAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd() when the key is already taken`` () =
        let succeed, chr = SimpleCatalog.TryAdd(userGregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    //
    // Successful ops
    //

    // NB: we keep together all the tests that actually modify the state of
    // the catalog to keep track of all used keys.

    [<Fact>]
    let ``GetOrAdd()`` () =
        let key = "SimpleCatalogTests.GetOrAdd"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let chr = SimpleCatalog.GetOrAdd(key, new GregorianSchema(), epoch, proleptic)

        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``Add()`` () =
        let key = "SimpleCatalogTests.Add"
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let chr = SimpleCatalog.Add(key, new GregorianSchema(), epoch, proleptic)

        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``TryAdd()`` () =
        let key = "SimpleCatalogTests.TryAdd"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let succeed, chr = SimpleCatalog.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``TryAdd() when the key is empty`` () =
        let key = ""
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let succeed, chr = SimpleCatalog.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet key epoch chr proleptic
