// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.DateArithmeticTestSuite

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts

open Zorglub.Time.Simple

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Smoke)>]
type GregorianTests() =
    inherit IOrdinalDateArithmeticFacts<OrdinalDate, ProlepticGregorianDataSet>()

    override __.GetDate(y, doy) = GregorianCalendar.Instance.GetOrdinalDate(y, doy)
