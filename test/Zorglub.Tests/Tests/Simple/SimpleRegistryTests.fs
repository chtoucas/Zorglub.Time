// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.SimpleRegistryTests

open System.Linq
open System.Collections.Generic
open System.Threading.Tasks

open Zorglub.Testing
open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Simple

open Xunit

module TestCommon =
    let checkCounts (reg: SimpleRegistry) count usersCount =
        reg.CountCalendars() === count
        reg.CountUserCalendars() === usersCount

        if usersCount = 0 then
            reg.IsPristine |> ok
        else
            reg.IsPristine |> nok

    let onKeyNotSet (reg: SimpleRegistry) key =
        Assert.DoesNotContain(key, reg.Keys)

        keyNotFoundExn (fun () -> reg.GetCalendar(key))

        let succeed, cal = reg.TryGetCalendar(key)
        succeed |> nok
        cal     |> isnull

    let onKeySet (reg: SimpleRegistry) key epoch proleptic (chr: SimpleCalendar) =
        chr |> isnotnull

        chr.Key         === key
        chr.Epoch       === epoch
        chr.IsProleptic === proleptic

        Assert.Contains(key, reg.Keys)

        reg.GetCalendar(key) ==& chr

        let succeed, cal = reg.TryGetCalendar(key)
        succeed |> ok
        cal ==& chr

