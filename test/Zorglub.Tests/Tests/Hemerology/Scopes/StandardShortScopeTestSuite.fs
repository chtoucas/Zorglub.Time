// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.StandardShortScopeTestSuite

open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Hemerology
open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schemas.
// Add test for ProlepticShortScope.Create().

// Returns a StandardShortScope.
let private scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    StandardShortScope.Create(sch, DayZero.OldStyle)

type [<Sealed>] Coptic12Tests() =
    inherit StandardShortScopeFacts<Coptic12DataSet>(scopeOf<Coptic12Schema>())

type [<Sealed>] Coptic13Tests() =
    inherit StandardShortScopeFacts<Coptic13DataSet>(scopeOf<Coptic13Schema>())

type [<Sealed>] Egyptian12Tests() =
    inherit StandardShortScopeFacts<Egyptian12DataSet>(scopeOf<Egyptian12Schema>())

type [<Sealed>] Egyptian13Tests() =
    inherit StandardShortScopeFacts<Egyptian13DataSet>(scopeOf<Egyptian13Schema>())

type [<Sealed>] FrenchRepublican12Tests() =
    inherit StandardShortScopeFacts<FrenchRepublican12DataSet>(scopeOf<FrenchRepublican12Schema>())

type [<Sealed>] FrenchRepublican13Tests() =
    inherit StandardShortScopeFacts<FrenchRepublican13DataSet>(scopeOf<FrenchRepublican13Schema>())

type [<Sealed>] GregorianTests() =
    inherit StandardShortScopeFacts<GregorianDataSet>(scopeOf<GregorianSchema>())

type [<Sealed>] InternationalFixedTests() =
    inherit StandardShortScopeFacts<InternationalFixedDataSet>(scopeOf<InternationalFixedSchema>())

type [<Sealed>] JulianTests() =
    inherit StandardShortScopeFacts<JulianDataSet>(scopeOf<JulianSchema>())

type [<Sealed>] LunisolarTests() =
    inherit StandardShortScopeFacts<LunisolarDataSet>(scopeOf<LunisolarSchema>())

type [<Sealed>] PaxTests() =
    inherit StandardShortScopeFacts<PaxDataSet>(scopeOf<PaxSchema>())

type [<Sealed>] Persian2820Tests() =
    inherit StandardShortScopeFacts<Persian2820DataSet>(scopeOf<Persian2820Schema>())

type [<Sealed>] PositivistTests() =
    inherit StandardShortScopeFacts<PositivistDataSet>(scopeOf<PositivistSchema>())

type [<Sealed>] TabularIslamicTests() =
    inherit StandardShortScopeFacts<TabularIslamicDataSet>(scopeOf<TabularIslamicSchema>())

type [<Sealed>] TropicaliaTests() =
    inherit StandardShortScopeFacts<TropicaliaDataSet>(scopeOf<TropicaliaSchema>())

type [<Sealed>] Tropicalia3031Tests() =
    inherit StandardShortScopeFacts<Tropicalia3031DataSet>(scopeOf<Tropicalia3031Schema>())

type [<Sealed>] Tropicalia3130Tests() =
    inherit StandardShortScopeFacts<Tropicalia3130DataSet>(scopeOf<Tropicalia3130Schema>())

type [<Sealed>] WorldTests() =
    inherit StandardShortScopeFacts<WorldDataSet>(scopeOf<WorldSchema>())
