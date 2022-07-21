// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarRegistryTests

open System
open System.Collections.Concurrent

open Zorglub.Testing
open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Simple

open Xunit

/// 64
let private defaultMinUserId = CalendarRegistry.MinMinUserId

/// Creates a new empty "calendarsById" with max size = 255.
let private initCalendarsById () =
    let maxsize = 1 + CalendarRegistry.MaxMaxId
    Array.zeroCreate<Calendar>(maxsize)

/// Creates a new empty "calendarsByKey".
let private initCalendarsByKey () = new ConcurrentDictionary<string, Lazy<Calendar>>()

module TestCommon =
    let onKeyNotSet (reg: CalendarRegistry) key count =
        Assert.DoesNotContain(key, reg.Keys)

        keyNotFoundExn (fun () -> reg.GetCalendar(key))
        reg.CountUserCalendars() === count

    let onKeySet (reg: CalendarRegistry) key epoch proleptic (chr: Calendar)  count =
        chr |> isnotnull

        chr.Key         === key
        chr.Epoch       === epoch
        chr.IsProleptic === proleptic

        Assert.Contains(key, reg.Keys)

        reg.GetCalendar(key) ==& chr
        reg.CountUserCalendars() === count

module Prelude =
    [<Fact>]
    let ``Constructor throws when "calendarsByKey" is null`` () =
        let calendarsById = initCalendarsById()

        nullExn "calendarsByKey" (fun () -> new CalendarRegistry(null, calendarsById, defaultMinUserId))

    [<Fact>]
    let ``Constructor throws when "calendarsById" is null`` () =
        let calendarsByKey = initCalendarsByKey()

        nullExn "calendarsById" (fun () -> new CalendarRegistry(calendarsByKey, null, defaultMinUserId))

    [<Fact>]
    let ``Constructor throws when calendarsById.Length < MinMinUserId + 1`` () =
        let count = CalendarRegistry.MinMinUserId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        argExn "calendarsById" (fun () -> new CalendarRegistry(calendarsByKey, calendarsById, defaultMinUserId))

    [<Fact>]
    let ``Constructor does not throw when calendarsById.Length = MinMinUserId + 1`` () =
        let count = CalendarRegistry.MinMinUserId + 1
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        new CalendarRegistry(calendarsByKey, calendarsById, defaultMinUserId) |> ignore

    [<Fact>]
    let ``Constructor throws when calendarsById.Length > MaxMaxId + 1`` () =
        let count = CalendarRegistry.MaxMaxId + 2
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        argExn "calendarsById" (fun () -> new CalendarRegistry(calendarsByKey, calendarsById, defaultMinUserId))

    [<Fact>]
    let ``Constructor does not throw when calendarsById.Length = MaxMaxId + 1`` () =
        let count = CalendarRegistry.MaxMaxId + 1
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        new CalendarRegistry(calendarsByKey, calendarsById, defaultMinUserId) |> ignore

    [<Fact>]
    let ``Constructor throws when minUserId < MinMinUserId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let minUserId = CalendarRegistry.MinMinUserId - 1

        outOfRangeExn "minUserId" (fun () -> new CalendarRegistry(calendarsByKey, calendarsById, minUserId))

    [<Fact>]
    let ``Constructor does not throw when minUserId = MinMinUserId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let minUserId = CalendarRegistry.MinMinUserId

        new CalendarRegistry(calendarsByKey, calendarsById, minUserId) |> ignore

    [<Fact>]
    let ``Constructor throws when minUserId > MaxId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let maxId = calendarsById.Length - 1

        outOfRangeExn "minUserId" (fun () -> new CalendarRegistry(calendarsByKey, calendarsById, maxId + 1))

    [<Fact>]
    let ``Constructor does not throw when minUserId = MaxId`` () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()
        let maxId = calendarsById.Length - 1

        new CalendarRegistry(calendarsByKey, calendarsById, maxId) |> ignore

    [<Fact>]
    let ``Constructor (default size)`` () =
        let maxId = CalendarCatalog.MaxId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(maxId + 1)
        let minUserId = CalendarCatalog.MinUserId

        let reg = new CalendarRegistry(calendarsByKey, calendarsById, minUserId)

        reg.MinUserId === minUserId
        reg.MaxId     === maxId

        reg.MaxNumberOfUserCalendars === 64
        reg.CountUserCalendars() === 0

    [<Fact>]
    let ``Constructor (largest size)`` () =
        let maxId = CalendarRegistry.MaxMaxId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(maxId + 1)
        let minUserId = CalendarRegistry.MinMinUserId

        let reg = new CalendarRegistry(calendarsByKey, calendarsById, minUserId)

        reg.MinUserId === minUserId
        reg.MaxId     === maxId

        reg.MaxNumberOfUserCalendars === 191
        reg.CountUserCalendars() === 0

    [<Fact>]
    let ``Constructor (smallest size)`` () =
        let minUserId = CalendarRegistry.MinMinUserId
        let maxId = minUserId
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(maxId + 1)

        let reg = new CalendarRegistry(calendarsByKey, calendarsById, minUserId)

        reg.MinUserId === minUserId
        reg.MaxId     === maxId

        reg.MaxNumberOfUserCalendars === 1
        reg.CountUserCalendars() === 0

