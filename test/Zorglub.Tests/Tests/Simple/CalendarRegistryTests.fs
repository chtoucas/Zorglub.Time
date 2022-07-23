// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarRegistryTests

open Zorglub.Testing
open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Simple

open Xunit

/// 64
let private defaultMinId = CalendarRegistry.MinMinId
/// 127
let private defaultMaxId = CalendarRegistry.MaxMaxId

module TestCommon =
    let checkState (reg: CalendarRegistry) count countUsers =
        reg.CountCalendars() === count
        reg.CountUserCalendars() === countUsers

        if countUsers = 0 then
            reg.IsPristine |> ok
        else
            reg.IsPristine |> nok

    let onKeyNotSet (reg: CalendarRegistry) key =
        Assert.DoesNotContain(key, reg.Keys)

        keyNotFoundExn (fun () -> reg.GetCalendar(key))

    let onKeySet (reg: CalendarRegistry) key epoch proleptic (chr: Calendar) =
        chr |> isnotnull

        chr.Key         === key
        chr.Epoch       === epoch
        chr.IsProleptic === proleptic

        Assert.Contains(key, reg.Keys)

        reg.GetCalendar(key) ==& chr

module Prelude =
    [<Fact>]
    let ``Constructor throws when minId < MinMinId`` () =
        outOfRangeExn "minId" (fun () -> new CalendarRegistry(CalendarRegistry.MinMinId - 1, defaultMaxId))

    [<Fact>]
    let ``Constructor does not throw when minId = MinMinId`` () =
        new CalendarRegistry(CalendarRegistry.MinMinId, defaultMaxId) |> ignore

    [<Fact>]
    let ``Constructor throws when minId > MaxMaxId`` () =
        outOfRangeExn "minId" (fun () -> new CalendarRegistry(CalendarRegistry.MaxMaxId + 1, defaultMaxId))

    [<Fact>]
    let ``Constructor does not throw when minId = MaxMaxId`` () =
        new CalendarRegistry(CalendarRegistry.MaxMaxId, defaultMaxId) |> ignore

    [<Fact>]
    let ``Constructor throws when maxId < minId`` () =
        outOfRangeExn "maxId" (fun () -> new CalendarRegistry(defaultMinId, defaultMinId - 1))

    [<Fact>]
    let ``Constructor does not throw when maxId = minId`` () =
        new CalendarRegistry(defaultMinId, defaultMinId) |> ignore

    [<Fact>]
    let ``Constructor throws when maxId > MaxMaxId`` () =
        outOfRangeExn "maxId" (fun () -> new CalendarRegistry(defaultMinId, CalendarRegistry.MaxMaxId + 1))

    [<Fact>]
    let ``Constructor does not throw when maxId = MaxMaxId`` () =
        new CalendarRegistry(defaultMinId, CalendarRegistry.MaxMaxId) |> ignore

    [<Fact>]
    let ``Constructor (largest size)`` () =
        let minId = CalendarRegistry.MinMinId
        let maxId = CalendarRegistry.MaxMaxId

        let reg = new CalendarRegistry(minId, maxId)

        reg.MinId === minId
        reg.MaxId === maxId

        reg.IsPristine |> ok
        reg.NumberOfSystemCalendars === 0

        reg.MaxNumberOfCalendars === maxId + 1
        reg.CountCalendars() === 0

        reg.MaxNumberOfUserCalendars === 64
        reg.CountUserCalendars() === 0

    [<Fact>]
    let ``Constructor (smallest size)`` () =
        let minId = CalendarRegistry.MinMinId
        let maxId = minId

        let reg = new CalendarRegistry(minId, maxId)

        reg.MinId === minId
        reg.MaxId === maxId

        reg.IsPristine |> ok
        reg.NumberOfSystemCalendars === 0

        reg.MaxNumberOfCalendars === maxId + 1
        reg.CountCalendars() === 0

        reg.MaxNumberOfUserCalendars === 1
        reg.CountUserCalendars() === 0

    [<Fact>]
    let ``Constructor with calendars`` () =
        let minId = CalendarRegistry.MinMinId
        let maxId = CalendarRegistry.MaxMaxId

        let reg = new CalendarRegistry(minId, maxId, [| GregorianCalendar.Instance |])

        reg.MinId === minId
        reg.MaxId === maxId

        reg.IsPristine |> ok
        reg.NumberOfSystemCalendars === 1

        reg.MaxNumberOfCalendars === maxId + 1
        reg.CountCalendars() === 1

        reg.MaxNumberOfUserCalendars === 64
        reg.CountUserCalendars() === 0

