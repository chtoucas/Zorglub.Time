// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// TODO(code): WIP. In particular, don't forget tests for non-regular calendars (DefaultMath).

[<Sealed>]
type RegularMathTests() =
    inherit CalendarMathFacts<RegularMath, ProlepticGregorianDataSet>(new RegularMath(GregorianCalendar.Instance))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Regular12MathTests() =
    inherit CalendarMathFacts<Regular12Math, ProlepticGregorianDataSet>(new Regular12Math(GregorianCalendar.Instance))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Regular13MathTests() =
    inherit CalendarMathFacts<Regular13Math, StandardPositivistDataSet>(new Regular13Math(UserCalendars.Positivist))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type DefaultMathTests() =
    inherit DefaultMathFacts<ProlepticGregorianDataSet>(new DefaultMath(GregorianCalendar.Instance))
