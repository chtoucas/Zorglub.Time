// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.DateAdjustmentTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarDateAdjustmentFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarDateAdjustmentFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarDateAdjustmentFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

[<Sealed>]
type GregorianTests() =
    inherit CalendarDateAdjustmentFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarDateAdjustmentFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarDateAdjustmentFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarDateAdjustmentFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)
