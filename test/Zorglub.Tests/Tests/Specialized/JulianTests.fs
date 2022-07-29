// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.JulianTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    // NB: notice the use of ProlepticJulianDataSet.

    let private chr = new JulianCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<JulianDate, JulianCalendar, ProlepticJulianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override x.GetDate(y, m, d) = new JulianDate(y, m, d);
        override x.GetDate(y, doy) = new JulianDate(y, doy);
        override x.GetDate(dayNumber) = new JulianDate(dayNumber);

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<JulianDate, ProlepticJulianDataSet>(chr.SupportedYears.Range, chr.Domain)

        override __.MinDate = JulianDate.MinValue
        override __.MaxDate = JulianDate.MaxValue

        override __.GetDate(y, m, d) = new JulianDate(y, m, d)
