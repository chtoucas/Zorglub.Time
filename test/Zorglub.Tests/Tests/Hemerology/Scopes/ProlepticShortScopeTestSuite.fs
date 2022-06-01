// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.ProlepticShortScopeTestSuite

open Zorglub.Testing
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

// Solar12ProlepticShortScope
[<Sealed>]
type Coptic12Tests() =
    inherit ProlepticShortScopeFacts<Coptic12DataSet>(scopeOf<Coptic12Schema>())

// PlainProlepticShortScope
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic13Tests() =
    inherit ProlepticShortScopeFacts<Coptic13DataSet>(scopeOf<Coptic13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit ProlepticShortScopeFacts<Egyptian12DataSet>(scopeOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit ProlepticShortScopeFacts<Egyptian13DataSet>(scopeOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit ProlepticShortScopeFacts<FrenchRepublican12DataSet>(scopeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit ProlepticShortScopeFacts<FrenchRepublican13DataSet>(scopeOf<FrenchRepublican13Schema>())

// GregorianProlepticShortScope
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit ProlepticShortScopeFacts<GregorianDataSet>(scopeOf<GregorianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type InternationalFixedTests() =
    inherit ProlepticShortScopeFacts<InternationalFixedDataSet>(scopeOf<InternationalFixedSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type JulianTests() =
    inherit ProlepticShortScopeFacts<JulianDataSet>(scopeOf<JulianSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type LunisolarTests() =
    inherit ProlepticShortScopeFacts<LunisolarDataSet>(scopeOf<LunisolarSchema>())

//[<Sealed>]
//[<RedundantTestBundle>]
//type PaxTests() =
//    inherit ProlepticShortScopeFacts<PaxDataSet>(scopeOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit ProlepticShortScopeFacts<Persian2820DataSet>(scopeOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit ProlepticShortScopeFacts<PositivistDataSet>(scopeOf<PositivistSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TabularIslamicTests() =
    inherit ProlepticShortScopeFacts<TabularIslamicDataSet>(scopeOf<TabularIslamicSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit ProlepticShortScopeFacts<TropicaliaDataSet>(scopeOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit ProlepticShortScopeFacts<Tropicalia3031DataSet>(scopeOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit ProlepticShortScopeFacts<Tropicalia3130DataSet>(scopeOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit ProlepticShortScopeFacts<WorldDataSet>(scopeOf<WorldSchema>())
