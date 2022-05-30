// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz

open Checked

module Math =
    let div i n =
        if i >= 0 || i % n = 0 then i / n else (i / n - 1)

    let divrem i n =
        let rem = i % n
        if i >= 0 || rem = 0 then
            i / n, rem
        else
            i / n - 1, rem + n

module BitTwiddling =
    // References:
    // - https://en.wikipedia.org/wiki/Bit_manipulation
    // - https://graphics.stanford.edu/~seander/bithacks.html
    // - http://aggregate.org/MAGIC/
    // - http://bits.stephan-brumme.com/

    /// Returns true if two 32-bit signed integers share the same sign;
    /// otherwise returns false.
    let sameSign i j = (i ^^^ j) >= 0

    /// Returns the absolute value of a 32-bit signed integer.
    (*
        Explanation.
            n >>> 31 = n >= 0 ? 0x0000_0000 : 0xffff_ffff
            n + (n >>> 31) = n >= 0 ? n : n - 1
    *)
    let abs i =
        let mask = i >>> 31
        (i + mask) ^^^ mask

    /// Returns the absolute value of a 64-bit signed integer.
    let abs64 (i: int64) =
        let mask = i >>> 63
        (i + mask) ^^^ mask

    /// Returns the smaller of two 32-bit signed integers.
    /// No branching BUT may overflow.
    (*
        Explanation.
            x >>> 31 = x >= 0 ? 0x0000_0000 : 0xffff_ffff
            x &&& (x >>> 31) = x >= 0 ? 0 : x
    *)
    let min i j =
        let diff = i - j
        j + (diff &&& (diff >>> 31))

    let ``min (alt)`` i j =
        (((i - j) >>> 31) &&& (i ^^^ j)) ^^^ j

    /// Returns the smaller of two 64-bit signed integers.
    /// No branching BUT may overflow.
    let min64 (i: int64) (j: int64) =
        let diff = i - j
        j + (diff &&& (diff >>> 63))

    /// Returns the larger of two 32-bit signed integers.
    /// No branching BUT may overflow.
    let max i j =
        let diff = i - j
        i - (diff &&& (diff >>> 31))

    let ``max (alt)`` i j =
        (((i - j) >>> 31) &&& (j ^^^ i)) ^^^ i

    /// Returns the larger of two 64-bit signed integers.
    /// No branching BUT may overflow.
    let max64 (i: int64) (j: int64) =
        let diff = i - j
        i - (diff &&& (diff >>> 63))

    /// Returns true if a non-zero 32-bit unsigned integer is a power of two;
    /// otherwise returns false.
    /// Overflows with zero.
    (*
        Explanation. A power of 2 is a 1 followed by 0s and (n - 1) replace 1
        by 0 and vice versa (but only for the trailing zeroes). For instance,
        8 = 0b1000, 7 = 0b01111, 8 &&& 7 = 0b0000.
    *)
    let isPowerOfTwo n = (n &&& (n - 1u)) = 0u
