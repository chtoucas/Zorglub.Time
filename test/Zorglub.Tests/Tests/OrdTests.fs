// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.OrdTests

open System
open System.Runtime.CompilerServices

open Zorglub.Testing
open Zorglub.Time

open Xunit
open FsCheck
open FsCheck.Xunit

module TestCommon =
    /// Arbitrary for (x, y, y - x) where x and y are Ord instances such that x < y.
    let xynArbitrary =
        Arb.fromGen <| gen {
            let! i, j, n = IntGenerators.orderedPair
            let v = Ord.FromInt32(i)
            let w = Ord.FromInt32(j)
            return v, w, n
        }

    /// Represents the absolute value of the Rank of an Ord, its position.
    [<Struct; IsReadOnly>]
    type Position = { Value: int } with
        override x.ToString() = x.Value.ToString()
        static member op_Explicit (x: Position) = x.Value

    /// Represents the algebraic value of an Ord.
    [<Struct; IsReadOnly>]
    type AlgebraicValue = { Value: int } with
        override x.ToString() = x.Value.ToString()
        static member op_Explicit (x: AlgebraicValue) = x.Value

    [<Sealed>]
    type Arbitraries =
        /// Gets an arbitrary for the absolute value of the Rank of an Ord, its position.
        static member GetPositionArbitrary() =
            IntArbitraries.greaterThanZero
            |> Arb.convert (fun i -> { Position.Value = i }) int

        /// Gets an arbitrary for the algebraic value of an Ord.
        static member GetAlgebraicValueArbitrary2() =
            DomainArbitraries.algebraicOrd
            |> Arb.convert (fun i -> { AlgebraicValue.Value = i }) int

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module Prelude =
    open TestCommon

    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<Ord> === Ord.Zeroth

    // NB: the constructor is private.

    [<Property>]
    let Deconstructor (x: AlgebraicValue) =
        let ord = Ord.FromInt32(x.Value)
        let rank = ord.Rank
        let pos, afterZeroth = ord.Deconstruct()

        pos === abs rank
        if rank > 0 then
            afterZeroth |> ok
        else
            afterZeroth |> nok

    [<Property>]
    let ``ToString() returns the string representation of its Rank (> 0) using the current culture`` (x: Position) =
        let rank = x.Value
        let ord = Ord.FromRank(rank)

        ord.ToString() = rank.ToString(System.Globalization.CultureInfo.CurrentCulture)

    [<Property>]
    let ``ToString() returns the string representation of its Rank (< 0) using the current culture`` (x: Position) =
        let rank = -x.Value
        let ord = Ord.FromRank(rank)

        ord.ToString() = rank.ToString(System.Globalization.CultureInfo.CurrentCulture)

    //
    // Properties
    //

    [<Fact>]
    let ``Static property Zeroth`` () =
        let zeroth = Ord.Zeroth
        let pos, afterZeroth = zeroth.Deconstruct()

        zeroth.Rank === -1
        pos === 1
        afterZeroth |> nok
        // Conversion
        int(zeroth) === 0

    [<Fact>]
    let ``Static property First`` () =
        let first = Ord.First
        let pos, afterZeroth = first.Deconstruct()

        first.Rank === 1
        pos === 1
        afterZeroth |> ok
        // Conversion
        int(first) === 1

    [<Fact>]
    let ``Static property MinValue`` () =
        let min = Ord.MinValue
        let pos, afterZeroth = min.Deconstruct()

        min.Rank === -Int32.MaxValue
        pos === Int32.MaxValue
        afterZeroth |> nok
        // Conversion
        int(min) === Ord.MinAlgebraicValue

    [<Fact>]
    let ``Static property MaxValue`` () =
        let max = Ord.MaxValue
        let pos, afterZeroth = max.Deconstruct()

        max.Rank === Int32.MaxValue
        pos === Int32.MaxValue
        afterZeroth |> ok
        // Conversion
        int(max) === Ord.MaxAlgebraicValue

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module Factories =
    open TestCommon

    [<Fact>]
    let ``FromRank() throws when "rank" is out of range`` () =
        outOfRangeExn "rank" (fun () -> Ord.FromRank(Int32.MinValue))
        outOfRangeExn "rank" (fun () -> Ord.FromRank(0))

    [<Property>]
    let ``FromRank(-Int32.MaxValue) = Ord.MinValue`` () =
        let ord = Ord.FromRank(-Int32.MaxValue)

        ord = Ord.MinValue

    [<Property>]
    let ``FromRank(Int32.MaxValue) = Ord.MaxValue`` () =
        let ord = Ord.FromRank(Int32.MaxValue)

        ord = Ord.MaxValue

    [<Property>]
    let ``FromRank() when "rank" > 0`` (x: Position) =
        let rank = x.Value
        let ord = Ord.FromRank(rank)
        let pos, afterZeroth = ord.Deconstruct()

        ord.Rank === rank
        ord >= Ord.First    |> ok
        pos === rank
        afterZeroth         |> ok
        ord === Ord.Zeroth + x.Value

    [<Property>]
    let ``FromRank() when "rank" < 0`` (x: Position) =
        let rank = -x.Value
        let ord = Ord.FromRank(rank)
        let pos, afterZeroth = ord.Deconstruct()

        ord.Rank === rank
        ord < Ord.First     |> ok
        pos === abs rank
        afterZeroth         |> nok
        ord === Ord.First - x.Value

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module Conversions =
    open TestCommon

    [<Fact>]
    let ``FromInt32() throws when "value" is out of range`` () =
        outOfRangeExn "value" (fun () -> Ord.FromInt32(Int32.MinValue))
        outOfRangeExn "value" (fun () -> Ord.FromInt32(Int32.MinValue + 1))

    [<Property>]
    let ``FromInt32()`` (x: AlgebraicValue) =
        let ord = Ord.FromInt32(x.Value)

        ord = Ord.Zeroth + x.Value

    [<Property>]
    let ``ToInt32()`` (x: AlgebraicValue) =
        let ord = Ord.FromInt32(x.Value)

        ord.ToInt32() === x.Value

    [<Property>]
    let ``Explicit conversion to int`` (x: AlgebraicValue) =
        let ord = Ord.FromInt32(x.Value)

        int(ord) === x.Value

