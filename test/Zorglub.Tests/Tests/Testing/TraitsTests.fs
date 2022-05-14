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
//   "TraitsTests&ExcludeFrom!=Smoke&Performance!=SlowGroup"
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
// SlowUnit
//

[<Fact>]
[<TestPerformance(TestPerformance.SlowUnit)>]
let ``Performance = SlowUnit`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.SlowUnit)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
let ``Performance = SlowUnit AND ExcludeFrom = Smoke`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.SlowUnit)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = SlowUnit AND ExcludeFrom = CodeCoverage`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.SlowUnit)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = SlowUnit AND ExcludeFrom = Smoke and CodeCoverage`` () = ()

//
// SlowGroup
//

[<Fact>]
[<TestPerformance(TestPerformance.SlowGroup)>]
let ``Performance = SlowGroup`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
let ``Performance = SlowGroup AND ExcludeFrom = Smoke`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = SlowGroup AND ExcludeFrom = CodeCoverage`` () = ()

[<Fact>]
[<TestPerformance(TestPerformance.SlowGroup)>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
let ``Performance = SlowGroup AND ExcludeFrom = Smoke and CodeCoverage`` () = ()
