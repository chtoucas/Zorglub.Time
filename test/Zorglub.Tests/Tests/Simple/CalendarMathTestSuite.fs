// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// TODO(code): WIP. In particular, don't forget to test non-regular calendars (PlainMath).

//
// RegularMath
//

[<Sealed>]
type RegularMathTests() =
    inherit CalendarMathFacts<RegularMath, ProlepticGregorianDataSet>(new RegularMath(GregorianCalendar.Instance))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type RegularMathAdvancedTests() =
    inherit CalendarMathAdvancedFacts<RegularMath, GregorianMathDataSetCutOff>(new RegularMath(GregorianCalendar.Instance))

//
// Regular12Math
//

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Regular12MathTests() =
    inherit CalendarMathFacts<Regular12Math, ProlepticGregorianDataSet>(new Regular12Math(GregorianCalendar.Instance))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Regular12MathAdvancedTests() =
    inherit CalendarMathAdvancedFacts<Regular12Math, GregorianMathDataSetCutOff>(new Regular12Math(GregorianCalendar.Instance))

//
// Regular13Math
//

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Regular13MathTests() =
    inherit CalendarMathFacts<Regular13Math, StandardPositivistDataSet>(new Regular13Math(UserCalendars.Positivist))

//
// PlainMath
//

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PlainMathTests() =
    inherit PlainMathFacts<ProlepticGregorianDataSet>(new PlainMath(GregorianCalendar.Instance))
