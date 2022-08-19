// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

let other = SimpleCalendar.Gregorian

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit OrdinalDateFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian, other)

[<Sealed>]
[<RedundantTestBundle>]
type CivilTests() =
    inherit OrdinalDateFacts<StandardGregorianDataSet>(SimpleCalendar.Civil, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit OrdinalDateFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit OrdinalDateFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic, other)

[<Sealed>]
type GregorianTests() =
    inherit OrdinalDateFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian, SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit OrdinalDateFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit OrdinalDateFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit OrdinalDateFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian, other)

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
