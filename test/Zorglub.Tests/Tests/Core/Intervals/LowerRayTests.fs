// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Intervals.LowerRayTests

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
        let v = LowerRay.EndingAt(0)

        Unchecked.defaultof<LowerRay<int>> === v

module Factories =
    [<Property>]
    let ``LowerRay.EndingAt()`` (i: int) =
        let v = LowerRay.EndingAt(i)

        v.Max === i
        v.IsLeftOpen        |> ok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> nok
        v.IsRightBounded    |> ok
        v.ToString() === sprintf "[..%i]" i

        // IRay
        let ray = v :> IRay<int>
        ray.Endpoint === i

        // Constructor
        let other = new LowerRay<int>(i)
        v === other

    [<Fact>]
    let ``LowerRay.EndingAt(Int32.MinValue) is a singleton`` () =
        let v = LowerRay.EndingAt(Int32.MinValue)

        v.Max === Int32.MinValue
        v.IsLeftOpen        |> ok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> nok
        v.IsRightBounded    |> ok
        v.ToString() === "[..-2147483648]"

        // IRay
        let ray = v :> IRay<int>
        ray.Endpoint === Int32.MinValue

        // Constructor
        let other = new LowerRay<int>(Int32.MinValue)
        v === other

    [<Fact>]
    let ``LowerRay.EndingAt(Int32.MaxValue)`` () =
        let v = LowerRay.EndingAt(Int32.MaxValue)

        v.Max === Int32.MaxValue
        v.IsLeftOpen        |> ok
        v.IsRightOpen       |> nok
        v.IsLeftBounded     |> nok
        v.IsRightBounded    |> ok
        v.ToString() === "[..2147483647]"

        // IRay
        let ray = v :> IRay<int>
        ray.Endpoint === Int32.MaxValue

        // Constructor
        let other = new LowerRay<int>(Int32.MaxValue)
        v === other

module SetOperations =
    //
    // Membership
    //

    [<Fact>]
    let ``LowerRay.EndingAt(Int32.MinValue).Contains(Int32.MinValue)`` () =
        let v = LowerRay.EndingAt(Int32.MinValue)

        v.Contains(Int32.MinValue) |> ok

    [<Property>]
    let ``LowerRay.EndingAt(Int32.MinValue).Contains() (almost) always returns false`` (i: int) = i <> Int32.MinValue &&&& (
        let v = LowerRay.EndingAt(Int32.MinValue)

        not (v.Contains(i))
    )

    [<Fact>]
    let ``LowerRay.EndingAt(Int32.MaxValue).Contains(Int32.Min/MaxValue)`` () =
        let v = LowerRay.EndingAt(Int32.MaxValue)

        v.Contains(Int32.MinValue) |> ok
        v.Contains(Int32.MaxValue) |> ok

    [<Property>]
    let ``LowerRay.EndingAt(Int32.MaxValue).Contains() always returns true`` (i: int) =
        let v = LowerRay.EndingAt(Int32.MaxValue)

        v.Contains(i)

    [<Property>]
    let ``Contains()`` (x: LowerRay<int>) = x.Max <> Int32.MaxValue &&&& (
        x.Contains(Int32.MinValue)  |> ok
        x.Contains(x.Max - 1)       |> ok
        x.Contains(x.Max)           |> ok
        x.Contains(x.Max + 1)       |> nok
        x.Contains(Int32.MaxValue)  |> nok
    )

    //
    // Set inclusion
    //

    [<Property>]
    let ``Set inclusion of a ray with itself`` (x: LowerRay<int>) =
        x.IsSubsetOf(x)         |> ok
        x.IsSupersetOf(x)       |> ok
        x.IsProperSubsetOf(x)   |> nok
        x.IsProperSupersetOf(x) |> nok

    [<Fact>]
    let ``Set inclusion of a ray with a proper superset`` () =
        let v = LowerRay.EndingAt(4)
        let w = LowerRay.EndingAt(5)

        v.IsSubsetOf(w)   |> ok
        v.IsSupersetOf(w) |> nok

        v.IsProperSubsetOf(w)   |> ok
        v.IsProperSupersetOf(w) |> nok

    [<Fact>]
    let ``Set inclusion of a ray with a proper subset`` () =
        let v = LowerRay.EndingAt(4)
        let w = LowerRay.EndingAt(3)

        v.IsSubsetOf(w)   |> nok
        v.IsSupersetOf(w) |> ok

        v.IsProperSubsetOf(w)   |> nok
        v.IsProperSupersetOf(w) |> ok

    //
    // Set equality
    //

    [<Property>]
    let ``SetEquals() when both rays are identical`` (x: LowerRay<int>) =
        x.SetEquals(x)

    [<Property>]
    let ``SetEquals() when both rays are distinct`` (x: LowerRay<int>) (y: LowerRay<int>) = x <> y &&&& (
        not (x.SetEquals(y))
    )

module Extensions =
    //
    // LowerRay<int>
    //

    [<Fact>]
    let ``LowerRay.EndingAt(Int32.MaxValue).Complement() throws`` () =
        let v = LowerRay.EndingAt(Int32.MaxValue)

        throws<InvalidOperationException> (fun () -> v.Complement())

    [<Property>]
    let ``LowerRay<int>.Complement()`` (x: LowerRay<int>) = x.Max <> Int32.MaxValue &&&& lazy (
        let complement = UpperRay.StartingAt(x.Max + 1)

        x.Complement() = complement
    )

    //
    // LowerRay<DayNumber>
    //

    [<Fact>]
    let ``LowerRay.EndingAt(DayNumber.MaxValue).Complement() throws`` () =
        let v = LowerRay.EndingAt(DayNumber.MaxValue)

        throws<InvalidOperationException> (fun () -> v.Complement())

    [<Property>]
    let ``LowerRay<DayNumber>.Complement()`` (n: DayNumber) = n <> DayNumber.MaxValue &&&& lazy (
        let w = LowerRay.EndingAt(n)
        let complement = UpperRay.StartingAt(n + 1)

        w.Complement() = complement
    )

module Equality =
    open NonStructuralComparison

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: LowerRay<int>) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equality when both operands are distinct`` (i: int) (j: int) = i <> j &&&& (
        let v = LowerRay.EndingAt(i)
        let w = LowerRay.EndingAt(j)

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
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: LowerRay<int>) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: LowerRay<int>) =
        x.GetHashCode() = x.GetHashCode()
