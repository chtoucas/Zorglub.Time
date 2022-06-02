// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.WideDateTests

open Zorglub.Testing
open Zorglub.Testing.Data.Unbounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time.Hemerology

module FactSuite =
    // NB: notice the use of UnboundedGregorianDataSet.

    let private other = WideCalendar.Julian

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit WideDateFacts<UnboundedGregorianDataSet>(WideCalendar.Gregorian, other)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type AdjustableDateFacts() =
        inherit IAdjustableDateFacts<WideDate, UnboundedGregorianDataSet>(WideCalendar.Gregorian.SupportedYears)

        override __.GetDate(y, m, d) = WideCalendar.Gregorian.GetWideDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<WideDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = WideCalendar.Gregorian.GetWideDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MathFacts() =
        inherit IDateArithmeticFacts<WideDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = WideCalendar.Gregorian.GetWideDate(y, m, d)
