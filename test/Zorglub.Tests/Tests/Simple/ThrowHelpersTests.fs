// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Simple.ThrowHelpersTests

open Zorglub.Testing
open Zorglub.Time.Simple

open Xunit

let private paramName = "paramName"

[<Fact>]
let BadCuid () =
    argExn paramName (fun () -> ThrowHelpers.BadCuid(paramName, Cuid.Armenian, Cuid.Gregorian))

