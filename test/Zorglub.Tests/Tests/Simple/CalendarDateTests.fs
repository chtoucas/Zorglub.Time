// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDateTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

module GregorianCase =
    let private gchr = GregorianCalendar.Instance
    let private supportedYearsTester = new SupportedYearsTester(gchr.SupportedYears)

    let private dataSet = ProlepticGregorianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
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
        date.Calendar  ==& gchr

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
        let date = gchr.GetCalendarDate(y, m, d)

        date.ToString() === str

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Roundtrip serialization`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        let date = new CalendarDate(y, m, d)

        CalendarDate.FromBinary(date.ToBinary()) === date

    [<Fact>]
    let ``Today()`` () =
        let now = DateTime.Now
        let today = CalendarDate.Today()

        today.Year  === now.Year
        today.Month === now.Month
        today.Day   === now.Day

module JulianCase =
    let private jchr = JulianCalendar.Instance

    let private dataSet = ProlepticJulianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Roundtrip serialization`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        let date = jchr.GetCalendarDate(y, m, d)

        CalendarDate.FromBinary(date.ToBinary()) === date

module Conversions =
    let private gchr = GregorianCalendar.Instance
    let private jchr = JulianCalendar.Instance

    // TODO(code): filter data. Idem with OrdinalDateTests and CalendarDayTests.
    let data = CalCalDataSet.GregorianJulianData

    let ``WithCalendar() throws when the date is out of range`` () =
        // Julian.MinDayNumber < Gregorian.MinDayNumber.
        let minDayNumber = jchr.Domain.Min
        let date = jchr.GetCalendarDateOn(minDayNumber)

        outOfRangeExn "dayNumber" (fun () -> date.WithCalendar(gchr))

    [<Theory; MemberData(nameof(data))>]
    let ``WithCalendar() Gregorian <-> Julian`` (g: Yemoda) (j: Yemoda) =
        let gdate = gchr.GetCalendarDate(g.Year, g.Month, g.Day)
        let jdate = jchr.GetCalendarDate(j.Year, j.Month, j.Day)

        gdate.WithCalendar(jchr) === jdate
        jdate.WithCalendar(gchr) === gdate