module Prelude =
    [<Fact>]
    let ``Constructor throws when "calendars" is too large`` () =
        // Here we can use an array of nulls because the ctor will throw before
        // accessing any element.
        let calendars = Array.zeroCreate<SimpleCalendar>(1 + SimpleRegistry.MinMinId)

        argExn "calendars" (fun () -> new SimpleRegistry(calendars))

    [<Fact>]
    let ``Constructor throws when "calendars" contains a user-defined calendar`` () =
        // The order is not arbitrary, we MUST ensure that the index of a system
        // calendar in "calendars" is given by its ID.
        let calendars = [|
            SimpleCalendar.Gregorian;
            SimpleCalendar.Julian;
            UserCalendars.Gregorian;
        |]

        argExn "calendars" (fun () -> new SimpleRegistry(calendars))

    [<Fact>]
    let ``Constructor throws when "calendars" contains the same system calendar twice`` () =
        // The order is not arbitrary, we MUST ensure that the index of a system
        // calendar in "calendars" is given by its ID.
        let calendars = [|
            SimpleCalendar.Gregorian;
            SimpleCalendar.Julian;
            SimpleCalendar.Julian
        |]

        argExn "calendars" (fun () -> new SimpleRegistry(calendars))

    [<Fact>]
    let ``Constructor throws when minId < MinMinId`` () =
        outOfRangeExn "minId" (fun () -> new SimpleRegistry(SimpleRegistry.MinMinId - 1, SimpleRegistry.MaxMaxId))

    [<Fact>]
    let ``Constructor throws when minId > MaxMaxId`` () =
        outOfRangeExn "minId" (fun () -> new SimpleRegistry(SimpleRegistry.MaxMaxId + 1, SimpleRegistry.MaxMaxId))

    [<Fact>]
    let ``Constructor throws when maxId < minId`` () =
        outOfRangeExn "maxId" (fun () -> new SimpleRegistry(SimpleRegistry.MinMinId, SimpleRegistry.MinMinId - 1))

    [<Fact>]
    let ``Constructor throws when maxId > MaxMaxId`` () =
        outOfRangeExn "maxId" (fun () -> new SimpleRegistry(SimpleRegistry.MinMinId, SimpleRegistry.MaxMaxId + 1))

    [<Fact>]
    let ``Constructor does not throw when maxId = minId = MinMinId`` () =
        new SimpleRegistry(SimpleRegistry.MinMinId, SimpleRegistry.MinMinId) |> ignore

    [<Fact>]
    let ``Constructor does not throw when maxId = minId = MaxMaxId`` () =
        new SimpleRegistry(SimpleRegistry.MaxMaxId, SimpleRegistry.MaxMaxId) |> ignore

    [<Fact>]
    let ``Constructor`` () =
        let reg = new SimpleRegistry()

        reg.MinId === SimpleRegistry.MinMinId
        reg.MaxId === SimpleRegistry.MaxMaxId

        reg.IsPristine |> ok
        reg.IsFull |> nok

        reg.NumberOfSystemCalendars === 0
        reg.MaxNumberOfUserCalendars === 64
        reg.MaxNumberOfCalendars === 128

        reg.CountUserCalendars() === 0
        reg.CountCalendars() === 0
        reg.RawCount === 0

    [<Fact>]
    let ``Constructor with one system calendar`` () =
        let calendars = [| SimpleCalendar.Gregorian |]
        let reg = new SimpleRegistry(calendars)

        reg.MinId === SimpleRegistry.MinMinId
        reg.MaxId === SimpleRegistry.MaxMaxId

        reg.IsPristine |> ok
        reg.IsFull |> nok

        reg.NumberOfSystemCalendars === 1
        reg.MaxNumberOfUserCalendars === 64
        reg.MaxNumberOfCalendars === 128

        reg.CountUserCalendars() === 0
        reg.CountCalendars() === 1
        reg.RawCount === 1

    [<Fact>]
    let ``Constructor with three system calendars`` () =
        // The order is not arbitrary, we MUST ensure that the index of a calendar
        // in "calendars" is its ID.
        let calendars = [|
            SimpleCalendar.Gregorian;
            SimpleCalendar.Julian;
            SimpleCalendar.Civil
        |]
        let reg = new SimpleRegistry(calendars)

        reg.MinId === SimpleRegistry.MinMinId
        reg.MaxId === SimpleRegistry.MaxMaxId

        reg.IsPristine |> ok
        reg.IsFull |> nok

        reg.NumberOfSystemCalendars === 3
        reg.MaxNumberOfUserCalendars === 64
        reg.MaxNumberOfCalendars === 128

        reg.CountUserCalendars() === 0
        reg.CountCalendars() === 3
        reg.RawCount === 3


    [<Fact>]
    let ``Constructor (smallest size, no system calendar)`` () =
        let reg = new SimpleRegistry(SimpleRegistry.MinMinId, SimpleRegistry.MinMinId)

        reg.MinId === SimpleRegistry.MinMinId
        reg.MaxId === SimpleRegistry.MinMinId

        reg.IsPristine |> ok
        reg.IsFull |> nok

        reg.NumberOfSystemCalendars === 0
        reg.MaxNumberOfUserCalendars === 1
        reg.MaxNumberOfCalendars === 65

        reg.CountUserCalendars() === 0
        reg.CountCalendars() === 0
        reg.RawCount === 0

module Snapshot =
    [<Fact>]
    let ``TakeSnapshot()`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])
        let usr = reg.Add("User Gregorian", new GregorianSchema(), DayZero.NewStyle, false)

        let dict = reg.TakeSnapshot()

        dict.[sys.Key] ==& sys
        dict.[usr.Key] ==& usr

    [<Fact>]
    let ``TakeSnapshot() when the registry contains a dirty key`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])
        let usr = reg.Add("User Gregorian", new GregorianSchema(), DayZero.NewStyle, false)

        let dirty = new SimpleCalendar(Cuid.Invalid, "Dirty Gregorian", new GregorianSchema(), DayZero.NewStyle, false)
        reg.AddRaw(dirty)

        let dict = reg.TakeSnapshot()

        dict.ContainsKey(dirty.Key) |> nok

        dict.[sys.Key] ==& sys
        dict.[usr.Key] ==& usr

