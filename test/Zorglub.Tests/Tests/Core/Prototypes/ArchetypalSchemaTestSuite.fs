// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Schemas.ArchetypalSchemaTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Prototypes
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Utilities

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

/// Creates a new instance of the schema prototype of type 'a.
let private archetypeOf<'a when 'a : not struct and 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    new ArchetypalSchema(sch, sch.MinDaysInYear, sch.MinDaysInMonth)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Coptic12Tests() =
    inherit PrototypalSchemaFacts<Coptic12DataSet>(archetypeOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Coptic13Tests() =
    inherit PrototypalSchemaFacts<Coptic13DataSet>(archetypeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Egyptian12Tests() =
    inherit PrototypalSchemaFacts<Egyptian12DataSet>(archetypeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Egyptian13Tests() =
    inherit PrototypalSchemaFacts<Egyptian13DataSet>(archetypeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type FrenchRepublican12Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican12DataSet>(archetypeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type FrenchRepublican13Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican13DataSet>(archetypeOf<FrenchRepublican13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type GregorianTests() =
    inherit PrototypalSchemaFacts<GregorianDataSet>(archetypeOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type InternationalFixedTests() =
    inherit PrototypalSchemaFacts<InternationalFixedDataSet>(archetypeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type JulianTests() =
    inherit PrototypalSchemaFacts<JulianDataSet>(archetypeOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type LunisolarTests() =
    inherit PrototypalSchemaFacts<LunisolarDataSet>(archetypeOf<LunisolarSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type PaxTests() =
    inherit PrototypalSchemaFacts<PaxDataSet>(archetypeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Persian2820Tests() =
    inherit PrototypalSchemaFacts<Persian2820DataSet>(archetypeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type PositivistTests() =
    inherit PrototypalSchemaFacts<PositivistDataSet>(archetypeOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type TabularIslamicTests() =
    inherit PrototypalSchemaFacts<TabularIslamicDataSet>(archetypeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type TropicaliaTests() =
    inherit PrototypalSchemaFacts<TropicaliaDataSet>(archetypeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Tropicalia3031Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3031DataSet>(archetypeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Tropicalia3130Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3130DataSet>(archetypeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type WorldTests() =
    inherit PrototypalSchemaFacts<WorldDataSet>(archetypeOf<WorldSchema>())
