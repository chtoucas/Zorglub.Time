// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarMonthTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

module UserCase =
    [<Fact>]
    let ``FromBinary() throws`` () =
        let parts = new Yemox(1, 1, int(UserCalendars.Gregorian.Id))
        let data = parts.ToBinary()

        // NB: data = Int32.MaxValue is not valid, contrary to the other types
        // like CalendarYear or CalendarDate.
        outOfRangeExn "ident" (fun () -> CalendarMonth.FromBinary(data))

    [<Fact>]
    let ``ToBinary() throws`` () =
        let month = UserCalendars.Gregorian.GetCalendarMonth(1, 1)

        throws<NotSupportedException> (fun () -> month.ToBinary())

module GregorianCase =
    let private chr = GregorianCalendar.Instance
    let private dataSet = ProlepticGregorianDataSet.Instance
    let private domainTester = new DomainTester(chr.Domain)

    let dayNumberInfoData = dataSet.DayNumberInfoData
    let monthInfoData = dataSet.MonthInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        let supportedYearsTester = new SupportedYearsTester(chr.SupportedYears)

        supportedYearsTester.TestInvalidYear(fun y -> new CalendarMonth(y, 1))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``Constructor throws when "month" is out of range`` (y: int) m =
        outOfRangeExn "month" (fun () -> new CalendarMonth(y, m))

    [<Theory; MemberData(nameof(monthInfoData))>]
    let Constructor (info: MonthInfo) =
        let y, m = info.Yemo.Deconstruct()
        let month = new CalendarMonth(y, m)

        month.Year     === y
        month.Month    === m
        month.Calendar ==& chr

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
        let month = chr.GetCalendarMonth(y, m)

        month.ToString() === str

    //
    // Factories
    //

    [<Fact>]
    let ``GetCurrentMonth()`` () =
        let now = DateTime.Now
        let month = CalendarMonth.GetCurrentMonth()

        month.Year  === now.Year
        month.Month === now.Month

    [<Fact>]
    let ``FromDayNumber() invalid dayNumber`` () =
        domainTester.TestInvalidDayNumber(CalendarMonth.FromDayNumber)

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``FromDayNumber()`` (info: DayNumberInfo) =
        let (dayNumber, y, m, _) = info.Deconstruct()
        let date = CalendarMonth.FromDayNumber(dayNumber)

        date.Year  === y
        date.Month === m