module Lookup =
    let private newRegistry  =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])
        let usr = reg.Add("User Gregorian", new GregorianSchema(), DayZero.NewStyle, false)
        reg, sys, usr

    //
    // GetCalendar()
    //

    [<Fact>]
    let ``GetCalendar(unknown key)`` () =
        let reg, _, _ = newRegistry

        throws<KeyNotFoundException> (fun () -> reg.GetCalendar("Unknown Key"))

    [<Fact>]
    let ``GetCalendar(system key) is not null and always returns the same reference`` () =
        let reg, sys, _ = newRegistry
        let chr1 = reg.GetCalendar(sys.Key)
        let chr2 = reg.GetCalendar(sys.Key)

        chr1 |> isnotnull
        chr1.Key === sys.Key
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendar(user key) is not null and always returns the same reference`` () =
        let reg, _, usr = newRegistry
        let chr1 = reg.GetCalendar(usr.Key)
        let chr2 = reg.GetCalendar(usr.Key)

        chr1 |> isnotnull
        chr1.Key === usr.Key
        chr2 ==& chr1

    [<Fact>]
    let ``GetCalendar(dirty key)`` () =
        let reg, _, _ = newRegistry
        let dirty = new SimpleCalendar(Cuid.Invalid, "Dirty Gregorian", new GregorianSchema(), DayZero.NewStyle, false)
        reg.AddRaw(dirty)

        throws<KeyNotFoundException> (fun () -> reg.GetCalendar(dirty.Key))

    //
    // TryGetCalendar()
    //

    [<Fact>]
    let ``TryGetCalendar(unknown key)`` () =
        let reg, _, _ = newRegistry
        let succeed, cal = reg.TryGetCalendar("Unknown Key")

        succeed |> nok
        cal |> isnull

    [<Fact>]
    let ``TryGetCalendar(system key) succeeds and always returns the same reference`` () =
        let reg, sys, _ = newRegistry
        let succeed1, cal1 = reg.TryGetCalendar(sys.Key)
        let succeed2, cal2 = reg.TryGetCalendar(sys.Key)

        succeed1 |> ok
        succeed2 |> ok
        cal1 |> isnotnull
        cal1.Key === sys.Key
        cal2 ==& cal1

    [<Fact>]
    let ``TryGetCalendar(user key) succeeds and always returns the same reference`` () =
        let reg, _, usr = newRegistry
        let succeed1, cal1 = reg.TryGetCalendar(usr.Key)
        let succeed2, cal2 = reg.TryGetCalendar(usr.Key)

        succeed1 |> ok
        succeed2 |> ok
        cal1 |> isnotnull
        cal1.Key === usr.Key
        cal2 ==& cal1

    [<Fact>]
    let ``TryGetCalendar(dirty key)`` () =
        let reg, _, _ = newRegistry
        let dirty = new SimpleCalendar(Cuid.Invalid, "Dirty Gregorian", new GregorianSchema(), DayZero.NewStyle, false)
        reg.AddRaw(dirty)

        let succeed, cal = reg.TryGetCalendar(dirty.Key)

        succeed |> nok
        cal |> isnull

