// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateAdjustmentTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestGroup>]
type ArmenianTests() =
    inherit OrdinalDateAdjustmentFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type CopticTests() =
    inherit OrdinalDateAdjustmentFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type EthiopicTests() =
    inherit OrdinalDateAdjustmentFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

[<Sealed>]
type GregorianTests() =
    inherit OrdinalDateAdjustmentFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit OrdinalDateAdjustmentFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type TabularIslamicTests() =
    inherit OrdinalDateAdjustmentFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type ZoroastrianTests() =
    inherit OrdinalDateAdjustmentFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

