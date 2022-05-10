// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.MathUTests

open Zorglub.Testing
open Zorglub.Time.Core.Utilities

open Xunit

[<Theory>]
[<InlineData(7, 2, 1)>]
[<InlineData(6, 2, 0)>]
[<InlineData(5, 1, 2)>]
[<InlineData(4, 1, 1)>]
[<InlineData(3, 1, 0)>]
[<InlineData(2, 0, 2)>]
[<InlineData(1, 0, 1)>]
[<InlineData(0, 0, 0)>]
let ``Divide()`` (m: uint) (q: uint) (r: uint) =
    MathU.Divide(m, 3u) === (q, r)

[<Theory>]
[<InlineData(7, 3)>]
[<InlineData(6, 2)>]
[<InlineData(5, 2)>]
[<InlineData(4, 2)>]
[<InlineData(3, 1)>]
[<InlineData(2, 1)>]
[<InlineData(1, 1)>]
[<InlineData(0, 0)>]
let ``AdjustedDivide()`` (m: uint) (q: uint) =
    MathU.AdjustedDivide(m, 3u) === q

[<Theory>]
[<InlineData(7, 1)>]
[<InlineData(6, 3)>]
[<InlineData(5, 2)>]
[<InlineData(4, 1)>]
[<InlineData(3, 3)>]
[<InlineData(2, 2)>]
[<InlineData(1, 1)>]
[<InlineData(0, 3)>]
let ``AdjustedModulo()`` (m: uint) (r: uint) =
    MathU.AdjustedModulo(m, 3u) === r
