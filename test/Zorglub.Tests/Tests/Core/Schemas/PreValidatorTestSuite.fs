// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.PreValidatorTestSuite

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

// Solar12PreValidator
[<Sealed>]
type Coptic12Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic12DataSet>(schemaOf<Coptic12Schema>())

// DefaultPreValidator
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic13Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(schemaOf<Coptic13Schema>())

    override x.ValidateMonthDay_AtAbsoluteMaxYear() =
        let validator = x.ValidatorUT
        // DefaultPreValidator: no shortcut for short values of the day of the
        // month and Coptic13Schema.CountDaysInMonth() overflows.
        (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Egyptian12Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian12DataSet>(schemaOf<Egyptian12Schema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Egyptian13Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian13DataSet>(schemaOf<Egyptian13Schema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type FrenchRepublican12Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican12DataSet>(schemaOf<FrenchRepublican12Schema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type FrenchRepublican13Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican13DataSet>(schemaOf<FrenchRepublican13Schema>())

// GregorianPreValidator
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit ICalendricalPreValidatorFacts<GregorianDataSet>(schemaOf<GregorianSchema>())

// Solar13PreValidator
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type InternationalFixedTests() =
    inherit ICalendricalPreValidatorFacts<InternationalFixedDataSet>(schemaOf<InternationalFixedSchema>())

// JulianPreValidator
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type JulianTests() =
    inherit ICalendricalPreValidatorFacts<JulianDataSet>(schemaOf<JulianSchema>())

// LunisolarPreValidator
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit ICalendricalPreValidatorFacts<LunisolarDataSet>(schemaOf<LunisolarSchema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PaxTests() =
    inherit ICalendricalPreValidatorFacts<PaxDataSet>(schemaOf<PaxSchema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Persian2820Tests() =
    inherit ICalendricalPreValidatorFacts<Persian2820DataSet>(schemaOf<Persian2820Schema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type PositivistTests() =
    inherit ICalendricalPreValidatorFacts<PositivistDataSet>(schemaOf<PositivistSchema>())

// LunarPreValidator
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit ICalendricalPreValidatorFacts<TabularIslamicDataSet>(schemaOf<TabularIslamicSchema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TropicaliaTests() =
    inherit ICalendricalPreValidatorFacts<TropicaliaDataSet>(schemaOf<TropicaliaSchema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Tropicalia3031Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3031DataSet>(schemaOf<Tropicalia3031Schema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Tropicalia3130Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3130DataSet>(schemaOf<Tropicalia3130Schema>())

[<TestRedundant>]
[<Sealed; TestExcludeFrom(TestExcludeFrom.Smoke)>]
type WorldTests() =
    inherit ICalendricalPreValidatorFacts<WorldDataSet>(schemaOf<WorldSchema>())
