// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.CalendarEpochTests

open System

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Hemerology
open Zorglub.Time.Specialized

open Xunit

// See D.&R., p.15.

[<Fact>]
let ``Static property Armenian`` () =
    CalendarEpoch.Armenian === DayZero.NewStyle + 201_442

[<Fact>]
let ``Static property Coptic`` () =
    CalendarEpoch.Coptic === DayZero.NewStyle + 103_604

[<Fact>]
let ``Static property Egyptian`` () =
    CalendarEpoch.Egyptian === DayZero.NewStyle - 272_788

[<Fact>]
let ``Static property Ethiopic`` () =
    CalendarEpoch.Ethiopic === DayZero.NewStyle + 2795

[<Fact>]
let ``Static property FrenchRepublican`` () =
    CalendarEpoch.FrenchRepublican === DayZero.NewStyle + 654_414

[<Fact>]
let ``Static property SundayBeforeGregorian`` () =
    CalendarEpoch.SundayBeforeGregorian === DayZero.NewStyle - 1
    CalendarEpoch.SundayBeforeGregorian.DayOfWeek = DayOfWeek.Sunday

[<Fact>]
let ``Static property Persian`` () =
    CalendarEpoch.Persian === DayZero.NewStyle + 226_895

[<Fact>]
let ``Static property Positivist`` () =
    CalendarEpoch.Positivist === DayZero.NewStyle + 653_054

[<Fact>]
let ``Static property TabularIslamic`` () =
    CalendarEpoch.TabularIslamic === DayZero.NewStyle + 227_014

[<Fact>]
let ``Static property Zoroastrian`` () =
    CalendarEpoch.Zoroastrian === DayZero.NewStyle + 230_637

module More =
    [<Fact>]
    let ``Static property Gregorian`` () =
        CalendarEpoch2.Gregorian === DayZero.NewStyle

    [<Fact>]
    let ``Static property Holocene`` () =
        CalendarEpoch2.Holocene === DayZero.NewStyle - 3_652_425

    [<Fact>]
    let ``Static property Julian`` () =
        CalendarEpoch2.Julian === DayZero.OldStyle

    [<Fact>]
    let ``Static property Tropicalia`` () =
        let epoch = CalendarEpoch2.Tropicalia
        let date = new CivilDate(epoch)
        let chr = new CivilSystem()
        let startOfYear = chr.GetStartOfYear(date.Year)

        date === startOfYear
        epoch.DayOfWeek === date.DayOfWeek
