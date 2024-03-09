// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Intervals.UpperRayTests

open System

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Intervals

open Xunit
open FsCheck
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        let v = UpperRay.StartingAt(0)

        Unchecked.defaultof<UpperRay<int>> === v

module Factories =
    [<Property>]
    let ``UpperRay.StartingAt()`` (i: int) =
        let v = UpperRay.StartingAt(i)

        v.Min === i
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> ok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> nok
        v.ToString() === sprintf "[%i..]" i

        // IRay
        let ray = v :> IRay<int>
        ray.Endpoint === i

        // Constructor
        let other = new UpperRay<int>(i)
        v === other

    [<Fact>]
    let ``UpperRay.StartingAt(Int32.MinValue)`` () =
        let v = UpperRay.StartingAt(Int32.MinValue)

        v.Min === Int32.MinValue
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> ok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> nok
        v.ToString() === "[-2147483648..]"

        // IRay
        let ray = v :> IRay<int>
        ray.Endpoint === Int32.MinValue

        // Constructor
        let other = new UpperRay<int>(Int32.MinValue)
        v === other

    [<Fact>]
    let ``UpperRay.StartingAt(Int32.MaxValue) is a singleton`` () =
        let v = UpperRay.StartingAt(Int32.MaxValue)

        v.Min === Int32.MaxValue
        v.IsLeftOpen        |> nok
        v.IsRightOpen       |> ok
        v.IsLeftBounded     |> ok
        v.IsRightBounded    |> nok
        v.ToString() === "[2147483647..]"

        // IRay
        let ray = v :> IRay<int>
        ray.Endpoint === Int32.MaxValue

        // Constructor
        let other = new UpperRay<int>(Int32.MaxValue)
        v === other

module SetOperations =
    //
    // Membership
    //

    [<Fact>]
    let ``UpperRay.StartingAt(Int32.MaxValue).Contains(Int32.MaxValue)`` () =
        let v = UpperRay.StartingAt(Int32.MaxValue)

        v.Contains(Int32.MaxValue) |> ok

    [<Property>]
    let ``UpperRay.StartingAt(Int32.MaxValue).Contains() almost always returns false`` (i: int) = i <> Int32.MaxValue &&&& (
        let v = UpperRay.StartingAt(Int32.MaxValue)

        not (v.Contains(i))
    )

    [<Fact>]
    let ``UpperRay.StartingAt(Int32.MinValue).Contains(Int32.Min/MaxValue)`` () =
        let v = UpperRay.StartingAt(Int32.MinValue)

        v.Contains(Int32.MinValue) |> ok
        v.Contains(Int32.MaxValue) |> ok

    [<Property>]
    let ``UpperRay.StartingAt(Int32.MinValue).Contains() always returns true`` (i: int) =
        let v = UpperRay.StartingAt(Int32.MinValue)

        v.Contains(i)

    [<Property>]
    let ``Contains()`` (x: UpperRay<int>) = x.Min <> Int32.MinValue &&&& (
        x.Contains(Int32.MaxValue)  |> ok
        x.Contains(x.Min + 1)       |> ok
        x.Contains(x.Min)           |> ok
        x.Contains(x.Min - 1)       |> nok
        x.Contains(Int32.MinValue)  |> nok
    )

    //
    // Set inclusion
    //

    [<Property>]
    let ``Set inclusion of a ray with itself`` (x: UpperRay<int>) =
        x.IsSubsetOf(x)         |> ok
        x.IsSupersetOf(x)       |> ok
        x.IsProperSubsetOf(x)   |> nok
        x.IsProperSupersetOf(x) |> nok

    [<Fact>]
    let ``Set inclusion of a ray with a proper superset`` () =
        let v = UpperRay.StartingAt(4)
        let w = UpperRay.StartingAt(3)

        v.IsSubsetOf(w)   |> ok
        v.IsSupersetOf(w) |> nok

        v.IsProperSubsetOf(w)   |> ok
        v.IsProperSupersetOf(w) |> nok

    [<Fact>]
    let ``Set inclusion of a ray with a proper subset`` () =
        let v = UpperRay.StartingAt(4)
        let w = UpperRay.StartingAt(5)

        v.IsSubsetOf(w)   |> nok
        v.IsSupersetOf(w) |> ok

        v.IsProperSubsetOf(w)   |> nok
        v.IsProperSupersetOf(w) |> ok

    //
    // Set equality
    //

    [<Property>]
    let ``SetEquals() when both rays are identical`` (x: UpperRay<int>) =
        x.SetEquals(x)

    [<Property>]
    let ``SetEquals() when both rays are distinct`` (x: UpperRay<int>) (y: UpperRay<int>) = x <> y &&&& (
        not (x.SetEquals(y))
    )

module Extensions =
    //
    // UpperRay<int>
    //

    [<Fact>]
    let ``UpperRay.StartingAt(Int32.MinValue).Complement() throws`` () =
        let v = UpperRay.StartingAt(Int32.MinValue)

        throws<InvalidOperationException> (fun () -> v.Complement())

    [<Property>]
    let ``UpperRay<int>.Complement()`` (x: UpperRay<int>) = x.Min <> Int32.MinValue &&&& lazy (
        let complement = LowerRay.EndingAt(x.Min - 1)

        x.Complement() = complement
    )

    //
    // UpperRay<DayNumber>
    //

    [<Fact>]
    let ``UpperRay.StartingAt(DayNumber.MinValue).Complement() throws`` () =
        let v = UpperRay.StartingAt(DayNumber.MinValue)

        throws<InvalidOperationException> (fun () -> v.Complement())

    [<Property>]
    let ``UpperRay<DayNumber>.Complement()`` (n: DayNumber) = n <> DayNumber.MinValue &&&& lazy (
        let v = UpperRay.StartingAt(n)
        let complement = LowerRay.EndingAt(n - 1)

        v.Complement() = complement
    )

module Equality =
    open NonStructuralComparison

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: UpperRay<int>) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equality when both operands are distinct`` (i: int) (j: int) = i <> j &&&& (
        let v = UpperRay.StartingAt(i)
        let w = UpperRay.StartingAt(j)

        not (v = w)
        .&. (v <> w)
        .&. not (v.Equals(w))
        .&. not (v.Equals(w :> obj))
        // Flipped
        .&. not (w = v)
        .&. (w <> v)
        .&. not (w.Equals(v))
        .&. not (w.Equals(v :> obj))
    )
    // fsharplint:enable

    [<Property>]
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: UpperRay<int>) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: UpperRay<int>) =
        x.GetHashCode() = x.GetHashCode()
