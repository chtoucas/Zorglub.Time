// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Horology.Ntp.Duration32Tests

open Zorglub.Testing

open Zorglub.Time.Horology.Ntp

open FsCheck
open FsCheck.Xunit

module Equality =
    open NonStructuralComparison

    /// Arbitrary for (x, y) where x and y are DateParts instances such that x <> y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! parts =
            Gen.elements [ (2us, 1us); (1us, 2us); (2us, 2us) ]
            |> Gen.map (fun (i, j) -> new Duration32(i, j))
        return new Duration32(1us, 1us), parts
    }

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: Duration32) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
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
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: Duration32) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: Duration32) =
        x.GetHashCode() = x.GetHashCode()
