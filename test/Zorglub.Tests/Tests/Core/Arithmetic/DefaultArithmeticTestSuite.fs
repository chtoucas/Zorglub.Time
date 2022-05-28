// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.DefaultArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

let private arOf x = new DefaultArithmetic(x) :> ICalendricalArithmetic

[<Sealed>]
type Coptic13Tests() =
    inherit CalendricalArithmeticFacts<Coptic13DataSet>(syschemaOf<Coptic13Schema>(), arOf)

[<Sealed>]
[<RedundantTestGroup>]
type GregorianTests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>(), arOf)
