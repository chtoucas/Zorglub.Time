// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.StandardScopeTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schemas.

// Returns a StandardScope.
let private scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    StandardScope.Create(sch, DayZero.OldStyle)

[<Sealed>]
type Coptic12Tests() =
    inherit StandardScopeFacts<Coptic12DataSet>(scopeOf<Coptic12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Coptic13Tests() =
    inherit StandardScopeFacts<Coptic13DataSet>(scopeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit StandardScopeFacts<Egyptian12DataSet>(scopeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit StandardScopeFacts<Egyptian13DataSet>(scopeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit StandardScopeFacts<FrenchRepublican12DataSet>(scopeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit StandardScopeFacts<FrenchRepublican13DataSet>(scopeOf<FrenchRepublican13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type GregorianTests() =
    inherit StandardScopeFacts<GregorianDataSet>(scopeOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit StandardScopeFacts<InternationalFixedDataSet>(scopeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit StandardScopeFacts<JulianDataSet>(scopeOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit StandardScopeFacts<LunisolarDataSet>(scopeOf<LunisolarSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type PaxTests() =
    inherit StandardScopeFacts<PaxDataSet>(scopeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit StandardScopeFacts<Persian2820DataSet>(scopeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit StandardScopeFacts<PositivistDataSet>(scopeOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit StandardScopeFacts<TabularIslamicDataSet>(scopeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit StandardScopeFacts<TropicaliaDataSet>(scopeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit StandardScopeFacts<Tropicalia3031DataSet>(scopeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit StandardScopeFacts<Tropicalia3130DataSet>(scopeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit StandardScopeFacts<WorldDataSet>(scopeOf<WorldSchema>())
