// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Intervals.RangeSetTests

open System

open Zorglub.Testing

open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Utilities

open Xunit
open FsCheck
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<RangeSet<int>> === RangeSet<int>.Empty

    [<Fact>]
    let ``Static property Empty`` () =
        let v = RangeSet<int>.Empty

        v.IsEmpty |> ok
        throws<InvalidOperationException> (fun () -> v.Range)
        v.ToString() === "[]"

module Factories =
    [<Fact>]
    let ``RangeSet.Empty()`` () =
        let v = RangeSet.Empty<int>()

        v.IsEmpty |> ok
        throws<InvalidOperationException> (fun () -> v.Range)
        v.ToString() === "[]"

    [<Property>]
    let ``RangeSet.Create() throws when max < min`` (x: Pair<int>) =
        outOfRangeExn "max" (fun () -> RangeSet.Create(x.Max, x.Min))

    [<Property>]
    let ``RangeSet.Create()`` (x: Pair<int>) =
        let v = RangeSet.Create(x.Min, x.Max)
        let range = Range.Create(x.Min, x.Max)

        v.IsEmpty |> nok
        v.Range === range
        v.ToString() === sprintf "[%i..%i]" x.Min x.Max

        // Constructor
        let other = new RangeSet<int>(x.Min, x.Max)
        v === other

    [<Property>]
    let ``RangeSet.Create() when singleton`` (i: int) =
        let v = RangeSet.Create(i, i)
        let range = Range.Singleton(i)

        v.IsEmpty |> nok
        v.Range === range
        v.ToString() === sprintf "[%i]" i

        // Constructor
        let other = new RangeSet<int>(i, i)
        v === other

    [<Property>]
    let ``RangeSet.FromEndpoints()`` (x: OrderedPair<int>) =
        let v = RangeSet.FromEndpoints(x)
        let range = new Range<int>(x.LowerValue, x.UpperValue)

        let isSingleton = x.LowerValue = x.UpperValue

        v.IsEmpty |> nok
        v.Range === range
        v.ToString() ===
            if isSingleton then
                sprintf "[%i]" x.LowerValue
            else
                sprintf "[%i..%i]" x.LowerValue x.UpperValue

        // Constructor
        let other = new RangeSet<int>(x.LowerValue, x.UpperValue)
        v === other

module Equality =
    open NonStructuralComparison

    /// Arbitrary for (x, y) where x and y are Range<int> instances such that x <> y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! range =
            Gen.elements [ (0, 1); (1, 2); (2, 3) ]
            |> Gen.map (fun (i, j) -> new RangeSet<int>(i, j))
        return new RangeSet<int>(1, 1), range
    }

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: RangeSet<int>) =
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

    [<Property>]
    let ``Equality when only one operand is empty`` (x: RangeSet<int>) =
        let empty = RangeSet<int>.Empty

        not (x = empty)
        .&. (x <> empty)
        .&. not (x.Equals(empty))
        .&. not (x.Equals(empty :> obj))
        // Flipped
        .&. not (empty = x)
        .&. (empty <> x)
        .&. not (empty.Equals(x))
        .&. not (empty.Equals(x :> obj))

    [<Fact>]
    let ``Equality when both operands are empty`` () =
        let v = RangeSet<int>.Empty

        v = v               |> ok
        not (v <> v)        |> ok
        v.Equals(v)         |> ok
        v.Equals(v :> obj)  |> ok
    // fsharplint:enable

    [<Property>]
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: RangeSet<int>) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: RangeSet<int>) =
        x.GetHashCode() = x.GetHashCode()


