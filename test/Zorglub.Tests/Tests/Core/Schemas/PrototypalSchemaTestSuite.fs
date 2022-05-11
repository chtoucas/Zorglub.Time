// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.PrototypalSchemaTestSuite

open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.
// TODO(code): PrototypalSchema does not work for a bunch of schemas.

/// Creates a new instance of the schema prototype of type 'a.
let private prototypeOf<'a when 'a : not struct and 'a :> ICalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    new PrototypalSchema(sch, sch.MinDaysInYear, sch.MinDaysInMonth)

//type [<Sealed>] Coptic12Tests() =
//    inherit ArchetypalSchemaFacts<Coptic12DataSet>(prototypeOf<Coptic12Schema>())

//type [<Sealed>] Coptic13Tests() =
//    inherit ArchetypalSchemaFacts<Coptic13DataSet>(prototypeOf<Coptic13Schema>())

type [<Sealed>] Egyptian12Tests() =
    inherit ArchetypalSchemaFacts<Egyptian12DataSet>(prototypeOf<Egyptian12Schema>())

type [<Sealed>] Egyptian13Tests() =
    inherit ArchetypalSchemaFacts<Egyptian13DataSet>(prototypeOf<Egyptian13Schema>())

//type [<Sealed>] FrenchRepublican12Tests() =
//    inherit ArchetypalSchemaFacts<FrenchRepublican12DataSet>(prototypeOf<FrenchRepublican12Schema>())

//type [<Sealed>] FrenchRepublican13Tests() =
//    inherit ArchetypalSchemaFacts<FrenchRepublican13DataSet>(prototypeOf<FrenchRepublican13Schema>())

type [<Sealed>] GregorianTests() =
    inherit ArchetypalSchemaFacts<GregorianDataSet>(prototypeOf<GregorianSchema>())

type [<Sealed>] InternationalFixedTests() =
    inherit ArchetypalSchemaFacts<InternationalFixedDataSet>(prototypeOf<InternationalFixedSchema>())

type [<Sealed>] JulianTests() =
    inherit ArchetypalSchemaFacts<JulianDataSet>(prototypeOf<JulianSchema>())

//type [<Sealed>] LunisolarTests() =
//    inherit ArchetypalSchemaFacts<LunisolarDataSet>(prototypeOf<LunisolarSchema>())

type [<Sealed>] PaxTests() =
    inherit ArchetypalSchemaFacts<PaxDataSet>(prototypeOf<PaxSchema>())

//type [<Sealed>] Persian2820Tests() =
//    inherit ArchetypalSchemaFacts<Persian2820DataSet>(prototypeOf<Persian2820Schema>())

type [<Sealed>] PositivistTests() =
    inherit ArchetypalSchemaFacts<PositivistDataSet>(prototypeOf<PositivistSchema>())

//type [<Sealed>] TabularIslamicTests() =
//    inherit ArchetypalSchemaFacts<TabularIslamicDataSet>(prototypeOf<TabularIslamicSchema>())

type [<Sealed>] TropicaliaTests() =
    inherit ArchetypalSchemaFacts<TropicaliaDataSet>(prototypeOf<TropicaliaSchema>())

type [<Sealed>] Tropicalia3031Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3031DataSet>(prototypeOf<Tropicalia3031Schema>())

type [<Sealed>] Tropicalia3130Tests() =
    inherit ArchetypalSchemaFacts<Tropicalia3130DataSet>(prototypeOf<Tropicalia3130Schema>())

type [<Sealed>] WorldTests() =
    inherit ArchetypalSchemaFacts<WorldDataSet>(prototypeOf<WorldSchema>())
