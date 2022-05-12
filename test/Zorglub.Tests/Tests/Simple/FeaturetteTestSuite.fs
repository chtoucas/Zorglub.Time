// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.FeaturetteTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

module EpagomenalDay =
    [<Sealed>]
    type ArmenianTests() =
        inherit IEpagomenalCalendarFacts<ArmenianCalendar, StandardArmenian12DataSet>(ArmenianCalendar.Instance)

    [<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type CopticTests() =
        inherit IEpagomenalCalendarFacts<CopticCalendar, StandardCoptic12DataSet>(CopticCalendar.Instance)

    [<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type EthiopicTests() =
        inherit IEpagomenalCalendarFacts<EthiopicCalendar, StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

    [<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type ZoroastrianTests() =
        inherit IEpagomenalCalendarFacts<ZoroastrianCalendar, StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)
