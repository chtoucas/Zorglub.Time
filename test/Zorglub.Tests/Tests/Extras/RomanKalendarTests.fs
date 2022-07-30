// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Time.Extras.RomanKalendarTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time.Extras
open Zorglub.Time.Specialized

open Xunit

let epiphanyData = RomanKalendarDataSet.EpiphanyData
let easterData = RomanKalendarDataSet.EasterData
let paschalMoonData = RomanKalendarDataSet.PaschalMoonData

[<Theory; MemberData(nameof(epiphanyData))>]
let ``Property EpiphanySunday`` y d =
    let epiphanySunday = new GregorianDate(y, 1, d)
    let kalendar = new RomanKalendar(y)

    epiphanySunday.DayOfWeek === DayOfWeek.Sunday
    kalendar.EpiphanySunday  === epiphanySunday

[<Theory; MemberData(nameof(easterData))>]
let ``Property Easter`` y m d =
    let easter = new GregorianDate(y, m, d)
    let kalendar = new RomanKalendar(y)

    easter.DayOfWeek === DayOfWeek.Sunday
    kalendar.Easter  === easter

[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<Theory(Skip = "D&R data does not match our definition of the Paschal Moon?"); MemberData(nameof(paschalMoonData))>]
let ``Property PaschalMoon`` y m d =
    let moon = new GregorianDate(y, m, d)
    let kalendar = new RomanKalendar(y)

    kalendar.PaschalMoon === moon
