// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Arithmetic.SystemArithmeticTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas

open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
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
    let ``Constructor throws for null segment`` () =
        nullExn "segment" (fun () -> new PlainSystemArithmetic(null))
        nullExn "segment" (fun () -> new RegularSystemArithmetic(null))
        nullExn "segment" (fun () -> new LunarSystemArithmetic(null))
        nullExn "segment" (fun () -> new LunisolarSystemArithmetic(null))
        nullExn "segment" (fun () -> new Solar12SystemArithmetic(null))
        nullExn "segment" (fun () -> new Solar13SystemArithmetic(null))

    [<Theory; MemberData(nameof(badLunarProfile))>]
    let ``LunarSystemArithmetic constructor throws for non-lunar schema`` (sch) =
        let seg = SystemSegment.Create(sch, sch.SupportedYears)

        argExn "segment" (fun () -> new LunarSystemArithmetic(seg))

    [<Theory; MemberData(nameof(badLunisolarProfile))>]
    let ``LunisolarSystemArithmetic constructor throws for non-lunisolar schema`` (sch) =
        let seg = SystemSegment.Create(sch, sch.SupportedYears)

        argExn "segment" (fun () -> new LunisolarSystemArithmetic(seg))

    [<Theory; MemberData(nameof(badSolar12Profile))>]
    let ``Solar12SystemArithmetic constructor throws for non-solar12 schema`` (sch) =
        let seg = SystemSegment.Create(sch, sch.SupportedYears)

        argExn "segment" (fun () -> new Solar12SystemArithmetic(seg))

    [<Theory; MemberData(nameof(badSolar13Profile))>]
    let ``Solar13SystemArithmetic constructor throws for non-solar13 schema`` (sch) =
        let seg = SystemSegment.Create(sch, sch.SupportedYears)

        argExn "segment" (fun () -> new Solar13SystemArithmetic(seg))

    [<Theory>]
    [<InlineData 1>]
    [<InlineData 2>]
    [<InlineData 3>]
    [<InlineData 4>]
    [<InlineData 5>]
    [<InlineData 6>]
    let ``RegularSystemArithmetic constructor throws when MinDaysInMonth < 7`` i =
        let sch = FauxSystemSchema.WithMinDaysInMonth(i)
        let seg = SystemSegment.Create(sch, sch.SupportedYears)

        argExn "segment" (fun () -> new RegularSystemArithmetic(seg))

module Factories =
    [<Fact>]
    let ``SystemArithmetic.CreateDefault()`` () =
        SystemArithmetic.CreateDefault(sysegmentOf<Coptic12Schema>())           |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<Coptic13Schema>())           |> is<PlainSystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<Egyptian12Schema>())         |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<Egyptian13Schema>())         |> is<PlainSystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<FrenchRepublican12Schema>()) |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<FrenchRepublican13Schema>()) |> is<PlainSystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<GregorianSchema>())          |> is<GregorianSystemArithmetic>
        //SystemArithmetic.CreateDefault(sysegmentOf<HebrewSchema>())             |> is<LunisolarSystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<InternationalFixedSchema>()) |> is<Solar13SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<JulianSchema>())             |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<Persian2820Schema>())        |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<PositivistSchema>())         |> is<Solar13SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<TabularIslamicSchema>())     |> is<LunarSystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<TropicaliaSchema>())         |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<Tropicalia3031Schema>())     |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<Tropicalia3130Schema>())     |> is<Solar12SystemArithmetic>
        SystemArithmetic.CreateDefault(sysegmentOf<WorldSchema>())              |> is<Solar12SystemArithmetic>

// We have to test AddDaysViaDayOfMonth() separately because PlainSystemArithmetic
// and RegularSystemArithmetic do not use it internally.

module PlainCase =
    let private seg = sysegmentOf<GregorianSchema>()
    let private ari = new PlainSystemArithmetic(seg)
    let private wrapper = new ArithmeticWrapper(ari)

    let addDaysData = getAddDaysData ari

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the start of MinYear`` () =
        let min = ari.Segment.MinMaxDateParts.LowerValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(min, -1)) |> overflows

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the end of MaxYear`` () =
        let max = ari.Segment.MinMaxDateParts.UpperValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(max, 1)) |> overflows

    [<Theory; MemberData(nameof(addDaysData))>]
    let ``AddDaysViaDayOfMonth()`` (pair: YemodaPairAnd<int>) =
        let days = pair.Value
        let date = pair.First
        let other = pair.Second

        wrapper.AddDaysViaDayOfMonth(date, days)   === other
        wrapper.AddDaysViaDayOfMonth(other, -days) === date

module RegularCase =
    let private seg = sysegmentOf<GregorianSchema>()
    let private ari = new RegularSystemArithmetic(seg)
    let private wrapper = new ArithmeticWrapper(ari)

    let addDaysData = getAddDaysData ari

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the start of MinYear`` () =
        let min = ari.Segment.MinMaxDateParts.LowerValue

        (fun () -> wrapper.AddDaysViaDayOfMonth(min, -1)) |> overflows

    [<Fact>]
    let ``AddDaysViaDayOfMonth() overflows at the end of MaxYear`` () =
        let max = ari.Segment.MinMaxDateParts.UpperValue

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
    let private ari = new GregorianSystemArithmetic(sch, sch.SupportedYears)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Solar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Solar.MinDaysInMonth

module LunarCase =
    let private seg = sysegmentOf<TabularIslamicSchema>()
    let private ari = new LunarSystemArithmetic(seg)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Lunar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Lunar.MinDaysInMonth

module LunisolarCase =
    let private seg = sysegmentOf<LunisolarSchema>()
    let private ari = new LunisolarSystemArithmetic(seg)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Lunisolar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Lunisolar.MinDaysInMonth

module Solar12Case =
    let private seg = sysegmentOf<GregorianSchema>()
    let private ari = new Solar12SystemArithmetic(seg)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Solar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Solar.MinDaysInMonth

module Solar13Case =
    let private seg = sysegmentOf<PositivistSchema>()
    let private ari = new Solar13SystemArithmetic(seg)

    [<Fact>]
    let ``Property MaxDaysViaDayOfYear`` () =
        ari.MaxDaysViaDayOfYear === CalendricalConstants.Solar.MinDaysInYear

    [<Fact>]
    let ``Property MaxDaysViaDayOfMonth`` () =
        ari.MaxDaysViaDayOfMonth === CalendricalConstants.Solar.MinDaysInMonth
