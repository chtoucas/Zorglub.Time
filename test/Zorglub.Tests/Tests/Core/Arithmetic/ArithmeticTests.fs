// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.ArithmeticTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas

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

    [<Theory>]
    [<InlineData 1>]
    [<InlineData 2>]
    [<InlineData 3>]
    [<InlineData 4>]
    [<InlineData 5>]
    [<InlineData 6>]
    let ``FastArithmetic constructor throws when MinDaysInMonth < 7`` i =
        let sch = FauxSystemSchema.WithMinDaysInMonth(i)

        argExn "schema" (fun () -> new DefaultFastArithmetic(sch))

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

// REVIEW(code): can we do better than that? I mean do it for all schemas. Sure,
// but is it useful? Here we have to test it separetely because DefaultFastArithmetic
// does not use it internally.
module DefaultFastCase =
    // Type to avoid the error FS0405 because AddDaysViaDayOfMonth() is a
    // protected internal method.
    [<Sealed>]
    type private ArithmeticWrapper(arithmetic: DefaultFastArithmetic) =
        member private __.Arithmetic = arithmetic
        member x.AddDaysViaDayOfMonth(ymd, days) = x.Arithmetic.AddDaysViaDayOfMonth(ymd, days)

    let private sch = schemaOf<GregorianSchema>()
    let private ari = new DefaultFastArithmetic(sch)
    let private wrapper = new ArithmeticWrapper(ari)

    let private maxDaysViaDayOfMonth = ari.MaxDaysViaDayOfMonth
    let private filter = fun (x: YemodaPairAnd<int>) ->
        -maxDaysViaDayOfMonth <= x.Value && x.Value <= maxDaysViaDayOfMonth
    let addDaysData = GregorianDataSet.Instance.AddDaysData.WhereT(filter)

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the start of MinYear`` () =
        let min = sch.MinMaxDateParts.LowerValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(min, -1)) |> overflows

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the end of MaxYear`` () =
        let max = sch.MinMaxDateParts.UpperValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(max, 1)) |> overflows

    [<Theory; MemberData(nameof(addDaysData))>]
    let ``AddDaysViaDayOfMonth()`` (pair: YemodaPairAnd<int>) =
        let days = pair.Value
        let date = pair.First
        let other = pair.Second

        wrapper.AddDaysViaDayOfMonth(date, days)   === other
        wrapper.AddDaysViaDayOfMonth(other, -days) === date

module GregorianCase =
    let private sch = schemaOf<GregorianSchema>()
    let private ari = new GregorianArithmetic(sch)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Solar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Solar.MinDaysInMonth

module LunarCase =
    let private sch = schemaOf<TabularIslamicSchema>()
    let private ari = new LunarArithmetic(sch)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Lunar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Lunar.MinDaysInMonth

module LunisolarCase =
    let private sch = schemaOf<LunisolarSchema>()
    let private ari = new LunisolarArithmetic(sch)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Lunisolar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Lunisolar.MinDaysInMonth

module Solar12Case =
    let private sch = schemaOf<GregorianSchema>()
    let private ari = new Solar12Arithmetic(sch)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Solar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Solar.MinDaysInMonth

module Solar13Case =
    let private sch = schemaOf<PositivistSchema>()
    let private ari = new Solar13Arithmetic(sch)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Solar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Solar.MinDaysInMonth
