// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.ArchetypalSchemaTestSuite

open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

/// Creates a new instance of the schema archetype of type 'a.
let private archetypeOf<'a when 'a : not struct and 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    new SchemaArchetype(sch)

type [<Sealed>] Coptic12Tests() =
    inherit ArchetypalSchemaFacts<Coptic12DataSet>(archetypeOf<Coptic12Schema>())

type [<Sealed>] Coptic13Tests() =
    inherit ArchetypalSchemaFacts<Coptic13DataSet>(archetypeOf<Coptic13Schema>())

type [<Sealed>] Egyptian12Tests() =
    inherit ArchetypalSchemaFacts<Egyptian12DataSet>(archetypeOf<Egyptian12Schema>())

type [<Sealed>] Egyptian13Tests() =
    inherit ArchetypalSchemaFacts<Egyptian13DataSet>(archetypeOf<Egyptian13Schema>())

type [<Sealed>] FrenchRepublican12Tests() =
    inherit ArchetypalSchemaFacts<FrenchRepublican12DataSet>(archetypeOf<FrenchRepublican12Schema>())

type [<Sealed>] FrenchRepublican13Tests() =
    inherit ArchetypalSchemaFacts<FrenchRepublican13DataSet>(archetypeOf<FrenchRepublican13Schema>())

type [<Sealed>] GregorianTests() =
    inherit ArchetypalSchemaFacts<GregorianDataSet>(archetypeOf<GregorianSchema>())

type [<Sealed>] InternationalFixedTests() =
    inherit ArchetypalSchemaFacts<InternationalFixedDataSet>(archetypeOf<InternationalFixedSchema>())

type [<Sealed>] JulianTests() =
    inherit ArchetypalSchemaFacts<JulianDataSet>(archetypeOf<JulianSchema>())

type [<Sealed>] LunisolarTests() =
    inherit ArchetypalSchemaFacts<LunisolarDataSet>(archetypeOf<LunisolarSchema>())

type [<Sealed>] PaxTests() =
    inherit ArchetypalSchemaFacts<PaxDataSet>(archetypeOf<PaxSchema>())

type [<Sealed>] Persian2820Tests() =
    inherit ArchetypalSchemaFacts<Persian2820DataSet>(archetypeOf<Persian2820Schema>())

type [<Sealed>] PositivistTests() =
    inherit ArchetypalSchemaFacts<PositivistDataSet>(archetypeOf<PositivistSchema>())

type [<Sealed>] TabularIslamicTests() =
    inherit ArchetypalSchemaFacts<TabularIslamicDataSet>(archetypeOf<TabularIslamicSchema>())

type [<Sealed>] TropicaliaTests() =
    inherit ArchetypalSchemaFacts<TropicaliaDataSet>(archetypeOf<TropicaliaSchema>())

type [<Sealed>] Tropicalia3031Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3031DataSet>(archetypeOf<Tropicalia3031Schema>())

type [<Sealed>] Tropicalia3130Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3130DataSet>(archetypeOf<Tropicalia3130Schema>())

type [<Sealed>] WorldTests() =
    inherit ArchetypalSchemaFacts<WorldDataSet>(archetypeOf<WorldSchema>())
