// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

open Xunit

// TODO(code): WIP. In particular, don't forget to test non-regular calendars (PlainMath).

// RegularMath
[<Sealed>]
type RegularMathTests() =
    inherit CalendarMathFacts<ProlepticGregorianDataSet>(new RegularMath(GregorianCalendar.Instance))

// RegularMath (advanced)
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type RegularMathAdvancedTests() =
    inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new RegularMath(GregorianCalendar.Instance))

// Regular12Math (advanced)
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Regular12MathAdvancedTests() =
    inherit CalendarMathAdvancedFacts<GregorianMathDataSetCutOff>(new Regular12Math(GregorianCalendar.Instance))

// Regular13Math
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Regular13MathTests() =
    inherit CalendarMathFacts<StandardPositivistDataSet>(new Regular13Math(UserCalendars.Positivist))

// PlainMath
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PlainMathTests() =
    inherit PlainMathFacts<ProlepticGregorianDataSet>(new PlainMath(GregorianCalendar.Instance))

//
// Default Math
//

[<Sealed>]
[<RedundantTestBundle>]
type ArmenianTests() =
    inherit CalendarMathFacts<StandardArmenian12DataSet>(ArmenianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type CopticTests() =
    inherit CalendarMathFacts<StandardCoptic12DataSet>(CopticCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type EthiopicTests() =
    inherit CalendarMathFacts<StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

// Regular12Math
[<Sealed>]
type GregorianTests() =
    inherit CalendarMathFacts<ProlepticGregorianDataSet>(GregorianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendarMathFacts<ProlepticJulianDataSet>(JulianCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendarMathFacts<StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

[<Sealed>]
[<RedundantTestBundle>]
type ZoroastrianTests() =
    inherit CalendarMathFacts<StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)
