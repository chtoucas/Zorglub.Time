// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.MathZTests

open Zorglub.Testing

open Zorglub.Time.Core.Utilities

open Xunit

[<Theory>]
[<InlineData(4, 1)>]
[<InlineData(3, 1)>]
[<InlineData(2, 0)>]
[<InlineData(1, 0)>]
[<InlineData(0, 0)>]
[<InlineData(-1, -1)>]
[<InlineData(-2, -1)>]
[<InlineData(-3, -1)>]
[<InlineData(-4, -2)>]
let ``Divide() rounds towards minus infinity`` m q =
    MathZ.Divide(m, 3) === q

[<Theory>]
[<InlineData(4, 1, 1)>]
[<InlineData(3, 1, 0)>]
[<InlineData(2, 0, 2)>]
[<InlineData(1, 0, 1)>]
[<InlineData(0, 0, 0)>]
[<InlineData(-1, -1, 2)>]
[<InlineData(-2, -1, 1)>]
[<InlineData(-3, -1, 0)>]
[<InlineData(-4, -2, 2)>]
let ``Divide() with remainder`` m q r =
    let mutable rA = 0
    let qA = MathZ.Divide(m, 3, &rA)

    (qA, rA) === (q, r)

[<Theory>]
[<InlineData(4, 1)>]
[<InlineData(3, 1)>]
[<InlineData(2, 0)>]
[<InlineData(1, 0)>]
[<InlineData(0, 0)>]
[<InlineData(-1, -1)>]
[<InlineData(-2, -1)>]
[<InlineData(-3, -1)>]
[<InlineData(-4, -2)>]
let ``Divide(long) rounds towards minus infinity`` m q =
    MathZ.Divide(m, 3L) === q

[<Theory>]
[<InlineData(4, 1, 1)>]
[<InlineData(3, 1, 0)>]
[<InlineData(2, 0, 2)>]
[<InlineData(1, 0, 1)>]
[<InlineData(0, 0, 0)>]
[<InlineData(-1, -1, 2)>]
[<InlineData(-2, -1, 1)>]
[<InlineData(-3, -1, 0)>]
[<InlineData(-4, -2, 2)>]
let ``Divide(long) with remainder`` m q r =
    let mutable rA: int64 = 0
    let qA = MathZ.Divide(m, 3L, &rA)

    (qA, rA) === (q, r)

[<Theory>]
[<InlineData(4, 1)>]
[<InlineData(3, 0)>]
[<InlineData(2, 2)>]
[<InlineData(1, 1)>]
[<InlineData(0, 0)>]
[<InlineData(-1, 2)>]
[<InlineData(-2, 1)>]
[<InlineData(-3, 0)>]
[<InlineData(-4, 2)>]
let Modulo m r =
    MathZ.Modulo(m, 3) === r

[<Theory>]
[<InlineData(4, 1, 1)>]
[<InlineData(3, 1, 0)>]
[<InlineData(2, 0, 2)>]
[<InlineData(1, 0, 1)>]
[<InlineData(0, 0, 0)>]
[<InlineData(-1, -1, 2)>]
[<InlineData(-2, -1, 1)>]
[<InlineData(-3, -1, 0)>]
[<InlineData(-4, -2, 2)>]
let ``Modulo() with quotient`` m q r =
    let mutable qA = 0
    let rA = MathZ.Modulo(m, 3, &qA)

    (qA, rA) === (q, r)

[<Theory>]
[<InlineData(4, 1)>]
[<InlineData(3, 0)>]
[<InlineData(2, 2)>]
[<InlineData(1, 1)>]
[<InlineData(0, 0)>]
[<InlineData(-1, 2)>]
[<InlineData(-2, 1)>]
[<InlineData(-3, 0)>]
[<InlineData(-4, 2)>]
let ``Modulo(long)`` m r =
    MathZ.Modulo(m, 3L) === r

[<Theory>]
[<InlineData(4, 2)>]
[<InlineData(3, 1)>]
[<InlineData(2, 1)>]
[<InlineData(1, 1)>]
[<InlineData(0, 0)>]
[<InlineData(-1, 0)>]
[<InlineData(-2, 0)>]
[<InlineData(-3, -1)>]
[<InlineData(-4, -1)>]
let AdjustedDivide m q =
    MathZ.AdjustedDivide(m, 3) === q

[<Theory>]
[<InlineData(4, 2, 1)>]
[<InlineData(3, 1, 3)>]
[<InlineData(2, 1, 2)>]
[<InlineData(1, 1, 1)>]
[<InlineData(0, 0, 3)>]
[<InlineData(-1, 0, 2)>]
[<InlineData(-2, 0, 1)>]
[<InlineData(-3, -1, 3)>]
[<InlineData(-4, -1, 2)>]
let ``AdjustedDivide() with remainder`` m q r =
    let mutable rA = 0
    let qA = MathZ.AdjustedDivide(m, 3, &rA)

    (qA, rA) === (q, r)

[<Theory>]
[<InlineData(4, 1)>]
[<InlineData(3, 3)>]
[<InlineData(2, 2)>]
[<InlineData(1, 1)>]
[<InlineData(0, 3)>]
[<InlineData(-1, 2)>]
[<InlineData(-2, 1)>]
[<InlineData(-3, 3)>]
[<InlineData(-4, 2)>]
let AdjustedModulo m r =
    MathZ.AdjustedModulo(m, 3) === r

[<Theory>]
[<InlineData(4, 1)>]
[<InlineData(3, 3)>]
[<InlineData(2, 2)>]
[<InlineData(1, 1)>]
[<InlineData(0, 3)>]
[<InlineData(-1, 2)>]
[<InlineData(-2, 1)>]
[<InlineData(-3, 3)>]
[<InlineData(-4, 2)>]
let ``AdjustedModulo(long)`` m r =
    MathZ.AdjustedModulo(m, 3L) === r

[<Theory>]
[<InlineData(4, 2)>]
[<InlineData(3, 2)>]
[<InlineData(2, 1)>]
[<InlineData(1, 1)>]
[<InlineData(0, 1)>]
[<InlineData(-1, 0)>]
[<InlineData(-2, 0)>]
[<InlineData(-3, 0)>]
[<InlineData(-4, -1)>]
let AugmentedDivide m q =
    MathZ.AugmentedDivide(m, 3) === q

[<Theory>]
[<InlineData(4, 2, 2)>]
[<InlineData(3, 2, 1)>]
[<InlineData(2, 1, 3)>]
[<InlineData(1, 1, 2)>]
[<InlineData(0, 1, 1)>]
[<InlineData(-1, 0, 3)>]
[<InlineData(-2, 0, 2)>]
[<InlineData(-3, 0, 1)>]
[<InlineData(-4, -1, 3)>]
let ``AugmentedDivide() with remainder`` m q r =
    let mutable rA = 0
    let qA = MathZ.AugmentedDivide(m, 3, &rA)

    (qA, rA) === (q, r)
