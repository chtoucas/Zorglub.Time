// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.BoxExtensionsTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Utilities
open Zorglub.Time.Simple

open Xunit

open SimpleCatalogTests.TestCommon

// Sync with SimpleCatalogTests

// Tests setup.
// We MUST initialize the user-defined calendar very early on, otherwise the
// tests checking that we cannot create a calendar with an already taken key
// might fail.
let private userGregorian = UserCalendars.Gregorian

module NoWrite =
    //
    // GetOrCreateCalendar()
    //

    [<Fact>]
    let ``GetOrCreateCalendar() throws for null key`` () =
        let sch = JulianSchema.GetInstance()

        nullExn "key" (fun () -> sch.GetOrCreateCalendar(null, DayZero.OldStyle, false))

    [<Fact>]
    let ``GetOrCreateCalendar() throws for empty schema`` () =
        let key = "key"
        let sch = Box<JulianSchema>.Empty

        argExn "schema" (fun () -> sch.GetOrCreateCalendar(key, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``GetOrCreateCalendar() when the key is a system key`` () =
        let sys = SimpleCalendar.Gregorian
        let sch = JulianSchema.GetInstance()
        let chr = sch.GetOrCreateCalendar(sys.Key, DayZero.OldStyle, false)

        chr ==& sys

    [<Fact>]
    let ``GetOrCreateCalendar() when the key already exists`` () =
        let sch = JulianSchema.GetInstance()
        let chr = sch.GetOrCreateCalendar(userGregorian.Key, DayZero.OldStyle, false)

        chr ==& userGregorian

    //
    // CreateCalendar()
    //

    [<Fact>]
    let ``CreateCalendar() throws for null key`` () =
        let sch = JulianSchema.GetInstance()

        nullExn "key" (fun () -> sch.CreateCalendar(null, DayZero.OldStyle, false))

    [<Fact>]
    let ``CreateCalendar() throws for empty schema`` () =
        let key = "key"
        let sch = Box<JulianSchema>.Empty

        argExn "schema" (fun () -> sch.CreateCalendar(key, DayZero.OldStyle, false))
        onKeyNotSet key

    [<Fact>]
    let ``CreateCalendar() when the key is a system key`` () =
        let sys = SimpleCalendar.Gregorian
        let sch = JulianSchema.GetInstance()

        argExn "key" (fun () -> sch.CreateCalendar(sys.Key, DayZero.OldStyle, false))

    [<Fact>]
    let ``CreateCalendar() when the key already exists`` () =
        let sch = JulianSchema.GetInstance()

        argExn "key" (fun () -> sch.CreateCalendar(userGregorian.Key, DayZero.OldStyle, false))

    //
    // TryCreateCalendar()
    //

    [<Fact>]
    let ``TryCreateCalendar() throws for null key`` () =
        let sch = JulianSchema.GetInstance()

        nullExn "key" (fun () -> sch.TryCreateCalendar(null,DayZero.OldStyle))

    [<Fact>]
    let ``TryCreateCalendar() when the schema is empty`` () =
        let key = "key"
        let sch = Box<JulianSchema>.Empty
        let succeed, chr = sch.TryCreateCalendar(key, DayZero.OldStyle)

        succeed |> nok
        chr     |> isnull
        onKeyNotSet key

    [<Fact>]
    let ``TryCreateCalendar() when the key is a system key`` () =
        let sys = SimpleCalendar.Gregorian
        let sch = JulianSchema.GetInstance()
        let succeed, chr = sch.TryCreateCalendar(sys.Key, DayZero.OldStyle)

        succeed |> nok
        chr     |> isnull

    [<Fact>]
    let ``TryCreateCalendar() when the key already exists`` () =
        let sch = JulianSchema.GetInstance()
        let succeed, chr = sch.TryCreateCalendar(userGregorian.Key, DayZero.OldStyle)

        succeed |> nok
        chr     |> isnull

module Write =
    [<Fact>]
    let ``GetOrCreateCalendar()`` () =
        let key = "BoxExtensionsTests.GetOrCreateCalendar"
        let epoch = DayNumber.Zero + 1234
        let proleptic = false
        let sch = GregorianSchema.GetInstance()
        let chr = sch.GetOrCreateCalendar(key, epoch, proleptic)

        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``CreateCalendar()`` () =
        let key = "BoxExtensionsTests.CreateCalendar"
        let epoch = DayNumber.Zero + 1234
        let proleptic = true
        let sch = GregorianSchema.GetInstance()
        let chr = sch.CreateCalendar(key, epoch, proleptic)

        onKeySet key epoch chr proleptic

    [<Fact>]
    let ``TryCreateCalendar() with default proleptic param`` () =
        let key = "BoxExtensionsTests.TryCreateCalendar+proleptic=false"
        let epoch = DayNumber.Zero + 1234
        let sch = GregorianSchema.GetInstance()
        let succeed, chr = sch.TryCreateCalendar(key, epoch)

        succeed |> ok
        onKeySet key epoch chr false

    [<Fact>]
    let ``TryCreateCalendar()`` () =
        let key = "BoxExtensionsTests.TryCreateCalendar+proleptic=true"
        let epoch = DayNumber.Zero + 1234
        let mutable chr = null
        let proleptic = true
        let sch = GregorianSchema.GetInstance()
        let succeed = sch.TryCreateCalendar(key, epoch, &chr, proleptic)

        succeed |> ok
        onKeySet key epoch chr proleptic
