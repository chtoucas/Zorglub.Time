// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Domains.RangeExtensionsTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Domains
open Zorglub.Time.Core.Intervals

open Xunit

open type Zorglub.Time.Core.Domains.RangeExtensions

[<Fact>]
let ``Validate()`` () =
    let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

    outOfRangeExn "dayNumber" (fun () -> range.Validate(DayNumber.Zero - 1))
    range.Validate(DayNumber.Zero)
    range.Validate(DayNumber.Zero + 1)
    range.Validate(DayNumber.Zero + 2)
    outOfRangeExn "dayNumber" (fun () -> range.Validate(DayNumber.Zero + 3))

[<Fact>]
let ``CheckOverflow()`` () =
    let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

    // REVIEW(code): why can't I use the extension method syntax?
    (fun () -> RangeExtensions.CheckOverflow(range, DayNumber.Zero - 1)) |> overflows
    RangeExtensions.CheckOverflow(range, DayNumber.Zero)
    RangeExtensions.CheckOverflow(range, DayNumber.Zero + 1)
    RangeExtensions.CheckOverflow(range, DayNumber.Zero + 2)
    (fun () -> RangeExtensions.CheckOverflow(range, DayNumber.Zero + 3)) |> overflows
