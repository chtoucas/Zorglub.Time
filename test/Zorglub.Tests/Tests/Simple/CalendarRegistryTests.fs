// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarRegistryTests

open System.Collections.Generic

open Zorglub.Testing
open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Simple

open Xunit

module TestCommon =
    let checkState (reg: CalendarRegistry) count usersCount =
        reg.CountCalendars() === count
        reg.CountUserCalendars() === usersCount

        if usersCount = 0 then
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
    let ``Constructor throws when "calendars" is too large`` () =
        // Here we can use an array of nulls because the ctor will throw before
        // accessing any element.
        let calendars = Array.zeroCreate<Calendar>(1 + CalendarRegistry.MinMinId)

        argExn "calendars" (fun () -> new CalendarRegistry(calendars))

    [<Fact>]
    let ``Constructor throws when "calendars" contains a user-defined calendar`` () =
        // The order is not arbitrary, we MUST ensure that the index of a system
        // calendar in "calendars" is given by its ID.
        let calendars = [|
            GregorianCalendar.Instance :> Calendar;
            JulianCalendar.Instance :> Calendar;
            UserCalendars.Gregorian;
        |]

        argExn "calendars" (fun () -> new CalendarRegistry(calendars))

    [<Fact>]
    let ``Constructor throws when "calendars" contains the same system calendar twice`` () =
        // The order is not arbitrary, we MUST ensure that the index of a system
        // calendar in "calendars" is given by its ID.
        let calendars = [|
            GregorianCalendar.Instance :> Calendar;
            JulianCalendar.Instance :> Calendar;
            JulianCalendar.Instance :> Calendar
        |]

        argExn "calendars" (fun () -> new CalendarRegistry(calendars))

    [<Fact>]
    let ``Constructor throws when minId < MinMinId`` () =
        outOfRangeExn "minId" (fun () -> new CalendarRegistry(CalendarRegistry.MinMinId - 1, CalendarRegistry.MaxMaxId))

    [<Fact>]
    let ``Constructor throws when minId > MaxMaxId`` () =
        outOfRangeExn "minId" (fun () -> new CalendarRegistry(CalendarRegistry.MaxMaxId + 1, CalendarRegistry.MaxMaxId))

    [<Fact>]
    let ``Constructor throws when maxId < minId`` () =
        outOfRangeExn "maxId" (fun () -> new CalendarRegistry(CalendarRegistry.MinMinId, CalendarRegistry.MinMinId - 1))

    [<Fact>]
    let ``Constructor throws when maxId > MaxMaxId`` () =
        outOfRangeExn "maxId" (fun () -> new CalendarRegistry(CalendarRegistry.MinMinId, CalendarRegistry.MaxMaxId + 1))

    [<Fact>]
    let ``Constructor does not throw when maxId = minId = MinMinId`` () =
        new CalendarRegistry(CalendarRegistry.MinMinId, CalendarRegistry.MinMinId) |> ignore

    [<Fact>]
    let ``Constructor does not throw when maxId = minId = MaxMaxId`` () =
        new CalendarRegistry(CalendarRegistry.MaxMaxId, CalendarRegistry.MaxMaxId) |> ignore

    [<Fact>]
    let ``Constructor`` () =
        let reg = new CalendarRegistry()

        reg.MinId === CalendarRegistry.MinMinId
        reg.MaxId === CalendarRegistry.MaxMaxId

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
        let calendars = [| GregorianCalendar.Instance :> Calendar |]
        let reg = new CalendarRegistry(calendars)

        reg.MinId === CalendarRegistry.MinMinId
        reg.MaxId === CalendarRegistry.MaxMaxId

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
            GregorianCalendar.Instance :> Calendar;
            JulianCalendar.Instance :> Calendar;
            ArmenianCalendar.Instance :> Calendar
        |]
        let reg = new CalendarRegistry(calendars)

        reg.MinId === CalendarRegistry.MinMinId
        reg.MaxId === CalendarRegistry.MaxMaxId

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
        let reg = new CalendarRegistry(CalendarRegistry.MinMinId, CalendarRegistry.MinMinId)

        reg.MinId === CalendarRegistry.MinMinId
        reg.MaxId === CalendarRegistry.MinMinId

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
        let sys = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| sys |])
        let usr = reg.Add("User Key", new GregorianSchema(), DayZero.NewStyle, true)

        let dict = reg.TakeSnapshot()

        dict.[sys.Key] ==& sys
        dict.[usr.Key] ==& usr

    [<Fact>]
    let ``TakeSnapshot() when the registry contains a dirty key`` () =
        let sys = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| sys |])
        let usr = reg.Add("User Key", new GregorianSchema(), DayZero.NewStyle, true)

        let dirty = new Calendar(Cuid.Invalid, "Dirty Key", new GregorianSchema(), DayZero.NewStyle, true)
        reg.AddRaw(dirty)

        let dict = reg.TakeSnapshot()

        dict.ContainsKey(dirty.Key) |> nok

        dict.[sys.Key] ==& sys
        dict.[usr.Key] ==& usr

