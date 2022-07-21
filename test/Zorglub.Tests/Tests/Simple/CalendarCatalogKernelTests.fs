// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarCatalogKernelTests

open System
open System.Collections.Concurrent

open Zorglub.Testing
open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Simple

open Xunit

/// 64
let private defaultMinUserId = CalendarCatalogKernel.MinMinUserId

/// Creates a new empty "calendarsById" with max size = 255.
let private initCalendarsById () =
    let maxsize = 1 + CalendarCatalogKernel.MaxMaxId
    Array.zeroCreate<Calendar>(maxsize)

/// Creates a new empty "calendarsByKey".
let private initCalendarsByKey () = new ConcurrentDictionary<string, Lazy<Calendar>>()

module TestCommon =
    let onKeyNotSet (kern: CalendarCatalogKernel) key count =
        Assert.DoesNotContain(key, kern.Keys)

        keyNotFoundExn (fun () -> kern.GetCalendarByKey(key))
        kern.CountUserCalendars() === count

    let onKeySet (kern: CalendarCatalogKernel) key epoch (chr: Calendar) proleptic count =
        chr |> isnotnull

        chr.Key         === key
        chr.Epoch       === epoch
        chr.IsProleptic === proleptic

        Assert.Contains(key, kern.Keys)

        kern.GetCalendarByKey(key) ==& chr
        kern.CountUserCalendars() === count

module Prelude =
    [<Fact>]
    let ``Constructor throws when "calendarsByKey" is null`` () =
        let calendarsById = initCalendarsById()

        nullExn "calendarsByKey" (fun () -> new CalendarCatalogKernel(null, calendarsById, defaultMinUserId))

    [<Fact>]
    let ``Constructor throws when "calendarsById" is null`` () =
        let calendarsByKey = initCalendarsByKey()

        nullExn "calendarsById" (fun () -> new CalendarCatalogKernel(calendarsByKey, null, defaultMinUserId))

    [<Fact>]
    let ``Constructor throws when calendarsById.Length < MinMinUserId + 1`` () =
        let count = CalendarCatalogKernel.MinMinUserId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        argExn "calendarsById" (fun () -> new CalendarCatalogKernel(calendarsByKey, calendarsById, defaultMinUserId))

    [<Fact>]
    let ``Constructor does not throw when calendarsById.Length = MinMinUserId + 1`` () =
        let count = CalendarCatalogKernel.MinMinUserId + 1
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        new CalendarCatalogKernel(calendarsByKey, calendarsById, defaultMinUserId) |> ignore

    [<Fact>]
    let ``Constructor throws when calendarsById.Length > MaxMaxId + 1`` () =
        let count = CalendarCatalogKernel.MaxMaxId + 2
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        argExn "calendarsById" (fun () -> new CalendarCatalogKernel(calendarsByKey, calendarsById, defaultMinUserId))

    [<Fact>]
    let ``Constructor does not throw when calendarsById.Length = MaxMaxId + 1`` () =
        let count = CalendarCatalogKernel.MaxMaxId + 1
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        new CalendarCatalogKernel(calendarsByKey, calendarsById, defaultMinUserId) |> ignore

    [<Fact>]
    let ``Constructor throws when minUserId < MinMinUserId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let minUserId = CalendarCatalogKernel.MinMinUserId - 1

        outOfRangeExn "minUserId" (fun () -> new CalendarCatalogKernel(calendarsByKey, calendarsById, minUserId))

    [<Fact>]
    let ``Constructor does not throw when minUserId = MinMinUserId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let minUserId = CalendarCatalogKernel.MinMinUserId

        new CalendarCatalogKernel(calendarsByKey, calendarsById, minUserId) |> ignore

    [<Fact>]
    let ``Constructor throws when minUserId > MaxId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let maxId = calendarsById.Length - 1

        outOfRangeExn "minUserId" (fun () -> new CalendarCatalogKernel(calendarsByKey, calendarsById, maxId + 1))

    [<Fact>]
    let ``Constructor does not throw when minUserId = MaxId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let maxId = calendarsById.Length - 1

        new CalendarCatalogKernel(calendarsByKey, calendarsById, maxId) |> ignore

    [<Fact>]
    let ``Constructor (default size)`` () =
        let maxId = CalendarCatalog.MaxId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(maxId + 1)
        let minUserId = CalendarCatalog.MinUserId

        let kern = new CalendarCatalogKernel(calendarsByKey, calendarsById, minUserId)

        kern.MinUserId === minUserId
        kern.MaxId     === maxId

        kern.MaxNumberOfUserCalendars === 64
        kern.CountUserCalendars() === 0

    [<Fact>]
    let ``Constructor (largest size)`` () =
        let maxId = CalendarCatalogKernel.MaxMaxId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(maxId + 1)
        let minUserId = CalendarCatalogKernel.MinMinUserId

        let kern = new CalendarCatalogKernel(calendarsByKey, calendarsById, minUserId)

        kern.MinUserId === minUserId
        kern.MaxId     === maxId

        kern.MaxNumberOfUserCalendars === 191
        kern.CountUserCalendars() === 0

    [<Fact>]
    let ``Constructor (smallest size)`` () =
        let minUserId = CalendarCatalogKernel.MinMinUserId
        let maxId = minUserId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(maxId + 1)

        let kern = new CalendarCatalogKernel(calendarsByKey, calendarsById, minUserId)

        kern.MinUserId === minUserId
        kern.MaxId     === maxId

        kern.MaxNumberOfUserCalendars === 1
        kern.CountUserCalendars() === 0

