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

let private epoch = DayZero.OldStyle

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new FauxCalendarScope(null, 1, 2))

module StandardScope =
    // Returns an ICalendarScope.
    let scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
        let sch = SchemaActivator.CreateInstance<'a>()
        ICalendarScope.CreateStandardScope(sch, epoch)

    [<Fact>]
    let ``CreateStandardScope() throws when "schema" is null`` () =
        nullExn "schema" (fun () -> ICalendarScope.CreateStandardScope(null, epoch))

    [<Fact>]
    let ``CreateStandardScope()`` () =
        scopeOf<Coptic12Schema>()           |> is<Solar12StandardShortScope>
        scopeOf<Coptic13Schema>()           |> is<DefaultStandardShortScope>
        scopeOf<Egyptian12Schema>()         |> is<Solar12StandardShortScope>
        scopeOf<Egyptian13Schema>()         |> is<DefaultStandardShortScope>
        scopeOf<FrenchRepublican12Schema>() |> is<Solar12StandardShortScope>
        scopeOf<FrenchRepublican13Schema>() |> is<DefaultStandardShortScope>
        scopeOf<GregorianSchema>()          |> is<GregorianStandardShortScope>
        scopeOf<HebrewSchema>()             |> is<LunisolarStandardShortScope>
        scopeOf<InternationalFixedSchema>() |> is<Solar13StandardShortScope>
        scopeOf<JulianSchema>()             |> is<Solar12StandardShortScope>
        scopeOf<LunisolarSchema>()          |> is<LunisolarStandardShortScope>
        scopeOf<PaxSchema>()                |> is<DefaultStandardShortScope>
        scopeOf<PositivistSchema>()         |> is<Solar13StandardShortScope>
        scopeOf<Persian2820Schema>()        |> is<Solar12StandardShortScope>
        scopeOf<TabularIslamicSchema>()     |> is<LunarStandardShortScope>
        scopeOf<Tropicalia3031Schema>()     |> is<Solar12StandardShortScope>
        scopeOf<Tropicalia3130Schema>()     |> is<Solar12StandardShortScope>
        scopeOf<TropicaliaSchema>()         |> is<Solar12StandardShortScope>
        scopeOf<WorldSchema>()              |> is<Solar12StandardShortScope>

module ProlepticScope =
    // Returns an ICalendarScope.
    let scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
        let sch = SchemaActivator.CreateInstance<'a>()
        ICalendarScope.CreateProlepticScope(sch, epoch)

    [<Fact>]
    let ``CreateProlepticScope() when "schema" is null`` () =
        nullExn "schema" (fun () -> ICalendarScope.CreateProlepticScope(null, epoch))

    [<Fact>]
    let ``CreateProlepticScope()`` () =
        scopeOf<Coptic12Schema>()           |> is<Solar12ProlepticShortScope>
        scopeOf<Coptic13Schema>()           |> is<DefaultProlepticShortScope>
        scopeOf<Egyptian12Schema>()         |> is<Solar12ProlepticShortScope>
        scopeOf<Egyptian13Schema>()         |> is<DefaultProlepticShortScope>
        scopeOf<FrenchRepublican12Schema>() |> is<Solar12ProlepticShortScope>
        scopeOf<FrenchRepublican13Schema>() |> is<DefaultProlepticShortScope>
        scopeOf<GregorianSchema>()          |> is<GregorianProlepticShortScope>
        scopeOf<HebrewSchema>()             |> is<DefaultProlepticShortScope>
        scopeOf<InternationalFixedSchema>() |> is<DefaultProlepticShortScope>
        scopeOf<JulianSchema>()             |> is<Solar12ProlepticShortScope>
        scopeOf<LunisolarSchema>()          |> is<DefaultProlepticShortScope>
        argExn "schema" (fun () -> scopeOf<PaxSchema>()) // PaxSchema.MinYear = 1 > -9999.
        scopeOf<PositivistSchema>()         |> is<DefaultProlepticShortScope>
        scopeOf<Persian2820Schema>()        |> is<Solar12ProlepticShortScope>
        scopeOf<TabularIslamicSchema>()     |> is<DefaultProlepticShortScope>
        scopeOf<Tropicalia3031Schema>()     |> is<Solar12ProlepticShortScope>
        scopeOf<Tropicalia3130Schema>()     |> is<Solar12ProlepticShortScope>
        scopeOf<TropicaliaSchema>()         |> is<Solar12ProlepticShortScope>
        scopeOf<WorldSchema>()              |> is<Solar12ProlepticShortScope>
