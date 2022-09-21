// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.MomentTests

open Zorglub.Testing

open Zorglub.Time

open Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<Moment> === Moment.Zero

    //
    // Properties
    //

    [<Fact>]
    let ``Static property Zero`` () =
        Moment.Zero.DayNumber === DayNumber.Zero
        Moment.Zero.TimeOfDay === TimeOfDay.Midnight
        Moment.Zero.SecondsSinceZero === 0L
        Moment.Zero.MillisecondsSinceZero === 0L

    [<Fact>]
    let ``Static property MinValue`` () =
        Moment.MinValue.DayNumber === DayNumber.MinValue
        Moment.MinValue.TimeOfDay === TimeOfDay.MinValue
        // Along the way, we also check that the foll. props do not overflow.
        Moment.MinValue.SecondsSinceZero      |> ignore
        Moment.MinValue.MillisecondsSinceZero |> ignore

    [<Fact>]
    let ``Static property MaxValue`` () =
        Moment.MaxValue.DayNumber === DayNumber.MaxValue
        Moment.MaxValue.TimeOfDay === TimeOfDay.MaxValue
        // Along the way, we also check that the foll. props do not overflow.
        Moment.MaxValue.SecondsSinceZero      |> ignore
        Moment.MaxValue.MillisecondsSinceZero |> ignore
