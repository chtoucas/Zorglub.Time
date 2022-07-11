// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.BasicArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf<'a when 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = schemaOf<'a>()
    let seg = CalendricalSegment.CreateMaximal(sch)
    new BasicArithmetic(seg)

[<Sealed>]
[<RedundantTestBundle>]
type Coptic12Tests() =
    inherit ICalendricalArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Coptic13Tests() =
    inherit ICalendricalArithmeticFacts<Coptic13DataSet>(ariOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit ICalendricalArithmeticFacts<Egyptian12DataSet>(ariOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit ICalendricalArithmeticFacts<Egyptian13DataSet>(ariOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit ICalendricalArithmeticFacts<FrenchRepublican12DataSet>(ariOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit ICalendricalArithmeticFacts<FrenchRepublican13DataSet>(ariOf<FrenchRepublican13Schema>())

[<Sealed>]
type GregorianTests() =
    inherit ICalendricalArithmeticFacts<GregorianDataSet>(ariOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit ICalendricalArithmeticFacts<InternationalFixedDataSet>(ariOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit ICalendricalArithmeticFacts<JulianDataSet>(ariOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit ICalendricalArithmeticFacts<LunisolarDataSet>(ariOf<LunisolarSchema>())

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit ICalendricalArithmeticFacts<PaxDataSet>(ariOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit ICalendricalArithmeticFacts<Persian2820DataSet>(ariOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit ICalendricalArithmeticFacts<PositivistDataSet>(ariOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit ICalendricalArithmeticFacts<TabularIslamicDataSet>(ariOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit ICalendricalArithmeticFacts<TropicaliaDataSet>(ariOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit ICalendricalArithmeticFacts<Tropicalia3031DataSet>(ariOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit ICalendricalArithmeticFacts<Tropicalia3130DataSet>(ariOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit ICalendricalArithmeticFacts<WorldDataSet>(ariOf<WorldSchema>())
