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

let private ariOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    new PlainArithmetic(sch, sch.SupportedYears)

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for Coptic13Schema is PlainArithmetic`` () =
    SystemArithmetic.CreateDefault(schemaOf<Coptic13Schema>()) |> is<PlainArithmetic>

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit SystemArithmeticFacts<Egyptian12DataSet>(ariOf<Egyptian12Schema>())

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for Egyptian13Schema is PlainArithmetic`` () =
    SystemArithmetic.CreateDefault(schemaOf<Egyptian13Schema>()) |> is<PlainArithmetic>

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican12DataSet>(ariOf<FrenchRepublican12Schema>())

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for FrenchRepublican13Schema is PlainArithmetic`` () =
    SystemArithmetic.CreateDefault(schemaOf<FrenchRepublican13Schema>()) |> is<PlainArithmetic>

[<Sealed>]
type GregorianTests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(ariOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit SystemArithmeticFacts<InternationalFixedDataSet>(ariOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit SystemArithmeticFacts<JulianDataSet>(ariOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit SystemArithmeticFacts<LunisolarDataSet>(ariOf<LunisolarSchema>())

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for PaxSchema is PlainArithmetic`` () =
    SystemArithmetic.CreateDefault(schemaOf<PaxSchema>()) |> is<PlainArithmetic>

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit SystemArithmeticFacts<Persian2820DataSet>(ariOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit SystemArithmeticFacts<PositivistDataSet>(ariOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit SystemArithmeticFacts<TabularIslamicDataSet>(ariOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit SystemArithmeticFacts<TropicaliaDataSet>(ariOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit SystemArithmeticFacts<Tropicalia3031DataSet>(ariOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit SystemArithmeticFacts<Tropicalia3130DataSet>(ariOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit SystemArithmeticFacts<WorldDataSet>(ariOf<WorldSchema>())
