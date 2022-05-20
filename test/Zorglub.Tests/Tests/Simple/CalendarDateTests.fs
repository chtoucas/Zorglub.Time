// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDateTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Simple

open Xunit

module GregorianCase =
    let private dataSet = ProlepticGregorianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        let chr = GregorianCalendar.Instance
        let supportedYearsTester = new SupportedYearsTester(chr.SupportedYears)

        supportedYearsTester.TestInvalidYear(fun y -> new CalendarDate(y, 1, 1))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``Constructor throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> new CalendarDate(y, m, 1))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``Constructor throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> new CalendarDate(y, m, d))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let Constructor (info: DateInfo) =
        let y, m, d, doy = info.Deconstruct()
        let date = new CalendarDate(y, m, d)

        date.Year      === y
        date.Month     === m
        date.DayOfYear === doy
        date.Day       === d
        date.Calendar  ==& GregorianCalendar.Instance

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
        let date = GregorianCalendar.Instance.GetCalendarDate(y, m, d)

        date.ToString() === str

    [<Fact>]
    let ``Today()`` () =
        let now = DateTime.Now
        let today = CalendarDate.Today()

        today.Year  === now.Year
        today.Month === now.Month
        today.Day   === now.Day

module JulianCase =
    let private dataSet = ProlepticJulianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData

    // Added because, currently, the test suite only works with the Gregorian calendar.
    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Roundtrip serialization`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        let date = JulianCalendar.Instance.GetCalendarDate(y, m, d)

        CalendarDate.FromBinary(date.ToBinary()) === date

module Conversions =
    // TODO(code): filter data. Idem with OrdinalDateTests and CalendarDayTests.
    let data = CalCalDataSet.GregorianToJulianData

    let ``WithCalendar() throws when the result is out of range`` () =
        let chr = JulianCalendar.Instance
        // Julian.MinDayNumber < Gregorian.MinDayNumber.
        let date = chr.GetCalendarDateOn(chr.Domain.Min)

        outOfRangeExn "dayNumber" (fun () -> date.WithCalendar(GregorianCalendar.Instance))

    [<Theory; MemberData(nameof(data))>]
    let ``WithCalendar() Gregorian <-> Julian`` (pair: YemodaPair) =
        let (g, j) = pair.Deconstruct()
        let gdate = GregorianCalendar.Instance.GetCalendarDate(g.Year, g.Month, g.Day)
        let jdate = JulianCalendar.Instance.GetCalendarDate(j.Year, j.Month, j.Day)

        gdate.WithCalendar(JulianCalendar.Instance)    === jdate
        jdate.WithCalendar(GregorianCalendar.Instance) === gdate
