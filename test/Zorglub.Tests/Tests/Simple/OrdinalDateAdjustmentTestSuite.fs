// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateAdjustmentTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

[<Sealed>]
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
[<RedundantTestGroup>]
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

