// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Horology.Ntp.Timestamp64Tests

open Zorglub.Testing

open Zorglub.Time.Horology.Ntp

open Xunit

// DateTime values are wrong (WIP).

module Prelude =
    [<Fact>]
    let ``Property Zero``() =
        let ts = Timestamp64.Zero
        let time = ts.ToDateTime()

        ts.PseudoEra === 1

        time.Year === 1900
        time.Month === 1
        time.Day === 1
        time.Hour === 0
        time.Minute === 0
        time.Second === 0

    [<Fact>]
    let ``Property MaxValue``() =
        let ts = Timestamp64.MaxValue
        let time = ts.ToDateTime()

        ts.PseudoEra === 0

        time.Year === 2036
        time.Month === 2
        time.Day === 7
        time.Hour === 6
        time.Minute === 28
        time.Second === 15

    [<Fact>]
    let ``Last timestamp for which PseudoEra = 1``() =
        let ts = new Timestamp64(2_147_483_647u, 0u)
        let time = ts.ToDateTime()

        ts.PseudoEra === 1

        time.Year === 1968
        time.Month === 1
        time.Day === 20
        time.Hour === 3
        time.Minute === 14
        time.Second === 7

    [<Fact>]
    let ``First timestamp for which PseudoEra = 0``() =
        let ts = new Timestamp64(2_147_483_648u, 0u)
        let time = ts.ToDateTime()

        ts.PseudoEra === 0

        time.Year === 1968
        time.Month === 1
        time.Day === 20
        time.Hour === 3
        time.Minute === 14
        time.Second === 8
