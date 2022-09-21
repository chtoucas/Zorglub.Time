// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Horology.GregorianInstantTests

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Horology

open Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<GregorianInstant> === GregorianInstant.Zero

    //
    // Properties
    //

    [<Fact>]
    let ``Static property Zero`` () =
        GregorianInstant.Zero.DayNumber === DayNumber.Zero
        GregorianInstant.Zero.InstantOfDay === InstantOfDay.Midnight
        GregorianInstant.Zero.SecondsSinceZero === 0L
        GregorianInstant.Zero.MillisecondsSinceZero === 0L
        GregorianInstant.Zero.NanosecondsSinceZero === 0L

    [<Fact>]
    let ``Static property MinValue`` () =
        GregorianInstant.MinValue.DayNumber === DayNumber.MinValue
        GregorianInstant.MinValue.InstantOfDay === InstantOfDay.MinValue
        // Along the way, we also check that the foll. props do not overflow.
        GregorianInstant.MinValue.SecondsSinceZero      |> ignore
        GregorianInstant.MinValue.MillisecondsSinceZero |> ignore
        overflows (fun () -> GregorianInstant.MinValue.NanosecondsSinceZero)

    [<Fact>]
    let ``Static property MaxValue`` () =
        GregorianInstant.MaxValue.DayNumber === DayNumber.MaxValue
        GregorianInstant.MaxValue.InstantOfDay === InstantOfDay.MaxValue
        // Along the way, we also check that the foll. props do not overflow.
        GregorianInstant.MaxValue.SecondsSinceZero      |> ignore
        GregorianInstant.MaxValue.MillisecondsSinceZero |> ignore
        overflows (fun () -> GregorianInstant.MaxValue.NanosecondsSinceZero)
