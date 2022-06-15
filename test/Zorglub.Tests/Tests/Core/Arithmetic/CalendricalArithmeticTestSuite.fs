// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.CalendricalArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf x = new CalendricalArithmetic(x) :> ICalendricalArithmetic

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit CalendricalArithmeticFacts<Coptic12DataSet>(syschemaOf<Coptic12Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Coptic13Tests() =
    inherit CalendricalArithmeticFacts<Coptic13DataSet>(syschemaOf<Coptic13Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit CalendricalArithmeticFacts<Egyptian12DataSet>(syschemaOf<Egyptian12Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit CalendricalArithmeticFacts<Egyptian13DataSet>(syschemaOf<Egyptian13Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican12DataSet>(syschemaOf<FrenchRepublican12Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican13DataSet>(syschemaOf<FrenchRepublican13Schema>(), ariOf)

[<Sealed>]
type GregorianTests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(syschemaOf<GregorianSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit CalendricalArithmeticFacts<InternationalFixedDataSet>(syschemaOf<InternationalFixedSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendricalArithmeticFacts<JulianDataSet>(syschemaOf<JulianSchema>(), ariOf)

//[<Sealed>]
//[<RedundantTestBundle>]
//type LunisolarTests() =
//    inherit CalendricalArithmeticFacts<LunisolarDataSet>(syschemaOf<LunisolarSchema>(), ariOf)

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit CalendricalArithmeticFacts<PaxDataSet>(syschemaOf<PaxSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit CalendricalArithmeticFacts<Persian2820DataSet>(syschemaOf<Persian2820Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit CalendricalArithmeticFacts<PositivistDataSet>(syschemaOf<PositivistSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendricalArithmeticFacts<TabularIslamicDataSet>(syschemaOf<TabularIslamicSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit CalendricalArithmeticFacts<TropicaliaDataSet>(syschemaOf<TropicaliaSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3031DataSet>(syschemaOf<Tropicalia3031Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3130DataSet>(syschemaOf<Tropicalia3130Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit CalendricalArithmeticFacts<WorldDataSet>(syschemaOf<WorldSchema>(), ariOf)
