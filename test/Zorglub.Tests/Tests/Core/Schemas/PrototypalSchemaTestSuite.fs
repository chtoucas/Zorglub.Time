// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.PrototypalSchemaTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

/// Creates a new instance of the schema prototype of type 'a.
let private prototypeOf<'a when 'a : not struct and 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    new PrototypalSchema(sch, sch.MinDaysInYear, sch.MinDaysInMonth)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Coptic12Tests() =
    inherit ArchetypalSchemaFacts<Coptic12DataSet>(prototypeOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Coptic13Tests() =
    inherit ArchetypalSchemaFacts<Coptic13DataSet>(prototypeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Egyptian12Tests() =
    inherit ArchetypalSchemaFacts<Egyptian12DataSet>(prototypeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Egyptian13Tests() =
    inherit ArchetypalSchemaFacts<Egyptian13DataSet>(prototypeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type FrenchRepublican12Tests() =
    inherit ArchetypalSchemaFacts<FrenchRepublican12DataSet>(prototypeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type FrenchRepublican13Tests() =
    inherit ArchetypalSchemaFacts<FrenchRepublican13DataSet>(prototypeOf<FrenchRepublican13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type GregorianTests() =
    inherit ArchetypalSchemaFacts<GregorianDataSet>(prototypeOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type InternationalFixedTests() =
    inherit ArchetypalSchemaFacts<InternationalFixedDataSet>(prototypeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type JulianTests() =
    inherit ArchetypalSchemaFacts<JulianDataSet>(prototypeOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type LunisolarTests() =
    inherit ArchetypalSchemaFacts<LunisolarDataSet>(prototypeOf<LunisolarSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type PaxTests() =
    inherit ArchetypalSchemaFacts<PaxDataSet>(prototypeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Persian2820Tests() =
    inherit ArchetypalSchemaFacts<Persian2820DataSet>(prototypeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type PositivistTests() =
    inherit ArchetypalSchemaFacts<PositivistDataSet>(prototypeOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type TabularIslamicTests() =
    inherit ArchetypalSchemaFacts<TabularIslamicDataSet>(prototypeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type TropicaliaTests() =
    inherit ArchetypalSchemaFacts<TropicaliaDataSet>(prototypeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Tropicalia3031Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3031DataSet>(prototypeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type Tropicalia3130Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3130DataSet>(prototypeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
[<TestPerformance(TestPerformance.SlowBundle)>]
type WorldTests() =
    inherit ArchetypalSchemaFacts<WorldDataSet>(prototypeOf<WorldSchema>())
