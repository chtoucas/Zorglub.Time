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

module UserCase =
    [<Fact>]
    let ``FromBinary() throws`` () =
        let parts = new Yemodax(1, 1, 1, int(UserCalendars.Gregorian.Id))
        let data = parts.ToBinary()

        outOfRangeExn "ident" (fun () -> CalendarDate.FromBinary(data))
        outOfRangeExn "ident" (fun () -> CalendarDate.FromBinary(Int32.MaxValue))

    [<Fact>]
    let ``ToBinary() throws`` () =
        let date = UserCalendars.Gregorian.GetCalendarDate(1, 1, 1)

        throws<NotSupportedException> (fun () -> date.ToBinary())

module GregorianCase =
    let private chr = GregorianCalendar.Instance
    let private dataSet = ProlepticGregorianDataSet.Instance
    let private domainTester = new DomainTester(chr.Domain)

    let dayNumberInfoData = dataSet.DayNumberInfoData
    let dateInfoData = dataSet.DateInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        let supportedYearsTester = new SupportedYearsTester(chr.SupportedYearsObsolete)

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
        date.Calendar  ==& chr

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
        let date = new CalendarDate(y, m, d)

        date.ToString() === str

    //
    // Factories
    //

    [<Fact>]
    let ``Today()`` () =
        let now = DateTime.Now
        let today = CalendarDate.Today()

        today.Year  === now.Year
        today.Month === now.Month
        today.Day   === now.Day

    //
    // Conversions
    //

    [<Fact>]
    let ``FromDayNumber() invalid dayNumber`` () =
        domainTester.TestInvalidDayNumber(CalendarDate.FromDayNumber)

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``FromDayNumber()`` (info: DayNumberInfo) =
        let (dayNumber, y, m, d) = info.Deconstruct()
        let date = CalendarDate.FromDayNumber(dayNumber)

        date.Year  === y
        date.Month === m
        date.Day   === d

    //
    // Adjustments
    //

    [<Fact>]
    let ``WithYear() invalid result`` () =
        // Intercalary day mapped to a common year.
        let date = new CalendarDate(4, 2, 29)

        outOfRangeExn "newYear" (fun () -> date.WithYear(3))

    [<Fact>]
    let ``WithYear() valid result`` () =
        // Intercalary day mapped to another leap year.
        let date = new CalendarDate(4, 2, 29)
        let exp = new CalendarDate(8, 2, 29)

        date.WithYear(8) === exp

module JulianCase =
    let private chr = JulianCalendar.Instance
    let private dataSet = ProlepticJulianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData

    // Added because, currently, the test suite only works with the Gregorian calendar.
    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Roundtrip serialization`` (info: DateInfo) =
        let y, m, d = info.Yemoda.Deconstruct()
        let date = chr.GetCalendarDate(y, m, d)

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
