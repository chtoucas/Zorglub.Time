// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Specialized.GregorianTests

open Zorglub.Testing
open Zorglub.Testing.Data.Unbounded
open Zorglub.Testing.Facts

open Zorglub.Time
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    // NB: notice the use of ProlepticGregorianDataSet.

    let private chr = new GregorianCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<GregorianDate, GregorianCalendar, UnboundedGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d);
        override __.GetDate(y, doy) = new GregorianDate(y, doy);
        override __.GetDate(dayNumber) = new GregorianDate(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DateFacts() =
        inherit IDateFacts<GregorianDate, UnboundedGregorianDataSet>(chr.Domain)

        override __.MinDate = GregorianDate.MinValue
        override __.MaxDate = GregorianDate.MaxValue

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
