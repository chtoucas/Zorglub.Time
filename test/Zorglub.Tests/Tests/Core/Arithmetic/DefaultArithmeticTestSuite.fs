// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.DefaultArithmeticTestSuite

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

let private ariOf x = new DefaultArithmetic(x) :> ICalendricalArithmetic

[<Sealed>]
[<RedundantTestGroup>]
type Coptic12Tests() =
    inherit CalendricalArithmeticFacts<Coptic12DataSet>(syschemaOf<Coptic12Schema>(), ariOf)

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for Coptic13Schema is DefaultArithmetic`` () =
    schemaOf<Coptic13Schema>().Arithmetic |> is<DefaultArithmetic>

[<Sealed>]
[<RedundantTestGroup>]
type Egyptian12Tests() =
    inherit CalendricalArithmeticFacts<Egyptian12DataSet>(syschemaOf<Egyptian12Schema>(), ariOf)

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for Egyptian13Schema is DefaultArithmetic`` () =
    schemaOf<Egyptian13Schema>().Arithmetic |> is<DefaultArithmetic>

[<Sealed>]
[<RedundantTestGroup>]
type FrenchRepublican12Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican12DataSet>(syschemaOf<FrenchRepublican12Schema>(), ariOf)

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for FrenchRepublican13Schema is DefaultArithmetic`` () =
    schemaOf<FrenchRepublican13Schema>().Arithmetic |> is<DefaultArithmetic>

[<Sealed>]
type GregorianTests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(syschemaOf<GregorianSchema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type InternationalFixedTests() =
    inherit CalendricalArithmeticFacts<InternationalFixedDataSet>(syschemaOf<InternationalFixedSchema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit CalendricalArithmeticFacts<JulianDataSet>(syschemaOf<JulianSchema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type LunisolarTests() =
    inherit CalendricalArithmeticFacts<LunisolarDataSet>(syschemaOf<LunisolarSchema>(), ariOf)

//[<Sealed>]
//[<RedundantTestGroup>]
//type PaxTests() =
//    inherit CalendricalArithmeticFacts<PaxDataSet>(syschemaOf<PaxSchema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type Persian2820Tests() =
    inherit CalendricalArithmeticFacts<Persian2820DataSet>(syschemaOf<Persian2820Schema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type PositivistTests() =
    inherit CalendricalArithmeticFacts<PositivistDataSet>(syschemaOf<PositivistSchema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type TabularIslamicTests() =
    inherit CalendricalArithmeticFacts<TabularIslamicDataSet>(syschemaOf<TabularIslamicSchema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type TropicaliaTests() =
    inherit CalendricalArithmeticFacts<TropicaliaDataSet>(syschemaOf<TropicaliaSchema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type Tropicalia3031Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3031DataSet>(syschemaOf<Tropicalia3031Schema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type Tropicalia3130Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3130DataSet>(syschemaOf<Tropicalia3130Schema>(), ariOf)

[<Sealed>]
[<RedundantTestGroup>]
type WorldTests() =
    inherit CalendricalArithmeticFacts<WorldDataSet>(syschemaOf<WorldSchema>(), ariOf)
