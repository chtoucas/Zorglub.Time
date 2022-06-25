// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.ArithmeticTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas

open Xunit

// Type to avoid the error FS0405 because AddDaysViaDayOfMonth() is a protected
// internal method.
[<Sealed>]
type private ArithmeticWrapper(arithmetic: SystemArithmetic) =
    member private __.Arithmetic = arithmetic
    member x.AddDaysViaDayOfMonth(ymd, days) = x.Arithmetic.AddDaysViaDayOfMonth(ymd, days)

let private getAddDaysData (ari: SystemArithmetic) =
    let maxDaysViaDayOfMonth = ari.MaxDaysViaDayOfMonth
    let filter = fun (x: YemodaPairAnd<int>) ->
        -maxDaysViaDayOfMonth <= x.Value && x.Value <= maxDaysViaDayOfMonth
    GregorianDataSet.Instance.AddDaysData.WhereT(filter)

module Prelude =
    let badLunarProfile = FauxSystemSchema.NotLunar
    let badLunisolarProfile = FauxSystemSchema.NotLunisolar
    let badSolar12Profile = FauxSystemSchema.NotSolar12
    let badSolar13Profile = FauxSystemSchema.NotSolar13

    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new CalendricalArithmetic(null))
        nullExn "schema" (fun () -> new PlainArithmetic(null))
        nullExn "schema" (fun () -> new RegularArithmetic(null))
        nullExn "schema" (fun () -> new LunarArithmetic(null))
        nullExn "schema" (fun () -> new LunisolarArithmetic(null))
        nullExn "schema" (fun () -> new Solar12Arithmetic(null))
        nullExn "schema" (fun () -> new Solar13Arithmetic(null))

    [<Fact>]
    let ``Constructor throws for schemas with bad range of supported years`` () =
        let range = Range.StartingAt(Yemoda.MaxYear + 1)
        let sch = new FauxCalendricalSchema(range)

        argExn "schema" (fun () -> new CalendricalArithmetic(sch))
        argExn "schema" (fun () -> new PlainArithmetic(sch))
        argExn "schema" (fun () -> new RegularArithmetic(sch))
        argExn "schema" (fun () -> new LunarArithmetic(sch))
        argExn "schema" (fun () -> new LunisolarArithmetic(sch))
        argExn "schema" (fun () -> new Solar12Arithmetic(sch))
        argExn "schema" (fun () -> new Solar13Arithmetic(sch))

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
    let ``RegularArithmetic constructor throws when MinDaysInMonth < 7`` i =
        let sch = FauxSystemSchema.WithMinDaysInMonth(i)

        argExn "schema" (fun () -> new RegularArithmetic(sch))

module Factories =
    [<Fact>]
    let ``SystemArithmetic.CreateDefault()`` () =
        SystemArithmetic.CreateDefault(schemaOf<Coptic12Schema>())           |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<Coptic13Schema>())           |> is<PlainArithmetic>
        SystemArithmetic.CreateDefault(schemaOf<Egyptian12Schema>())         |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<Egyptian13Schema>())         |> is<PlainArithmetic>
        SystemArithmetic.CreateDefault(schemaOf<FrenchRepublican12Schema>()) |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<FrenchRepublican13Schema>()) |> is<PlainArithmetic>
        SystemArithmetic.CreateDefault(schemaOf<GregorianSchema>())          |> is<GregorianArithmetic>
        //SystemArithmetic.CreateDefault(schemaOf<HebrewSchema>())             |> is<LunisolarArithmetic>
        SystemArithmetic.CreateDefault(schemaOf<InternationalFixedSchema>()) |> is<Solar13Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<JulianSchema>())             |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<Persian2820Schema>())        |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<PositivistSchema>())         |> is<Solar13Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<TabularIslamicSchema>())     |> is<LunarArithmetic>
        SystemArithmetic.CreateDefault(schemaOf<TropicaliaSchema>())         |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<Tropicalia3031Schema>())     |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<Tropicalia3130Schema>())     |> is<Solar12Arithmetic>
        SystemArithmetic.CreateDefault(schemaOf<WorldSchema>())              |> is<Solar12Arithmetic>

// We have to test AddDaysViaDayOfMonth() separately because PlainArithmetic
// and RegularArithmetic do not use it internally.
module PlainCase =
    let private sch = schemaOf<GregorianSchema>()
    let private ari = new PlainArithmetic(sch)
    let private wrapper = new ArithmeticWrapper(ari)

    let addDaysData = getAddDaysData ari

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the start of MinYear`` () =
        let min = sch.Segment.MinMaxDateParts.LowerValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(min, -1)) |> overflows

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the end of MaxYear`` () =
        let max = sch.Segment.MinMaxDateParts.UpperValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(max, 1)) |> overflows

    [<Theory; MemberData(nameof(addDaysData))>]
    let ``AddDaysViaDayOfMonth()`` (pair: YemodaPairAnd<int>) =
        let days = pair.Value
        let date = pair.First
        let other = pair.Second

        wrapper.AddDaysViaDayOfMonth(date, days)   === other
        wrapper.AddDaysViaDayOfMonth(other, -days) === date

module RegularCase =
    let private sch = schemaOf<GregorianSchema>()
    let private ari = new RegularArithmetic(sch)
    let private wrapper = new ArithmeticWrapper(ari)

    let addDaysData = getAddDaysData ari

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the start of MinYear`` () =
        let min = sch.Segment.MinMaxDateParts.LowerValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(min, -1)) |> overflows

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the end of MaxYear`` () =
        let max = sch.Segment.MinMaxDateParts.UpperValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(max, 1)) |> overflows

    [<Theory; MemberData(nameof(addDaysData))>]
    let ``AddDaysViaDayOfMonth()`` (pair: YemodaPairAnd<int>) =
        let days = pair.Value
        let date = pair.First
        let other = pair.Second

        wrapper.AddDaysViaDayOfMonth(date, days)   === other
        wrapper.AddDaysViaDayOfMonth(other, -days) === date

module GregorianCase =
    let private ari = new GregorianArithmetic()

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
