// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarYearTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

let other = GregorianCalendar.Instance

// Since the Gregorian calendar has the richest dataset, we use it as a default
// model for testing.

[<Sealed>]
[<RedundantTestGroup>]
type ArmenianTests() =
    inherit CalendarYearFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance, other)

[<Sealed>]
[<RedundantTestGroup>]
type CopticTests() =
    inherit CalendarYearFacts<StandardCoptic12DataSet>(CopticCalendar.Instance, other)

[<Sealed>]
[<RedundantTestGroup>]
type EthiopicTests() =
    inherit CalendarYearFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance, other)

[<Sealed>]
type GregorianTests() =
    inherit CalendarYearFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit CalendarYearFacts<ProlepticJulianDataSet>(JulianCalendar.Instance, other)

[<Sealed>]
[<RedundantTestGroup>]
type TabularIslamicTests() =
    inherit CalendarYearFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance, other)

[<Sealed>]
[<RedundantTestGroup>]
type ZoroastrianTests() =
    inherit CalendarYearFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance, other)


