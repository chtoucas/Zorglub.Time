// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.RegularArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf x = new RegularArithmetic(x) :> SystemArithmetic

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(syschemaOf<Coptic12Schema>(), ariOf)

// Coptic13 -> not compatible with RegularArithmetic.

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit SystemArithmeticFacts<Egyptian12DataSet>(syschemaOf<Egyptian12Schema>(), ariOf)

// Egyptian13 -> not compatible with RegularArithmetic.

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican12DataSet>(syschemaOf<FrenchRepublican12Schema>(), ariOf)

// FrenchRepublican13 -> not compatible with RegularArithmetic.

[<Sealed>]
type GregorianTests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(syschemaOf<GregorianSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit SystemArithmeticFacts<InternationalFixedDataSet>(syschemaOf<InternationalFixedSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit SystemArithmeticFacts<JulianDataSet>(syschemaOf<JulianSchema>(), ariOf)

// Lunisolar -> not compatible with RegularArithmetic.

// Pax -> not compatible with RegularArithmetic.

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit SystemArithmeticFacts<Persian2820DataSet>(syschemaOf<Persian2820Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit SystemArithmeticFacts<PositivistDataSet>(syschemaOf<PositivistSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit SystemArithmeticFacts<TabularIslamicDataSet>(syschemaOf<TabularIslamicSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit SystemArithmeticFacts<TropicaliaDataSet>(syschemaOf<TropicaliaSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit SystemArithmeticFacts<Tropicalia3031DataSet>(syschemaOf<Tropicalia3031Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit SystemArithmeticFacts<Tropicalia3130DataSet>(syschemaOf<Tropicalia3130Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit SystemArithmeticFacts<WorldDataSet>(syschemaOf<WorldSchema>(), ariOf)
