// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.Specialized.GregorianReformTests

open Zorglub.Testing

open Zorglub.Time.Simple.Specialized

open Xunit

[<Fact>]
let ``Property Official.SecularShift`` () =
    GregorianReform.Official.SecularShift === 10

[<Fact>]
let ``Property Official (from last Julian date)`` () =
    let reform = GregorianReform.FromLastJulianDate(1582, 10, 4)

    GregorianReform.Official === reform

[<Fact>]
let ``Property Official (from first Gregorian date)`` () =
    let reform = GregorianReform.FromFirstGregorianDate(1582, 10, 15)

    GregorianReform.Official === reform

