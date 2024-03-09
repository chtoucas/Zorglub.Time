// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

[<AutoOpen>]
module Zorglub.Testing.XunitModule

open System
open System.Collections.Generic

open Zorglub.Time.Core.Utilities

open Xunit

/// Verifies that two objects are equal, using a default comparer.
/// Another way to write such a test is "a = b |> ok" but we ought to be careful
/// because equality in F# is structural.
let inline ( === ) (actual: ^a) (expected: ^a) = Assert.Equal<'a>(expected, actual)

/// Verifies that two objects are not equal, using a default comparer.
let inline ( !== ) (actual: ^a) (expected: ^a) = Assert.NotEqual<'a>(expected, actual)

/// Verifies that two objects are the same instance.
let inline ( ==& ) actual expected = Assert.Same(expected, actual)

/// Verifies that an expression is true.
let inline ok condition = Assert.True(condition)

/// Verifies that an expression is false.
let inline nok condition = Assert.False(condition)

/// Verifies that an object is exactly the given type (and not a derived one).
let inline is<'a> object = Assert.IsType<'a>(object) |> ignore

/// Verifies that an object reference is null.
let inline isnull (object: 'a when 'a: not struct) = Assert.Null(object)

/// Verifies that an object reference is null.
let inline isnotnull (object: 'a when 'a: not struct) = Assert.NotNull(object)

/// Verifies that the exact exception is thrown (and not a derived exception type).
let inline throws<'a when 'a :> exn> (testCode: unit -> obj) = Assert.Throws<'a>(testCode) |> ignore

/// Verifies that an ArgumentException is thrown and that it has the given parameter name.
let inline argExn paramName (testCode: unit -> obj) =
    Assert.Throws<ArgumentException>(paramName, testCode) |> ignore

/// Verifies that an ArgumentNullException is thrown and that it has the given parameter name.
let inline nullExn paramName (testCode: unit -> obj) =
    Assert.Throws<ArgumentNullException>(paramName, testCode) |> ignore

/// Verifies that an ArgumentOutOfRangeException is thrown and that it has the given parameter name.
let inline outOfRangeExn paramName (testCode: unit -> obj) =
    Assert.Throws<ArgumentOutOfRangeException>(paramName, testCode) |> ignore

/// Verifies that an KeyNotFoundException is thrown.
let inline keyNotFoundExn (testCode: unit -> obj) =
    Assert.Throws<KeyNotFoundException>(testCode) |> ignore

///// Verifies that an OverflowException is thrown.
//let inline overflows (testCode: unit -> obj) = Assert.Throws<OverflowException>(testCode) |> ignore

/// Verifies that an OverflowException is thrown.
let inline overflows (testCode: unit -> ^a) =
    Assert.Throws<OverflowException>(fun () -> testCode() |> ignore) |> ignore

/// Custom assertions related to Box<'a>.
module BoxAssertions =
    /// Verifies that a box is empty.
    let isempty<'a when 'a: not struct> (box: Box<'a>) =
        box.IsEmpty |> ok
        box.Content |> isnull

    /// Verifies that a box is not empty and contains "expected".
    let issome<'a when 'a: not struct> box expected =
        AssertEx.Some<'a>(expected, box)
        // Not necessary, but we want to ensure that Content returns the genuine object.
        box.Content ==& expected
