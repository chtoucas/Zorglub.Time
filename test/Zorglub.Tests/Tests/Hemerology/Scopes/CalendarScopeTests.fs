// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.CalendarScopeTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology
open Zorglub.Time.Hemerology.Scopes

open Xunit

// TODO(test): to be removed!

let private epoch = DayZero.OldStyle

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new FauxCalendarScope(null, 1, 2))

module StandardScope =
    // Returns an ICalendarScope.
    let scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
        let sch = SchemaActivator.CreateInstance<'a>()
        new StandardScope(sch, epoch)

    [<Fact>]
    let ``CreateStandardScope()`` () =
        scopeOf<Coptic12Schema>()           |> is<StandardScope>
        scopeOf<Coptic13Schema>()           |> is<StandardScope>
        scopeOf<Egyptian12Schema>()         |> is<StandardScope>
        scopeOf<Egyptian13Schema>()         |> is<StandardScope>
        scopeOf<FrenchRepublican12Schema>() |> is<StandardScope>
        scopeOf<FrenchRepublican13Schema>() |> is<StandardScope>
        scopeOf<GregorianSchema>()          |> is<StandardScope>
        scopeOf<HebrewSchema>()             |> is<StandardScope>
        scopeOf<InternationalFixedSchema>() |> is<StandardScope>
        scopeOf<JulianSchema>()             |> is<StandardScope>
        scopeOf<LunisolarSchema>()          |> is<StandardScope>
        scopeOf<PaxSchema>()                |> is<StandardScope>
        scopeOf<PositivistSchema>()         |> is<StandardScope>
        scopeOf<Persian2820Schema>()        |> is<StandardScope>
        scopeOf<TabularIslamicSchema>()     |> is<StandardScope>
        scopeOf<Tropicalia3031Schema>()     |> is<StandardScope>
        scopeOf<Tropicalia3130Schema>()     |> is<StandardScope>
        scopeOf<TropicaliaSchema>()         |> is<StandardScope>
        scopeOf<WorldSchema>()              |> is<StandardScope>

module ProlepticScope =
    // Returns an ICalendarScope.
    let scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
        let sch = SchemaActivator.CreateInstance<'a>()
        new ProlepticScope(sch, epoch)

    [<Fact>]
    let ``CreateProlepticScope()`` () =
        scopeOf<Coptic12Schema>()           |> is<ProlepticScope>
        scopeOf<Coptic13Schema>()           |> is<ProlepticScope>
        scopeOf<Egyptian12Schema>()         |> is<ProlepticScope>
        scopeOf<Egyptian13Schema>()         |> is<ProlepticScope>
        scopeOf<FrenchRepublican12Schema>() |> is<ProlepticScope>
        scopeOf<FrenchRepublican13Schema>() |> is<ProlepticScope>
        scopeOf<GregorianSchema>()          |> is<ProlepticScope>
        scopeOf<HebrewSchema>()             |> is<ProlepticScope>
        scopeOf<InternationalFixedSchema>() |> is<ProlepticScope>
        scopeOf<JulianSchema>()             |> is<ProlepticScope>
        scopeOf<LunisolarSchema>()          |> is<ProlepticScope>
        argExn "schema" (fun () -> scopeOf<PaxSchema>()) // PaxSchema.MinYear = 1 > -9999.
        scopeOf<PositivistSchema>()         |> is<ProlepticScope>
        scopeOf<Persian2820Schema>()        |> is<ProlepticScope>
        scopeOf<TabularIslamicSchema>()     |> is<ProlepticScope>
        scopeOf<Tropicalia3031Schema>()     |> is<ProlepticScope>
        scopeOf<Tropicalia3130Schema>()     |> is<ProlepticScope>
        scopeOf<TropicaliaSchema>()         |> is<ProlepticScope>
        scopeOf<WorldSchema>()              |> is<ProlepticScope>
