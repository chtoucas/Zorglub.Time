// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.BasicArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf x = new BasicArithmetic(x) :> ICalendricalArithmetic

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit CalendricalArithmeticFacts<Coptic12DataSet>(schemaOf<Coptic12Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Coptic13Tests() =
    inherit CalendricalArithmeticFacts<Coptic13DataSet>(schemaOf<Coptic13Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit CalendricalArithmeticFacts<Egyptian12DataSet>(schemaOf<Egyptian12Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit CalendricalArithmeticFacts<Egyptian13DataSet>(schemaOf<Egyptian13Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican12DataSet>(schemaOf<FrenchRepublican12Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit CalendricalArithmeticFacts<FrenchRepublican13DataSet>(schemaOf<FrenchRepublican13Schema>(), ariOf)

[<Sealed>]
type GregorianTests() =
    inherit CalendricalArithmeticFacts<GregorianDataSet>(schemaOf<GregorianSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit CalendricalArithmeticFacts<InternationalFixedDataSet>(schemaOf<InternationalFixedSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit CalendricalArithmeticFacts<JulianDataSet>(schemaOf<JulianSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit CalendricalArithmeticFacts<LunisolarDataSet>(schemaOf<LunisolarSchema>(), ariOf)

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit CalendricalArithmeticFacts<PaxDataSet>(schemaOf<PaxSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit CalendricalArithmeticFacts<Persian2820DataSet>(schemaOf<Persian2820Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit CalendricalArithmeticFacts<PositivistDataSet>(schemaOf<PositivistSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit CalendricalArithmeticFacts<TabularIslamicDataSet>(schemaOf<TabularIslamicSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit CalendricalArithmeticFacts<TropicaliaDataSet>(schemaOf<TropicaliaSchema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3031DataSet>(schemaOf<Tropicalia3031Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit CalendricalArithmeticFacts<Tropicalia3130DataSet>(schemaOf<Tropicalia3130Schema>(), ariOf)

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit CalendricalArithmeticFacts<WorldDataSet>(schemaOf<WorldSchema>(), ariOf)
