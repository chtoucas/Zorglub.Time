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
        inherit IEpagomenalCalendarFacts<ArmenianSimpleCalendar, StandardArmenian12DataSet>(ArmenianSimpleCalendar.Instance, other)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type CopticTests() =
        inherit IEpagomenalCalendarFacts<CopticSimpleCalendar, StandardCoptic12DataSet>(CopticSimpleCalendar.Instance, other)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EthiopicTests() =
        inherit IEpagomenalCalendarFacts<EthiopicSimpleCalendar, StandardEthiopic12DataSet>(EthiopicSimpleCalendar.Instance, other)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type ZoroastrianTests() =
        inherit IEpagomenalCalendarFacts<ZoroastrianSimpleCalendar, StandardZoroastrian12DataSet>(ZoroastrianSimpleCalendar.Instance, other)
