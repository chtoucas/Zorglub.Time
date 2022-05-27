// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.GregorianDayTests

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time.Core.Intervals
open Zorglub.Time.Hemerology

module Facts =
    // NB: we use ProlepticGregorianDataSet which has the same limits as GregorianDay.

    let private supportedYears = Range.Create(GregorianDay.MinYear, GregorianDay.MaxYear)

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<GregorianDay, ProlepticGregorianDataSet>(supportedYears, GregorianDay.Domain)

        override __.MinDate = GregorianDay.MinValue
        override __.MaxDate = GregorianDay.MaxValue

        override __.GetDate(y, m, d) = new GregorianDay(y, m, d)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDay(y, m, d)

    [<Sealed>]
    type MathFacts() =
        inherit IDateMathFacts<GregorianDay, ProlepticGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDay(y, m, d)

