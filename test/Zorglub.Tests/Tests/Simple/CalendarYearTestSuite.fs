// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarYearTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

[<Sealed>]
type ArmenianTests() =
    inherit CalendarYearFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type CopticTests() =
    inherit CalendarYearFacts<StandardCoptic12DataSet>(CopticCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type EthiopicTests() =
    inherit CalendarYearFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type GregorianTests() =
    inherit CalendarYearFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit CalendarYearFacts<ProlepticJulianDataSet>(JulianCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type TabularIslamicTests() =
    inherit CalendarYearFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type ZoroastrianTests() =
    inherit CalendarYearFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance, GregorianCalendar.Instance)


