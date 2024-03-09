// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Utilities.MathNTests

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
let ``Divide()`` m q r =
    MathN.Divide(m, 3) === (q, r)

[<Theory>]
[<InlineData(7, 3)>]
[<InlineData(6, 2)>]
[<InlineData(5, 2)>]
[<InlineData(4, 2)>]
[<InlineData(3, 1)>]
[<InlineData(2, 1)>]
[<InlineData(1, 1)>]
[<InlineData(0, 0)>]
let ``AdjustedDivide()`` m q =
    MathN.AdjustedDivide(m, 3) === q

[<Theory>]
[<InlineData(7, 1)>]
[<InlineData(6, 3)>]
[<InlineData(5, 2)>]
[<InlineData(4, 1)>]
[<InlineData(3, 3)>]
[<InlineData(2, 2)>]
[<InlineData(1, 1)>]
[<InlineData(0, 3)>]
let ``AdjustedModulo()`` m r =
    MathN.AdjustedModulo(m, 3) === r

[<Theory>]
[<InlineData(7, 3, 2)>]
[<InlineData(6, 3, 1)>]
[<InlineData(5, 2, 3)>]
[<InlineData(4, 2, 2)>]
[<InlineData(3, 2, 1)>]
[<InlineData(2, 1, 3)>]
[<InlineData(1, 1, 2)>]
[<InlineData(0, 1, 1)>]
let ``AugmentedDivide()`` m q r =
    MathN.AugmentedDivide(m, 3) === (q, r)
