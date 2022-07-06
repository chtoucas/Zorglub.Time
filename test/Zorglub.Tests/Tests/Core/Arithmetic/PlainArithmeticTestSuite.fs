// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.PlainArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

open Xunit

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf x = new PlainArithmetic(x) :> SystemArithmetic

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(syschemaOf<Coptic12Schema>(), ariOf)

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for Coptic13Schema is PlainArithmetic`` () =
    schemaOf<Coptic13Schema>().Arithmetic |> is<PlainArithmetic>

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit SystemArithmeticFacts<Egyptian12DataSet>(syschemaOf<Egyptian12Schema>(), ariOf)

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for Egyptian13Schema is PlainArithmetic`` () =
    schemaOf<Egyptian13Schema>().Arithmetic |> is<PlainArithmetic>

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican12DataSet>(syschemaOf<FrenchRepublican12Schema>(), ariOf)

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for FrenchRepublican13Schema is PlainArithmetic`` () =
    schemaOf<FrenchRepublican13Schema>().Arithmetic |> is<PlainArithmetic>

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

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit SystemArithmeticFacts<LunisolarDataSet>(syschemaOf<LunisolarSchema>(), ariOf)

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for PaxSchema is PlainArithmetic`` () =
    schemaOf<PaxSchema>().Arithmetic |> is<PlainArithmetic>

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
