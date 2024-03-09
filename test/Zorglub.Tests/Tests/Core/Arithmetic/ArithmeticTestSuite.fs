// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.ArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Utilities

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

let private ariOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    let seg = SystemSegment.Create(sch, sch.SupportedYears)
    SystemArithmetic.CreateDefault(seg)

// Solar12SystemArithmetic is not the default arithmetic for the Gregorian schema, but
// we still use it because it's the schema has a the most data to offer.
let private solar12Of<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    let seg = SystemSegment.Create(sch, sch.SupportedYears)
    new Solar12SystemArithmetic(seg)
[<Sealed>]
type Solar12Tests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(solar12Of<GregorianSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<Solar12SystemArithmetic>

//
// Normal test suite.
//

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

// PlainSystemArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic13Tests() =
    inherit SystemArithmeticFacts<Coptic13DataSet>(ariOf<Coptic13Schema>())

    member x.Arithmetic() = x.Arithmetic |> is<PlainSystemArithmetic>

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

// GregorianSystemArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(ariOf<GregorianSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<GregorianSystemArithmetic>

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit SystemArithmeticFacts<InternationalFixedDataSet>(ariOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit SystemArithmeticFacts<JulianDataSet>(ariOf<JulianSchema>())

// LunisolarSystemArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit SystemArithmeticFacts<LunisolarDataSet>(ariOf<LunisolarSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunisolarSystemArithmetic>

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit SystemArithmeticFacts<PaxDataSet>(ariOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit SystemArithmeticFacts<Persian2820DataSet>(ariOf<Persian2820Schema>())

// Solar13SystemArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PositivistTests() =
    inherit SystemArithmeticFacts<PositivistDataSet>(ariOf<PositivistSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<Solar13SystemArithmetic>

// LunarSystemArithmetic
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit SystemArithmeticFacts<TabularIslamicDataSet>(ariOf<TabularIslamicSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunarSystemArithmetic>

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
