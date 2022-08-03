// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.ArmenianTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    let private chr = new ArmenianCalendar()

    [<Sealed>]
    [<TestExtrasAssembly>]
    type CalendaTests() =
        inherit ICalendarTFacts<ArmenianDate, ArmenianCalendar, StandardArmenian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d);
        override __.GetDate(y, doy) = new ArmenianDate(y, doy);
        override __.GetDate(dayNumber) = new ArmenianDate(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateFacts() =
        inherit ISpecializedDateFacts<ArmenianDate, ArmenianCalendar, StandardArmenian12DataSet>(chr)

        override __.MinDate = ArmenianDate.MinValue
        override __.MaxDate = ArmenianDate.MaxValue

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = ArmenianDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjusters_Prop() = ArmenianDate.Adjusters |> isnotnull

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateAdjustersFacts() =
        inherit IDateAdjustersFacts<ArmenianDate, StandardArmenian12DataSet>(new ArmenianAdjusters())

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)

    [<Sealed>]
    [<TestExtrasAssembly>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ArmenianDate, StandardArmenian12DataSet>()

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)


module Bundles13 =
    let private chr = new Armenian13Calendar()

    [<Sealed>]
    [<TestExtrasAssembly>]
    type CalendaTests() =
        inherit ICalendarTFacts<Armenian13Date, Armenian13Calendar, StandardArmenian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d);
        override __.GetDate(y, doy) = new Armenian13Date(y, doy);
        override __.GetDate(dayNumber) = new Armenian13Date(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 13

        [<Fact>]
        member x.VirtualMonth_Prop() = x.CalendarUT.VirtualMonth === 13

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateFacts() =
        inherit ISpecializedDateFacts<Armenian13Date, Armenian13Calendar, StandardArmenian13DataSet>(chr)

        override __.MinDate = Armenian13Date.MinValue
        override __.MaxDate = Armenian13Date.MaxValue

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Armenian13Date.Calendar |> isnotnull

        [<Fact>]
        static member Adjusters_Prop() = Armenian13Date.Adjusters |> isnotnull

    [<Sealed>]
    [<TestExtrasAssembly>]
    type DateAdjustersFacts() =
        inherit IDateAdjustersFacts<Armenian13Date, StandardArmenian13DataSet>(new Armenian13Adjusters())

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)

    [<Sealed>]
    [<TestExtrasAssembly>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Armenian13Date, StandardArmenian13DataSet>()

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)
