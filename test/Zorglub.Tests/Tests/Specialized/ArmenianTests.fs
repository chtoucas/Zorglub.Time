// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.ArmenianTests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    let private chr = new ArmenianCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<ArmenianDate, ArmenianCalendar, StandardArmenian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override x.GetDate(y, m, d) = new ArmenianDate(y, m, d);
        override x.GetDate(y, doy) = new ArmenianDate(y, doy);
        override x.GetDate(dayNumber) = new ArmenianDate(dayNumber);

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<ArmenianDate, StandardArmenian12DataSet>(chr.SupportedYears.Range, chr.Domain)

        override __.MinDate = ArmenianDate.MinValue
        override __.MaxDate = ArmenianDate.MaxValue

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)

