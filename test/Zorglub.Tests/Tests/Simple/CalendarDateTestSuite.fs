// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDateTestSuite

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
    inherit CalendarDateFacts<StandardArmenian12DataSet>(SimpleArmenian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarDateFacts<StandardCoptic12DataSet>(SimpleCoptic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarDateFacts<StandardEthiopic12DataSet>(SimpleEthiopic.Instance, other)

[<Sealed>]
type GregorianTests() =
    inherit CalendarDateFacts<ProlepticGregorianDataSet>(SimpleGregorian.Instance, SimpleJulian.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarDateFacts<ProlepticJulianDataSet>(SimpleJulian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarDateFacts<StandardTabularIslamicDataSet>(SimpleTabularIslamic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarDateFacts<StandardZoroastrian12DataSet>(SimpleZoroastrian.Instance, other)

//
// User-defined calendars
//

// TODO(fact): test bundle does not work with user-defined calendars.
// Idem with the other date types.

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserGregorianTests() =
//    inherit CalendarDateFacts<StandardGregorianDataSet>(UserCalendars.Gregorian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserJulianTests() =
//    inherit CalendarDateFacts<ProlepticJulianDataSet>(UserCalendars.Julian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserLunisolarTests() =
//    inherit CalendarDateFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserPositivistTests() =
//    inherit CalendarDateFacts<StandardPositivistDataSet>(UserCalendars.Positivist, other)
