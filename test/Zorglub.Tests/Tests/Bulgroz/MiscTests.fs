// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Bulgroz.MiscTests

open Zorglub.Testing

open Xunit

module Integers =
    [<Fact>]
    let ``n >>> 1 rounds towards minus infinity`` () =
        4 >>> 1  ===  2
        3 >>> 1  ===  1
        2 >>> 1  ===  1
        1 >>> 1  ===  0
        0 >>> 1  ===  0
        -1 >>> 1 === -1
        -2 >>> 1 === -1
        -3 >>> 1 === -2
        -4 >>> 1 === -2

    [<Fact>]
    let ``n / 2 rounds towards zero`` () =
        4 / 2  ===  2
        3 / 2  ===  1
        2 / 2  ===  1
        1 / 2  ===  0
        0 / 2  ===  0
        -1 / 2 ===  0
        -2 / 2 === -1
        -3 / 2 === -1
        -4 / 2 === -2

    [<Fact>]
    let ``m % n does NOT follow mathematical rules w/ m < 0`` () =
        4 % 3  ===  1
        3 % 3  ===  0
        2 % 3  ===  2
        1 % 3  ===  1
        0 % 3  ===  0
        -1 % 3 === -1
        -2 % 3 === -2
        -3 % 3 ===  0
        -4 % 3 === -1

module Floats =
    [<Fact>]
    let ``Casting a floating-point number to an int rounds towards zero`` () =
        // Single-precision floating-point number.
        int  1.5f ===  1
        int -1.5f === -1
        // Double-precision floating-point number.
        int  1.5  ===  1
        int -1.5  === -1
        // Decimal floating-point number.
        int  1.5m ===  1
        int -1.5m === -1

    [<Fact>]
    let ``Casting a floating-point number to a long rounds towards zero`` () =
        // Single-precision floating-point number.
        int64  1.5f ===  1L
        int64 -1.5f === -1L
        // Double-precision floating-point number.
        int64  1.5  ===  1L
        int64 -1.5  === -1L
        // Decimal floating-point number.
        int64  1.5m ===  1L
        int64 -1.5m === -1L

    [<Theory>]
    [<InlineData( 2.0,  0.0)>]
    [<InlineData( 1.9,  0.9)>]
    [<InlineData( 1.5,  0.5)>]
    [<InlineData( 1.1,  0.1)>]
    [<InlineData( 1.0,  0.0)>]
    [<InlineData( 0.9,  0.9)>]
    [<InlineData( 0.5,  0.5)>]
    [<InlineData( 0.1,  0.1)>]
    [<InlineData( 0.0,  0.0)>]
    [<InlineData(-0.1, -0.1)>]
    [<InlineData(-0.5, -0.5)>]
    [<InlineData(-0.9, -0.9)>]
    [<InlineData(-1.0,  0.0)>]
    [<InlineData(-1.1, -0.1)>]
    [<InlineData(-1.5, -0.5)>]
    [<InlineData(-1.9, -0.9)>]
    [<InlineData(-2.0,  0.0)>]
    let ``x % 1.0 returns the signed fractional part of x`` x fp =
        Assert.Equal(fp, x % 1.0, 15)
