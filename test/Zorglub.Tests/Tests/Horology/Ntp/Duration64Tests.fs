// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Horology.Ntp.Duration64Tests

open Zorglub.Testing

open Zorglub.Time.Horology.Ntp

open FsCheck
open FsCheck.Xunit

module Equality =
    open NonStructuralComparison

    /// Arbitrary for (x, y) where x and y are DateParts instances such that x <> y.
    let private xynArbitrary =
        Arb.fromGen <| gen {
            let! i, j, n = IntGenerators.orderedPair
            let v = new Duration64(int64 i)
            let w = new Duration64(int64 j)
            return v, w, n
        }

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: Duration64) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equality when both operands are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        not (x = y)
        .&. (x <> y)
        .&. not (x.Equals(y))
        .&. not (x.Equals(y :> obj))
        // Flipped
        .&. not (y = x)
        .&. (y <> x)
        .&. not (y.Equals(x))
        .&. not (y.Equals(x :> obj))
    // fsharplint:enable

    [<Property>]
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: Duration64) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: Duration64) =
        x.GetHashCode() = x.GetHashCode()
