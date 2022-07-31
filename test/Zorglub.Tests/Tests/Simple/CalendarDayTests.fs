// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.CalendarDayTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time
open Zorglub.Time.Simple

open Xunit

module UserCase =
    [<Fact>]
    let ``FromBinary() throws`` () =
        outOfRangeExn "ident" (fun () -> CalendarDay.FromBinary(Int32.MaxValue))

    [<Fact>]
    let ``ToBinary() throws`` () =
        let date = UserCalendars.Gregorian.GetCalendarDay(DayZero.NewStyle)

        throws<NotSupportedException> (fun () -> date.ToBinary())

module GregorianCase =
    let private chr = SimpleGregorian.Instance
    let private dataSet = ProlepticGregorianDataSet.Instance

    let dayNumberInfoData = dataSet.DayNumberInfoData

    [<Fact>]
    let ``Constructor throws when "dayNumber" is out of range`` () =
        let domainTester = new DomainTester(chr.Domain)

        domainTester.TestInvalidDayNumber(fun x -> new CalendarDay(x))

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let Constructor (info: DayNumberInfo) =
        let dayNumber, y, m, d = info.Deconstruct()
        let date = new CalendarDay(dayNumber)

        date.Year     === y
        date.Month    === m
        date.Day      === d
        date.Calendar ==& chr

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
        let date = chr.GetCalendarDate(y, m, d).ToCalendarDay()

        date.ToString() === str

    //
    // Factories
    //

    [<Fact>]
    let ``Today()`` () =
        let now = DateTime.Now
        let today = CalendarDay.Today()

        today.Year  === now.Year
        today.Month === now.Month
        today.Day   === now.Day

module JulianCase =
    let private chr = SimpleJulian.Instance
    let private dataSet = ProlepticJulianDataSet.Instance

    let dayNumberInfoData = dataSet.DayNumberInfoData

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Roundtrip serialization`` (info: DayNumberInfo) =
        let date = chr.GetCalendarDay(info.DayNumber)

        CalendarDay.FromBinary(date.ToBinary()) === date

module Conversions =
    let data = CalCalDataSet.GregorianToJulianData

    let ``WithCalendar() throws when the result is out of range`` () =
        let chr = SimpleJulian.Instance
        // Julian.MinDayNumber < Gregorian.MinDayNumber.
        let date = chr.GetCalendarDay(chr.Domain.Min)

        outOfRangeExn "dayNumber" (fun () -> date.WithCalendar(SimpleGregorian.Instance))

    [<Theory; MemberData(nameof(data))>]
    let ``WithCalendar() Gregorian <-> Julian`` (pair: YemodaPair) =
        let (g, j) = pair.Deconstruct()
        let gdate = SimpleGregorian.Instance.GetCalendarDate(g.Year, g.Month, g.Day).ToCalendarDay()
        let jdate = SimpleJulian.Instance.GetCalendarDate(j.Year, j.Month, j.Day).ToCalendarDay()

        gdate.WithCalendar(SimpleJulian.Instance)    === jdate
        jdate.WithCalendar(SimpleGregorian.Instance) === gdate
