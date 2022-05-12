// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDayTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

module GregorianCase =
    let private gchr = GregorianCalendar.Instance
    let private domainTester = new DomainTester(gchr.Domain)

    let private dataSet = ProlepticGregorianDataSet.Instance

    let dayNumberInfoData = dataSet.DayNumberInfoData

    [<Fact>]
    let ``Constructor throws when "dayNumber" is out of range`` () =
        domainTester.TestInvalidDayNumber(fun x -> new CalendarDay(x))

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let Constructor (info: DayNumberInfo) =
        let dayNumber, y, m, d = info.Deconstruct()
        let date = new CalendarDay(dayNumber)

        date.Year     === y
        date.Month    === m
        date.Day      === d
        date.Calendar ==& gchr

    [<Theory>]
    [<InlineData(-1, 1, 1, "01/01/-0001 (Gregorian)")>]
    [<InlineData(0, 1, 1, "01/01/0000 (Gregorian)")>]
    [<InlineData(1, 1, 1, "01/01/0001 (Gregorian)")>]
    [<InlineData(1, 2, 3, "03/02/0001 (Gregorian)")>]
    [<InlineData(11, 12, 13, "13/12/0011 (Gregorian)")>]
    [<InlineData(111, 3, 6, "06/03/0111 (Gregorian)")>]
    [<InlineData(2019, 1, 3, "03/01/2019 (Gregorian)")>]
    [<InlineData(9999, 12, 31, "31/12/9999 (Gregorian)")>]
    let ``ToString()`` y m d str =
        let date = gchr.GetCalendarDate(y, m, d).ToCalendarDay()

        date.ToString() === str

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Roundtrip serialization`` (info: DayNumberInfo) =
        let date = new CalendarDay(info.DayNumber)

        CalendarDay.FromBinary(date.ToBinary()) === date

    [<Fact>]
    let ``Today()`` () =
        let now = DateTime.Now
        let today = CalendarDay.Today()

        today.Year  === now.Year
        today.Month === now.Month
        today.Day   === now.Day

module JulianCase =
    let private jchr = JulianCalendar.Instance

    let private dataSet = ProlepticJulianDataSet.Instance

    let dayNumberInfoData = dataSet.DayNumberInfoData

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Roundtrip serialization`` (info: DayNumberInfo) =
        let date = jchr.GetCalendarDay(info.DayNumber)

        CalendarDay.FromBinary(date.ToBinary()) === date

module Conversions =
    let private gchr = GregorianCalendar.Instance
    let private jchr = JulianCalendar.Instance

    let data = CalCalDataSet.GregorianJulianData

    let ``WithCalendar() throws when the date is out of range`` () =
        // Julian.MinDayNumber < Gregorian.MinDayNumber.
        let minDayNumber = jchr.Domain.Min
        let date = jchr.GetCalendarDay(minDayNumber)

        outOfRangeExn "dayNumber" (fun () -> date.WithCalendar(gchr))

    [<Theory; MemberData(nameof(data))>]
    let ``WithCalendar() Gregorian <-> Julian`` (g: Yemoda) (j: Yemoda) =
        let gdate = gchr.GetCalendarDate(g.Year, g.Month, g.Day).ToCalendarDay()
        let jdate = jchr.GetCalendarDate(j.Year, j.Month, j.Day).ToCalendarDay()

        gdate.WithCalendar(jchr) === jdate
        jdate.WithCalendar(gchr) === gdate
