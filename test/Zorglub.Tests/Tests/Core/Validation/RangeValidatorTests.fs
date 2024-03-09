// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.Validation.RangeValidatorTests

open Zorglub.Testing

open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Utilities
open Zorglub.Time.Core.Validation

open Xunit
open FsCheck.Xunit

module DaysValidator =
    let private validator = new DaysValidator(Range.Create(0, 2))

    [<Property>]
    let ``Properties`` i j =
        let range = Range.FromEndpoints(OrderedPair.Create(i, j))
        let validator = new DaysValidator(range)

        validator.Range === range
        validator.MinDaysSinceEpoch === range.Min
        validator.MaxDaysSinceEpoch === range.Max

    [<Property>]
    let ``ToString()`` i j =
        let range = Range.FromEndpoints(OrderedPair.Create(i, j))
        let validator = new DaysValidator(range)

        validator.ToString() === range.ToString()

    [<Fact>]
    let ``Validate()`` () =
        outOfRangeExn "paramName" (fun () -> validator.Validate(-1, "paramName"))
        outOfRangeExn "daysSinceEpoch" (fun () -> validator.Validate(-1))
        validator.Validate(0)
        validator.Validate(1)
        validator.Validate(2)
        outOfRangeExn "daysSinceEpoch" (fun () -> validator.Validate(3))
        outOfRangeExn "paramName" (fun () -> validator.Validate(3, "paramName"))

    [<Fact>]
    let ``CheckOverflow()`` () =
        (fun () -> validator.CheckOverflow(-1)) |> overflows
        validator.CheckOverflow(0)
        validator.CheckOverflow(1)
        validator.CheckOverflow(2)
        (fun () -> validator.CheckOverflow(3)) |> overflows

    [<Fact>]
    let ``CheckUpperBound()`` () =
        validator.CheckUpperBound(-1)
        validator.CheckUpperBound(0)
        validator.CheckUpperBound(1)
        validator.CheckUpperBound(2)
        (fun () -> validator.CheckUpperBound(3)) |> overflows

    [<Fact>]
    let ``CheckLowerBound()`` () =
        (fun () -> validator.CheckLowerBound(-1)) |> overflows
        validator.CheckLowerBound(0)
        validator.CheckLowerBound(1)
        validator.CheckLowerBound(2)
        validator.CheckLowerBound(3)

module MonthsValidator =
    let private validator = new MonthsValidator(Range.Create(0, 2))

    [<Property>]
    let ``Properties`` i j =
        let range = Range.FromEndpoints(OrderedPair.Create(i, j))
        let validator = new MonthsValidator(range)

        validator.Range === range
        validator.MinMonthsSinceEpoch === range.Min
        validator.MaxMonthsSinceEpoch === range.Max

    [<Property>]
    let ``ToString()`` i j =
        let range = Range.FromEndpoints(OrderedPair.Create(i, j))
        let validator = new MonthsValidator(range)

        validator.ToString() === range.ToString()

    [<Fact>]
    let ``Validate()`` () =
        outOfRangeExn "paramName" (fun () -> validator.Validate(-1, "paramName"))
        outOfRangeExn "monthsSinceEpoch" (fun () -> validator.Validate(-1))
        validator.Validate(0)
        validator.Validate(1)
        validator.Validate(2)
        outOfRangeExn "monthsSinceEpoch" (fun () -> validator.Validate(3))
        outOfRangeExn "paramName" (fun () -> validator.Validate(3, "paramName"))

    [<Fact>]
    let ``CheckOverflow()`` () =
        (fun () -> validator.CheckOverflow(-1)) |> overflows
        validator.CheckOverflow(0)
        validator.CheckOverflow(1)
        validator.CheckOverflow(2)
        (fun () -> validator.CheckOverflow(3)) |> overflows

    [<Fact>]
    let ``CheckUpperBound()`` () =
        validator.CheckUpperBound(-1)
        validator.CheckUpperBound(0)
        validator.CheckUpperBound(1)
        validator.CheckUpperBound(2)
        (fun () -> validator.CheckUpperBound(3)) |> overflows

    [<Fact>]
    let ``CheckLowerBound()`` () =
        (fun () -> validator.CheckLowerBound(-1)) |> overflows
        validator.CheckLowerBound(0)
        validator.CheckLowerBound(1)
        validator.CheckLowerBound(2)
        validator.CheckLowerBound(3)

module YearsValidator =
    let private validator = new YearsValidator(Range.Create(0, 2))

    [<Property>]
    let ``Properties`` i j =
        let range = Range.FromEndpoints(OrderedPair.Create(i, j))
        let validator = new YearsValidator(range)

        validator.Range === range
        validator.MinYear === range.Min
        validator.MaxYear === range.Max

    [<Property>]
    let ``ToString()`` i j =
        let range = Range.FromEndpoints(OrderedPair.Create(i, j))
        let validator = new YearsValidator(range)

        validator.ToString() === range.ToString()

    [<Fact>]
    let ``Validate()`` () =
        outOfRangeExn "paramName" (fun () -> validator.Validate(-1, "paramName"))
        outOfRangeExn "year" (fun () -> validator.Validate(-1))
        validator.Validate(0)
        validator.Validate(1)
        validator.Validate(2)
        outOfRangeExn "year" (fun () -> validator.Validate(3))
        outOfRangeExn "paramName" (fun () -> validator.Validate(3, "paramName"))

    [<Fact>]
    let ``CheckOverflow()`` () =
        (fun () -> validator.CheckOverflow(-1)) |> overflows
        validator.CheckOverflow(0)
        validator.CheckOverflow(1)
        validator.CheckOverflow(2)
        (fun () -> validator.CheckOverflow(3)) |> overflows

    [<Fact>]
    let ``CheckUpperBound()`` () =
        validator.CheckUpperBound(-1)
        validator.CheckUpperBound(0)
        validator.CheckUpperBound(1)
        validator.CheckUpperBound(2)
        (fun () -> validator.CheckUpperBound(3)) |> overflows

    [<Fact>]
    let ``CheckLowerBound()`` () =
        (fun () -> validator.CheckLowerBound(-1)) |> overflows
        validator.CheckLowerBound(0)
        validator.CheckLowerBound(1)
        validator.CheckLowerBound(2)
        validator.CheckLowerBound(3)
