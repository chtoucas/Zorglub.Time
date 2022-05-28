// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.ArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

// Generic engines.
let private plainOf x       = new CalendricalArithmetic(x) :> ICalendricalArithmetic
let private defaultOf x     = new DefaultArithmetic(x)     :> ICalendricalArithmetic

// Specialized engines.
let private defaultFastOf x = new DefaultFastArithmetic(x) :> ICalendricalArithmetic
//let private lunarOf x       = new LunarArithmetic(x)       :> ICalendricalArithmetic
//let private lunisolarOf x   = new LunisolarArithmetic(x)   :> ICalendricalArithmetic
let private solar12Of x     = new Solar12Arithmetic(x)     :> ICalendricalArithmetic
//let private solar13Of x     = new Solar13Arithmetic(x)     :> ICalendricalArithmetic

// Coptic12Tests           -> Solar12Arithmetic
// Coptic13Tests           -> DefaultArithmetic
// Egyptian12Tests         -> Solar12Arithmetic
// Egyptian13Tests         -> DefaultArithmetic
// FrenchRepublican12Tests -> Solar12Arithmetic
// FrenchRepublican13Tests -> DefaultArithmetic
// GregorianTests          -> GregorianArithmetic
// InternationalFixedTests -> Solar13Arithmetic
// JulianTests             -> Solar12Arithmetic
// LunisolarTests          -> LunisolarArithmetic
// PaxTests                -> DefaultFastArithmetic
// Persian2820Tests        -> Solar12Arithmetic
// PositivistTests         -> Solar13Arithmetic
// TabularIslamicTests     -> LunarArithmetic
// TropicaliaTests         -> Solar12Arithmetic
// Tropicalia3031Tests     -> Solar12Arithmetic
// Tropicalia3130Tests     -> Solar12Arithmetic
// WorldTests              -> Solar12Arithmetic

module Coptic13Case =
    // DefaultArithmetic
    [<Sealed>]
    type Coptic13Tests() =
        inherit CalendricalArithmeticFacts<Coptic13DataSet>(syschemaOf<Coptic13Schema>())

        member x.Arithmetic() = x.Arithmetic |> is<DefaultArithmetic>

    // CalendricalArithmetic
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type CalendricalTests() =
        inherit CalendricalArithmeticFacts<Coptic13DataSet>(schemaOf<Coptic13Schema>(), plainOf)

module GregorianCase =
    // GregorianArithmetic
    [<Sealed>]
    type GregorianTests() =
        inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>())

        member x.Arithmetic() = x.Arithmetic |> is<GregorianArithmetic>

    // Solar12Arithmetic
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type Solar12Tests() =
        inherit CalendricalArithmeticFacts<GregorianDataSet>(syschemaOf<GregorianSchema>(), solar12Of)

    // CalendricalArithmetic
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type CalendricalTests() =
        inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>(), plainOf)

    // DefaultArithmetic
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DefaultTests() =
        inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>(), defaultOf)

    // DefaultFastArithmetic
    //[<Sealed>]
    //[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    //type DefaultFastTests() =
    //    inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>(), defaultFastOf)

module PositivistCase =
    // Solar13Arithmetic
    [<Sealed>]
    type Coptic13Tests() =
        inherit CalendricalArithmeticFacts<PositivistDataSet>(syschemaOf<PositivistSchema>())

        member x.Arithmetic() = x.Arithmetic |> is<Solar13Arithmetic>

    // CalendricalArithmetic
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type CalendricalTests() =
        inherit CalendricalArithmeticFacts<PositivistDataSet>(schemaOf<PositivistSchema>(), plainOf)

    // DefaultArithmetic
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type DefaultTests() =
        inherit CalendricalArithmeticFacts<PositivistDataSet>(schemaOf<PositivistSchema>(), defaultOf)

    // DefaultFastArithmetic
    //[<Sealed>]
    //[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    //type DefaultFastTests() =
    //    inherit CalendricalArithmeticFacts<PositivistDataSet>(schemaOf<PositivistSchema>(), defaultFastOf)

