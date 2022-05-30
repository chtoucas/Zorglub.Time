// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.DefaultFastArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

open Xunit

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf x = new DefaultFastArithmetic(x) :> ICalendricalArithmetic

[<Sealed>]
[<RedundantTestGroup>]
type Coptic12Tests() =
    inherit CalendricalArithmeticFacts<Coptic12DataSet>(syschemaOf<Coptic12Schema>(), ariOf)

// Coptic13 -> not compatible with DefaultFastArithmetic.

[<Sealed>]
[<RedundantTestGroup>]
type Egyptian12Tests() =
    inherit CalendricalArithmeticFacts<Egyptian12DataSet>(syschemaOf<Egyptian12Schema>(), ariOf)

// Egyptian13 -> not compatible with DefaultFastArithmetic.

[<Sealed>]
[<RedundantTestGroup>]
type FrenchRepublican12Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican12DataSet>(syschemaOf<FrenchRepublican12Schema>(), ariOf)

// FrenchRepublican13 -> not compatible with DefaultFastArithmetic.

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

// Already tested in ArithmeticTestSuite.
[<Fact>]
let ``Default arithmetic for PaxSchema is DefaultFastArithmetic`` () =
    schemaOf<PaxSchema>().Arithmetic |> is<DefaultFastArithmetic>

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
