// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateProvidersTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit OrdinalDateProvidersFacts<StandardArmenian12DataSet>(SimpleCalendar.Armenian)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit OrdinalDateProvidersFacts<StandardCoptic12DataSet>(SimpleCalendar.Coptic)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit OrdinalDateProvidersFacts<StandardEthiopic12DataSet>(SimpleCalendar.Ethiopic)

[<Sealed>]
type GregorianTests() =
    inherit OrdinalDateProvidersFacts<ProlepticGregorianDataSet>(SimpleCalendar.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit OrdinalDateProvidersFacts<ProlepticJulianDataSet>(SimpleCalendar.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit OrdinalDateProvidersFacts<StandardTabularIslamicDataSet>(SimpleCalendar.TabularIslamic)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit OrdinalDateProvidersFacts<StandardZoroastrian12DataSet>(SimpleCalendar.Zoroastrian)

//
// User-defined calendars
//

[<Sealed>]
[<RedundantTestBundle>]
type UserGregorianTests() =
    inherit OrdinalDateProvidersFacts<StandardGregorianDataSet>(UserCalendars.Gregorian)

[<Sealed>]
[<RedundantTestBundle>]
type UserJulianTests() =
    inherit OrdinalDateProvidersFacts<ProlepticJulianDataSet>(UserCalendars.Julian)

[<Sealed>]
[<RedundantTestBundle>]
type UserLunisolarTests() =
    inherit OrdinalDateProvidersFacts<StandardLunisolarDataSet>(UserCalendars.Lunisolar)

[<Sealed>]
[<RedundantTestBundle>]
type UserPositivistTests() =
    inherit OrdinalDateProvidersFacts<StandardPositivistDataSet>(UserCalendars.Positivist)


