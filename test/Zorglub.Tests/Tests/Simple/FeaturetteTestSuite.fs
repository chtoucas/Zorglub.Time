// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.FeaturetteTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

let private other = SimpleCalendar.Gregorian

module EpagomenalDay =
    [<Sealed>]
    type ArmenianTests() =
        inherit IEpagomenalCalendarFacts<SimpleArmenian, StandardArmenian12DataSet>(SimpleArmenian.Instance, other)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type CopticTests() =
        inherit IEpagomenalCalendarFacts<SimpleCoptic, StandardCoptic12DataSet>(SimpleCoptic.Instance, other)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EthiopicTests() =
        inherit IEpagomenalCalendarFacts<SimpleEthiopic, StandardEthiopic12DataSet>(SimpleEthiopic.Instance, other)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type ZoroastrianTests() =
        inherit IEpagomenalCalendarFacts<SimpleZoroastrian, StandardZoroastrian12DataSet>(SimpleZoroastrian.Instance, other)
