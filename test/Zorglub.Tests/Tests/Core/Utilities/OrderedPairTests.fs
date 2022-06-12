// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Utilities.OrderedPairTests

open Zorglub.Testing

open Zorglub.Time.Core.Utilities

open Xunit
open FsCheck
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        let pair = new OrderedPair<int>(0, 0)

        Unchecked.defaultof<OrderedPair<int>> === pair

    [<Property>]
    let ``Constructor when ordered`` (x: Pair<int>) =
        let pair = new OrderedPair<int>(x.Min, x.Max)

        (pair.LowerValue, pair.UpperValue) = (x.Min, x.Max)

    [<Property>]
    let ``Constructor when unordered`` (x: Pair<int>) =
        let pair = new OrderedPair<int>(x.Max, x.Min)

        (pair.LowerValue, pair.UpperValue) = (x.Min, x.Max)

    [<Property>]
    let ``Constructor when singleton`` (i: int) =
        let pair = new OrderedPair<int>(i, i)

        (pair.LowerValue, pair.UpperValue) = (i, i)

    [<Property>]
    let ``Deconstructor when ordered`` (x: Pair<int>) =
        let pair = new OrderedPair<int>(x.Max, x.Min)
        let a, b = pair.Deconstruct()

        (a, b) = (x.Min, x.Max)

    [<Property>]
    let ``Deconstructor when unordered`` (x: Pair<int>) =
        let pair = new OrderedPair<int>(x.Max, x.Min)
        let a, b = pair.Deconstruct()

        (a, b) = (x.Min, x.Max)

    [<Property>]
    let ``Deconstructor when singleton`` (i: int) =
        let pair = new OrderedPair<int>(i, i)
        let a, b = pair.Deconstruct()

        (a, b) = (i, i)

    [<Theory>]
    [<InlineData(1, 1, "(1, 1)")>] // Singleton
    [<InlineData(1, 2, "(1, 2)")>] // Ordered
    [<InlineData(2, 1, "(1, 2)")>] // Unordered
    let ``ToString()`` i j str =
        let pair = new OrderedPair<int>(i, j)

        pair.ToString() === str

module Factories =
    [<Property>]
    let ``Create() when ordered`` (x: Pair<int>) =
        let pair = OrderedPair.Create(x.Min, x.Max)

        (pair.LowerValue, pair.UpperValue) = (x.Min, x.Max)

    [<Property>]
    let ``Create() when unordered`` (x: Pair<int>) =
        let pair = OrderedPair.Create(x.Max, x.Min)

        (pair.LowerValue, pair.UpperValue) = (x.Min, x.Max)

    [<Property>]
    let ``Create() when singleton`` (i: int) =
        let pair = OrderedPair.Create(i, i)

        (pair.LowerValue, pair.UpperValue) = (i, i)

    [<Property>]
    let ``FromOrderedValues()`` (x: Pair<int>) =
        let pair = OrderedPair.FromOrderedValues(x.Min, x.Max)

        (pair.LowerValue, pair.UpperValue) = (x.Min, x.Max)

    [<Property>]
    let ``FromOrderedValues() when singleton`` (i: int) =
        let pair = OrderedPair.FromOrderedValues(i, i)

        (pair.LowerValue, pair.UpperValue) = (i, i)

module Mapping =
    let private uncalled _ = invalidOp "This lambda should not have been called"

    [<Property>]
    let ``Select() throws when "selector" is null`` (x: OrderedPair<int>) =
        nullExn "selector" (fun () -> x.Select(null))

    [<Property>]
    let ``Select(,) throws when "lowerValueSelector" is null`` (x: OrderedPair<int>) =
        nullExn "lowerValueSelector" (fun () -> x.Select(null, uncalled))

    [<Property>]
    let ``Select(,) throws when "upperValueSelector" is null`` (x: OrderedPair<int>) =
        nullExn "upperValueSelector" (fun () -> x.Select(uncalled, null))

    [<Property>]
    let ``Select()`` (x: OrderedPair<int>) (selector: int -> int) =
        let i, j = x.Deconstruct()
        let pair = new OrderedPair<int>(selector i, selector j)

        x.Select(selector) = pair

    [<Property>]
    let ``Select(,)`` (x: OrderedPair<int>) (lowerSel: int -> int) (upperSel: int -> int) =
        let i, j = x.Deconstruct()
        let w = new OrderedPair<int>(lowerSel i, upperSel j)

        x.Select(lowerSel, upperSel) = w

module Equality =
    open NonStructuralComparison

    /// Arbitrary for (x, y) where x and y are OrderedPair<int> instances such that x <> y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! range =
            Gen.elements [ (0, 1); (1, 2); (2, 3) ]
            |> Gen.map (fun (i, j) -> new OrderedPair<int>(i, j))
        return new OrderedPair<int>(1, 1), range
    }

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: OrderedPair<int>) =
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
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: OrderedPair<int>) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: OrderedPair<int>) =
        x.GetHashCode() = x.GetHashCode()
