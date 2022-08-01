// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarYearTestSuite

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
    inherit CalendarYearFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarYearFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarYearFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic, other)

[<Sealed>]
type GregorianTests() =
    inherit CalendarYearFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian, SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarYearFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarYearFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarYearFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian, other)

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
