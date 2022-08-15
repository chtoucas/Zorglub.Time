// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDateAdjusterTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarDateAdjusterFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarDateAdjusterFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarDateAdjusterFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic)

[<Sealed>]
type GregorianTests() =
    inherit CalendarDateAdjusterFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarDateAdjusterFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarDateAdjusterFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarDateAdjusterFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian)

//
// User-defined calendars
//

[<Sealed>]
[<RedundantTestBundle>]
type UserGregorianTests() =
    inherit CalendarDateAdjusterFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit CalendarDateAdjusterFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit CalendarDateAdjusterFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit CalendarDateAdjusterFacts<StandardPositivistDataSet>(UserCalendars.Positivist)
