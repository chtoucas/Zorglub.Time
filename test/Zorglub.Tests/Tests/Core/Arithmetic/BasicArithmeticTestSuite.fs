// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.BasicArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
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
    inherit PartsArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Coptic13Tests() =
    inherit PartsArithmeticFacts<Coptic13DataSet>(ariOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit PartsArithmeticFacts<Egyptian12DataSet>(ariOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit PartsArithmeticFacts<Egyptian13DataSet>(ariOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit PartsArithmeticFacts<FrenchRepublican12DataSet>(ariOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit PartsArithmeticFacts<FrenchRepublican13DataSet>(ariOf<FrenchRepublican13Schema>())

[<Sealed>]
type GregorianTests() =
    inherit PartsArithmeticFacts<GregorianDataSet>(ariOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit PartsArithmeticFacts<InternationalFixedDataSet>(ariOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit PartsArithmeticFacts<JulianDataSet>(ariOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit PartsArithmeticFacts<LunisolarDataSet>(ariOf<LunisolarSchema>())

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit PartsArithmeticFacts<PaxDataSet>(ariOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit PartsArithmeticFacts<Persian2820DataSet>(ariOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit PartsArithmeticFacts<PositivistDataSet>(ariOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit PartsArithmeticFacts<TabularIslamicDataSet>(ariOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit PartsArithmeticFacts<TropicaliaDataSet>(ariOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit PartsArithmeticFacts<Tropicalia3031DataSet>(ariOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit PartsArithmeticFacts<Tropicalia3130DataSet>(ariOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit PartsArithmeticFacts<WorldDataSet>(ariOf<WorldSchema>())
