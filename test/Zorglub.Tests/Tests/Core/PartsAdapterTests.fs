// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.PartsAdapterTests

open Zorglub.Testing
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Core

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas

open Xunit

module Bundles =
    [<Sealed>]
    type PartsAdapterTests() =
        inherit PartsAdapterFacts<GregorianDataSet>(
            new PartsAdapter(schemaOf<GregorianSchema>()))

module Prelude =
    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new PartsAdapter(null))

