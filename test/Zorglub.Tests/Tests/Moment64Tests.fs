// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Moment64Tests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Horology

open Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<Moment64> === Moment64.Zero

    //
    // Properties
    //

    [<Fact>]
    let ``Static property Zero`` () =
        Moment64.Zero.DayNumber === DayNumber.Zero
        Moment64.Zero.TimeOfDay === TimeOfDay64.Midnight
        Moment64.Zero.SecondsSinceZero === 0
        Moment64.Zero.MillisecondsSinceZero === 0
        Moment64.Zero.NanosecondsSinceZero === 0

    [<Fact>]
    let ``Static property MinValue`` () =
        Moment64.MinValue.DayNumber === DayNumber.MinValue
        Moment64.MinValue.TimeOfDay === TimeOfDay64.MinValue
        // Along the way, we also check that the foll. props do not overflow.
        Moment64.MinValue.SecondsSinceZero      |> ignore
        Moment64.MinValue.MillisecondsSinceZero |> ignore
        overflows (fun () -> Moment64.MinValue.NanosecondsSinceZero)

    [<Fact>]
    let ``Static property MaxValue`` () =
        Moment64.MaxValue.DayNumber === DayNumber.MaxValue
        Moment64.MaxValue.TimeOfDay === TimeOfDay64.MaxValue
        // Along the way, we also check that the foll. props do not overflow.
        Moment64.MaxValue.SecondsSinceZero      |> ignore
        Moment64.MaxValue.MillisecondsSinceZero |> ignore
        overflows (fun () -> Moment64.MaxValue.NanosecondsSinceZero)
