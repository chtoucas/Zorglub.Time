// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.DefaultArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

[<Sealed>]
type GregorianTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(new DefaultArithmetic(schemaOf<GregorianSchema>()))