module Lookup =
    let private userKey = "User Key"
    let private dirtyKey = "Dirty Key"
    let private dirtyGregorian =
        new Calendar(Cuid.Invalid, dirtyKey, new GregorianSchema(), DayZero.NewStyle, true)
    let private systemGregorian = GregorianCalendar.Instance
    let private systemKey = systemGregorian.Key
    let private newRegistry  =
        let reg = new CalendarRegistry([| systemGregorian |])
        reg.Add(userKey, new GregorianSchema(), DayZero.NewStyle, true) |> ignore
        reg

    //
    // GetCalendar()
    //

    [<Fact>]
    let ``GetCalendar(unknown key)`` () =
        let reg = newRegistry

        throws<KeyNotFoundException> (fun () -> reg.GetCalendar("Unknown Key"))

    [<Fact>]
    let ``GetCalendar(system key) is not null and always returns the same reference`` () =
        let reg = newRegistry
        let chr1 = reg.GetCalendar(systemKey)
        let chr2 = reg.GetCalendar(systemKey)

        chr1 |> isnotnull
        chr1.Key === systemKey
        chr1 ==& chr2

    [<Fact>]
    let ``GetCalendar(user key) is not null and always returns the same reference`` () =
        let reg = newRegistry
        let chr1 = reg.GetCalendar(userKey)
        let chr2 = reg.GetCalendar(userKey)

        chr1 |> isnotnull
        chr1.Key === userKey
        chr1 ==& chr2

    [<Fact>]
    let ``GetCalendar(dirty key)`` () =
        let reg = newRegistry
        reg.AddRaw(dirtyGregorian)

        throws<KeyNotFoundException> (fun () -> reg.GetCalendar(dirtyKey))

    //
    // TryGetCalendar()
    //

    [<Fact>]
    let ``TryGetCalendar(unknown key)`` () =
        let reg = newRegistry
        let succeed, chr = reg.TryGetCalendar("Unknown Key")

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryGetCalendar(system key) succeeds and always returns the same reference`` () =
        let reg = newRegistry
        let succeed1, chr1 = reg.TryGetCalendar(systemKey)
        let succeed2, chr2 = reg.TryGetCalendar(systemKey)

        succeed1 |> ok
        succeed2 |> ok
        chr1     |> isnotnull
        chr1.Key === systemKey
        chr1 ==& chr2

    [<Fact>]
    let ``TryGetCalendar(user key) succeeds and always returns the same reference`` () =
        let reg = newRegistry
        let succeed1, chr1 = reg.TryGetCalendar(userKey)
        let succeed2, chr2 = reg.TryGetCalendar(userKey)

        succeed1 |> ok
        succeed2 |> ok
        chr1     |> isnotnull
        chr1.Key === userKey
        chr1 ==& chr2

    [<Fact>]
    let ``TryGetCalendar(dirty key)`` () =
        let reg = newRegistry
        reg.AddRaw(dirtyGregorian)

        let succeed, chr = reg.TryGetCalendar(dirtyKey)

        succeed |> nok
        chr     |> isnull

