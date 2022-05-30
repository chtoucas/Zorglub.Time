// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarYearTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Simple

open Xunit

module UserCase =
    [<Fact>]
    let ``FromBinary() throws`` () =
        outOfRangeExn "ident" (fun () -> CalendarYear.FromBinary(Int32.MaxValue))

    [<Fact>]
    let ``ToBinary() throws`` () =
        let year = UserCalendars.Gregorian.GetCalendarYear(1)

        throws<NotSupportedException> (fun () -> year.ToBinary())

module GregorianCase =
    let private chr = GregorianCalendar.Instance
    let private dataSet = ProlepticGregorianDataSet.Instance
    let private domainTester = new DomainTester(chr.Domain)

    let dayNumberInfoData = dataSet.DayNumberInfoData
    let yearInfoData = dataSet.YearInfoData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        let supportedYearsTester = new SupportedYearsTester(chr.SupportedYears)

        supportedYearsTester.TestInvalidYear(fun y -> new CalendarYear(y))

    [<Theory; MemberData(nameof(yearInfoData))>]
    let Constructor (info: YearInfo) =
        let y = info.Year
        let year = new CalendarYear(y)

        year.Year      === y
        year.Calendar  ==& chr

    [<Theory>]
    [<InlineData(-1, "-0001 (Gregorian)")>]
    [<InlineData(0, "0000 (Gregorian)")>]
    [<InlineData(1, "0001 (Gregorian)")>]
    [<InlineData(11, "0011 (Gregorian)")>]
    [<InlineData(111, "0111 (Gregorian)")>]
    [<InlineData(2019, "2019 (Gregorian)")>]
    [<InlineData(9999, "9999 (Gregorian)")>]
    let ``ToString()`` y str =
        let year = chr.GetCalendarYear(y)

        year.ToString() === str

    //
    // Factories
    //

    [<Fact>]
    let ``GetCurrentYear()`` () =
        let now = DateTime.Now
        let year = CalendarYear.GetCurrentYear()

        year.Year  === now.Year

    [<Fact>]
    let ``FromDayNumber() invalid dayNumber`` () =
        domainTester.TestInvalidDayNumber(CalendarYear.FromDayNumber)

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``FromDayNumber()`` (info: DayNumberInfo) =
        let (dayNumber, y, _, _) = info.Deconstruct()
        let year = CalendarYear.FromDayNumber(dayNumber)

        year.Year  === y
