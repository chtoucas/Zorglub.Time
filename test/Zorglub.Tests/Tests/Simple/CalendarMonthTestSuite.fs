// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMonthTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

[<Sealed>]
type ArmenianTests() =
    inherit CalendarMonthFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type CopticTests() =
    inherit CalendarMonthFacts<StandardCoptic12DataSet>(CopticCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type EthiopicTests() =
    inherit CalendarMonthFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type GregorianTests() =
    inherit CalendarMonthFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance, JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit CalendarMonthFacts<ProlepticJulianDataSet>(JulianCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type TabularIslamicTests() =
    inherit CalendarMonthFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance, GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestGroup>]
type ZoroastrianTests() =
    inherit CalendarMonthFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance, GregorianCalendar.Instance)

