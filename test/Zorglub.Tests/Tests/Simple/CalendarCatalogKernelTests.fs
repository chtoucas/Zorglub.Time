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

module TestCommon =
    let onKeyNotSet (writer: CalendarCatalogKernel) key count =
        Assert.DoesNotContain(key, writer.Keys)

        keyNotFoundExn (fun () -> writer.GetCalendarByKey(key))
        writer.CountUserCalendars() === count

    let onKeySet (writer: CalendarCatalogKernel) key epoch (chr: Calendar) proleptic count =
        chr |> isnotnull

        chr.Key         === key
        chr.Epoch       === epoch
        chr.IsProleptic === proleptic

        Assert.Contains(key, writer.Keys)

        writer.GetCalendarByKey(key) ==& chr
        writer.CountUserCalendars() === count

module Prelude =
    [<Fact>]
    let ``Constructor throws for null calendarsByKey`` () =
        let calendarsById = Array.zeroCreate<Calendar>(1)

        nullExn "calendarsByKey" (fun () -> new CalendarCatalogKernel(null, calendarsById, 1))

    [<Fact>]
    let ``Constructor throws for null calendarsById`` () =
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<Calendar>>()

        nullExn "calendarsById" (fun () -> new CalendarCatalogKernel(calendarsByKey, null, 1))

    [<Fact>]
    let ``Constructor throws when minUserId > maxId`` () =
        let count = 255
        let maxId = count - 1
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<Calendar>>()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        outOfRangeExn "minUserId" (fun () -> new CalendarCatalogKernel(calendarsByKey, calendarsById, maxId + 1))

    [<Fact>]
    let ``Constructor does not throw when minUserId = maxId`` () =
        let count = 255
        let maxId = count - 1
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<Calendar>>()
        let calendarsById = Array.zeroCreate<Calendar>(count)

        new CalendarCatalogKernel(calendarsByKey, calendarsById, maxId) |> ignore

    [<Fact>]
    let ``Constructor`` () =
        let count = 255
        let maxId = count - 1
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<Calendar>>()
        let calendarsById = Array.zeroCreate<Calendar>(count)
        let minUserId = 128

        let writer = new CalendarCatalogKernel(calendarsByKey, calendarsById, minUserId)

        writer.MinUserId === minUserId
        writer.MaxId     === maxId
        writer.CountUserCalendars() === 0

module AddOps =
    open TestCommon

    let private gregorian = GregorianCalendar.Instance

    let private newEmptyWriter () =
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<Calendar>>()
        let calendarsById = Array.zeroCreate<Calendar>(255)
        new CalendarCatalogKernel(calendarsByKey, calendarsById, 0)

    let private newWriter (chr: Calendar) =
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<Calendar>>()
        let calendarsById = Array.zeroCreate<Calendar>(255)

        calendarsByKey.[chr.Key] <- new Lazy<Calendar>(chr)
        calendarsById.[0] <- chr

        new CalendarCatalogKernel(calendarsByKey, calendarsById, 1)

    //
    // GetOrAdd() --- failure
    //

    [<Fact>]
    let ``GetOrAdd() throws for null key`` () =
        let writer = newEmptyWriter()

        nullExn "key" (fun () -> writer.GetOrAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``GetOrAdd() throws for null schema`` () =
        let writer = newEmptyWriter()
        let key = "key"

        nullExn "schema" (fun () -> writer.GetOrAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet writer key 0

    [<Fact>]
    let ``GetOrAdd() when the key is already taken`` () =
        let writer = newWriter gregorian
        let chr = writer.GetOrAdd(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        chr ==& gregorian

    //
    // Add() --- failure
    //

    [<Fact>]
    let ``Add() throws for null key`` () =
        let writer = newEmptyWriter()

        nullExn "key" (fun () -> writer.Add(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``Add() throws for null schema`` () =
        let writer = newEmptyWriter()
        let key = "key"

        nullExn "schema" (fun () -> writer.Add(key, null, DayZero.OldStyle, false))
        onKeyNotSet writer key 0

    [<Fact>]
    let ``Add() throws when the key is already taken`` () =
        let writer = newWriter gregorian

        argExn "key" (fun () -> writer.Add(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false))

    //
    // TryAdd() --- failure
    //

    [<Fact>]
    let ``TryAdd() throws for null key`` () =
        let writer = newEmptyWriter()

        nullExn "key" (fun () -> writer.TryAdd(null, new JulianSchema(), DayZero.OldStyle, false))

    [<Fact>]
    let ``TryAdd() throws for null schema`` () =
        let writer = newEmptyWriter()
        let key = "key"

        nullExn "schema" (fun () -> writer.TryAdd(key, null, DayZero.OldStyle, false))
        onKeyNotSet writer key 0

    [<Fact>]
    let ``TryAdd() when the key is already taken`` () =
        let writer = newWriter gregorian
        let succeed, chr = writer.TryAdd(gregorian.Key, new JulianSchema(), DayZero.OldStyle, false)

        succeed |> nok
        chr     |> isnull
