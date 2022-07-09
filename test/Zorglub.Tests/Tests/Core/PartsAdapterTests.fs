// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.PartsAdapterTests

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit
open FsCheck.Xunit

module Bundles =
    [<Sealed>]
    type PartsAdapterTests() =
        inherit PartsAdapterFacts<GregorianDataSet>(
            new PartsAdapter(schemaOf<GregorianSchema>()))

module Prelude =
    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new PartsAdapter(null))

module Methods =
    [<Property>]
    let ``GetMonthPartsAtStartOfYear()`` y =
        let parts = new MonthParts(y, 1)

        PartsAdapter.GetMonthPartsAtStartOfYear(y) === parts

    [<Property>]
    let ``GetDatePartsAtStartOfYear()`` y =
        let parts = new DateParts(y, 1, 1)

        PartsAdapter.GetDatePartsAtStartOfYear(y) === parts

    [<Property>]
    let ``GetOrdinalPartsAtStartOfYear()`` y =
        let parts = new OrdinalParts(y, 1)

        PartsAdapter.GetOrdinalPartsAtStartOfYear(y) === parts

    [<Property>]
    let ``GetDatePartsAtStartOfMonth()`` y m =
        let parts = new DateParts(y, m, 1)

        PartsAdapter.GetDatePartsAtStartOfMonth(y, m) === parts

