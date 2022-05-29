// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.ArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

// Solar12Arithmetic is not the default arithmetic for the Gregorian schema, but
// we still use it because it's the schema has a the most data to offer.
let private solar12Of x = new Solar12Arithmetic(x) :> ICalendricalArithmetic
[<Sealed>]
type Solar12Tests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(syschemaOf<GregorianSchema>(), solar12Of)

    member x.Arithmetic() = x.Arithmetic |> is<Solar12Arithmetic>

//
// Normal test suite.
//

[<Sealed>]
[<RedundantTestGroup>]
type Coptic12Tests() =
    inherit CalendricalArithmeticFacts<Coptic12DataSet>(syschemaOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestGroup>]
type Coptic13Tests() =
    inherit CalendricalArithmeticFacts<Coptic13DataSet>(syschemaOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestGroup>]
type Egyptian12Tests() =
    inherit CalendricalArithmeticFacts<Egyptian12DataSet>(syschemaOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestGroup>]
type Egyptian13Tests() =
    inherit CalendricalArithmeticFacts<Egyptian13DataSet>(syschemaOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestGroup>]
type FrenchRepublican12Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican12DataSet>(syschemaOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestGroup>]
type FrenchRepublican13Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican13DataSet>(syschemaOf<FrenchRepublican13Schema>())

// GregorianArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<GregorianArithmetic>

[<Sealed>]
[<RedundantTestGroup>]
type InternationalFixedTests() =
    inherit CalendricalArithmeticFacts<InternationalFixedDataSet>(syschemaOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestGroup>]
type JulianTests() =
    inherit CalendricalArithmeticFacts<JulianDataSet>(syschemaOf<JulianSchema>())

// LunisolarArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit CalendricalArithmeticFacts<LunisolarDataSet>(syschemaOf<LunisolarSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunisolarArithmetic>

//[<Sealed>]
//[<RedundantTestGroup>]
//type PaxTests() =
//    inherit CalendricalArithmeticFacts<PaxDataSet>(syschemaOf<PaxSchema>())

[<Sealed>]
[<RedundantTestGroup>]
type Persian2820Tests() =
    inherit CalendricalArithmeticFacts<Persian2820DataSet>(syschemaOf<Persian2820Schema>())

// Solar13Arithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PositivistTests() =
    inherit CalendricalArithmeticFacts<PositivistDataSet>(syschemaOf<PositivistSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<Solar13Arithmetic>

// LunarArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit CalendricalArithmeticFacts<TabularIslamicDataSet>(syschemaOf<TabularIslamicSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunarArithmetic>

[<Sealed>]
[<RedundantTestGroup>]
type TropicaliaTests() =
    inherit CalendricalArithmeticFacts<TropicaliaDataSet>(syschemaOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestGroup>]
type Tropicalia3031Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3031DataSet>(syschemaOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestGroup>]
type Tropicalia3130Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3130DataSet>(syschemaOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestGroup>]
type WorldTests() =
    inherit CalendricalArithmeticFacts<WorldDataSet>(syschemaOf<WorldSchema>())
