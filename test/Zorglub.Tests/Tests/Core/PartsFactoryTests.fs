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

// TODO(code): test PartsFactory with a schema for which
// Yemoda.Create() & co throw.

module Bundles =
    [<Sealed>]
    type PartsFactoryCheckedTests() =
        inherit ICalendricalPartsFactoryFacts<GregorianDataSet>(
            new PartsFactoryChecked(schemaOf<GregorianSchema>()))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type PartsFactoryUncheckedTests() =
        inherit ICalendricalPartsFactoryFacts<GregorianDataSet>(
            new PartsFactoryUnchecked(schemaOf<GregorianSchema>()))

module Factories =
    [<Fact>]
    let ``ICalendricalPartsFactory.Create() throws for null schema`` () =
        nullExn "schema" (fun () -> ICalendricalPartsFactory2.Create(null))

module Methods =
    [<Sealed>]
    type BadYearData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(Yemoda.MinYear - 1)
            self.Add(Yemoda.MaxYear + 1)
            self.Add(Int32.MaxValue)

    let private sch = schemaOf<GregorianSchema>()
    let private factory = new PartsFactoryChecked(sch)

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetMonthPartsAtStartOfYear() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> factory.GetMonthPartsAtStartOfYear(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetDatePartsAtStartOfYear() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> factory.GetDatePartsAtStartOfYear(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetOrdinalPartsAtStartOfYear() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> factory.GetOrdinalPartsAtStartOfYear(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetDatePartsAtStartOfMonth() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> factory.GetDatePartsAtStartOfMonth(y, 1))
