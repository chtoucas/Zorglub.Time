// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.CivilDayTests

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time.Core.Intervals
open Zorglub.Time.Hemerology

module Facts =
    // NB: we use StandardGregorianDataSet which has the same limits as CivilDay.

    let private supportedYears = Range.Create(CivilDay.MinYear, CivilDay.MaxYear)

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<CivilDay, StandardGregorianDataSet>(supportedYears, CivilDay.Domain)

        override __.MinDate = CivilDay.MinValue
        override __.MaxDate = CivilDay.MaxValue

        override __.GetDate(y, m, d) = new CivilDay(y, m, d)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDay, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDay(y, m, d)

    [<Sealed>]
    type MathFacts() =
        inherit IDateMathFacts<CivilDay, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDay(y, m, d)
