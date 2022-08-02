// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.MinMaxYearCalendarTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Specialized

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "name" is null`` () =
        let name: string = null
        let scope = new StandardScope(new GregorianSchema(), DayZero.NewStyle)

        nullExn "name" (fun () -> new MinMaxYearCalendar(name, scope))

    [<Fact>]
    let ``Constructor throws when "scope" is null`` () =
        let scope: CalendarScope = null

        nullExn "scope" (fun () -> new MinMaxYearCalendar("Name", scope))

    [<Fact(Skip = "To be fixed")>]
    let ``Constructor throws when "scope" is not complete`` () =
        let min = new DateParts(2, 1, 1)
        let scope = new BoundedBelowScope(new GregorianSchema(), DayZero.NewStyle, min, 2)

        scope.IsComplete |> nok

        argExn "scope" (fun () -> new MinMaxYearCalendar("Name", scope))


module Bundles =
    let private scope = new StandardScope(new Coptic12Schema(), CalendarEpoch.Coptic)
    let private chr = new MinMaxYearCalendar<CopticDate>("Coptic", scope)

    // This bundle is necessary in order to test the base method GetDate(int daysSinceEpoch).
    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<CopticDate, MinMaxYearCalendar<CopticDate>, StandardCoptic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new CopticDate(y, m, d);
        override __.GetDate(y, doy) = new CopticDate(y, doy);
        override __.GetDate(dayNumber) = new CopticDate(dayNumber);

