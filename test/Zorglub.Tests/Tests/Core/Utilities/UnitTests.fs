// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Utilities.UnitTests

open System

open Zorglub.Testing

open Zorglub.Time.Core.Utilities

open Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<Unit> === Unit.Value

    [<Fact>]
    let ``Constructor`` () =
        let unit = new Unit()

        unit === Unit.Value

    [<Fact>]
    let ``ToString()`` ()=
        Unit.Value.ToString() === "()"

module Equality =
    open NonStructuralComparison

    [<Fact>]
    let ``Equality when both operands are identical`` () =
        let x = Unit.Value

        x = x |> ok
        x <> x |> nok
        x.Equals(x) |> ok
        x.Equals(x :> obj) |> ok

    [<Fact>]
    let ``Equality with ValueTuple`` () =
        let x = Unit.Value
        let y = new ValueTuple()

        Unit.op_Equality(x, y) |> ok
        Unit.op_Equality(y, x) |> ok
        Unit.op_Inequality(x, y) |> nok
        Unit.op_Inequality(y, x) |> nok
        x.Equals(y) |> ok
        x.Equals(y :> obj) |> ok

    [<Fact>]
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` () =
        let x = Unit.Value

        x.Equals(null) |> nok
        x.Equals(new obj()) |> nok

    [<Fact>]
    let ``GetHashCode() returns 0`` () =
        Unit.Value.GetHashCode() === 0
