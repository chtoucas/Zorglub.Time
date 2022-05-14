// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDateTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

[<Sealed>]
type ArmenianTests() =
    inherit CalendarDateFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance, GregorianCalendar.Instance)

[<RedundantTesting>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type CopticTests() =
    inherit CalendarDateFacts<StandardCoptic12DataSet>(CopticCalendar.Instance, GregorianCalendar.Instance)

[<RedundantTesting>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type EthiopicTests() =
    inherit CalendarDateFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance, GregorianCalendar.Instance)

[<RedundantTesting>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit CalendarDateFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)

[<RedundantTesting>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type JulianTests() =
    inherit CalendarDateFacts<ProlepticJulianDataSet>(JulianCalendar.Instance, GregorianCalendar.Instance)

[<RedundantTesting>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit CalendarDateFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance, GregorianCalendar.Instance)

[<RedundantTesting>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type ZoroastrianTests() =
    inherit CalendarDateFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance, GregorianCalendar.Instance)
