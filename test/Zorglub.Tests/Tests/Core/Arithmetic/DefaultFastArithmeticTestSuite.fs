// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.DefaultFastArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

// FIXME(code): it fails hard...

//let private arOf x = new DefaultFastArithmetic(x) :> ICalendricalArithmetic

//[<Sealed>]
//type GregorianTests() =
//    inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>(), arOf)

