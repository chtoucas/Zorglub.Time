// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Simple.CalendarDayTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

let private other = SimpleCalendar.Gregorian

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarDayFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian, other)

[<Sealed>]
[<RedundantTestBundle>]
type CivilTests() =
    inherit CalendarDayFacts<StandardGregorianDataSet>(SimpleCalendar.Civil, other)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarDayFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic, other)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarDayFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic, other)

[<Sealed>]
type GregorianTests() =
    inherit CalendarDayFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian, SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarDayFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian, other)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarDayFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic, other)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarDayFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian, other)

//
// User-defined calendars
//

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserGregorianTests() =
//    inherit CalendarDayFacts<StandardGregorianDataSet>(UserCalendars.Gregorian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserJulianTests() =
//    inherit CalendarDayFacts<ProlepticJulianDataSet>(UserCalendars.Julian, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserLunisolarTests() =
//    inherit CalendarDayFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar, other)

//[<Sealed>]
//[<RedundantTestBundle>]
//type UserPositivistTests() =
//    inherit CalendarDayFacts<StandardPositivistDataSet>(UserCalendars.Positivist, other)
