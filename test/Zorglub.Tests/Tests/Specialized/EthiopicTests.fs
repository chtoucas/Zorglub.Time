﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Specialized.Ethiopic12Tests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology
open Zorglub.Testing.Facts.Specialized

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    let private chr = new EthiopicCalendar()

    [<Sealed>]
    [<TestExtrasAssembly>]
    type CalendaTests() =
        inherit ICalendarTFacts<EthiopicDate, EthiopicCalendar, StandardEthiopic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d);
        override __.GetDate(y, doy) = new EthiopicDate(y, doy);
        override __.GetDate(dayNumber) = EthiopicDate.FromDayNumber(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateFacts() =
        inherit IDateFacts<EthiopicDate, EthiopicCalendar, StandardEthiopic12DataSet>(chr)

        override __.MinDate = EthiopicDate.MinValue
        override __.MaxDate = EthiopicDate.MaxValue

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = EthiopicDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = EthiopicDate.Adjuster |> isnotnull

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<EthiopicDate, StandardEthiopic12DataSet>(new EthiopicAdjuster())

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)
        override __.GetDate(y, doy) = new EthiopicDate(y, doy)

    [<Sealed>]
    [<TestExtrasAssembly>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<EthiopicDate, StandardEthiopic12DataSet>()

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)

module Bundles13 =
    let private chr = new Ethiopic13Calendar()

    [<Sealed>]
    [<TestExtrasAssembly>]
    type CalendaTests() =
        inherit ICalendarTFacts<Ethiopic13Date, Ethiopic13Calendar, StandardEthiopic13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d);
        override __.GetDate(y, doy) = new Ethiopic13Date(y, doy);
        override __.GetDate(dayNumber) = Ethiopic13Date.FromDayNumber(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 13

        [<Fact>]
        member x.VirtualMonth_Prop() = x.CalendarUT.VirtualMonth === 13

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateFacts() =
        inherit IDateFacts<Ethiopic13Date, Ethiopic13Calendar, StandardEthiopic13DataSet>(chr)

        override __.MinDate = Ethiopic13Date.MinValue
        override __.MaxDate = Ethiopic13Date.MaxValue

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Ethiopic13Date.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = Ethiopic13Date.Adjuster |> isnotnull

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<Ethiopic13Date, StandardEthiopic13DataSet>(new Ethiopic13Adjuster())

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d)
        override __.GetDate(y, doy) = new Ethiopic13Date(y, doy)

    [<Sealed>]
    [<TestExtrasAssembly>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Ethiopic13Date, StandardEthiopic13DataSet>()

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d)
