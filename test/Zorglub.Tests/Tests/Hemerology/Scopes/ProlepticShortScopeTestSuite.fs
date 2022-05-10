// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.ProlepticShortScopeTestSuite

open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Hemerology
open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

// TODO(code): Hebrew (unfinished, no data), Pax (not proleptic) and lunisolar (fake) schema.

// Returns a ProlepticShortScope.
let private scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    ProlepticShortScope.Create(sch, DayZero.OldStyle)

type [<Sealed>] Coptic12Tests() =
    inherit ProlepticShortScopeFacts<Coptic12DataSet>(scopeOf<Coptic12Schema>())

type [<Sealed>] Coptic13Tests() =
    inherit ProlepticShortScopeFacts<Coptic13DataSet>(scopeOf<Coptic13Schema>())

type [<Sealed>] Egyptian12Tests() =
    inherit ProlepticShortScopeFacts<Egyptian12DataSet>(scopeOf<Egyptian12Schema>())

type [<Sealed>] Egyptian13Tests() =
    inherit ProlepticShortScopeFacts<Egyptian13DataSet>(scopeOf<Egyptian13Schema>())

type [<Sealed>] FrenchRepublican12Tests() =
    inherit ProlepticShortScopeFacts<FrenchRepublican12DataSet>(scopeOf<FrenchRepublican12Schema>())

type [<Sealed>] FrenchRepublican13Tests() =
    inherit ProlepticShortScopeFacts<FrenchRepublican13DataSet>(scopeOf<FrenchRepublican13Schema>())

type [<Sealed>] GregorianTests() =
    inherit ProlepticShortScopeFacts<GregorianDataSet>(scopeOf<GregorianSchema>())

type [<Sealed>] InternationalFixedTests() =
    inherit ProlepticShortScopeFacts<InternationalFixedDataSet>(scopeOf<InternationalFixedSchema>())

type [<Sealed>] JulianTests() =
    inherit ProlepticShortScopeFacts<JulianDataSet>(scopeOf<JulianSchema>())

type [<Sealed>] LunisolarTests() =
    inherit ProlepticShortScopeFacts<LunisolarDataSet>(scopeOf<LunisolarSchema>())

//type [<Sealed>] PaxTests() =
//    inherit ProlepticShortScopeFacts<PaxDataSet>(scopeOf<PaxSchema>())

type [<Sealed>] Persian2820Tests() =
    inherit ProlepticShortScopeFacts<Persian2820DataSet>(scopeOf<Persian2820Schema>())

type [<Sealed>] PositivistTests() =
    inherit ProlepticShortScopeFacts<PositivistDataSet>(scopeOf<PositivistSchema>())

type [<Sealed>] TabularIslamicTests() =
    inherit ProlepticShortScopeFacts<TabularIslamicDataSet>(scopeOf<TabularIslamicSchema>())

type [<Sealed>] TropicaliaTests() =
    inherit ProlepticShortScopeFacts<TropicaliaDataSet>(scopeOf<TropicaliaSchema>())

type [<Sealed>] Tropicalia3031Tests() =
    inherit ProlepticShortScopeFacts<Tropicalia3031DataSet>(scopeOf<Tropicalia3031Schema>())

type [<Sealed>] Tropicalia3130Tests() =
    inherit ProlepticShortScopeFacts<Tropicalia3130DataSet>(scopeOf<Tropicalia3130Schema>())

type [<Sealed>] WorldTests() =
    inherit ProlepticShortScopeFacts<WorldDataSet>(scopeOf<WorldSchema>())
