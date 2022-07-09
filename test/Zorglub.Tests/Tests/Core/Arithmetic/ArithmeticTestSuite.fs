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

let private ariOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    SystemArithmetic.CreateDefault(sch, sch.SupportedYears)

// Solar12Arithmetic is not the default arithmetic for the Gregorian schema, but
// we still use it because it's the schema has a the most data to offer.
let private solar12Of<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    new Solar12Arithmetic(sch, sch.SupportedYears)
[<Sealed>]
type Solar12Tests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(solar12Of<GregorianSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<Solar12Arithmetic>

//
// Normal test suite.
//

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Coptic13Tests() =
    inherit SystemArithmeticFacts<Coptic13DataSet>(ariOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit SystemArithmeticFacts<Egyptian12DataSet>(ariOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit SystemArithmeticFacts<Egyptian13DataSet>(ariOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican12DataSet>(ariOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican13DataSet>(ariOf<FrenchRepublican13Schema>())

// GregorianArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(ariOf<GregorianSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<GregorianArithmetic>

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit SystemArithmeticFacts<InternationalFixedDataSet>(ariOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit SystemArithmeticFacts<JulianDataSet>(ariOf<JulianSchema>())

// LunisolarArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit SystemArithmeticFacts<LunisolarDataSet>(ariOf<LunisolarSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunisolarArithmetic>

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit SystemArithmeticFacts<PaxDataSet>(syschemaOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit SystemArithmeticFacts<Persian2820DataSet>(ariOf<Persian2820Schema>())

// Solar13Arithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PositivistTests() =
    inherit SystemArithmeticFacts<PositivistDataSet>(ariOf<PositivistSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<Solar13Arithmetic>

// LunarArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit SystemArithmeticFacts<TabularIslamicDataSet>(ariOf<TabularIslamicSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunarArithmetic>

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
