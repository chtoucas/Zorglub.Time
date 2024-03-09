// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Validation.DomainExtensionsTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Validation

open Xunit

[<Fact>]
let ``Validate()`` () =
    let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

    outOfRangeExn "paramName" (fun () -> range.Validate(DayNumber.Zero - 1, "paramName"))
    outOfRangeExn "dayNumber" (fun () -> range.Validate(DayNumber.Zero - 1))
    range.Validate(DayNumber.Zero)
    range.Validate(DayNumber.Zero + 1)
    range.Validate(DayNumber.Zero + 2)
    outOfRangeExn "dayNumber" (fun () -> range.Validate(DayNumber.Zero + 3))
    outOfRangeExn "paramName" (fun () -> range.Validate(DayNumber.Zero + 3, "paramName"))

[<Fact>]
let ``CheckOverflow()`` () =
    let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

    (fun () -> range.CheckOverflow(DayNumber.Zero - 1)) |> overflows
    range.CheckOverflow(DayNumber.Zero)
    range.CheckOverflow(DayNumber.Zero + 1)
    range.CheckOverflow(DayNumber.Zero + 2)
    (fun () -> range.CheckOverflow(DayNumber.Zero + 3)) |> overflows

[<Fact>]
let ``CheckUpperBound()`` () =
    let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

    range.CheckUpperBound(DayNumber.Zero - 1)
    range.CheckUpperBound(DayNumber.Zero)
    range.CheckUpperBound(DayNumber.Zero + 1)
    range.CheckUpperBound(DayNumber.Zero + 2)
    (fun () -> range.CheckUpperBound(DayNumber.Zero + 3)) |> overflows

[<Fact>]
let ``CheckLowerBound()`` () =
    let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

    (fun () -> range.CheckLowerBound(DayNumber.Zero - 1)) |> overflows
    range.CheckLowerBound(DayNumber.Zero)
    range.CheckLowerBound(DayNumber.Zero + 1)
    range.CheckLowerBound(DayNumber.Zero + 2)
    range.CheckLowerBound(DayNumber.Zero + 3)
