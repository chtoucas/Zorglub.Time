// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.MonthsCalculatorTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit

module Prelude =
    [<Fact>]
    let ``Property Schema`` () =
        let sch = new GregorianSchema()
        let calc = MonthsCalculator.Create(sch) :> ISchemaBound

        calc.Schema ==& sch

module Factories =
    [<Fact>]
    let ``Create() throws for null schema`` () =
        nullExn "schema" (fun () -> MonthsCalculator.Create(null))

    [<Fact>]
    let ``Create()`` () =
        MonthsCalculator.Create(FauxCalendricalSchema.Regular14)      |> is<MonthsCalculator.Regular>

        MonthsCalculator.Create(schemaOf<Coptic12Schema>())           |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<Coptic13Schema>())           |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(schemaOf<Egyptian12Schema>())         |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<Egyptian13Schema>())         |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(schemaOf<FrenchRepublican12Schema>()) |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<FrenchRepublican13Schema>()) |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(schemaOf<GregorianSchema>())          |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<HebrewSchema>())             |> is<MonthsCalculator.Plain>
        MonthsCalculator.Create(schemaOf<InternationalFixedSchema>()) |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(schemaOf<JulianSchema>())             |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<Persian2820Schema>())        |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<PositivistSchema>())         |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(schemaOf<TabularIslamicSchema>())     |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<TropicaliaSchema>())         |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<Tropicalia3031Schema>())     |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<Tropicalia3130Schema>())     |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(schemaOf<WorldSchema>())              |> is<MonthsCalculator.Regular12>

module PlainCase =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Plain(schemaOf<GregorianSchema>())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module RegularCase =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Regular(schemaOf<GregorianSchema>(), 12)

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module Regular12Case =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Regular12(schemaOf<GregorianSchema>())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module Regular13Case =
    let private dataSet = Coptic13DataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Regular13(schemaOf<Coptic13Schema>())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch
