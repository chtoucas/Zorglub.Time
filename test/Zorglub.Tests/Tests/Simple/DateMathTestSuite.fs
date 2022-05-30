// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.DateMathTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

module CalendarDateCase =
    [<Sealed>]
    type GregorianTests() =
        inherit IDateMathFacts<CalendarDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d)

module OrdinalDateCase =
    [<Sealed>]
    type GregorianTests() =
        inherit IDateOrdinalMathFacts<OrdinalDate, ProlepticGregorianDataSet>()

        override __.GetDate(y, doy) = GregorianCalendar.Instance.GetOrdinalDate(y, doy)

    //[<Sealed>]
    //type GregorianIndirectTests() =
    //    inherit IDateMathFacts<OrdinalDate, ProlepticGregorianDataSet>()

    //    override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToOrdinalDate()

    // Years math operations.
    [<Sealed>]
    type GregorianMoreTests() =
        inherit OrdinalDateMathFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

module CalendarDayCase =
    [<Sealed>]
    type GregorianTests() =
        inherit IDateMathFacts<CalendarDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = GregorianCalendar.Instance.GetCalendarDate(y, m, d).ToCalendarDay()
