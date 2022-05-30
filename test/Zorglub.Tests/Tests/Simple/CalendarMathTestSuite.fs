// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMathTestSuite

open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts.Simple

open Zorglub.Time.Simple

// TODO(code): WIP.

[<Sealed>]
type RegularMathTests() =
    inherit CalendarMathFacts<RegularMath, ProlepticGregorianDataSet>(new RegularMath(GregorianCalendar.Instance))

[<Sealed>]
type Regular12MathTests() =
    inherit CalendarMathFacts<Regular12Math, ProlepticGregorianDataSet>(new Regular12Math(GregorianCalendar.Instance))
