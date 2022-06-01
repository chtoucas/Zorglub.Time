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
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit OrdinalDateAdjustmentFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit OrdinalDateAdjustmentFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit OrdinalDateAdjustmentFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

[<Sealed>]
type GregorianTests() =
    inherit OrdinalDateAdjustmentFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit OrdinalDateAdjustmentFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit OrdinalDateAdjustmentFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit OrdinalDateAdjustmentFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

