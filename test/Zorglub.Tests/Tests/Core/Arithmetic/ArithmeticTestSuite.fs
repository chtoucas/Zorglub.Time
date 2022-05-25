// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.ArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

// Right now, we can only test Solar12Arithmetic with the Gregorian schema; we
// don't have test data for any other schemas with profile Solar12.
[<Sealed>]
type Solar12ArithmeticTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(new Solar12Arithmetic(schemaOf<GregorianSchema>()))

//
// Arithmetic automatically selected by a schema
//

// GregorianArithmetic
[<Sealed>]
type GregorianTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>())
