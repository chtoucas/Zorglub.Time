// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.StandardShortScopeTestSuite

open Zorglub.Testing
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
    new StandardShortScope(sch, DayZero.OldStyle)

// Solar12StandardShortScope
[<Sealed>]
type Coptic12Tests() =
    inherit StandardShortScopeFacts<Coptic12DataSet>(scopeOf<Coptic12Schema>())

// PlainStandardShortScope
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic13Tests() =
    inherit StandardShortScopeFacts<Coptic13DataSet>(scopeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit StandardShortScopeFacts<Egyptian12DataSet>(scopeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit StandardShortScopeFacts<Egyptian13DataSet>(scopeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit StandardShortScopeFacts<FrenchRepublican12DataSet>(scopeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit StandardShortScopeFacts<FrenchRepublican13DataSet>(scopeOf<FrenchRepublican13Schema>())

// GregorianStandardShortScope
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit StandardShortScopeFacts<GregorianDataSet>(scopeOf<GregorianSchema>())

// Solar13StandardShortScope
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type InternationalFixedTests() =
    inherit StandardShortScopeFacts<InternationalFixedDataSet>(scopeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit StandardShortScopeFacts<JulianDataSet>(scopeOf<JulianSchema>())

// LunisolarStandardShortScope
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit StandardShortScopeFacts<LunisolarDataSet>(scopeOf<LunisolarSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type PaxTests() =
    inherit StandardShortScopeFacts<PaxDataSet>(scopeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit StandardShortScopeFacts<Persian2820DataSet>(scopeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit StandardShortScopeFacts<PositivistDataSet>(scopeOf<PositivistSchema>())

// LunarStandardShortScope
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit StandardShortScopeFacts<TabularIslamicDataSet>(scopeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit StandardShortScopeFacts<TropicaliaDataSet>(scopeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit StandardShortScopeFacts<Tropicalia3031DataSet>(scopeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit StandardShortScopeFacts<Tropicalia3130DataSet>(scopeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit StandardShortScopeFacts<WorldDataSet>(scopeOf<WorldSchema>())
