// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Simple.CuidTests

open Zorglub.Testing
open Zorglub.Testing.Data

open Zorglub.Time.Simple

open Xunit

open Zorglub.Time.FSharpExtensions

let unfixedCuidData = EnumDataSet.UnfixedCuidData
let fixedCuidData = EnumDataSet.FixedCuidData

[<Theory; MemberData(nameof(unfixedCuidData))>]
let ``IsFixed() returns false when the id is not fixed`` (cuid: Cuid) =
    isFixed cuid |> nok

[<Theory; MemberData(nameof(fixedCuidData))>]
let ``IsFixed() returns true when the id is fixed`` (cuid: Cuid) =
    isFixed cuid |> ok
