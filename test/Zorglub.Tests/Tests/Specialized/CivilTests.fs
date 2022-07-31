// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.CivilTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    // NB: notice the use of StandardGregorianDataSet.

    let private chr = new CivilCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<CivilDate, CivilCalendar, StandardGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override x.GetDate(y, m, d) = new CivilDate(y, m, d);
        override x.GetDate(y, doy) = new CivilDate(y, doy);
        override x.GetDate(dayNumber) = new CivilDate(dayNumber);

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<CivilDate, StandardGregorianDataSet>(chr.SupportedYears.Range, chr.Domain)

        override __.MinDate = CivilDate.MinValue
        override __.MaxDate = CivilDate.MaxValue

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
