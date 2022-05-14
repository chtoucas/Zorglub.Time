// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.SystemSchemaTestSuite

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Core

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Arithmetic
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Validation

open Xunit

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

let private instanceOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    SchemaActivator.CreateInstance<'a>()

// Type to avoid the error FS0405 because TryGetCustomArithmetic() is a
// protected internal method.
[<Sealed>]
type private SchemaWrapper(schema: SystemSchema) =
    member __.Schema = schema
    member x.TryGetCustomArithmetic() = x.Schema.TryGetCustomArithmetic()

let private verifyThatPreValidatorIs<'a> (sch: ICalendricalSchema) =
    sch.PreValidator |> is<'a>

let private verifyThatArithmeticIs<'a> (sch: ICalendricalSchema) =
    sch.Arithmetic |> is<'a>

let private verifyThatTryGetCustomArithmeticSucceeds<'a> (sch: SystemSchema) =
    let wrapper = new SchemaWrapper(sch)
    let passed, arithmetic = wrapper.TryGetCustomArithmetic()
    passed |> ok
    arithmetic |> is<'a>

[<Sealed>]
type Coptic12Tests() =
    inherit SystemSchemaFacts<Coptic12DataSet>(instanceOf<Coptic12Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic13Tests() =
    inherit SystemSchemaFacts<Coptic13DataSet>(instanceOf<Coptic13Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<DefaultPreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<DefaultArithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticFails()

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    static member VirtualMonth_Prop() = Coptic13Schema.VirtualMonth === 13

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Egyptian12Tests() =
    inherit SystemSchemaFacts<Egyptian12DataSet>(instanceOf<Egyptian12Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Egyptian13Tests() =
    inherit SystemSchemaFacts<Egyptian13DataSet>(instanceOf<Egyptian13Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<DefaultPreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<DefaultArithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticFails()

    [<Fact>]
    static member VirtualMonth_Prop() = Egyptian13Schema.VirtualMonth === 13

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type FrenchRepublican12Tests() =
    inherit SystemSchemaFacts<FrenchRepublican12DataSet>(instanceOf<FrenchRepublican12Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type FrenchRepublican13Tests() =
    inherit SystemSchemaFacts<FrenchRepublican13DataSet>(instanceOf<FrenchRepublican13Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<DefaultPreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<DefaultArithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticFails()

    [<Fact>]
    static member VirtualMonth_Prop() = FrenchRepublican13Schema.VirtualMonth === 13

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit SystemSchemaFacts<GregorianDataSet>(instanceOf<GregorianSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<GregorianArithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<GregorianArithmetic>()

    // Misc

    [<Fact>]
    member x.MinYear_Const() = GregorianSchema.MinYear === x.SchemaUT.SupportedYears.Min

    [<Fact>]
    member x.MaxYear_Const() = GregorianSchema.MaxYear === x.SchemaUT.SupportedYears.Max

    [<Fact>]
    member x.DaysIn4YearCycle_Prop() =
        // We also check DaysInCommonYear, DaysInLeapYear and DaysPer4YearSubcycle.

        let sch = x.SchemaUT
        let daysInYear0 = sch.CountDaysSinceEpoch(1, 1, 1) - sch.CountDaysSinceEpoch(0, 1, 1)
        let daysInYear1 = sch.CountDaysSinceEpoch(2, 1, 1) - sch.CountDaysSinceEpoch(1, 1, 1)
        let daysInYear2 = sch.CountDaysSinceEpoch(3, 1, 1) - sch.CountDaysSinceEpoch(2, 1, 1)
        let daysInYear3 = sch.CountDaysSinceEpoch(4, 1, 1) - sch.CountDaysSinceEpoch(3, 1, 1)

        let daysIn4YearCycle = GJSchema.DaysIn4YearCycle
        daysIn4YearCycle.Length === 4
        int(daysIn4YearCycle.[0]) === daysInYear0
        int(daysIn4YearCycle.[1]) === daysInYear1
        int(daysIn4YearCycle.[2]) === daysInYear2
        int(daysIn4YearCycle.[3]) === daysInYear3

        daysInYear0 === GJSchema.DaysInLeapYear // Year 0 is leap
        daysInYear1 === GJSchema.DaysInCommonYear
        daysInYear2 === GJSchema.DaysInCommonYear
        daysInYear3 === GJSchema.DaysInCommonYear

        let sum = Array.sum <| daysIn4YearCycle.ToArray()
        int(sum) === GregorianSchema.DaysPer4YearSubcycle

    [<Fact>]
    member x.DaysIn4CenturyCycle_Prop() =
        // We also check DaysPer100YearSubcycle.

        let sch = x.SchemaUT
        let daysInCentury0 = sch.CountDaysSinceEpoch(1, 1, 1) - sch.CountDaysSinceEpoch(-99, 1, 1)
        let daysInCentury1 = sch.CountDaysSinceEpoch(101, 1, 1) - sch.CountDaysSinceEpoch(1, 1, 1)
        let daysInCentury2 = sch.CountDaysSinceEpoch(201, 1, 1) - sch.CountDaysSinceEpoch(101, 1, 1)
        let daysInCentury3 = sch.CountDaysSinceEpoch(301, 1, 1) - sch.CountDaysSinceEpoch(201, 1, 1)

        let daysIn4CenturyCycle = GregorianSchema.DaysIn4CenturyCycle
        daysIn4CenturyCycle.Length === 4
        int(daysIn4CenturyCycle.[0]) === daysInCentury0
        int(daysIn4CenturyCycle.[1]) === daysInCentury1
        int(daysIn4CenturyCycle.[2]) === daysInCentury2
        int(daysIn4CenturyCycle.[3]) === daysInCentury3

        daysInCentury0 === GregorianSchema.DaysPer100YearSubcycle + 1 // Long century
        daysInCentury1 === GregorianSchema.DaysPer100YearSubcycle
        daysInCentury2 === GregorianSchema.DaysPer100YearSubcycle
        daysInCentury3 === GregorianSchema.DaysPer100YearSubcycle

    [<Theory>]
    [<InlineData -10>] [<InlineData -9>] [<InlineData -8>] [<InlineData -7>] [<InlineData -6>] [<InlineData -5>]
    [<InlineData -4>] [<InlineData -3>] [<InlineData -2>] [<InlineData -1>] [<InlineData 0>] [<InlineData 1>]
    [<InlineData 2>] [<InlineData 3>] [<InlineData 4>] [<InlineData 5>] [<InlineData 6>] [<InlineData 7>]
    [<InlineData 8>] [<InlineData 9>] [<InlineData 10>]
    member x.DaysPer400YearCycle_Const c =
        let sch = x.SchemaUT
        let daysInCycle = sch.CountDaysSinceEpoch((c + 1) * 400, 1, 1) - sch.CountDaysSinceEpoch(c * 400, 1, 1)
        GregorianSchema.DaysPer400YearCycle === daysInCycle

[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
module HebrewTests =
    let private sch = instanceOf<HebrewSchema>()

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
    let ``Property Arithmetic`` () = verifyThatArithmeticIs<LunisolarArithmetic>(sch)

    [<Fact>]
    let ``IsRegular()`` () = sch.IsRegular() === (false, 0)

    [<Fact>]
    let ``TryGetCustomArithmetic()`` () = verifyThatTryGetCustomArithmeticSucceeds<LunisolarArithmetic>(sch)

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type InternationalFixedTests() =
    inherit SystemSchemaFacts<InternationalFixedDataSet>(instanceOf<InternationalFixedSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar13Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar13Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type JulianTests() =
    inherit SystemSchemaFacts<JulianDataSet>(instanceOf<JulianSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<JulianPreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit SystemSchemaFacts<LunisolarDataSet>(instanceOf<LunisolarSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunisolar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Months
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunisolar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunisolarPreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<LunisolarArithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<LunisolarArithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PaxTests() as self =
    inherit ICalendricalSchemaBasicFacts<PaxSchema, PaxDataSet>(instanceOf<PaxSchema>())
    do
        self.TestGetMonthAnyway <- true

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Other
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Weeks
    override x.PreValidator_Prop() = verifyThatPreValidatorIs<DefaultPreValidator>(x.SchemaUT)
    override x.Arithmetic_Prop() = verifyThatArithmeticIs<DefaultFastArithmetic>(x.SchemaUT)
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

    override x.SupportedYears_Prop() =
        let range = SystemSchema.DefaultSupportedYears.WithMin(1)
        x.SchemaUT.SupportedYears === range

    [<Fact>]
    member x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other

    [<Fact>]
    member x.TryGetCustomArithmetic() =
        verifyThatTryGetCustomArithmeticSucceeds<DefaultFastArithmetic>(x.SchemaUT)

    [<Fact>]
    member x.FirstDayOfWeek_Prop() = x.SchemaUT.FirstDayOfWeek === DayOfWeek.Sunday

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Persian2820Tests() =
    inherit SystemSchemaFacts<Persian2820DataSet>(instanceOf<Persian2820Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

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

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PositivistTests() =
    inherit SystemSchemaFacts<PositivistDataSet>(instanceOf<PositivistSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar13Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar13Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit SystemSchemaFacts<TabularIslamicDataSet>(instanceOf<TabularIslamicSchema>()) with

    let isnot12 m = m <> 12

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunarPreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<LunarArithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<LunarArithmetic>()

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

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TropicaliaTests() =
    inherit SystemSchemaFacts<TropicaliaDataSet>(instanceOf<TropicaliaSchema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Tropicalia3031Tests() =
    inherit SystemSchemaFacts<Tropicalia3031DataSet>(instanceOf<Tropicalia3031Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Tropicalia3130Tests() =
    inherit SystemSchemaFacts<Tropicalia3130DataSet>(instanceOf<Tropicalia3130Schema>())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type WorldTests() =
    inherit SystemSchemaFacts<WorldDataSet>(instanceOf<WorldSchema>())

    static member MoreMonthInfoData = WorldDataSet.MoreMonthInfoData

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.Arithmetic_Prop() = x.VerifyThatArithmeticIs<Solar12Arithmetic>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
    override x.TryGetCustomArithmetic() = x.VerifyThatTryGetCustomArithmeticSucceeds<Solar12Arithmetic>()

    [<Theory; MemberData(nameof(WorldTests.MoreMonthInfoData))>]
    static member CountDaysInWorldMonth _ m daysInMonth =
        WorldSchema.CountDaysInWorldMonth(m) === daysInMonth
