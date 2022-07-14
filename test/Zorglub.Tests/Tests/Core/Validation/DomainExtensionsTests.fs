// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Domains.DomainExtensionsTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Validation

open Xunit

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
    (fun () -> DomainExtensions.CheckOverflow(range, DayNumber.Zero - 1)) |> overflows
    DomainExtensions.CheckOverflow(range, DayNumber.Zero)
    DomainExtensions.CheckOverflow(range, DayNumber.Zero + 1)
    DomainExtensions.CheckOverflow(range, DayNumber.Zero + 2)
    (fun () -> DomainExtensions.CheckOverflow(range, DayNumber.Zero + 3)) |> overflows
