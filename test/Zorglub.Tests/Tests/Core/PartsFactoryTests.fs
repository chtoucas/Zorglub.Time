// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.PartsFactoryTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit

module Bundles =
    [<Sealed>]
    type CalendricalPartsFactoryCheckedTests() =
        inherit ICalendricalPartsFactoryFacts<GregorianDataSet>(
            new CalendricalPartsFactoryChecked(schemaOf<GregorianSchema>()))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type CalendricalPartsFactoryUncheckedTests() =
        inherit ICalendricalPartsFactoryFacts<GregorianDataSet>(
            new CalendricalPartsFactoryUnchecked(schemaOf<GregorianSchema>()))

module Prelude =
    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new CalendricalPartsFactoryChecked(null))
        nullExn "schema" (fun () -> new CalendricalPartsFactoryUnchecked(null))

module Factories =
    [<Sealed>]
    type BadYearData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(Yemoda.MinYear - 1)
            self.Add(Yemoda.MaxYear + 1)
            self.Add(Int32.MaxValue)

    let private factory = new CalendricalPartsFactoryChecked(schemaOf<GregorianSchema>())

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetStartOfYearParts() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> factory.GetStartOfYearParts(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetStartOfYearOrdinalParts() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> factory.GetStartOfYearOrdinalParts(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetStartOfMonthParts() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> factory.GetStartOfMonthParts(y, 1))
