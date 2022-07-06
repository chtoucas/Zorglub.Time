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
        scopeOf<Coptic12Schema>()           |> is<PlainStandardShortScope>
        scopeOf<Coptic13Schema>()           |> is<PlainStandardShortScope>
        scopeOf<Egyptian12Schema>()         |> is<PlainStandardShortScope>
        scopeOf<Egyptian13Schema>()         |> is<PlainStandardShortScope>
        scopeOf<FrenchRepublican12Schema>() |> is<PlainStandardShortScope>
        scopeOf<FrenchRepublican13Schema>() |> is<PlainStandardShortScope>
        scopeOf<GregorianSchema>()          |> is<GregorianStandardShortScope>
        scopeOf<HebrewSchema>()             |> is<PlainStandardShortScope>
        scopeOf<InternationalFixedSchema>() |> is<PlainStandardShortScope>
        scopeOf<JulianSchema>()             |> is<PlainStandardShortScope>
        scopeOf<LunisolarSchema>()          |> is<PlainStandardShortScope>
        scopeOf<PaxSchema>()                |> is<PlainStandardShortScope>
        scopeOf<PositivistSchema>()         |> is<PlainStandardShortScope>
        scopeOf<Persian2820Schema>()        |> is<PlainStandardShortScope>
        scopeOf<TabularIslamicSchema>()     |> is<PlainStandardShortScope>
        scopeOf<Tropicalia3031Schema>()     |> is<PlainStandardShortScope>
        scopeOf<Tropicalia3130Schema>()     |> is<PlainStandardShortScope>
        scopeOf<TropicaliaSchema>()         |> is<PlainStandardShortScope>
        scopeOf<WorldSchema>()              |> is<PlainStandardShortScope>

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
        scopeOf<Coptic12Schema>()           |> is<PlainProlepticShortScope>
        scopeOf<Coptic13Schema>()           |> is<PlainProlepticShortScope>
        scopeOf<Egyptian12Schema>()         |> is<PlainProlepticShortScope>
        scopeOf<Egyptian13Schema>()         |> is<PlainProlepticShortScope>
        scopeOf<FrenchRepublican12Schema>() |> is<PlainProlepticShortScope>
        scopeOf<FrenchRepublican13Schema>() |> is<PlainProlepticShortScope>
        scopeOf<GregorianSchema>()          |> is<GregorianProlepticShortScope>
        scopeOf<HebrewSchema>()             |> is<PlainProlepticShortScope>
        scopeOf<InternationalFixedSchema>() |> is<PlainProlepticShortScope>
        scopeOf<JulianSchema>()             |> is<PlainProlepticShortScope>
        scopeOf<LunisolarSchema>()          |> is<PlainProlepticShortScope>
        argExn "schema" (fun () -> scopeOf<PaxSchema>()) // PaxSchema.MinYear = 1 > -9999.
        scopeOf<PositivistSchema>()         |> is<PlainProlepticShortScope>
        scopeOf<Persian2820Schema>()        |> is<PlainProlepticShortScope>
        scopeOf<TabularIslamicSchema>()     |> is<PlainProlepticShortScope>
        scopeOf<Tropicalia3031Schema>()     |> is<PlainProlepticShortScope>
        scopeOf<Tropicalia3130Schema>()     |> is<PlainProlepticShortScope>
        scopeOf<TropicaliaSchema>()         |> is<PlainProlepticShortScope>
        scopeOf<WorldSchema>()              |> is<PlainProlepticShortScope>
