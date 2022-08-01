// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMonthTestSuite

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
    inherit CalendarMonthFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarMonthFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarMonthFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic, other)

[<Sealed>]
type GregorianTests() =
    inherit CalendarMonthFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian, SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarMonthFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarMonthFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarMonthFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian, other)

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
