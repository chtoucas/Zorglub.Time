// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDateProvidersTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarDateProvidersFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarDateProvidersFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarDateProvidersFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic)

[<Sealed>]
type GregorianTests() =
    inherit CalendarDateProvidersFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarDateProvidersFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarDateProvidersFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarDateProvidersFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian)

//
// User-defined calendars
//

[<Sealed>]
[<RedundantTestBundle>]
type UserGregorianTests() =
    inherit CalendarDateProvidersFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit CalendarDateProvidersFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit CalendarDateProvidersFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit CalendarDateProvidersFacts<StandardPositivistDataSet>(UserCalendars.Positivist)
