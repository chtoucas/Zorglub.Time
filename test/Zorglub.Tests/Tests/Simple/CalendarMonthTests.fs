// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMonthTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Simple

open Xunit

module GregorianCase =
    let private dataSet = ProlepticGregorianDataSet.Instance

    let monthInfoData = dataSet.MonthInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        let chr = GregorianCalendar.Instance
        let supportedYearsTester = new SupportedYearsTester(chr.SupportedYears)

        supportedYearsTester.TestInvalidYear(fun y -> new CalendarMonth(y, 1))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``Constructor throws when "month" is out of range`` (y: int) m =
        outOfRangeExn "month" (fun () -> new CalendarMonth(y, m))

    [<Theory; MemberData(nameof(monthInfoData))>]
    let Constructor (info: MonthInfo) =
        let y, m = info.Yemo.Deconstruct()
        let month = new CalendarMonth(y, m)

        month.Year        === y
        month.MonthOfYear === m
        month.Calendar    ==& GregorianCalendar.Instance

    [<Theory>]
    [<InlineData(-1, 1, "01/-0001 (Gregorian)")>]
    [<InlineData(0, 1, "01/0000 (Gregorian)")>]
    [<InlineData(1, 1, "01/0001 (Gregorian)")>]
    [<InlineData(1, 2, "02/0001 (Gregorian)")>]
    [<InlineData(11, 12, "12/0011 (Gregorian)")>]
    [<InlineData(111, 3, "03/0111 (Gregorian)")>]
    [<InlineData(2019, 1, "01/2019 (Gregorian)")>]
    [<InlineData(9999, 12,  "12/9999 (Gregorian)")>]
    let ``ToString()`` y m str =
        let month = GregorianCalendar.Instance.GetCalendarMonth(y, m)

        month.ToString() === str

    [<Fact>]
    let ``GetCurrentMonth()`` () =
        let now = DateTime.Now
        let month = CalendarMonth.GetCurrentMonth()

        month.Year        === now.Year
        month.MonthOfYear === now.Month