module AddOps =
    open TestCommon

    //
    // AddRaw()
    //

    [<Fact>]
    let ``AddRaw() breaks the counting methods`` () =
        let reg = new SimpleRegistry([| SimpleCalendar.Gregorian |])
        let chr = reg.Add("Key", new GregorianSchema(), DayZero.NewStyle, false)

        reg.RawCount === 2
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding an already included system calendar.
        reg.AddRaw(SimpleCalendar.Gregorian)

        reg.RawCount === 2
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding an already included user-defined calendar.
        reg.AddRaw(chr)

        reg.RawCount === 2
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding a system calendar.
        reg.AddRaw(SimpleCalendar.Julian)

        reg.RawCount === 3 // Count increased by 1
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding a user-defined calendar.
        reg.AddRaw(new SimpleCalendar(Cuid.MinUser, "User Gregorian", new GregorianSchema(), DayZero.NewStyle, false))

        reg.RawCount === 4 // Count increased by 1
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding a dirty calendar.
        reg.AddRaw(new SimpleCalendar(Cuid.Invalid, "Dirty Gregorian", new GregorianSchema(), DayZero.NewStyle, false))

        reg.RawCount === 5 // Count increased by 1
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

    //
    // GetOrAdd()
    //

    [<Fact>]
    let ``GetOrAdd() throws for null key`` () =
        let reg = new SimpleRegistry()

        nullExn "key" (fun () -> reg.GetOrAdd(null, new GregorianSchema(), DayZero.NewStyle, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let reg = new SimpleRegistry()

        nullExn "schema" (fun () -> reg.GetOrAdd("Key", null, DayZero.NewStyle, false))
        checkCounts reg 0 0

        onKeyNotSet reg "Key"

    [<Fact>]
    let ``GetOrAdd() throws for unsupported schema`` () =
        let reg = new SimpleRegistry()
        let sch = new FauxSystemSchema(Range.Create(-2, -1))

        argExn "supportedYears" (fun () -> reg.GetOrAdd("Key", sch, DayZero.NewStyle, false))
        checkCounts reg 0 0

        onKeyNotSet reg "Key"

    [<Fact>]
    let ``GetOrAdd() when the key is already taken`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])

        checkCounts reg 1 0
        let chr = reg.GetOrAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, true)
        checkCounts reg 1 0

        chr ==& sys

    [<Fact>]
    let ``GetOrAdd()`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])
        let epoch = DayZero.NewStyle
        let proleptic = false

        checkCounts reg 1 0
        let chr = reg.GetOrAdd("Key", new GregorianSchema(), epoch, proleptic)
        checkCounts reg 2 1

        onKeySet reg "Key" epoch proleptic chr

    //
    // Add()
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        let reg = new SimpleRegistry()

        nullExn "key" (fun () -> reg.Add(null, new GregorianSchema(), DayZero.NewStyle, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let reg = new SimpleRegistry()

        nullExn "schema" (fun () -> reg.Add("Key", null, DayZero.NewStyle, false))
        checkCounts reg 0 0

        onKeyNotSet reg "Key"

    [<Fact>]
    let ``Add() throws for unsupported schema`` () =
        let reg = new SimpleRegistry()
        let sch = new FauxSystemSchema(Range.Create(-2, -1))

        argExn "supportedYears" (fun () -> reg.Add("Key", sch, DayZero.NewStyle, false))
        checkCounts reg 0 0

        onKeyNotSet reg "Key"

    [<Fact>]
    let ``Add() throws when the key is already taken`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])

        checkCounts reg 1 0
        argExn "key" (fun () -> reg.Add(sys.Key, new JulianSchema(), DayZero.OldStyle, true))
        checkCounts reg 1 0

    [<Fact>]
    let ``Add()`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])
        let epoch = DayZero.NewStyle
        let proleptic = false

        checkCounts reg 1 0
        let chr = reg.Add("Key", new GregorianSchema(), epoch, proleptic)
        checkCounts reg 2 1

        onKeySet reg "Key" epoch proleptic chr

    //
    // TryAdd()
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        let reg = new SimpleRegistry()

        nullExn "key" (fun () -> reg.TryAdd(null, new JulianSchema(), DayZero.OldStyle, true))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let reg = new SimpleRegistry()

        nullExn "schema" (fun () -> reg.TryAdd("Key", null, DayZero.OldStyle, false))
        checkCounts reg 0 0

        onKeyNotSet reg "Key"

    [<Fact>]
    let ``TryAdd() for unsupported schema`` () =
        let reg = new SimpleRegistry()
        let sch = new FauxSystemSchema(Range.Create(-2, -1))

        let succeed, chr = reg.TryAdd("Key", sch, DayZero.OldStyle, false)
        checkCounts reg 0 0

        succeed |> nok
        chr     |> isnull
        onKeyNotSet reg "Key"

    [<Fact>]
    let ``TryAdd() when the key is already taken`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])

        checkCounts reg 1 0
        let succeed, chr = reg.TryAdd(sys.Key, new JulianSchema(), DayZero.OldStyle, true)
        checkCounts reg 1 0

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryAdd()`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])
        let epoch = DayZero.NewStyle
        let proleptic = false

        checkCounts reg 1 0
        let succeed, chr = reg.TryAdd("Key", new GregorianSchema(), epoch, proleptic)
        checkCounts reg 2 1

        succeed |> ok
        onKeySet reg "Key" epoch proleptic chr

    [<Fact>]
    let ``TryAdd() when the key is empty`` () =
        let sys = SimpleCalendar.Gregorian
        let reg = new SimpleRegistry([| sys |])
        let epoch = DayZero.NewStyle
        let proleptic = false

        checkCounts reg 1 0
        let succeed, chr = reg.TryAdd("Key", new GregorianSchema(), epoch, proleptic)
        checkCounts reg 2 1

        succeed |> ok
        onKeySet reg "Key" epoch proleptic chr

