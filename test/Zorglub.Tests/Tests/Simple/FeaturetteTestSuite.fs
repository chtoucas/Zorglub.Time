// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.FeaturetteTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple
open Zorglub.Time.Simple

module EpagomenalDay =
    type [<Sealed>] ArmenianTests() =
        inherit IEpagomenalCalendarFacts<ArmenianCalendar, StandardArmenian12DataSet>(ArmenianCalendar.Instance)

    type [<Sealed>] CopticTests() =
        inherit IEpagomenalCalendarFacts<CopticCalendar, StandardCoptic12DataSet>(CopticCalendar.Instance)

    type [<Sealed>] EthiopicTests() =
        inherit IEpagomenalCalendarFacts<EthiopicCalendar, StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

    type [<Sealed>] ZoroastrianTests() =
        inherit IEpagomenalCalendarFacts<ZoroastrianCalendar, StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)
