// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.ArithmeticTests

open Zorglub.Testing

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

open Xunit

module Prelude =
    let badLunarProfile = FauxSystemSchema.NotLunar
    let badLunisolarProfile = FauxSystemSchema.NotLunisolar
    let badSolar12Profile = FauxSystemSchema.NotSolar12
    let badSolar13Profile = FauxSystemSchema.NotSolar13

    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new CalendricalArithmetic(null))
        nullExn "schema" (fun () -> new DefaultArithmetic(null))
        nullExn "schema" (fun () -> new DefaultFastArithmetic(null))
        nullExn "schema" (fun () -> new GregorianArithmetic(null))
        nullExn "schema" (fun () -> new LunarArithmetic(null))
        nullExn "schema" (fun () -> new LunisolarArithmetic(null))
        nullExn "schema" (fun () -> new Solar12Arithmetic(null))
        nullExn "schema" (fun () -> new Solar13Arithmetic(null))

    [<Theory; MemberData(nameof(badLunarProfile))>]
    let ``LunarArithmetic constructor throws for non-lunar schema`` (sch) =
        argExn "schema" (fun () -> new LunarArithmetic(sch))

    [<Theory; MemberData(nameof(badLunisolarProfile))>]
    let ``LunisolarArithmetic constructor throws for non-lunisolar schema`` (sch) =
        argExn "schema" (fun () -> new LunisolarArithmetic(sch))

    [<Theory; MemberData(nameof(badSolar12Profile))>]
    let ``Solar12Arithmetic constructor throws for non-solar12 schema`` (sch) =
        argExn "schema" (fun () -> new Solar12Arithmetic(sch))

    [<Theory; MemberData(nameof(badSolar13Profile))>]
    let ``Solar13Arithmetic constructor throws for non-solar13 schema`` (sch) =
        argExn "schema" (fun () -> new Solar13Arithmetic(sch))

module Factories =
    [<Fact>]
    let ``FastArithmetic.Create()`` () =
        FastArithmetic.Create(schemaOf<Coptic12Schema>())           |> is<Solar12Arithmetic>
        argExn "schema" (fun () -> FastArithmetic.Create(schemaOf<Coptic13Schema>()))
        FastArithmetic.Create(schemaOf<Egyptian12Schema>())         |> is<Solar12Arithmetic>
        argExn "schema" (fun () -> FastArithmetic.Create(schemaOf<Egyptian13Schema>()))
        FastArithmetic.Create(schemaOf<FrenchRepublican12Schema>()) |> is<Solar12Arithmetic>
        argExn "schema" (fun () -> FastArithmetic.Create(schemaOf<FrenchRepublican13Schema>()))
        FastArithmetic.Create(schemaOf<GregorianSchema>())          |> is<GregorianArithmetic>
        //FastArithmetic.Create(schemaOf<HebrewSchema>())             |> is<LunisolarArithmetic>
        FastArithmetic.Create(schemaOf<InternationalFixedSchema>()) |> is<Solar13Arithmetic>
        FastArithmetic.Create(schemaOf<JulianSchema>())             |> is<Solar12Arithmetic>
        FastArithmetic.Create(schemaOf<Persian2820Schema>())        |> is<Solar12Arithmetic>
        FastArithmetic.Create(schemaOf<PositivistSchema>())         |> is<Solar13Arithmetic>
        FastArithmetic.Create(schemaOf<TabularIslamicSchema>())     |> is<LunarArithmetic>
        FastArithmetic.Create(schemaOf<TropicaliaSchema>())         |> is<Solar12Arithmetic>
        FastArithmetic.Create(schemaOf<Tropicalia3031Schema>())     |> is<Solar12Arithmetic>
        FastArithmetic.Create(schemaOf<Tropicalia3130Schema>())     |> is<Solar12Arithmetic>
        FastArithmetic.Create(schemaOf<WorldSchema>())              |> is<Solar12Arithmetic>
