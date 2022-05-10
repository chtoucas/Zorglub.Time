// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.MathTTests

open Zorglub.Testing
open Zorglub.Time.Core.Utilities

open Xunit

[<Fact>]
let ``Min()`` () =
    MathT.Min(1, 1) === 1
    MathT.Min(2, 2) === 2
    MathT.Min(1, 2) === 1
    MathT.Min(2, 1) === 1

[<Fact>]
let ``Max()`` () =
    MathT.Max(1, 1) === 1
    MathT.Max(2, 2) === 2
    MathT.Max(1, 2) === 2
    MathT.Max(2, 1) === 2
