// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.ProlepticScopeTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

// TODO(code): Hebrew (unfinished, no data), Pax (not proleptic) and lunisolar (fake) schema.

// Returns a ProlepticScope.
let private scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    new ProlepticScope(sch, DayZero.OldStyle)

[<Sealed>]
type Coptic12Tests() =
    inherit ProlepticScopeFacts<Coptic12DataSet>(scopeOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Coptic13Tests() =
    inherit ProlepticScopeFacts<Coptic13DataSet>(scopeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit ProlepticScopeFacts<Egyptian12DataSet>(scopeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit ProlepticScopeFacts<Egyptian13DataSet>(scopeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit ProlepticScopeFacts<FrenchRepublican12DataSet>(scopeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit ProlepticScopeFacts<FrenchRepublican13DataSet>(scopeOf<FrenchRepublican13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type GregorianTests() =
    inherit ProlepticScopeFacts<GregorianDataSet>(scopeOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit ProlepticScopeFacts<InternationalFixedDataSet>(scopeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit ProlepticScopeFacts<JulianDataSet>(scopeOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit ProlepticScopeFacts<LunisolarDataSet>(scopeOf<LunisolarSchema>())

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit ProlepticScopeFacts<PaxDataSet>(scopeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit ProlepticScopeFacts<Persian2820DataSet>(scopeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit ProlepticScopeFacts<PositivistDataSet>(scopeOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit ProlepticScopeFacts<TabularIslamicDataSet>(scopeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit ProlepticScopeFacts<TropicaliaDataSet>(scopeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit ProlepticScopeFacts<Tropicalia3031DataSet>(scopeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit ProlepticScopeFacts<Tropicalia3130DataSet>(scopeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit ProlepticScopeFacts<WorldDataSet>(scopeOf<WorldSchema>())