module AddLimits =
    open TestCommon

    let private newMiniRegistry size =
        let minId = SimpleRegistry.MinMinId
        let maxId = minId + (size - 1)

        new SimpleRegistry(minId, maxId)

    [<Fact>]
    let ``GetOrAdd()`` () =
        let reg = newMiniRegistry 2
        let epoch = DayZero.NewStyle
        let proleptic = false

        reg.IsFull |> nok

        reg.MaxNumberOfUserCalendars === 2
        checkCounts reg 0 0

        let chr = reg.GetOrAdd("Key", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "Key" epoch proleptic chr
        checkCounts reg 1 1

        // Using the same key, we obtain the same calendar.
        let chr1 = reg.GetOrAdd("Key", new JulianSchema(), DayZero.OldStyle, true)
        chr1 ==& chr
        checkCounts reg 1 1

        // Using a different key, we create a new calendar.
        let otherChr = reg.GetOrAdd("Another Key", new GregorianSchema(), DayZero.NewStyle, false)
        onKeySet reg "Another Key" epoch proleptic otherChr
        checkCounts reg 2 2

        //
        // Now, the registry is full.
        //

        reg.IsFull |> ok

        // Using an old key.
        let chr2 = reg.GetOrAdd("Key", new JulianSchema(), DayZero.OldStyle, true)
        chr2 ==& chr
        checkCounts reg 2 2

        // Using a new key.
        overflows (fun () -> reg.GetOrAdd("New Key", new JulianSchema(), DayZero.OldStyle, true))
        onKeyNotSet reg "New Key"
        checkCounts reg 2 2

        reg.DisableFailFast <- true

        overflows (fun () -> reg.GetOrAdd("New Key", new JulianSchema(), DayZero.OldStyle, true))
        onKeyNotSet reg "New Key"
        checkCounts reg 2 2

    [<Fact>]
    let ``Add()`` () =
        let reg = newMiniRegistry 2
        let epoch = DayZero.NewStyle
        let proleptic = false

        reg.IsFull |> nok

        reg.MaxNumberOfUserCalendars === 2
        checkCounts reg 0 0

        let chr = reg.Add("Key", new GregorianSchema(), epoch, proleptic)
        onKeySet reg "Key" epoch proleptic chr
        checkCounts reg 1 1

        // Using the same key.
        argExn "key" (fun () -> reg.Add("Key", new JulianSchema(), DayZero.OldStyle, true))

        // Using a different key, we create a new calendar.
        let otherChr = reg.Add("Another Key", new GregorianSchema(), DayZero.NewStyle, false)
        onKeySet reg "Another Key" epoch proleptic otherChr
        checkCounts reg 2 2

        //
        // Now, the registry is full.
        //

        reg.IsFull |> ok

        // Using an old key.
        overflows (fun () -> reg.Add("Key", new JulianSchema(), DayZero.OldStyle, true))
        checkCounts reg 2 2

        // Using a new key.
        overflows (fun () -> reg.Add("New Key", new JulianSchema(), DayZero.OldStyle, true))
        onKeyNotSet reg "New Key"
        checkCounts reg 2 2

        reg.DisableFailFast <- true

        overflows (fun () -> reg.Add("New Key", new JulianSchema(), DayZero.OldStyle, true))
        onKeyNotSet reg "New Key"
        checkCounts reg 2 2

    [<Fact>]
    let ``TryAdd()`` () =
        let reg = newMiniRegistry 2
        let key = "Key"
        let epoch = DayZero.NewStyle
        let proleptic = true

        reg.IsFull |> nok

        reg.MaxNumberOfUserCalendars === 2
        checkCounts reg 0 0

        let (succeed, chr) = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)
        succeed |> ok
        chr     |> isnotnull
        onKeySet reg key epoch proleptic chr
        checkCounts reg 1 1

        // Using the same key.
        let (succeed1, chr1) = reg.TryAdd(key, new JulianSchema(), DayZero.OldStyle, true)
        succeed1 |> nok
        chr1     |> isnull
        checkCounts reg 1 1

        // Using a different key, we create a new calendar.
        let (otherSucceed, otherChr) = reg.TryAdd("Another Key", new GregorianSchema(), DayZero.NewStyle, false)
        otherSucceed |> ok
        otherChr     |> isnotnull
        onKeySet reg key epoch proleptic chr
        checkCounts reg 2 2

        //
        // Now, the registry is full.
        //

        reg.IsFull |> ok

        // Using an old key.
        let (succeed2, chr2) = reg.TryAdd("Key", new JulianSchema(), DayZero.OldStyle, true)
        succeed2 |> nok
        chr2     |> isnull
        checkCounts reg 2 2

        // Using a new key.
        let (succeed3, chr3) = reg.TryAdd("New Key", new JulianSchema(), DayZero.OldStyle, true)
        succeed3 |> nok
        chr3     |> isnull
        onKeyNotSet reg "New Key"
        checkCounts reg 2 2

        reg.DisableFailFast <- true

        let (succeed4, chr4) = reg.TryAdd("New Key", new JulianSchema(), DayZero.OldStyle, true)
        succeed4 |> nok
        chr4     |> isnull
        onKeyNotSet reg "New Key"
        checkCounts reg 2 2

    //
    // Parallel
    //

    [<Fact>]
    let ``GetOrAdd() parallel`` () =
        let reg = new SimpleRegistry()
        let epoch = DayZero.NewStyle;
        let proleptic = false

        reg.CountCalendars() === 0

        Parallel.ForEach(
            Enumerable.Range(0, reg.MaxNumberOfUserCalendars),
            fun i ->
                let key = $"Key {i}";
                let chr = reg.GetOrAdd(key, new GregorianSchema(), epoch, proleptic)

                onKeySet reg key epoch proleptic chr
        ) |> ignore

        reg.IsFull |> ok
        reg.CountCalendars() === reg.MaxNumberOfUserCalendars

    [<Fact>]
    let ``Add() parallel`` () =
        let reg = new SimpleRegistry()
        let epoch = DayZero.NewStyle;
        let proleptic = false

        reg.CountCalendars() === 0

        Parallel.ForEach(
            Enumerable.Range(0, reg.MaxNumberOfUserCalendars),
            fun i ->
                let key = $"Key {i}";
                let chr = reg.Add(key, new GregorianSchema(), epoch, proleptic)

                onKeySet reg key epoch proleptic chr
        ) |> ignore

        reg.IsFull |> ok
        reg.CountCalendars() === reg.MaxNumberOfUserCalendars

    [<Fact>]
    let ``TryAdd() parallel`` () =
        let reg = new SimpleRegistry()
        let epoch = DayZero.NewStyle;
        let proleptic = false

        reg.CountCalendars() === 0

        Parallel.ForEach(
            Enumerable.Range(0, reg.MaxNumberOfUserCalendars),
            fun i ->
                let key = $"Key {i}";
                let succeed, chr = reg.TryAdd(key, new GregorianSchema(), epoch, proleptic)

                succeed |> ok
                onKeySet reg key epoch proleptic chr
        ) |> ignore

        reg.IsFull |> ok
        reg.CountCalendars() === reg.MaxNumberOfUserCalendars
