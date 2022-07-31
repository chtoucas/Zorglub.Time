// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTestSuite

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
    inherit OrdinalDateFacts<StandardArmenian12DataSet>(SimpleArmenian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit OrdinalDateFacts<StandardCoptic12DataSet>(SimpleCoptic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit OrdinalDateFacts<StandardEthiopic12DataSet>(SimpleEthiopic.Instance, other)

[<Sealed>]
type GregorianTests() =
    inherit OrdinalDateFacts<ProlepticGregorianDataSet>(SimpleGregorian.Instance, SimpleJulian.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit OrdinalDateFacts<ProlepticJulianDataSet>(SimpleJulian.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit OrdinalDateFacts<StandardTabularIslamicDataSet>(SimpleTabularIslamic.Instance, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit OrdinalDateFacts<StandardZoroastrian12DataSet>(SimpleZoroastrian.Instance, other)

//
// User-defined calendars
//

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserGregorianTests() =
//    inherit OrdinalDateFacts<StandardGregorianDataSet>(UserCalendars.Gregorian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserJulianTests() =
//    inherit OrdinalDateFacts<ProlepticJulianDataSet>(UserCalendars.Julian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserLunisolarTests() =
//    inherit OrdinalDateFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserPositivistTests() =
//    inherit OrdinalDateFacts<StandardPositivistDataSet>(UserCalendars.Positivist, other)
