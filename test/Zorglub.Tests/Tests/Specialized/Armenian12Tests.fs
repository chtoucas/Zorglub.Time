// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.Armenian12Tests

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

module Bundles =
    let private chr = new Armenian12Calendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<Armenian12Date, Armenian12Calendar, StandardArmenian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override x.GetDate(y, m, d) = new Armenian12Date(y, m, d);
        override x.GetDate(y, doy) = new Armenian12Date(y, doy);
        override x.GetDate(dayNumber) = new Armenian12Date(dayNumber);

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Armenian12Date, StandardArmenian12DataSet>(chr.Domain)

        override __.MinDate = Armenian12Date.MinValue
        override __.MaxDate = Armenian12Date.MaxValue

        override __.GetDate(y, m, d) = new Armenian12Date(y, m, d)
