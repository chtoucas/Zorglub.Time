﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Schemas.SystemSchemaTestSuite

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Core

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Validation

open Xunit

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

let private verifyThatPreValidatorIs<'a> (sch: ICalendricalSchema) =
    sch.PreValidator |> is<'a>

[<Sealed>]
type CivilTests() =
    inherit SystemSchemaFacts<StandardGregorianDataSet>(syschemaOf<CivilSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        let range = SystemSchema.DefaultSupportedYears.WithMin(1)
        x.SchemaUT.SupportedYears === range

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic12Tests() =
    inherit SystemSchemaFacts<Coptic12DataSet>(syschemaOf<Coptic12Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

[<Sealed>]
[<TestExtrasAssembly>]
type Coptic13Tests() =
    inherit SystemSchemaFacts<Coptic13DataSet>(syschemaOf<Coptic13Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Egyptian12Tests() =
    inherit SystemSchemaFacts<Egyptian12DataSet>(syschemaOf<Egyptian12Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestExtrasAssembly>]
type Egyptian13Tests() =
    inherit SystemSchemaFacts<Egyptian13DataSet>(syschemaOf<Egyptian13Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestExtrasAssembly>]
type FrenchRepublican12Tests() =
    inherit SystemSchemaFacts<FrenchRepublican12DataSet>(syschemaOf<FrenchRepublican12Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestExtrasAssembly>]
type FrenchRepublican13Tests() =
    inherit SystemSchemaFacts<FrenchRepublican13DataSet>(syschemaOf<FrenchRepublican13Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit SystemSchemaFacts<GregorianDataSet>(syschemaOf<GregorianSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
module HebrewTests =
    let private sch = syschemaOf<HebrewSchema>()

    [<Fact>]
    let ``Property Algorithm`` () = sch.Algorithm === CalendricalAlgorithm.Arithmetical

    [<Fact>]
    let ``Property Family`` () = sch.Family === CalendricalFamily.Lunisolar

    [<Fact>]
    let ``Property PeriodicAdjustments`` () = sch.PeriodicAdjustments === CalendricalAdjustments.DaysAndMonths

    [<Fact>]
    let ``Property Profile`` () = sch.Profile === CalendricalProfile.Lunisolar

    [<Fact>]
    let ``Property PreValidator`` () = verifyThatPreValidatorIs<LunisolarPreValidator>(sch)

    [<Fact>]
    let ``IsRegular()`` () = sch.IsRegular() === (false, 0)

[<Sealed>]
[<TestExtrasAssembly>]
type InternationalFixedTests() =
    inherit SystemSchemaFacts<InternationalFixedDataSet>(syschemaOf<InternationalFixedSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type JulianTests() =
    inherit SystemSchemaFacts<JulianDataSet>(syschemaOf<JulianSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<JulianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit SystemSchemaFacts<LunisolarDataSet>(syschemaOf<LunisolarSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunisolar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Months
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunisolar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunisolarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

[<Sealed>]
[<TestExtrasAssembly>]
type PaxTests() as self =
    inherit ICalendricalSchemaBasicFacts<PaxSchema, PaxDataSet>(syschemaOf<PaxSchema>())
    do
        self.TestGetMonthAnyway <- true

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Other
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Weeks
    override x.PreValidator_Prop() = verifyThatPreValidatorIs<PlainPreValidator>(x.SchemaUT)
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

    override x.SupportedYears_Prop() =
        let range = SystemSchema.DefaultSupportedYears.WithMin(1)
        x.SchemaUT.SupportedYears === range

    [<Fact>]
    member x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other

[<Sealed>]
[<TestExtrasAssembly>]
type Persian2820Tests() =
    inherit SystemSchemaFacts<Persian2820DataSet>(syschemaOf<Persian2820Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.StartingAt(Int32.MinValue + Persian2820Schema.Year0)

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MinValue``() =
        let sch = x.SchemaUT

        // No overflow if y >= Int32.MinValue + Persian2820Schema.Year0.
        // It is a bit more complicated, it depends on the value of month.

        // No underflow if m < 12.
        [for m in 1 .. 11 -> sch.CountDaysInMonth(Int32.MinValue, m)] |> ignore

        for y0 in 0 .. (Persian2820Schema.Year0 - 1) do
            for m in 12 .. x.MaxMonth do
                (fun () -> sch.CountDaysInMonth(Int32.MinValue + y0, m)) |> overflows

[<Sealed>]
[<TestExtrasAssembly>]
type PositivistTests() =
    inherit SystemSchemaFacts<PositivistDataSet>(syschemaOf<PositivistSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit SystemSchemaFacts<TabularIslamicDataSet>(syschemaOf<TabularIslamicSchema>()) with

    let isnot12 m = m <> 12

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        x.SchemaUT.SupportedYears === Range.Create(-199_999, 200_000)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.Create(-199_999, 200_000)

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MinValue``() =
        let sch = x.SchemaUT
        let countDaysInMonth m = sch.CountDaysInMonth(Int32.MinValue, m)
        let maxMonth = x.MaxMonth

        (fun () -> countDaysInMonth 12) |> overflows
        // No overflow if m != 12.
        [1 .. maxMonth] |> List.filter isnot12 |> List.map countDaysInMonth

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        let countDaysInMonth m = sch.CountDaysInMonth(Int32.MaxValue, m)
        let maxMonth = x.MaxMonth

        (fun () -> countDaysInMonth 12) |> overflows
        // No overflow if m != 12.
        [1 .. maxMonth] |> List.filter isnot12 |> List.map countDaysInMonth

[<Sealed>]
[<TestExtrasAssembly>]
type TropicaliaTests() =
    inherit SystemSchemaFacts<TropicaliaDataSet>(syschemaOf<TropicaliaSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestExtrasAssembly>]
type Tropicalia3031Tests() =
    inherit SystemSchemaFacts<Tropicalia3031DataSet>(syschemaOf<Tropicalia3031Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestExtrasAssembly>]
type Tropicalia3130Tests() =
    inherit SystemSchemaFacts<Tropicalia3130DataSet>(syschemaOf<Tropicalia3130Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestExtrasAssembly>]
type WorldTests() =
    inherit SystemSchemaFacts<WorldDataSet>(syschemaOf<WorldSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