module AddOps =
    open TestCommon

    let private gregorian = GregorianCalendar.Instance

    let private newEmptyRegistry () =
        new CalendarRegistry(defaultMinId, defaultMaxId)

    /// Creates a new registry with one system calendar.
    let private newRegistry (chr: Calendar) =
        new CalendarRegistry(defaultMinId, defaultMaxId, [| chr |])

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
        onKeyNotSet reg key
        checkState reg 0 0

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

        checkState reg 1 0

        let chr = reg.GetOrAdd(key, new GregorianSchema(), epoch, proleptic)

        onKeySet reg key epoch proleptic chr
        checkState reg 2 1

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
        onKeyNotSet reg key
        checkState reg 0 0

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

        checkState reg 1 0

        let chr = reg.Add(key, new GregorianSchema(), epoch, proleptic)

        onKeySet reg key epoch proleptic chr
        checkState reg 2 1

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
        onKeyNotSet reg key
        checkState reg 0 0

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

        checkState reg 1 0

        let succeed, chr = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet reg key epoch proleptic chr
        checkState reg 2 1

    [<Fact>]
    let ``TryAdd() when the key is empty`` () =
        let reg = newRegistry gregorian
        let key = ""
        let epoch = DayNumber.Zero + 1234
        let proleptic = true

        checkState reg 1 0

        let succeed, chr = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet reg key epoch proleptic chr
        checkState reg 2 1

module AddLimits =
    open TestCommon

    let private newMiniRegistry size =
        let minId = CalendarRegistry.MinMinId
        let maxId = minId + (size - 1)

        new CalendarRegistry(minId, maxId)

    // TODO(test): use different params.

    [<Fact>]
    let ``GetOrAdd()`` () =
        let reg = newMiniRegistry 2
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        reg.MaxNumberOfUserCalendars === 2
        checkState reg 0 0

        let chr = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "key" epoch proleptic chr
        checkState reg 1 1

        // Using the same key, we obtain the same calendar.
        let chr1 = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)
        chr1 ==& chr
        checkState reg 1 1

        // Using a different key, we create a new calendar.
        let otherChr = reg.GetOrAdd("otherKey", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "otherKey" epoch proleptic otherChr
        checkState reg 2 2

        //
        // Now, the registry is full.
        //

        // Using an old key.
        let chr2 = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)
        chr2 ==& chr
        checkState reg 2 2

        // Using a new key.
        overflows (fun () -> reg.GetOrAdd("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"
        checkState reg 2 2

        reg.ForceCanAdd <- true

        overflows (fun () -> reg.GetOrAdd("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"
        checkState reg 2 2

    [<Fact>]
    let ``Add()`` () =
        let reg = newMiniRegistry 2
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        reg.MaxNumberOfUserCalendars === 2
        checkState reg 0 0

        let chr = reg.Add("key", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "key" epoch proleptic chr
        checkState reg 1 1

        // Using the same key.
        argExn "key" (fun () -> reg.Add("key", new GregorianSchema(), epoch, proleptic))

        // Using a different key, we create a new calendar.
        let otherChr = reg.Add("otherKey", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "otherKey" epoch proleptic otherChr
        checkState reg 2 2

        //
        // Now, the registry is full.
        //

        // Using an old key.
        overflows (fun () -> reg.Add("key", new GregorianSchema(), epoch, proleptic))
        checkState reg 2 2

        // Using a new key.
        overflows (fun () -> reg.Add("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"
        checkState reg 2 2

        reg.ForceCanAdd <- true

        overflows (fun () -> reg.Add("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"
        checkState reg 2 2

    [<Fact>]
    let ``TryAdd()`` () =
        let reg = newMiniRegistry 2
        let key = "key"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        reg.MaxNumberOfUserCalendars === 2
        checkState reg 0 0

        let (succeed, chr) = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)
        succeed |> ok
        chr     |> isnotnull
        onKeySet reg key epoch proleptic chr
        checkState reg 1 1

        // Using the same key.
        let (succeed1, chr1) = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)
        succeed1 |> nok
        chr1     |> isnull
        checkState reg 1 1

        // Using a different key, we create a new calendar.
        let (otherSucceed, otherChr) = reg.TryAdd("otherKey", new GregorianSchema(), epoch, proleptic)
        otherSucceed |> ok
        otherChr     |> isnotnull
        onKeySet reg key epoch proleptic chr
        checkState reg 2 2

        //
        // Now, the registry is full.
        //

        // Using an old key.
        let (succeed2, chr2) = reg.TryAdd("key", new GregorianSchema(), epoch, proleptic)
        succeed2 |> nok
        chr2     |> isnull
        checkState reg 2 2

        // Using a new key.
        let (succeed3, chr3) = reg.TryAdd("newKey", new GregorianSchema(), epoch, proleptic)
        succeed3 |> nok
        chr3     |> isnull
        onKeyNotSet reg "newKey"
        checkState reg 2 2

        reg.ForceCanAdd <- true

        let (succeed4, chr4) = reg.TryAdd("newKey", new GregorianSchema(), epoch, proleptic)
        succeed4 |> nok
        chr4     |> isnull
        onKeyNotSet reg "newKey"
        checkState reg 2 2
