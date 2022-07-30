// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Bulgroz.Obsolete.SchemaAdapterTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Bulgroz.Obsolete
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit

// TODO(code): test SchemaAdapter with a schema for which
// Yemoda.Create() & co throw.

module Bundles =
    [<Sealed>]
    type SchemaAdapterCheckedTests() =
        inherit ISchemaAdapterFacts<GregorianDataSet>(
            new SchemaAdapterChecked(schemaOf<GregorianSchema>()))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type SchemaAdapterSlimTests() =
        inherit ISchemaAdapterFacts<GregorianDataSet>(
            new SchemaAdapterSlim(schemaOf<GregorianSchema>()))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Smoke)>]
    type SchemaAdapterUncheckedTests() =
        inherit ISchemaAdapterFacts<GregorianDataSet>(
            new SchemaAdapterUnchecked(schemaOf<GregorianSchema>()))

module Factories =
    [<Fact>]
    let ``ISchemaAdapter.Create() throws for null schema`` () =
        nullExn "schema" (fun () -> ISchemaAdapter.Create(null))

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
    let private adapter = new SchemaAdapterChecked(sch)

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetMonthPartsAtStartOfYear() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> adapter.GetMonthPartsAtStartOfYear(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetDatePartsAtStartOfYear() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> adapter.GetDatePartsAtStartOfYear(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetOrdinalPartsAtStartOfYear() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> adapter.GetOrdinalPartsAtStartOfYear(y))

    [<Theory; ClassData(typeof<BadYearData>)>]
    let ``GetDatePartsAtStartOfMonth() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> adapter.GetDatePartsAtStartOfMonth(y, 1))
