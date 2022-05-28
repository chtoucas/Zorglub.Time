// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.ArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

// GregorianArithmetic
[<Sealed>]
type GregorianTests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>())

// Solar12Arithmetic
// Right now, we can only test Solar12Arithmetic with the Gregorian schema; we
// don't have test data for any other schemas with profile Solar12.

let private sysOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    SchemaActivator.CreateInstance<'a>()

let private arOf x = new DefaultArithmetic(x) :> ICalendricalArithmetic

[<Sealed>]
type Solar12ArithmeticTests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(sysOf<GregorianSchema>(), arOf)