module Equality =
    open NonStructuralComparison
    open TestCommon

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: Ord) =
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
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: Ord) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: Ord) =
        x.GetHashCode() = x.GetHashCode()

    [<Property>]
    let ``GetHashCode() returns the hashcode of its algebraic value`` (x: Ord) =
        let hash = int(x).GetHashCode()

        x.GetHashCode() = hash

module Comparison =
    open NonStructuralComparison
    open TestCommon

    // fsharplint:disable Hints
    [<Property>]
    let ``Comparisons when both operands are identical`` (x: Ord) =
        not (x > x)
        .&. not (x < x)
        .&. (x >= x)
        .&. (x <= x)

    [<Property>]
    let ``Comparisons when both operands are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        not (x > y)
        .&. not (x >= y)
        .&. (x < y)
        .&. (x <= y)
        // Flipped
        .&. (y > x)
        .&. (y >= x)
        .&. not (y < x)
        .&. not (y <= x)
    // fsharplint:enable

    //
    // CompareTo()
    //

    [<Property>]
    let ``CompareTo() returns 0 when both objects are identical`` (x: Ord) =
        (x.CompareTo(x) = 0)
        .&. (x.CompareTo(x :> obj) = 0)

    [<Property>]
    let ``CompareTo() when both objects are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        (x.CompareTo(y) <= 0)
        .&. (x.CompareTo(y :> obj) <= 0)
        // Flipped
        .&. (y.CompareTo(x) >= 0)
        .&. (y.CompareTo(x :> obj) >= 0)

    [<Property>]
    let ``CompareTo(obj) returns 1 when "obj" is null`` (x: Ord) =
         x.CompareTo(null) = 1

    [<Property>]
    let ``CompareTo(obj) throws when "obj" is a plain object`` (x: Ord) =
        argExn "obj" (fun () -> x.CompareTo(new obj()))

    //
    // Min() and Max()
    //

    [<Property>]
    let ``Min() when both values are identical`` (x: Ord) =
        Ord.Min(x, x) = x

    [<Property>]
    let ``Max() when both values are identical`` (x: Ord) =
        Ord.Max(x, x) = x

    [<Property>]
    let ``Min() when both values are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        Ord.Min(x, y) === x
        Ord.Min(y, x) === x

    [<Property>]
    let ``Max() when both values are distinct`` () = xynArbitrary @@@@ fun (x, y, _) ->
        Ord.Max(x, y) === y
        Ord.Max(y, x) === y

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module Math =
    open TestCommon

    //
    // Ord.MinValue
    //

    [<Fact>]
    let ``Ord.MinValue - 1 overflows`` () =
        (fun () -> Ord.MinValue - 1)               |> overflows
        (fun () -> Ord.MinValue + (-1))            |> overflows
        (fun () -> Ord.MinValue.Add(-1))           |> overflows
        (fun () -> Ord.MinValue.Decrement())       |> overflows
        (fun () -> Ord.op_Decrement(Ord.MinValue)) |> overflows

    [<Fact>]
    let ``Ord.MinValue + Int32.MaxValue does not overflow`` () =
        Ord.MinValue + Int32.MaxValue    === Ord.Zeroth + 1
        Ord.MinValue.Add(Int32.MaxValue) === Ord.Zeroth + 1

    [<Fact>]
    let ``-Ord.MinValue = Ord.MaxValue`` () =
        -Ord.MinValue === Ord.MaxValue
        Ord.MinValue.Negate() === Ord.MaxValue

    //
    // Ord.MaxValue
    //

    [<Fact>]
    let ``Ord.MaxValue + 1 overflows`` () =
        (fun () -> Ord.MaxValue + 1)               |> overflows
        (fun () -> Ord.MaxValue - (-1))            |> overflows
        (fun () -> Ord.MaxValue.Add(1))            |> overflows
        (fun () -> Ord.MaxValue.Increment())       |> overflows
        (fun () -> Ord.op_Increment(Ord.MaxValue)) |> overflows

    [<Fact>]
    let ``Ord.MaxValue - Int32.MaxValue does not overflow`` () =
        Ord.MaxValue - Int32.MaxValue     === Ord.Zeroth
        Ord.MaxValue.Add(-Int32.MaxValue) === Ord.Zeroth

    [<Fact>]
    let ``-Ord.MaxValue = Ord.MinValue`` () =
        -Ord.MaxValue === Ord.MinValue
        Ord.MaxValue.Negate() === Ord.MinValue

    //
    // Ord.Zeroth
    //
    // NB: addition to Ord.Zeroth because Ord.Zeroth + i means "i" is the
    // algebraic value of the result. Addition to Ord.First has no such meaning.

    [<Fact>]
    let ``Ord.Zeroth + Int32.MaxValue does not overflow`` () =
        Ord.Zeroth + Int32.MaxValue    |> ignore
        Ord.Zeroth.Add(Int32.MaxValue) |> ignore

    [<Fact>]
    let ``Ord.Zeroth + Int32.MinValue overflows`` () =
        (fun () -> Ord.Zeroth + Int32.MinValue)    |> overflows
        (fun () -> Ord.Zeroth.Add(Int32.MinValue)) |> overflows

    [<Fact>]
    let ``Ord.Zeroth + (Int32.MinValue + 1) overflows`` () =
        (fun () -> Ord.Zeroth + (Int32.MinValue + 1))  |> overflows
        (fun () -> Ord.Zeroth.Add(Int32.MinValue + 1)) |> overflows

    [<Fact>]
    let ``Ord.Zeroth + (Int32.MinValue + 2) does not overflow`` () =
        Ord.Zeroth + (Int32.MinValue + 2)  |> ignore
        Ord.Zeroth.Add(Int32.MinValue + 2) |> ignore

    //
    // Difference
    //

    [<Fact>]
    let ``Ord.MaxValue - Ord.MinValue overflows`` () =
        (fun () -> Ord.MaxValue - Ord.MinValue) |> overflows

    //
    // Operations
    //

    // fsharplint:disable Hints
    [<Property>]
    let ``0 is a neutral element (operators)`` (x: Ord) =
        (x + 0 = x)
        .&. (x - 0 = x)
        .&. (x - x = 0)
    // fsharplint:enable

    [<Property>]
    let ``0 is a neutral element (methods)`` (x: Ord) =
        (x.Add(0) = x)
        .&. (x.Subtract(x) = 0)

    [<Property>]
    let ``Addition and subtraction operators`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (x + n = y)
        .&. (y - n = x)

    [<Property>]
    let ``Difference operator`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (y - x = n)
        .&. (x - y = -n)

    [<Property>]
    let ``Increment operator`` (x: Ord) = x <> Ord.MaxValue &&&& lazy (
        Ord.op_Increment(x) = x + 1
    )

    [<Property>]
    let ``Decrement operator`` (x: Ord) = x <> Ord.MaxValue &&&& lazy (
        Ord.op_Decrement(x) = x - 1
    )

    [<Property>]
    let ``Increment()`` (x: Ord) = x <> Ord.MaxValue &&&& lazy (
        x.Increment() = x + 1
    )

    [<Property>]
    let ``Decrement()`` (x: Ord) = x <> Ord.MinValue &&&& lazy (
        x.Decrement() = x - 1
    )

    [<Property>]
    let ``Add()`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (x.Add(n) = y)
        .&. (y.Add(-n) = x)

    [<Property>]
    let ``Subtract()`` () = xynArbitrary @@@@ fun (x, y, n) ->
        (y.Subtract(x) = n)
        .&. (x.Subtract(y) = -n)

    [<Property>]
    let ``Unary negation operator when Rank > 0`` (x: Position) =
        let ord = Ord.FromRank(x.Value)
        let neg = Ord.FromRank(-x.Value)

        -ord = neg

    [<Property>]
    let ``Unary negation operator when Rank < 0`` (x: Position) =
        let ord = Ord.FromRank(-x.Value)
        let neg = Ord.FromRank(x.Value)

        -ord = neg

    [<Property>]
    let ``Negate() when Rank > 0`` (x: Position) =
        let ord = Ord.FromRank(x.Value)
        let neg = Ord.FromRank(-x.Value)

        ord.Negate() = neg

    [<Property>]
    let ``Negate() when "rank" < 0`` (x: Position) =
        let ord = Ord.FromRank(-x.Value)
        let neg = Ord.FromRank(x.Value)

        ord.Negate() = neg


