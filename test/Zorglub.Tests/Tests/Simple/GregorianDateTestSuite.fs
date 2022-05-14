// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.GregorianDateTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

[<Sealed>]
type GregorianTests() =
    inherit IAdjustableDateFacts<CalendarDate, ProlepticGregorianDataSet>(GregorianCalendar.Instance.SupportedYears)

    override __.CreateDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d)

module CalendarDateCase =
    [<Sealed>]
    type AdvancedMathTests() =
        inherit CalendarDateMathFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

    [<Sealed>]
    type DayOfWeekTests() =
        inherit IDateDayOfWeekFacts<CalendarDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d)

    [<Sealed>]
    type MathTests() =
        inherit IDateMathFacts<CalendarDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d)

module OrdinalDateCase =
    [<Sealed>]
    type DayOfWeekTests() =
        inherit IDateDayOfWeekFacts<OrdinalDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToOrdinalDate()

    [<Sealed>]
    type MathTests() =
        inherit IDateMathFacts<OrdinalDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToOrdinalDate()

module CalendarDayCase =
    [<Sealed>]
    type DayOfWeekTests() =
        inherit IDateDayOfWeekFacts<CalendarDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToCalendarDay()

    [<Sealed>]
    type MathTests() =
        inherit IDateMathFacts<CalendarDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToCalendarDay()
