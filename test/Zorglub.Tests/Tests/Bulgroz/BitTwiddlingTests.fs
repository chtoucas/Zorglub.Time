// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Bulgroz.BitTwiddlingTests

open Checked

open System

open Zorglub.Testing

open Zorglub.Bulgroz

open Xunit

//
// sameSign()
//

[<Fact>]
let ``sameSign(m, n) returns true when m and n have the same sign`` () =
    BitTwiddling.sameSign -2 -1 |> ok
    BitTwiddling.sameSign 0 0   |> ok
    BitTwiddling.sameSign 2 1   |> ok

[<Fact>]
let ``sameSign(m, n) returns false when m and n have opposite signs`` () =
    BitTwiddling.sameSign -2 1 |> nok
    BitTwiddling.sameSign 2 -1 |> nok

//
// abs() & abs64()
//

[<Fact>]
let ``abs(n) returns the absolute value of n`` () =
    BitTwiddling.abs -2 === 2
    BitTwiddling.abs 0  === 0
    BitTwiddling.abs 2  === 2

[<Fact>]
let ``abs64(n) returns the absolute value of n`` () =
    BitTwiddling.abs64(Int64.MinValue + 1L) === Int64.MaxValue
    BitTwiddling.abs64(int64(Int32.MinValue) - 1L) === 2_147_483_649L
    BitTwiddling.abs64 -2L === 2L
    BitTwiddling.abs64 0L  === 0L
    BitTwiddling.abs64 2L  === 2L
    BitTwiddling.abs64(int64(Int32.MaxValue) + 1L) === 2_147_483_648L
    BitTwiddling.abs64 Int64.MaxValue === Int64.MaxValue

//
// min() & min64()
//

[<Fact>]
let ``min(m, m) returns m`` () =
    BitTwiddling.min 2 2 === 2
    BitTwiddling.min Int32.MinValue Int32.MinValue === Int32.MinValue
    BitTwiddling.min Int32.MaxValue Int32.MaxValue === Int32.MaxValue

[<Fact>]
let ``min(m, n) returns m when m < n`` () =
    BitTwiddling.min 1 2 === 1
    BitTwiddling.min Int32.MinValue 0 === Int32.MinValue
    BitTwiddling.min 0 Int32.MaxValue === 0

[<Fact>]
let ``min(m, n) returns n when m > n`` () =
    BitTwiddling.min 2 1 === 1
    BitTwiddling.min Int32.MaxValue 0 === 0

[<Fact>]
let ``min64(m, m) returns m`` () =
    BitTwiddling.min64 2L 2L === 2L
    BitTwiddling.min64 Int64.MinValue Int64.MinValue === Int64.MinValue
    BitTwiddling.min64 Int64.MaxValue Int64.MaxValue === Int64.MaxValue

[<Fact>]
let ``min64(m, n) returns m when m < n`` () =
    BitTwiddling.min64 1L 2L === 1L
    BitTwiddling.min64 Int64.MinValue 0L === Int64.MinValue
    BitTwiddling.min64 0L Int64.MaxValue === 0L

[<Fact>]
let ``min64(m, n) returns n when m > n`` () =
    BitTwiddling.min64 2L 1L === 1L
    BitTwiddling.min64 Int64.MaxValue 0L === 0L

//
// max() & max64()
//

[<Fact>]
let ``max(m, m) returns m`` () = BitTwiddling.max 2 2 === 2

[<Fact>]
let ``max(m, n) returns m when m > n`` () = BitTwiddling.max 2 1 === 2

[<Fact>]
let ``max(m, n) returns n when m < n`` () = BitTwiddling.max 1 2 === 2

//
// isPowerOfTwo()
//

[<Fact>]
let ``isPowerOfTwo(0u) overflows`` () =
    AssertEx.Overflows(fun() -> BitTwiddling.isPowerOfTwo 0u |> ignore)

[<Fact>]
let ``isPowerOfTwo() returns true w/ powers of two`` () =
    // 2^0 -> 2^3
    BitTwiddling.isPowerOfTwo(0b1u) |> ok
    BitTwiddling.isPowerOfTwo(0b10u) |> ok
    BitTwiddling.isPowerOfTwo(0b100u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000u) |> ok
    // 2^4 -> 2^7
    BitTwiddling.isPowerOfTwo(0b1_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b10_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b100_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000_0000u) |> ok
    // 2^8 -> 2^11
    BitTwiddling.isPowerOfTwo(0b1_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b10_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b100_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000_0000_0000u) |> ok
    // 2^12 -> 2^15
    BitTwiddling.isPowerOfTwo(0b1_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b10_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b100_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000_0000_0000_0000u) |> ok
    // 2^16 -> 2^19
    BitTwiddling.isPowerOfTwo(0b1_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b10_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b100_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000_0000_0000_0000_0000u) |> ok
    // 2^20 -> 2^23
    BitTwiddling.isPowerOfTwo(0b1_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b10_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b100_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000_0000_0000_0000_0000_0000u) |> ok
    // 2^24 -> 2^27
    BitTwiddling.isPowerOfTwo(0b1_0000_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b10_0000_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b100_0000_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000_0000_0000_0000_0000_0000_0000u) |> ok
    // 2^28 -> 2^31
    BitTwiddling.isPowerOfTwo(0b1_0000_0000_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b10_0000_0000_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b100_0000_0000_0000_0000_0000_0000_0000u) |> ok
    BitTwiddling.isPowerOfTwo(0b1000_0000_0000_0000_0000_0000_0000_0000u) |> ok

[<Fact>]
let ``isPowerOfTwo() returns false with non powers of two`` () =
    BitTwiddling.isPowerOfTwo 3u  |> nok
    BitTwiddling.isPowerOfTwo 5u  |> nok
    BitTwiddling.isPowerOfTwo 7u  |> nok
    BitTwiddling.isPowerOfTwo 9u  |> nok
    BitTwiddling.isPowerOfTwo 10u |> nok
    BitTwiddling.isPowerOfTwo 11u |> nok
    BitTwiddling.isPowerOfTwo 12u |> nok
    BitTwiddling.isPowerOfTwo 13u |> nok
    BitTwiddling.isPowerOfTwo 14u |> nok
    BitTwiddling.isPowerOfTwo 15u |> nok
