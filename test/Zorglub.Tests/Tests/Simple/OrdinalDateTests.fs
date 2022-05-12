// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

module GregorianCase =
    let private dataSet = ProlepticGregorianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        let chr = GregorianCalendar.Instance
        let supportedYearsTester = new SupportedYearsTester(chr.SupportedYears)

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
        date.Calendar  ==& GregorianCalendar.Instance

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
        let date = GregorianCalendar.Instance.GetOrdinalDate(y, doy)

        date.ToString() === str

    [<Fact>]
    let ``Today()`` () =
        let now = DateTime.Now
        let today = OrdinalDate.Today()

        today.Year  === now.Year
        today.Month === now.Month
        today.Day   === now.Day

module JulianCase =
    let private dataSet = ProlepticJulianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Roundtrip serialization`` (info: DateInfo) =
        let y, doy = info.Yedoy.Deconstruct()
        let date = JulianCalendar.Instance.GetOrdinalDate(y, doy)

        OrdinalDate.FromBinary(date.ToBinary()) === date

module Conversions =
    let data = CalCalDataSet.GregorianJulianData

    let ``WithCalendar() throws when the result is out of range`` () =
        let chr = JulianCalendar.Instance
        // Julian.MinDayNumber < Gregorian.MinDayNumber.
        let date = chr.GetOrdinalDateOn(chr.Domain.Min)

        outOfRangeExn "dayNumber" (fun () -> date.WithCalendar(GregorianCalendar.Instance))

    [<Theory; MemberData(nameof(data))>]
    let ``WithCalendar() Gregorian <-> Julian`` (g: Yemoda) (j: Yemoda) =
        let gdate = GregorianCalendar.Instance.GetCalendarDate(g.Year, g.Month, g.Day).ToOrdinalDate()
        let jdate = JulianCalendar.Instance.GetCalendarDate(j.Year, j.Month, j.Day).ToOrdinalDate()

        gdate.WithCalendar(JulianCalendar.Instance)    === jdate
        jdate.WithCalendar(GregorianCalendar.Instance) === gdate
