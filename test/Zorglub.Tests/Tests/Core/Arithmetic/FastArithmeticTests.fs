// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.FastArithmeticTests

open Zorglub.Testing

open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Schemas

open Xunit

[<Fact>]
let ``Create()`` () =
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
