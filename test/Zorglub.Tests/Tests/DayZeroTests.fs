// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.DayZeroTests

open System

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Simple
open Zorglub.Time.Specialized

open Xunit

[<Fact>]
let ``Static property NewStyle`` () =
    let date = SimpleCalendar.Gregorian.GetDate(DayZero.NewStyle)
    let y, m, d = date.Deconstruct()

    DayZero.NewStyle === DayNumber.Zero
    DayZero.NewStyle.Ordinal === Ord.First
    DayZero.NewStyle.DayOfWeek === DayOfWeek.Monday
    (y, m, d) === (1, 1, 1)
    date.YearOfEra === Ord.First

[<Fact>]
let ``Static property OldStyle`` () =
    let date = SimpleCalendar.Julian.GetDate(DayZero.OldStyle)
    let y, m, d = date.Deconstruct()

    DayZero.OldStyle === DayNumber.Zero - 2
    DayZero.OldStyle.Ordinal === Ord.First - 2
    DayZero.OldStyle.DayOfWeek === DayOfWeek.Saturday
    (y, m, d) === (1, 1, 1)
    date.YearOfEra === Ord.First

[<Fact>]
let ``Static property RataDie`` () =
    DayZero.RataDie === DayNumber.Zero - 1
    DayZero.RataDie.Ordinal === Ord.First - 1

[<Fact>]
let ``Static property Gregorian`` () =
    DayZero.Gregorian === DayZero.NewStyle

[<Fact>]
let ``Static property Julian`` () =
    DayZero.Julian === DayZero.OldStyle

// See D.&R., p.15.

[<Fact>]
let ``Static property Armenian`` () =
    DayZero.Armenian === DayZero.NewStyle + 201_442

[<Fact>]
let ``Static property Coptic`` () =
    DayZero.Coptic === DayZero.NewStyle + 103_604

[<Fact>]
let ``Static property Egyptian`` () =
    DayZero.Egyptian === DayZero.NewStyle - 272_788

[<Fact>]
let ``Static property Ethiopic`` () =
    DayZero.Ethiopic === DayZero.NewStyle + 2795

[<Fact>]
let ``Static property FrenchRepublican`` () =
    DayZero.FrenchRepublican === DayZero.NewStyle + 654_414

[<Fact>]
let ``Static property Holocene`` () =
    DayZero.Holocene === DayZero.NewStyle - 3_652_425

[<Fact>]
let ``Static property Persian`` () =
    DayZero.Persian === DayZero.NewStyle + 226_895

[<Fact>]
let ``Static property Positivist`` () =
    DayZero.Positivist === DayZero.NewStyle + 653_054

[<Fact>]
let ``Static property SundayBeforeGregorian`` () =
    DayZero.SundayBeforeGregorian === DayZero.NewStyle - 1
    DayZero.SundayBeforeGregorian.DayOfWeek = DayOfWeek.Sunday

[<Fact>]
let ``Static property TabularIslamic`` () =
    DayZero.TabularIslamic === DayZero.NewStyle + 227_014

[<Fact>]
let ``Static property Tropicalia`` () =
    let epoch = DayZero.Tropicalia
    let date = new CivilDate(epoch)
    let chr = new CivilCalendar()
    let startOfYear = chr.GetStartOfYear(date.Year)

    date === startOfYear
    epoch.DayOfWeek === date.DayOfWeek

[<Fact>]
let ``Static property Zoroastrian`` () =
    DayZero.Zoroastrian === DayZero.NewStyle + 230_637