module AddOps =
    open TestCommon

    //
    // AddRaw()
    //

    [<Fact>]
    let ``AddRaw() breaks the counting methods`` () =
        let reg = new CalendarRegistry([| GregorianCalendar.Instance |])
        let chr = reg.Add("key", new GregorianSchema(), DayZero.NewStyle, true)

        reg.RawCount === 2
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding an already included system calendar.
        reg.AddRaw(GregorianCalendar.Instance)

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
        reg.AddRaw(JulianCalendar.Instance)

        reg.RawCount === 3 // Count increased by 1
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding a user-defined calendar.
        reg.AddRaw(new Calendar(Cuid.MinUser, "User Key", new GregorianSchema(), DayZero.NewStyle, true))

        reg.RawCount === 4 // Count increased by 1
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

        // Adding a dirty calendar.
        reg.AddRaw(new Calendar(Cuid.Invalid, "Dirty Key", new GregorianSchema(), DayZero.NewStyle, true))

        reg.RawCount === 5 // Count increased by 1
        reg.NumberOfSystemCalendars === 1
        reg.CountCalendars() === 2
        reg.CountUserCalendars() === 1

    //
    // GetOrAdd()
    //

    [<Fact>]
    let ``GetOrAdd() throws for null key`` () =
        let reg = new CalendarRegistry()

        nullExn "key" (fun () -> reg.GetOrAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let reg = new CalendarRegistry()

        nullExn "schema" (fun () -> reg.GetOrAdd("key", null, DayZero.OldStyle, false))
        onKeyNotSet reg "key"
        checkState reg 0 0

    [<Fact>]
    let ``GetOrAdd() throws for unsupported schema`` () =
        let reg = new CalendarRegistry()
        let sch = new FauxSystemSchema(Range.Create(-2, -1))

        outOfRangeExn "year" (fun () -> reg.GetOrAdd("key", sch, DayZero.OldStyle, false))
        onKeyNotSet reg "key"
        checkState reg 0 0

    [<Fact>]
    let ``GetOrAdd() when the key is already taken`` () =
        let gr = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| gr |])
        let chr = reg.GetOrAdd(gr.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& gr
        checkState reg 1 0

    [<Fact>]
    let ``GetOrAdd()`` () =
        let gr = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| gr |])
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        checkState reg 1 0

        let chr = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)

        onKeySet reg "key" epoch proleptic chr
        checkState reg 2 1

    //
    // Add()
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        let reg = new CalendarRegistry()

        nullExn "key" (fun () -> reg.Add(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let reg = new CalendarRegistry()

        nullExn "schema" (fun () -> reg.Add("key", null, DayZero.OldStyle, false))
        onKeyNotSet reg "key"
        checkState reg 0 0

    [<Fact>]
    let ``Add() throws for unsupported schema`` () =
        let reg = new CalendarRegistry()
        let sch = new FauxSystemSchema(Range.Create(-2, -1))

        outOfRangeExn "year" (fun () -> reg.Add("key", sch, DayZero.OldStyle, false))
        onKeyNotSet reg "key"
        checkState reg 0 0

    [<Fact>]
    let ``Add() throws when the key is already taken`` () =
        let gr = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| gr |])

        argExn "key" (fun () -> reg.Add(gr.Key, new JulianSchema(), DayZero.OldStyle, false))
        checkState reg 1 0

    [<Fact>]
    let ``Add()`` () =
        let gr = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| gr |])
        let epoch = DayNumber.Zero + 1234
        let proleptic = true

        checkState reg 1 0

        let chr = reg.Add("key", new GregorianSchema(), epoch, proleptic)

        onKeySet reg "key" epoch proleptic chr
        checkState reg 2 1

    //
    // TryAdd()
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        let reg = new CalendarRegistry()

        nullExn "key" (fun () -> reg.TryAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let reg = new CalendarRegistry()

        nullExn "schema" (fun () -> reg.TryAdd("key", null, DayZero.OldStyle, false))
        onKeyNotSet reg "key"
        checkState reg 0 0

    [<Fact>]
    let ``TryAdd() for unsupported schema`` () =
        let reg = new CalendarRegistry()
        let sch = new FauxSystemSchema(Range.Create(-2, -1))

        let succeed, chr = reg.TryAdd("key", sch, DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull
        onKeyNotSet reg "key"
        checkState reg 0 0

    [<Fact>]
    let ``TryAdd() when the key is already taken`` () =
        let gr = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| gr |])

        checkState reg 1 0

        let succeed, chr = reg.TryAdd(gr.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull
        checkState reg 1 0

    [<Fact>]
    let ``TryAdd()`` () =
        let gr = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| gr |])
        let epoch = DayNumber.Zero + 1234
        let proleptic = false

        checkState reg 1 0

        let succeed, chr = reg.TryAdd("key", new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet reg "key" epoch proleptic chr
        checkState reg 2 1

    [<Fact>]
    let ``TryAdd() when the key is empty`` () =
        let gr = GregorianCalendar.Instance
        let reg = new CalendarRegistry([| gr |])
        let epoch = DayNumber.Zero + 1234
        let proleptic = true

        checkState reg 1 0

        let succeed, chr = reg.TryAdd("key", new GregorianSchema(), epoch, proleptic)

        succeed |> ok
        onKeySet reg "key" epoch proleptic chr
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

        reg.IsFull |> ok

        // Using an old key.
        let chr2 = reg.GetOrAdd("key", new GregorianSchema(), epoch, proleptic)
        chr2 ==& chr
        checkState reg 2 2

        // Using a new key.
        overflows (fun () -> reg.GetOrAdd("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"
        checkState reg 2 2

        reg.DisableFailFast <- true

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

        reg.IsFull |> ok

        // Using an old key.
        overflows (fun () -> reg.Add("key", new GregorianSchema(), epoch, proleptic))
        checkState reg 2 2

        // Using a new key.
        overflows (fun () -> reg.Add("newKey", new GregorianSchema(), epoch, proleptic))
        onKeyNotSet reg "newKey"
        checkState reg 2 2

        reg.DisableFailFast <- true

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

        reg.IsFull |> ok

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

        reg.DisableFailFast <- true

        let (succeed4, chr4) = reg.TryAdd("newKey", new GregorianSchema(), epoch, proleptic)
        succeed4 |> nok
        chr4     |> isnull
        onKeyNotSet reg "newKey"
        checkState reg 2 2
