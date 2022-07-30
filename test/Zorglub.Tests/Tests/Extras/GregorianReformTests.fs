// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Bulgroz.Extras.GregorianReformTests

open Zorglub.Testing

open Zorglub.Bulgroz.Extras
open Zorglub.Time.Specialized

open Xunit

[<Fact>]
let ``Property Official.SecularShift`` () =
    GregorianReform.Official.SecularShift === 10

[<Fact>]
let ``Property Official (from last Julian date)`` () =
    let date = new JulianDate(1582, 10, 4)
    let reform = GregorianReform.FromLastJulianDate(date)

    GregorianReform.Official === reform

[<Fact>]
let ``Property Official (from first Gregorian date)`` () =
    let date = new GregorianDate(1582, 10, 15)
    let reform = GregorianReform.FromFirstGregorianDate(date)

    GregorianReform.Official === reform
