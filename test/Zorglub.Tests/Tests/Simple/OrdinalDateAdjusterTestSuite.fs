// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateAdjusterTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit OrdinalDateAdjusterFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit OrdinalDateAdjusterFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit OrdinalDateAdjusterFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic)

[<Sealed>]
type GregorianTests() =
    inherit OrdinalDateAdjusterFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit OrdinalDateAdjusterFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit OrdinalDateAdjusterFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit OrdinalDateAdjusterFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian)

//
// User-defined calendars
//

[<Sealed>]
[<RedundantTestBundle>]
type UserGregorianTests() =
    inherit OrdinalDateAdjusterFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit OrdinalDateAdjusterFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit OrdinalDateAdjusterFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit OrdinalDateAdjusterFacts<StandardPositivistDataSet>(UserCalendars.Positivist)
