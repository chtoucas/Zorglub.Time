// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.MinMaxYearCalendarTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes
open Zorglub.Time.Specialized

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "name" is null`` () =
        let scope = new StandardScope(new GregorianSchema(), DayZero.NewStyle)

        nullExn "name" (fun () -> new MinMaxYearBasicCalendar(null, scope))
        nullExn "name" (fun () -> new GregorianMinMaxYearCalendar(null, scope))

    [<Fact>]
    let ``Constructor throws when "scope" is null`` () =
        nullExn "scope" (fun () -> new MinMaxYearBasicCalendar("Name", (null: CalendarScope)))
        nullExn "scope" (fun () -> new MinMaxYearBasicCalendar("Name", (null: MinMaxYearScope)))
        nullExn "scope" (fun () -> new GregorianMinMaxYearCalendar("Name", (null: CalendarScope)))
        nullExn "scope" (fun () -> new GregorianMinMaxYearCalendar("Name", (null: MinMaxYearScope)))

    [<Fact>]
    let ``Constructor throws when "scope" is not complete`` () =
        let min = new DateParts(1, 1, 2)
        let scope = BoundedBelowScope.StartingAt(new GregorianSchema(), DayZero.NewStyle, min)

        scope.IsComplete |> nok

        argExn "scope" (fun () -> new MinMaxYearBasicCalendar("Name", scope))
        argExn "scope" (fun () -> new GregorianMinMaxYearCalendar("Name", scope))

module Bundles =
    // WARNING: we MUST use the same scope as the one defined in GregorianCalendar.
    let private scope = MinMaxYearScope.CreateMaximal(new GregorianSchema(), DayZero.NewStyle)
    let private chr = new GregorianMinMaxYearCalendar("Gregorian", scope)

    // This bundle is necessary in order to test the base method GetDate(int daysSinceEpoch).
    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<GregorianDate, MinMaxYearCalendar<GregorianDate>, StandardGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d);
        override __.GetDate(y, doy) = new GregorianDate(y, doy);
        override __.GetDate(dayNumber) = new GregorianDate(dayNumber);
