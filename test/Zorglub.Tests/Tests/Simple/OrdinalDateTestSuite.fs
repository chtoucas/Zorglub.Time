// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

[<Sealed>]
type ArmenianTests() =
    inherit OrdinalDateFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type CopticTests() =
    inherit OrdinalDateFacts<StandardCoptic12DataSet>(CopticCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type EthiopicTests() =
    inherit OrdinalDateFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type GregorianTests() =
    inherit OrdinalDateFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit OrdinalDateFacts<ProlepticJulianDataSet>(JulianCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type TabularIslamicTests() =
    inherit OrdinalDateFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type ZoroastrianTests() =
    inherit OrdinalDateFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance, GregorianCalendar.Instance)
