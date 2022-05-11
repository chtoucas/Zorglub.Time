// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTests

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Simple

open Xunit

let private chr = GregorianCalendar.Instance
let private supportedYearsTester = new SupportedYearsTester(chr.SupportedYears)

let private dataSet = ProlepticGregorianDataSet.Instance

module Prelude =
    let dateInfoData = dataSet.DateInfoData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        supportedYearsTester.TestInvalidYear(fun y -> new OrdinalDate(y, 1))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``Constructor throws when "dayOfYear" is out of range`` (y: int) doy =
        outOfRangeExn "dayOfYear" (fun () -> new OrdinalDate(y, doy))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let Constructor (info: DateInfo) =
        let y, m, d, doy = info.Deconstruct()
        let date = new OrdinalDate(y, doy)

        date.Year      === y
        date.Month     === m
        date.DayOfYear === doy
        date.Day       === d

    [<Theory>]
    [<InlineData(-1, 1, "001/-0001 (Gregorian)")>]
    [<InlineData(0, 1, "001/0000 (Gregorian)")>]
    [<InlineData(1, 1, "001/0001 (Gregorian)")>]
    [<InlineData(1, 3, "003/0001 (Gregorian)")>]
    [<InlineData(11, 254, "254/0011 (Gregorian)")>]
    [<InlineData(111, 26, "026/0111 (Gregorian)")>]
    [<InlineData(2019, 3, "003/2019 (Gregorian)")>]
    [<InlineData(9999, 365, "365/9999 (Gregorian)")>]
    let ``ToString()`` y doy str =
        let date = chr.GetOrdinalDate(y, doy);

        date.ToString() === str
