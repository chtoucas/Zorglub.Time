// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Core.TemporalArithmeticTests

open Checked

open System
open System.Security.Cryptography

open Zorglub.Testing

open Zorglub.Time.Core

open Xunit

[<Literal>]
let private RandomFrom = 11
let private maxMul = Int64.MaxValue / TemporalConstants.TicksPerDay

let private getFuzzyMul64ForTicksPerDay () =
    let max = int <| Math.Min(int64(Int32.MaxValue), maxMul)
    int64 <| RandomNumberGenerator.GetInt32(RandomFrom, max)

[<Sealed>]
type MultiplierData() as self =
    inherit TheoryData<int>()
    do
        [for i in 0 .. 10 -> self.Add(i)] |> ignore

[<Sealed>]
type ZeroToTwentyThreeData() as self =
    inherit TheoryData<int>()
    do
        [for i in 0 .. 23 -> self.Add(i)] |> ignore

//
// TicksPerDay
//

[<Theory; ClassData(typeof<MultiplierData>)>]
let ``DivideByTicksPerDay()`` (mul: int) =
    let mul64 = int64(mul)

    TemporalArithmetic.DivideByTicksPerDay(mul64 * TemporalConstants.TicksPerDay) === mul64

[<Fact>]
let ``DivideByTicksPerDay() fuzzy`` () =
    let mul64 = getFuzzyMul64ForTicksPerDay()

    TemporalArithmetic.DivideByTicksPerDay(mul64 * TemporalConstants.TicksPerDay) === mul64

[<Theory; ClassData(typeof<MultiplierData>)>]
let ``MultiplyByTicksPerDay()`` (mul: int) =
    let mul64 = int64(mul)

    TemporalArithmetic.MultiplyByTicksPerDay(mul64) === mul64 * TemporalConstants.TicksPerDay

[<Fact>]
let ``MultiplyByTicksPerDay() fuzzy`` () =
    let mul64 = getFuzzyMul64ForTicksPerDay()

    TemporalArithmetic.MultiplyByTicksPerDay(mul64) === mul64 * TemporalConstants.TicksPerDay

//
// NanosecondsPerHour
//

[<Theory; ClassData(typeof<ZeroToTwentyThreeData>)>]
let ``DivideByNanosecondsPerHour()`` (mul: int) =
    TemporalArithmetic.DivideByNanosecondsPerHour(int64(mul) * TemporalConstants.NanosecondsPerHour) === mul

[<Theory; ClassData(typeof<ZeroToTwentyThreeData>)>]
let ``MultiplyByNanosecondsPerHour()`` (mul: int) =
    TemporalArithmetic.MultiplyByNanosecondsPerHour(mul) === int64(mul) * TemporalConstants.NanosecondsPerHour

//
// NanosecondsPerMinute
//

[<Theory; ClassData(typeof<MultiplierData>)>]
let ``DivideByNanosecondsPerMinute()`` (mul: int) =
    TemporalArithmetic.DivideByNanosecondsPerMinute(int64(mul) * TemporalConstants.NanosecondsPerMinute) === mul

[<Fact>]
let ``DivideByNanosecondsPerMinute() fuzzy`` () =
    // In Debug mode, we check that the input is < NanosecondsPerDay,
    // which means that "mul" must be < MinutesPerDay.
    let mul = RandomNumberGenerator.GetInt32(RandomFrom, TemporalConstants.MinutesPerDay)

    TemporalArithmetic.DivideByNanosecondsPerMinute(int64(mul) * TemporalConstants.NanosecondsPerMinute) === mul

[<Theory; ClassData(typeof<MultiplierData>)>]
let ``MultiplyByNanosecondsPerMinute()`` (mul: int) =
    TemporalArithmetic.MultiplyByNanosecondsPerMinute(mul) === int64(mul) * TemporalConstants.NanosecondsPerMinute

[<Fact>]
let ``MultiplyByNanosecondsPerMinute() fuzzy`` () =
    let mul = RandomNumberGenerator.GetInt32(RandomFrom, TemporalConstants.MinutesPerDay)

    TemporalArithmetic.MultiplyByNanosecondsPerMinute(mul) === int64(mul) * TemporalConstants.NanosecondsPerMinute
