// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Testing.FsCheckTests

open Zorglub.Testing

open Zorglub.Time

open global.Xunit
open global.Xunit.Abstractions
open FsCheck
open FsCheck.Xunit

let private localProperty x = x = x
//let private localProperty2 x = x = x |@ sprintf "%A = %A" x x

// https://fscheck.github.io/FsCheck//RunningTests.html#Capturing-output-when-using
// https://stackoverflow.com/questions/31967975/how-to-capture-output-with-xunit-2-0-and-fsharp-style-tests
// https://stackoverflow.com/questions/45444084/customise-fscheck-output

//Arb.Default.Int32()
//Arb.from<int>
// This one does not work, it only generates two values.
//Gen.choose (DayNumber.MinDaysSinceZero, DayNumber.MaxDaysSinceZero)
//|> Gen.map (fun i -> DayNumber.Zero + i)
//|> Arb.fromGen

// Another one.
//Arb.generate<int>
//|> Gen.filter (fun i -> DayNumber.MinDaysSinceZero <= i && i <= DayNumber.MaxDaysSinceZero)
//|> Gen.map (fun i -> DayNumber.Zero + i)
//|> Arb.fromGen

// Using registered arbitrary for int.
[<Property(Verbose = true)>]
let CheckTest1 (x: int32) = localProperty x

#if false

type MyTest(outputHelper: ITestOutputHelper) =
    let mutable _outputHelper = outputHelper

    [<Fact>]
    member __.``Using VerboseCheckThrowOnFailure()`` () =
        let arb = Arb.Default.Int32()
        (Prop.forAll arb localProperty).VerboseCheckThrowOnFailure(_outputHelper)

// Using registered arbitrary for DayNumber.
[<Property(Verbose = true)>]
let CheckTest2 (x: DayNumber) = localProperty x

// Using Arb.Default.Int32().
[<Property(Verbose = true)>]
let CheckTest3 () =
    let arb = Arb.Default.Int32()
    Prop.forAll arb <| localProperty

// Using Arb.generate<int>.
[<Property(Verbose = true)>]
let CheckTest4 () =
    let arb = Arb.generate<int> |> Arb.fromGen
    Prop.forAll arb <| localProperty

// Using Gen.choose.
[<Property(Verbose = true)>]
let CheckTest5 () =
    let arb = Gen.choose (-1000, 1000) |> Arb.fromGen
    Prop.forAll arb <| localProperty

#endif
