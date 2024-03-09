// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Schemas.DaysInMonthDistributionTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core.Schemas

open Xunit

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished), Positivist (unfinished)
// and World (unfinished) schemas.

let private test = DaysInMonthDistributionFacts.Test

[<Fact>]
let Coptic12Tests () =
    test(schemaOf<Coptic12Schema>(), Coptic12DataSet.CommonYear, Coptic12DataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let Coptic13Tests () =
    test(schemaOf<Coptic13Schema>(), Coptic13DataSet.CommonYear, Coptic13DataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let Egyptian12Tests () =
    test(schemaOf<Egyptian12Schema>(), Egyptian12DataSet.SampleYear, Egyptian12DataSet.SampleYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let Egyptian13Tests () =
    test(schemaOf<Egyptian13Schema>(), Egyptian13DataSet.SampleYear, Egyptian13DataSet.SampleYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let FrenchRepublican12Tests () =
    test(schemaOf<FrenchRepublican12Schema>(), FrenchRepublican12DataSet.CommonYear, FrenchRepublican12DataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let FrenchRepublican13Tests () =
    test(schemaOf<FrenchRepublican13Schema>(), FrenchRepublican13DataSet.CommonYear, FrenchRepublican13DataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let GregorianTests () =
    test(schemaOf<GregorianSchema>(), GregorianDataSet.CommonYear, GregorianDataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let InternationalFixedTests () =
    test(schemaOf<InternationalFixedSchema>(), InternationalFixedDataSet.CommonYear, InternationalFixedDataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let JulianTests () =
    test(schemaOf<JulianSchema>(), JulianDataSet.CommonYear, JulianDataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let LunisolarTests () =
    test(schemaOf<LunisolarSchema>(), LunisolarDataSet.CommonYear, LunisolarDataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let Persian2820Tests () =
    test(schemaOf<Persian2820Schema>(), Persian2820DataSet.CommonYear, Persian2820DataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let TabularIslamicTests () =
    test(schemaOf<TabularIslamicSchema>(), TabularIslamicDataSet.CommonYear, TabularIslamicDataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let TropicaliaTests () =
    test(schemaOf<TropicaliaSchema>(), TropicaliaDataSet.CommonYear, TropicaliaDataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let Tropicalia3031Tests () =
    test(schemaOf<Tropicalia3031Schema>(), Tropicalia3031DataSet.CommonYear, Tropicalia3031DataSet.LeapYear)

[<Fact; TestExcludeFrom(TestExcludeFrom.Smoke)>]
let Tropicalia3130Tests () =
    test(schemaOf<Tropicalia3130Schema>(), Tropicalia3130DataSet.CommonYear, Tropicalia3130DataSet.LeapYear)
