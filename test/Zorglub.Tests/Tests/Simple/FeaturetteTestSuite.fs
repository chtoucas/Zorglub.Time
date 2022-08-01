// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.FeaturetteTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

let private other = SimpleCalendar.Gregorian

module EpagomenalDay =
    [<Sealed>]
    type ZoroastrianTests() =
        inherit IEpagomenalCalendarFacts<ZoroastrianSimpleCalendar, StandardZoroastrian12DataSet>(ZoroastrianSimpleCalendar.Instance, other)
