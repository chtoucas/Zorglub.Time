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
[<TestPerformance(TestPerformance.SlowGroup)>]
type Coptic12Tests() =
    inherit ArchetypalSchemaFacts<Coptic12DataSet>(prototypeOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic13Tests() =
    inherit ArchetypalSchemaFacts<Coptic13DataSet>(prototypeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Egyptian12Tests() =
    inherit ArchetypalSchemaFacts<Egyptian12DataSet>(prototypeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Egyptian13Tests() =
    inherit ArchetypalSchemaFacts<Egyptian13DataSet>(prototypeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type FrenchRepublican12Tests() =
    inherit ArchetypalSchemaFacts<FrenchRepublican12DataSet>(prototypeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type FrenchRepublican13Tests() =
    inherit ArchetypalSchemaFacts<FrenchRepublican13DataSet>(prototypeOf<FrenchRepublican13Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit ArchetypalSchemaFacts<GregorianDataSet>(prototypeOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type InternationalFixedTests() =
    inherit ArchetypalSchemaFacts<InternationalFixedDataSet>(prototypeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type JulianTests() =
    inherit ArchetypalSchemaFacts<JulianDataSet>(prototypeOf<JulianSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit ArchetypalSchemaFacts<LunisolarDataSet>(prototypeOf<LunisolarSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PaxTests() =
    inherit ArchetypalSchemaFacts<PaxDataSet>(prototypeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Persian2820Tests() =
    inherit ArchetypalSchemaFacts<Persian2820DataSet>(prototypeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PositivistTests() =
    inherit ArchetypalSchemaFacts<PositivistDataSet>(prototypeOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit ArchetypalSchemaFacts<TabularIslamicDataSet>(prototypeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TropicaliaTests() =
    inherit ArchetypalSchemaFacts<TropicaliaDataSet>(prototypeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Tropicalia3031Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3031DataSet>(prototypeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Tropicalia3130Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3130DataSet>(prototypeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestGroup>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type WorldTests() =
    inherit ArchetypalSchemaFacts<WorldDataSet>(prototypeOf<WorldSchema>())
