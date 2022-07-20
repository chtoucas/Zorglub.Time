// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.ZCatalogWriterTests

open System
open System.Collections.Concurrent

open Zorglub.Testing
open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws for null calendarsByKey`` () =
        let calendarsById = Array.zeroCreate<ZCalendar>(1)

        nullExn "calendarsByKey" (fun () -> new ZCatalogWriter(null, calendarsById, 1))

    [<Fact>]
    let ``Constructor throws for null calendarsById`` () =
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()

        nullExn "calendarsById" (fun () -> new ZCatalogWriter(calendarsByKey, null, 1))

    [<Fact>]
    let ``Constructor throws when minUserId > maxId`` () =
        let count = 256
        let maxId = count - 1
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()
        let calendarsById = Array.zeroCreate<ZCalendar>(count)

        outOfRangeExn "minUserId" (fun () -> new ZCatalogWriter(calendarsByKey, calendarsById, maxId + 1))

    [<Fact>]
    let ``Constructor does not throw when minUserId = maxId`` () =
        let count = 256
        let maxId = count - 1
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()
        let calendarsById = Array.zeroCreate<ZCalendar>(count)

        new ZCatalogWriter(calendarsByKey, calendarsById, maxId) |> ignore

    [<Fact>]
    let ``Constructor`` () =
        let count = 256
        let maxId = count - 1
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()
        let calendarsById = Array.zeroCreate<ZCalendar>(count)
        let minUserId = 128

        let writer = new ZCatalogWriter(calendarsByKey, calendarsById, minUserId)

        writer.MinUserId === minUserId
        writer.MaxId     === maxId
        writer.CountUserCalendars() === 0

module Write =
    let private emptyWriter  =
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()
        let calendarsById = Array.zeroCreate<ZCalendar>(256)
        new ZCatalogWriter(calendarsByKey, calendarsById, 0)

    //let private newWriter  =
    //    let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()
    //    let calendarsById = Array.zeroCreate<ZCalendar>(256)
    //    new ZCatalogWriter(calendarsByKey, calendarsById, 0)

    let onKeyNotSet (writer: ZCatalogWriter) key =
        Assert.DoesNotContain(key, writer.Keys)
        //keyNotFoundExn (() -> writer.GetCalendar(key))

    let private scope = new StandardScope(new GregorianSchema(), DayZero.NewStyle)

    [<Fact>]
    let ``Add() throws when key is null`` () =
        let writer = emptyWriter

        // FIXME(api): "name" vs "key".
        nullExn "name" (fun () -> writer.Add(null, scope))

        writer.CountUserCalendars() === 0