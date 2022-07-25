// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.MonthCalculatorTests

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
        let calc = MonthCalculator.Create(sch) :> ISchemaBound

        calc.Schema ==& sch

module Factories =
    [<Fact>]
    let ``Create() throws for null schema`` () =
        nullExn "schema" (fun () -> MonthCalculator.Create(null))

    [<Fact>]
    let ``Create()`` () =
        MonthCalculator.Create(FauxCalendricalSchema.Regular14)      |> is<MonthCalculator.RegularCase>

        MonthCalculator.Create(schemaOf<Coptic12Schema>())           |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<Coptic13Schema>())           |> is<MonthCalculator.Regular13Case>
        MonthCalculator.Create(schemaOf<Egyptian12Schema>())         |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<Egyptian13Schema>())         |> is<MonthCalculator.Regular13Case>
        MonthCalculator.Create(schemaOf<FrenchRepublican12Schema>()) |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<FrenchRepublican13Schema>()) |> is<MonthCalculator.Regular13Case>
        MonthCalculator.Create(schemaOf<GregorianSchema>())          |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<HebrewSchema>())             |> is<MonthCalculator.PlainCase>
        MonthCalculator.Create(schemaOf<InternationalFixedSchema>()) |> is<MonthCalculator.Regular13Case>
        MonthCalculator.Create(schemaOf<JulianSchema>())             |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<Persian2820Schema>())        |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<PositivistSchema>())         |> is<MonthCalculator.Regular13Case>
        MonthCalculator.Create(schemaOf<TabularIslamicSchema>())     |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<TropicaliaSchema>())         |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<Tropicalia3031Schema>())     |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<Tropicalia3130Schema>())     |> is<MonthCalculator.Regular12Case>
        MonthCalculator.Create(schemaOf<WorldSchema>())              |> is<MonthCalculator.Regular12Case>

module PlainCase =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let calculator = new MonthCalculator.PlainCase(schemaOf<GregorianSchema>())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module RegularCase =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let calculator = new MonthCalculator.RegularCase(schemaOf<GregorianSchema>(), 12)

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module Regular12Case =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let calculator = new MonthCalculator.Regular12Case(schemaOf<GregorianSchema>())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module Regular13Case =
    let private dataSet = Coptic13DataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let calculator = new MonthCalculator.Regular13Case(schemaOf<Coptic13Schema>())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calculator.GetEndOfYear(x.Year) === x.MonthsSinceEpoch
