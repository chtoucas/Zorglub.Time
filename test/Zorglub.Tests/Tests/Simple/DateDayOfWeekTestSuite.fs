// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.DateDayOfWeekTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time.Simple

module CalendarDateCase =
    [<Sealed>]
    type GregorianTests() =
        inherit IDateDayOfWeekFacts<CalendarDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = SimpleCalendar.Gregorian.GetCalendarDate(y, m, d)

module OrdinalDateCase =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianTests() =
        inherit IDateDayOfWeekFacts<OrdinalDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = SimpleCalendar.Gregorian.GetCalendarDate(y, m, d).ToOrdinalDate()

module CalendarDayCase =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type GregorianTests() =
        inherit IDateDayOfWeekFacts<CalendarDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = SimpleCalendar.Gregorian.GetCalendarDate(y, m, d).ToCalendarDay()

