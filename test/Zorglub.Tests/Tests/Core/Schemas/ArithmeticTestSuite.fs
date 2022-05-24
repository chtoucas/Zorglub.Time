// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.ArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

[<Sealed>]
type CalendricalArithmeticTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(new CalendricalArithmetic(schemaOf<GregorianSchema>()))

[<Sealed>]
type DefaultArithmeticTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(new DefaultArithmetic(schemaOf<GregorianSchema>()))

//[<Sealed>]
//type DefaultFastArithmeticTests() =
//    inherit ICalendricalArithmeticFacts<GregorianDataSet>(new DefaultFastArithmetic(schemaOf<GregorianSchema>()))

[<Sealed>]
type GregorianArithmeticTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(new GregorianArithmetic(schemaOf<GregorianSchema>()))

[<Sealed>]
type Solar12ArithmeticTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(new Solar12Arithmetic(schemaOf<GregorianSchema>()))

//
// Arithmetic paired with a schema
//

[<Sealed>]
type GregorianTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>())
