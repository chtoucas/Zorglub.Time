// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.PreValidatorTestSuite

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Time.Core.Schemas

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

type [<Sealed>] Coptic12Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic12DataSet>(schemaOf<Coptic12Schema>())

type [<Sealed>] Coptic13Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(schemaOf<Coptic13Schema>())

    override x.ValidateMonthDay_AtAbsoluteMaxYear() =
        let validator = x.ValidatorUT
        // DefaultPreValidator: no shortcut for short values of the day of the
        // month and Coptic13Schema.CountDaysInMonth() overflows.
        (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

type [<Sealed>] Egyptian12Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian12DataSet>(schemaOf<Egyptian12Schema>())

type [<Sealed>] Egyptian13Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian13DataSet>(schemaOf<Egyptian13Schema>())

type [<Sealed>] FrenchRepublican12Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican12DataSet>(schemaOf<FrenchRepublican12Schema>())

type [<Sealed>] FrenchRepublican13Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican13DataSet>(schemaOf<FrenchRepublican13Schema>())

type [<Sealed>] GregorianTests() =
    inherit ICalendricalPreValidatorFacts<GregorianDataSet>(schemaOf<GregorianSchema>())

type [<Sealed>] InternationalFixedTests() =
    inherit ICalendricalPreValidatorFacts<InternationalFixedDataSet>(schemaOf<InternationalFixedSchema>())

type [<Sealed>] JulianTests() =
    inherit ICalendricalPreValidatorFacts<JulianDataSet>(schemaOf<JulianSchema>())

type [<Sealed>] LunisolarTests() =
    inherit ICalendricalPreValidatorFacts<LunisolarDataSet>(schemaOf<LunisolarSchema>())

type [<Sealed>] PaxTests() =
    inherit ICalendricalPreValidatorFacts<PaxDataSet>(schemaOf<PaxSchema>())

type [<Sealed>] Persian2820Tests() =
    inherit ICalendricalPreValidatorFacts<Persian2820DataSet>(schemaOf<Persian2820Schema>())

type [<Sealed>] PositivistTests() =
    inherit ICalendricalPreValidatorFacts<PositivistDataSet>(schemaOf<PositivistSchema>())

type [<Sealed>] TabularIslamicTests() =
    inherit ICalendricalPreValidatorFacts<TabularIslamicDataSet>(schemaOf<TabularIslamicSchema>())

type [<Sealed>] TropicaliaTests() =
    inherit ICalendricalPreValidatorFacts<TropicaliaDataSet>(schemaOf<TropicaliaSchema>())

type [<Sealed>] Tropicalia3031Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3031DataSet>(schemaOf<Tropicalia3031Schema>())

type [<Sealed>] Tropicalia3130Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3130DataSet>(schemaOf<Tropicalia3130Schema>())

type [<Sealed>] WorldTests() =
    inherit ICalendricalPreValidatorFacts<WorldDataSet>(schemaOf<WorldSchema>())
