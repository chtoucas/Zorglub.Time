// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.PrototypalSchemaTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

/// Creates a new instance of the schema archetype of type 'a.
let private prototypeOf<'a when 'a : not struct and 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    new SchemaPrototype(sch)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Coptic12Tests() =
    inherit PrototypalSchemaFacts<Coptic12DataSet>(prototypeOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Coptic13Tests() =
    inherit PrototypalSchemaFacts<Coptic13DataSet>(prototypeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Egyptian12Tests() =
    inherit PrototypalSchemaFacts<Egyptian12DataSet>(prototypeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Egyptian13Tests() =
    inherit PrototypalSchemaFacts<Egyptian13DataSet>(prototypeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type FrenchRepublican12Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican12DataSet>(prototypeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type FrenchRepublican13Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican13DataSet>(prototypeOf<FrenchRepublican13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type GregorianTests() =
    inherit PrototypalSchemaFacts<GregorianDataSet>(prototypeOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type InternationalFixedTests() =
    inherit PrototypalSchemaFacts<InternationalFixedDataSet>(prototypeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type JulianTests() =
    inherit PrototypalSchemaFacts<JulianDataSet>(prototypeOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type LunisolarTests() =
    inherit PrototypalSchemaFacts<LunisolarDataSet>(prototypeOf<LunisolarSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type PaxTests() =
    inherit PrototypalSchemaFacts<PaxDataSet>(prototypeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Persian2820Tests() =
    inherit PrototypalSchemaFacts<Persian2820DataSet>(prototypeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type PositivistTests() =
    inherit PrototypalSchemaFacts<PositivistDataSet>(prototypeOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type TabularIslamicTests() =
    inherit PrototypalSchemaFacts<TabularIslamicDataSet>(prototypeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type TropicaliaTests() =
    inherit PrototypalSchemaFacts<TropicaliaDataSet>(prototypeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Tropicalia3031Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3031DataSet>(prototypeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Tropicalia3130Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3130DataSet>(prototypeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type WorldTests() =
    inherit PrototypalSchemaFacts<WorldDataSet>(prototypeOf<WorldSchema>())
