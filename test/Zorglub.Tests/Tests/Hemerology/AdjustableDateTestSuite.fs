// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.AdjustableDateTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time.Core.Intervals
open Zorglub.Time.Hemerology

[<Sealed>]
type CivilDateTests() =
    inherit IAdjustableDateFacts<CivilDate, StandardGregorianDataSet>(
        Range.Create(CivilDate.MinYear, CivilDate.MaxYear))

    override __.CreateDate(y, m, d) = new CivilDate(y, m, d)

[<Sealed>]
type GregorianWideDateTests() =
    inherit IAdjustableDateFacts<WideDate, ProlepticGregorianDataSet>(WideCalendar.Gregorian.SupportedYears)

    override __.CreateDate(y, m, d) = WideCalendar.Gregorian.GetWideDate(y, m, d)
