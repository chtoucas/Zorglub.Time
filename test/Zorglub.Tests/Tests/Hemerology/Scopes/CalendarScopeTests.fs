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
        new StandardShortScope(sch, epoch)

    [<Fact>]
    let ``CreateStandardScope()`` () =
        scopeOf<Coptic12Schema>()           |> is<StandardShortScope>
        scopeOf<Coptic13Schema>()           |> is<StandardShortScope>
        scopeOf<Egyptian12Schema>()         |> is<StandardShortScope>
        scopeOf<Egyptian13Schema>()         |> is<StandardShortScope>
        scopeOf<FrenchRepublican12Schema>() |> is<StandardShortScope>
        scopeOf<FrenchRepublican13Schema>() |> is<StandardShortScope>
        scopeOf<GregorianSchema>()          |> is<StandardShortScope>
        scopeOf<HebrewSchema>()             |> is<StandardShortScope>
        scopeOf<InternationalFixedSchema>() |> is<StandardShortScope>
        scopeOf<JulianSchema>()             |> is<StandardShortScope>
        scopeOf<LunisolarSchema>()          |> is<StandardShortScope>
        scopeOf<PaxSchema>()                |> is<StandardShortScope>
        scopeOf<PositivistSchema>()         |> is<StandardShortScope>
        scopeOf<Persian2820Schema>()        |> is<StandardShortScope>
        scopeOf<TabularIslamicSchema>()     |> is<StandardShortScope>
        scopeOf<Tropicalia3031Schema>()     |> is<StandardShortScope>
        scopeOf<Tropicalia3130Schema>()     |> is<StandardShortScope>
        scopeOf<TropicaliaSchema>()         |> is<StandardShortScope>
        scopeOf<WorldSchema>()              |> is<StandardShortScope>

module ProlepticScope =
    // Returns an ICalendarScope.
    let scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
        let sch = SchemaActivator.CreateInstance<'a>()
        new ProlepticShortScope(sch, epoch)

    [<Fact>]
    let ``CreateProlepticScope()`` () =
        scopeOf<Coptic12Schema>()           |> is<ProlepticShortScope>
        scopeOf<Coptic13Schema>()           |> is<ProlepticShortScope>
        scopeOf<Egyptian12Schema>()         |> is<ProlepticShortScope>
        scopeOf<Egyptian13Schema>()         |> is<ProlepticShortScope>
        scopeOf<FrenchRepublican12Schema>() |> is<ProlepticShortScope>
        scopeOf<FrenchRepublican13Schema>() |> is<ProlepticShortScope>
        scopeOf<GregorianSchema>()          |> is<ProlepticShortScope>
        scopeOf<HebrewSchema>()             |> is<ProlepticShortScope>
        scopeOf<InternationalFixedSchema>() |> is<ProlepticShortScope>
        scopeOf<JulianSchema>()             |> is<ProlepticShortScope>
        scopeOf<LunisolarSchema>()          |> is<ProlepticShortScope>
        argExn "schema" (fun () -> scopeOf<PaxSchema>()) // PaxSchema.MinYear = 1 > -9999.
        scopeOf<PositivistSchema>()         |> is<ProlepticShortScope>
        scopeOf<Persian2820Schema>()        |> is<ProlepticShortScope>
        scopeOf<TabularIslamicSchema>()     |> is<ProlepticShortScope>
        scopeOf<Tropicalia3031Schema>()     |> is<ProlepticShortScope>
        scopeOf<Tropicalia3130Schema>()     |> is<ProlepticShortScope>
        scopeOf<TropicaliaSchema>()         |> is<ProlepticShortScope>
        scopeOf<WorldSchema>()              |> is<ProlepticShortScope>
