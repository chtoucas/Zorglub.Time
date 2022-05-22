// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.DateDayOfWeekTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time.Simple

module CalendarDateCase =
    [<Sealed>]
    type GregorianTests() =
        inherit IDateDayOfWeekFacts<CalendarDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d)

module OrdinalDateCase =
    [<Sealed>]
    type GregorianTests() =
        inherit IDateDayOfWeekFacts<OrdinalDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToOrdinalDate()

module CalendarDayCase =
    [<Sealed>]
    type GregorianTests() =
        inherit IDateDayOfWeekFacts<CalendarDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToCalendarDay()

