// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMonthTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

let other = SimpleGregorian.Instance

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarMonthFacts<StandardArmenian12DataSet>(SimpleArmenian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarMonthFacts<StandardCoptic12DataSet>(SimpleCoptic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarMonthFacts<StandardEthiopic12DataSet>(SimpleEthiopic.Instance, other)

[<Sealed>]
type GregorianTests() =
    inherit CalendarMonthFacts<ProlepticGregorianDataSet>(SimpleGregorian.Instance, SimpleJulian.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarMonthFacts<ProlepticJulianDataSet>(SimpleJulian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarMonthFacts<StandardTabularIslamicDataSet>(SimpleTabularIslamic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarMonthFacts<StandardZoroastrian12DataSet>(SimpleZoroastrian.Instance, other)

//
// User-defined calendars
//

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserGregorianTests() =
//    inherit CalendarMonthFacts<StandardGregorianDataSet>(UserCalendars.Gregorian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserJulianTests() =
//    inherit CalendarMonthFacts<ProlepticJulianDataSet>(UserCalendars.Julian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserLunisolarTests() =
//    inherit CalendarMonthFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserPositivistTests() =
//    inherit CalendarMonthFacts<StandardPositivistDataSet>(UserCalendars.Positivist, other)
