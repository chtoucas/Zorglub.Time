// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Testing.TraitsTests

open Zorglub.Testing

open Xunit

// One can manually check that traits are working.
//
// Plan: 11 tests
//   TraitsTests
// Plan: 5 tests
//   "TraitsTests&ExcludeFrom!=Smoke"
//   "TraitsTests&ExcludeFrom!=CodeCoverage"
// Plan: 3 tests
//   "TraitsTests&ExcludeFrom!=Smoke&Performance!=Slow"
// Plan: 2 tests
//   "TraitsTests&ExcludeFrom!=Smoke&ExcludeFrom!=CodeCoverage"
// Plan: 1 test
//   "TraitsTests&ExcludeFrom!=Smoke&Performance!~Slow"

[<Fact>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
let ``ExcludeFrom = Smoke`` () = ()

[<Fact>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``ExcludeFrom = CodeCoverage`` () = ()

[<Fact>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``ExcludeFrom = CodeCoverage and Smoke`` () = ()

//
// Slow
//

[<Fact>]
[<TestPerformance(TestPerformance.Slow)>]
let ``Performance = Slow`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.Slow)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
let ``Performance = Slow AND ExcludeFrom = Smoke`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.Slow)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = Slow AND ExcludeFrom = CodeCoverage`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.Slow)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = Slow AND ExcludeFrom = Smoke and CodeCoverage`` () = ()

//
// VerySlow
//

[<Fact>]
[<TestPerformance(TestPerformance.VerySlow)>]
let ``Performance = VerySlow`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.VerySlow)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
let ``Performance = VerySlow AND ExcludeFrom = Smoke`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.VerySlow)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = VerySlow AND ExcludeFrom = CodeCoverage`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.VerySlow)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = VerySlow AND ExcludeFrom = Smoke and CodeCoverage`` () = ()
