// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

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
let Coptic12Tests () = test(schemaOf<Coptic12Schema>(), Coptic12DataSet.CommonYear, Coptic12DataSet.LeapYear)

[<Fact>]
let Coptic13Tests () = test(schemaOf<Coptic13Schema>(), Coptic13DataSet.CommonYear, Coptic13DataSet.LeapYear)

[<Fact>]
let Egyptian12Tests () = test(schemaOf<Egyptian12Schema>(), Egyptian12DataSet.SampleYear, Egyptian12DataSet.SampleYear)

[<Fact>]
let Egyptian13Tests () = test(schemaOf<Egyptian13Schema>(), Egyptian13DataSet.SampleYear, Egyptian13DataSet.SampleYear)

[<Fact>]
let FrenchRepublican12Tests () = test(schemaOf<FrenchRepublican12Schema>(), FrenchRepublican12DataSet.CommonYear, FrenchRepublican12DataSet.LeapYear)

[<Fact>]
let FrenchRepublican13Tests () = test(schemaOf<FrenchRepublican13Schema>(), FrenchRepublican13DataSet.CommonYear, FrenchRepublican13DataSet.LeapYear)

[<Fact>]
let GregorianTests () = test(schemaOf<GregorianSchema>(), GregorianDataSet.CommonYear, GregorianDataSet.LeapYear)

[<Fact>]
let InternationalFixedTests () = test(schemaOf<InternationalFixedSchema>(), InternationalFixedDataSet.CommonYear, InternationalFixedDataSet.LeapYear)

[<Fact>]
let JulianTests () = test(schemaOf<JulianSchema>(), JulianDataSet.CommonYear, JulianDataSet.LeapYear)

[<Fact>]
let LunisolarTests () = test(schemaOf<LunisolarSchema>(), LunisolarDataSet.CommonYear, LunisolarDataSet.LeapYear)

[<Fact>]
let Persian2820Tests () = test(schemaOf<Persian2820Schema>(), Persian2820DataSet.CommonYear, Persian2820DataSet.LeapYear)

[<Fact>]
let TabularIslamicTests () = test(schemaOf<TabularIslamicSchema>(), TabularIslamicDataSet.CommonYear, TabularIslamicDataSet.LeapYear)

[<Fact>]
let TropicaliaTests () = test(schemaOf<TropicaliaSchema>(), TropicaliaDataSet.CommonYear, TropicaliaDataSet.LeapYear)

[<Fact>]
let Tropicalia3031Tests () = test(schemaOf<Tropicalia3031Schema>(), Tropicalia3031DataSet.CommonYear, Tropicalia3031DataSet.LeapYear)

[<Fact>]
let Tropicalia3130Tests () = test(schemaOf<Tropicalia3130Schema>(), Tropicalia3130DataSet.CommonYear, Tropicalia3130DataSet.LeapYear)
