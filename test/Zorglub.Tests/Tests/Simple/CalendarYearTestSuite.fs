// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarYearTestSuite

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
    inherit CalendarYearFacts<StandardArmenian12DataSet>(SimpleArmenian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarYearFacts<StandardCoptic12DataSet>(SimpleCoptic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarYearFacts<StandardEthiopic12DataSet>(SimpleEthiopic.Instance, other)

[<Sealed>]
type GregorianTests() =
    inherit CalendarYearFacts<ProlepticGregorianDataSet>(SimpleGregorian.Instance, SimpleJulian.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarYearFacts<ProlepticJulianDataSet>(SimpleJulian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarYearFacts<StandardTabularIslamicDataSet>(SimpleTabularIslamic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarYearFacts<StandardZoroastrian12DataSet>(SimpleZoroastrian.Instance, other)

//
// User-defined calendars
//

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserGregorianTests() =
//    inherit CalendarYearFacts<StandardGregorianDataSet>(UserCalendars.Gregorian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserJulianTests() =
//    inherit CalendarYearFacts<ProlepticJulianDataSet>(UserCalendars.Julian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserLunisolarTests() =
//    inherit CalendarYearFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserPositivistTests() =
//    inherit CalendarYearFacts<StandardPositivistDataSet>(UserCalendars.Positivist, other)