module AddOps =
    open TestCommon

    let private gregorian = GregorianCalendar.Instance

    let private newEmptyRegistry () =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()

        new CalendarRegistry(calendarsByKey, calendarsById, defaultMinUserId)

    let private newRegistry (chr: Calendar) =
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = initCalendarsById()

        calendarsByKey.[chr.Key] <- new Lazy<Calendar>(chr)
        calendarsById.[0] <- chr

        new CalendarRegistry(calendarsByKey, calendarsById, defaultMinUserId)

    //
    // GetOrAdd()
    //

    [<Fact>]
    let ``GetOrAdd() throws for null key`` () =
        let reg = newEmptyRegistry()

        nullExn "key" (fun () -> reg.GetOrAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let reg = newEmptyRegistry()
        let key = "key"

        nullExn "schema" (fun () -> reg.GetOrAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet reg key 0

    [<Fact>]
    let ``GetOrAdd() when the key is already taken`` () =
        let reg = newRegistry gregorian
        let chr = reg.GetOrAdd(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& gregorian

    [<Fact>]
    let ``GetOrAdd()`` () =
        let reg = newRegistry gregorian
        let key = "key"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let chr = reg.GetOrAdd(key, new GregorianSchema(), epoch, proleptic)

        onKeySet reg key epoch proleptic chr 1

    //
    // Add()
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        let reg = newEmptyRegistry()

        nullExn "key" (fun () -> reg.Add(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let reg = newEmptyRegistry()
        let key = "key"

        nullExn "schema" (fun () -> reg.Add(key, null, DayZero.OldStyle, false))
        onKeyNotSet reg key 0

    [<Fact>]
    let ``Add() throws when the key is already taken`` () =
        let reg = newRegistry gregorian

        argExn "key" (fun () -> reg.Add(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add()`` () =
        let reg = newRegistry gregorian
        let key = "key"
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let chr = reg.Add(key, new GregorianSchema(), epoch, proleptic)

        onKeySet reg key epoch proleptic chr 1

    //
    // TryAdd()
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        let reg = newEmptyRegistry()

        nullExn "key" (fun () -> reg.TryAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let reg = newEmptyRegistry()
        let key = "key"

        nullExn "schema" (fun () -> reg.TryAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet reg key 0

    [<Fact>]
    let ``TryAdd() when the key is already taken`` () =
        let reg = newRegistry gregorian
        let succeed, chr = reg.TryAdd(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd()`` () =
        let reg = newRegistry gregorian
        let key = "key"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let succeed, chr = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet reg key epoch proleptic chr 1

    [<Fact>]
    let ``TryAdd() when the key is empty`` () =
        let reg = newRegistry gregorian
        let key = ""
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let succeed, chr = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet reg key epoch proleptic chr 1

module AddLimits =
    open TestCommon

    let private newMiniRegistry size =
        let minUserId = CalendarRegistry.MinMinUserId
        let maxId = minUserId + (size - 1)
        let calendarsByKey = initCalendarsByKey()
        let calendarsById = Array.zeroCreate<Calendar>(maxId + 1)

        new CalendarRegistry(calendarsByKey, calendarsById, minUserId)

    // TODO(test): use different params.

    [<Fact>]
    let ``GetOrAdd()`` () =
        let reg = newMiniRegistry 2
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        reg.MaxNumberOfUserCalendars === 2
        reg.CountUserCalendars() === 0

        let chr = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "key" epoch proleptic chr 1

        // Using the same key, we obtain the same calendar.
        let chr1 = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)
        chr1 ==& chr
        reg.CountUserCalendars() === 1

        // Using a different key, we create a new calendar.
        let otherChr = reg.GetOrAdd("otherKey", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "otherKey" epoch proleptic otherChr 2

        //
        // Now, the registry is full.
        //

        // Using an old key.
        let chr2 = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)
        chr2 ==& chr
        reg.CountUserCalendars() === 2

        // Using a new key.
        overflows (fun () -> reg.GetOrAdd("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"  2

        reg.ForceCanAdd <- true

        overflows (fun () -> reg.GetOrAdd("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"  2

    [<Fact>]
    let ``Add()`` () =
        let reg = newMiniRegistry 2
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        reg.MaxNumberOfUserCalendars === 2
        reg.CountUserCalendars() === 0

        let chr = reg.Add("key", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "key" epoch proleptic chr 1

        // Using the same key.
        argExn "key" (fun () -> reg.Add("key", new GregorianSchema(), epoch, proleptic))

        // Using a different key, we create a new calendar.
        let otherChr = reg.Add("otherKey", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "otherKey" epoch proleptic otherChr 2

        //
        // Now, the registry is full.
        //

        // Using an old key.
        overflows (fun () -> reg.Add("key", new GregorianSchema(), epoch, proleptic))
        reg.CountUserCalendars() === 2

        // Using a new key.
        overflows (fun () -> reg.Add("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey" 2

        reg.ForceCanAdd <- true

        overflows (fun () -> reg.Add("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey" 2

    [<Fact>]
    let ``TryAdd()`` () =
        let reg = newMiniRegistry 2
        let key = "key"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        reg.MaxNumberOfUserCalendars === 2
        reg.CountUserCalendars() === 0

        let (succeed, chr) = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)
        succeed |> ok
        chr     |> isnotnull
        onKeySet reg key epoch proleptic chr 1

        // Using the same key.
        let (succeed1, chr1) = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)
        succeed1 |> nok
        chr1     |> isnull
        reg.CountUserCalendars() === 1

        // Using a different key, we create a new calendar.
        let (otherSucceed, otherChr) = reg.TryAdd("otherKey", new GregorianSchema(), epoch, proleptic)
        otherSucceed |> ok
        otherChr     |> isnotnull
        onKeySet reg key epoch proleptic chr 2

        //
        // Now, the registry is full.
        //

        // Using an old key.
        let (succeed2, chr2) = reg.TryAdd("key", new GregorianSchema(), epoch, proleptic)
        succeed2 |> nok
        chr2     |> isnull
        reg.CountUserCalendars() === 2

        // Using a new key.
        let (succeed3, chr3) = reg.TryAdd("newKey", new GregorianSchema(), epoch, proleptic)
        succeed3 |> nok
        chr3     |> isnull
        onKeyNotSet reg "newKey" 2

        reg.ForceCanAdd <- true

        let (succeed4, chr4) = reg.TryAdd("newKey", new GregorianSchema(), epoch, proleptic)
        succeed4 |> nok
        chr4     |> isnull
        onKeyNotSet reg "newKey" 2
