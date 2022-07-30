// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Time.Extras.GregorianReformTests

open Zorglub.Testing

open Xunit

module Specialized =
    open Zorglub.Time.Extras
    open Zorglub.Time.Specialized

    [<Fact>]
    let ``Property Official.SecularShift`` () =
        GregorianReform.Official.SecularShift === 10

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from last Julian date)`` () =
        let date = new JulianDate(1582, 10, 4)
        let reform = GregorianReform.FromLastJulianDate(date)

        GregorianReform.Official === reform

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from first Gregorian date)`` () =
        let date = new GregorianDate(1582, 10, 15)
        let reform = GregorianReform.FromFirstGregorianDate(date)

        GregorianReform.Official === reform

module Simple =
    open Zorglub.Time.Simple

    [<Fact>]
    let ``Property Official.SecularShift`` () =
        GregorianReform.Official.SecularShift === 10

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from last Julian date)`` () =
        let reform = GregorianReform.FromLastJulianDate(1582, 10, 4)

        GregorianReform.Official === reform

    [<Fact>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``Property Official (from first Gregorian date)`` () =
        let reform = GregorianReform.FromFirstGregorianDate(1582, 10, 15)

        GregorianReform.Official === reform
