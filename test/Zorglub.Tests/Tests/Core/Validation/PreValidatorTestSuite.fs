// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.PreValidatorTestSuite

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Validation

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

// Solar12PreValidator
[<Sealed>]
type Coptic12Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic12DataSet>(schemaOf<Coptic12Schema>())

    member x.PreValidator() = x.PreValidatorUT |> is<Solar12PreValidator>

// PlainPreValidator
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type Coptic13Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(schemaOf<Coptic13Schema>())

    member x.PreValidator() = x.PreValidatorUT |> is<PlainPreValidator>

    override x.ValidateMonthDay_AtAbsoluteMaxYear() =
        let validator = x.PreValidatorUT
        // PlainPreValidator: no shortcut for short values of the day of the
        // month and Coptic13Schema.CountDaysInMonth() overflows.
        (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian12Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian12DataSet>(schemaOf<Egyptian12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Egyptian13Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian13DataSet>(schemaOf<Egyptian13Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican12Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican12DataSet>(schemaOf<FrenchRepublican12Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type FrenchRepublican13Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican13DataSet>(schemaOf<FrenchRepublican13Schema>())

// GregorianPreValidator
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit ICalendricalPreValidatorFacts<GregorianDataSet>(schemaOf<GregorianSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<GregorianPreValidator>

// Solar13PreValidator
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type InternationalFixedTests() =
    inherit ICalendricalPreValidatorFacts<InternationalFixedDataSet>(schemaOf<InternationalFixedSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<Solar13PreValidator>

// JulianPreValidator
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type JulianTests() =
    inherit ICalendricalPreValidatorFacts<JulianDataSet>(schemaOf<JulianSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<JulianPreValidator>

// LunisolarPreValidator
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type LunisolarTests() =
    inherit ICalendricalPreValidatorFacts<LunisolarDataSet>(schemaOf<LunisolarSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<LunisolarPreValidator>

[<Sealed>]
[<RedundantTestBundle>]
type PaxTests() =
    inherit ICalendricalPreValidatorFacts<PaxDataSet>(schemaOf<PaxSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Persian2820Tests() =
    inherit ICalendricalPreValidatorFacts<Persian2820DataSet>(schemaOf<Persian2820Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type PositivistTests() =
    inherit ICalendricalPreValidatorFacts<PositivistDataSet>(schemaOf<PositivistSchema>())

// LunarPreValidator
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type TabularIslamicTests() =
    inherit ICalendricalPreValidatorFacts<TabularIslamicDataSet>(schemaOf<TabularIslamicSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<LunarPreValidator>

[<Sealed>]
[<RedundantTestBundle>]
type TropicaliaTests() =
    inherit ICalendricalPreValidatorFacts<TropicaliaDataSet>(schemaOf<TropicaliaSchema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3031Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3031DataSet>(schemaOf<Tropicalia3031Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type Tropicalia3130Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3130DataSet>(schemaOf<Tropicalia3130Schema>())

[<Sealed>]
[<RedundantTestBundle>]
type WorldTests() =
    inherit ICalendricalPreValidatorFacts<WorldDataSet>(schemaOf<WorldSchema>())
