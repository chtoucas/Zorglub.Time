// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.WorldTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology
open Zorglub.Testing.Facts.Specialized

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

let private chr = new WorldCalendar()

module Methods =
    let dateInfoData = WorldDataSet.Instance.DateInfoData
    let moreMonthInfoData = WorldDataSet.MoreMonthInfoData

    [<TestExtrasAssembly>]
    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Property IsBlank`` (info: DateInfo) =
        let (y, m, d) = info.Yemoda.Deconstruct()
        let date = new WorldDate(y, m, d)

        date.IsBlank === date.IsSupplementary

    [<TestExtrasAssembly>]
    [<Theory; MemberData(nameof(moreMonthInfoData))>]
    let ``CountDaysInWorldMonth()`` (info: YemoAnd<int>) =
        let (y, m, daysInMonth) = info.Deconstruct()

        chr.CountDaysInWorldMonth(y, m) === daysInMonth

module Bundles =
    // NB: notice the use of ProlepticJulianDataSet.

    let dateInfoData = WorldDataSet.Instance.DateInfoData
    let moreMonthInfoData = WorldDataSet.MoreMonthInfoData

    [<Sealed>]
    [<TestExtrasAssembly>]
    type CalendaTests() =
        inherit ICalendarTFacts<WorldDate, WorldCalendar, StandardWorldDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new WorldDate(y, m, d);
        override __.GetDate(y, doy) = new WorldDate(y, doy);
        override __.GetDate(dayNumber) = new WorldDate(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateFacts() =
        inherit IDateFacts<WorldDate, WorldCalendar, StandardWorldDataSet>(chr)

        override __.MinDate = WorldDate.MinValue
        override __.MaxDate = WorldDate.MaxValue

        override __.GetDate(y, m, d) = new WorldDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = WorldDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = WorldDate.Adjuster |> isnotnull

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<WorldDate, StandardWorldDataSet>(new WorldAdjuster())

        override __.GetDate(y, m, d) = new WorldDate(y, m, d)
        override __.GetDate(y, doy) = new WorldDate(y, doy)

