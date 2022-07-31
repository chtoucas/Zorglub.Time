// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.DateAdjustmentTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarDateAdjustmentFacts<StandardArmenian12DataSet>(SimpleArmenian.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarDateAdjustmentFacts<StandardCoptic12DataSet>(SimpleCoptic.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarDateAdjustmentFacts<StandardEthiopic12DataSet>(SimpleEthiopic.Instance)

[<Sealed>]
type GregorianTests() =
    inherit CalendarDateAdjustmentFacts<ProlepticGregorianDataSet>(SimpleGregorian.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarDateAdjustmentFacts<ProlepticJulianDataSet>(SimpleJulian.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarDateAdjustmentFacts<StandardTabularIslamicDataSet>(SimpleTabularIslamic.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarDateAdjustmentFacts<StandardZoroastrian12DataSet>(SimpleZoroastrian.Instance)

//
// User-defined calendars
//

[<Sealed>]
[<RedundantTestBundle>]
type UserGregorianTests() =
    inherit CalendarDateAdjustmentFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit CalendarDateAdjustmentFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit CalendarDateAdjustmentFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit CalendarDateAdjustmentFacts<StandardPositivistDataSet>(UserCalendars.Positivist)
