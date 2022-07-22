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
let private defaultMinId = CalendarRegistry.MinMinId
/// 127
let private defaultMaxId = CalendarRegistry.MaxMaxId

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
        nullExn "calendarsByKey" (fun () -> new CalendarRegistry(null, defaultMinId, defaultMaxId))

    [<Fact>]
    let ``Constructor throws when minId < MinMinId`` () =
        let minId = CalendarRegistry.MinMinId - 1
        let calendarsByKey = initCalendarsByKey()

        outOfRangeExn "minId" (fun () -> new CalendarRegistry(calendarsByKey, minId - 1, defaultMaxId))

    [<Fact>]
    let ``Constructor does not throw when minId = MinMinId`` () =
        let minId = CalendarRegistry.MinMinId
        let calendarsByKey = initCalendarsByKey()

        new CalendarRegistry(calendarsByKey, minId, defaultMaxId) |> ignore

    [<Fact>]
    let ``Constructor throws when minId > MaxMaxId`` () =
        let minId = CalendarRegistry.MaxMaxId + 1
        let calendarsByKey = initCalendarsByKey()

        outOfRangeExn "minId" (fun () -> new CalendarRegistry(calendarsByKey, minId, defaultMaxId))

    [<Fact>]
    let ``Constructor does not throw when minId = MaxMaxId`` () =
        let minId = CalendarRegistry.MaxMaxId
        let calendarsByKey = initCalendarsByKey()

        new CalendarRegistry(calendarsByKey, minId, defaultMaxId) |> ignore

    [<Fact>]
    let ``Constructor throws when maxId < minId`` () =
        let calendarsByKey = initCalendarsByKey()

        outOfRangeExn "maxId" (fun () -> new CalendarRegistry(calendarsByKey, defaultMinId, defaultMinId - 1))

    [<Fact>]
    let ``Constructor does not throw when maxId = minId`` () =
        let calendarsByKey = initCalendarsByKey()

        new CalendarRegistry(calendarsByKey, defaultMinId, defaultMinId) |> ignore

    [<Fact>]
    let ``Constructor throws when maxId > MaxMaxId`` () =
        let maxId = CalendarRegistry.MaxMaxId + 1
        let calendarsByKey = initCalendarsByKey()

        outOfRangeExn "maxId" (fun () -> new CalendarRegistry(calendarsByKey, defaultMinId, maxId))

    [<Fact>]
    let ``Constructor does not throw when maxId = MaxMaxId`` () =
        let maxId = CalendarRegistry.MaxMaxId
        let calendarsByKey = initCalendarsByKey()

        new CalendarRegistry(calendarsByKey, defaultMinId, maxId) |> ignore

    [<Fact>]
    let ``Constructor (largest size)`` () =
        let minId = CalendarRegistry.MinMinId
        let maxId = CalendarRegistry.MaxMaxId
        let calendarsByKey = initCalendarsByKey()

        let reg = new CalendarRegistry(calendarsByKey, minId, maxId)

        reg.MinId === minId
        reg.MaxId === maxId

        reg.MaxNumberOfUserCalendars === 64
        reg.CountUserCalendars() === 0

    [<Fact>]
    let ``Constructor (smallest size)`` () =
        let minId = CalendarRegistry.MinMinId
        let maxId = minId
        let calendarsByKey = initCalendarsByKey()

        let reg = new CalendarRegistry(calendarsByKey, minId, maxId)

        reg.MinId === minId
        reg.MaxId === maxId

        reg.MaxNumberOfUserCalendars === 1
        reg.CountUserCalendars() === 0

module AddOps =
    open TestCommon

    let private gregorian = GregorianCalendar.Instance

    let private newEmptyRegistry () =
        let calendarsByKey = initCalendarsByKey()

        new CalendarRegistry(calendarsByKey, defaultMinId, defaultMaxId)

    let private newRegistry (chr: Calendar) =
        let calendarsByKey = initCalendarsByKey()

        calendarsByKey.[chr.Key] <- new Lazy<Calendar>(chr)

        new CalendarRegistry(calendarsByKey, defaultMinId, defaultMaxId)

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
        let minId = CalendarRegistry.MinMinId
        let maxId = minId + (size - 1)
        let calendarsByKey = initCalendarsByKey()

        new CalendarRegistry(calendarsByKey, minId, maxId)

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