module AddOps =
    open TestCommon

    let private gregorian = GregorianCalendar.Instance

    let private newEmptyKernel () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        new CalendarCatalogKernel(calendarsByKey, calendarsById, defaultMinUserId)

    let private newKernel (chr: Calendar) =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()

        calendarsByKey.[chr.Key] <- new Lazy<Calendar>(chr)
        calendarsById.[0] <- chr

        new CalendarCatalogKernel(calendarsByKey, calendarsById, defaultMinUserId)

    //
    // GetOrAdd()
    //

    [<Fact>]
    let ``GetOrAdd() throws for null key`` () =
        let kern = newEmptyKernel()

        nullExn "key" (fun () -> kern.GetOrAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let kern = newEmptyKernel()
        let key = "key"

        nullExn "schema" (fun () -> kern.GetOrAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet kern key 0

    [<Fact>]
    let ``GetOrAdd() when the key is already taken`` () =
        let kern = newKernel gregorian
        let chr = kern.GetOrAdd(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& gregorian

    [<Fact>]
    let ``GetOrAdd()`` () =
        let kern = newKernel gregorian
        let key = "CalendarCatalogKernelTests.GetOrAdd"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let chr = kern.GetOrAdd(key, new GregorianSchema(), epoch, proleptic)

        onKeySet kern key epoch chr proleptic 1

    //
    // Add()
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        let kern = newEmptyKernel()

        nullExn "key" (fun () -> kern.Add(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let kern = newEmptyKernel()
        let key = "key"

        nullExn "schema" (fun () -> kern.Add(key, null, DayZero.OldStyle, false))
        onKeyNotSet kern key 0

    [<Fact>]
    let ``Add() throws when the key is already taken`` () =
        let kern = newKernel gregorian

        argExn "key" (fun () -> kern.Add(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add()`` () =
        let kern = newKernel gregorian
        let key = "CalendarCatalogKernelTests.Add"
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let chr = kern.Add(key, new GregorianSchema(), epoch, proleptic)

        onKeySet kern key epoch chr proleptic 1

    //
    // TryAdd()
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        let kern = newEmptyKernel()

        nullExn "key" (fun () -> kern.TryAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let kern = newEmptyKernel()
        let key = "key"

        nullExn "schema" (fun () -> kern.TryAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet kern key 0

    [<Fact>]
    let ``TryAdd() when the key is already taken`` () =
        let kern = newKernel gregorian
        let succeed, chr = kern.TryAdd(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd()`` () =
        let kern = newKernel gregorian
        let key = "CalendarCatalogKernelTests.TryAdd"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let succeed, chr = kern.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet kern key epoch chr proleptic 1

    [<Fact>]
    let ``TryAdd() when the key is empty`` () =
        let kern = newKernel gregorian
        let key = ""
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let succeed, chr = kern.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet kern key epoch chr proleptic 1
