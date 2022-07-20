// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.ZCatalogWriterTests

open System
open System.Collections.Concurrent

open Zorglub.Testing
open Zorglub.Time.Hemerology

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
    let ``Constructor throws for invalid startId = Int32.MaxValue`` () =
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()
        let calendarsById = Array.zeroCreate<ZCalendar>(1)

        outOfRangeExn "startId" (fun () -> new ZCatalogWriter(calendarsByKey, calendarsById, Int32.MaxValue))

    [<Fact>]
    let ``Constructor`` () =
        let startId = 128
        let count = 255
        let calendarsByKey = new ConcurrentDictionary<string, Lazy<ZCalendar>>()
        let calendarsById = Array.zeroCreate<ZCalendar>(count)

        let writer = new ZCatalogWriter(calendarsByKey, calendarsById, startId)

        writer.CalendarsByKey ==& calendarsByKey
        writer.CalendarsById  ==& calendarsById
        writer.StartId        === startId
        writer.MaxId          === count - 1
