// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.GregorianTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    // NB: notice the use of ProlepticGregorianDataSet.

    let private chr = new GregorianSystem()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<GregorianDate, GregorianSystem, ProlepticGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override x.GetDate(y, m, d) = new GregorianDate(y, m, d);
        override x.GetDate(y, doy) = new GregorianDate(y, doy);
        override x.GetDate(dayNumber) = new GregorianDate(dayNumber);

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<GregorianDate, ProlepticGregorianDataSet>(chr.SupportedYears.Range, chr.Domain)

        override __.MinDate = GregorianDate.MinValue
        override __.MaxDate = GregorianDate.MaxValue

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
