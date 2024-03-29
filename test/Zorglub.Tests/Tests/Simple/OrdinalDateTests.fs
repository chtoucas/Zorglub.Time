﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Zorglub.Tests.Simple.OrdinalDateTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Bounded

open Zorglub.Time.Core
open Zorglub.Time.Simple

open Xunit

open type Zorglub.Time.Extensions.SimpleInterconversions;

module UserCase =
    [<Fact>]
    let ``FromBinary()`` () =
        let parts = new Yedoyx(1, 1, int(UserCalendars.Gregorian.Id))
        let data = parts.ToBinary()

        outOfRangeExn "ident" (fun () -> OrdinalDate.FromBinary(data))

    [<Fact>]
    let ``ToBinary()`` () =
        let date = UserCalendars.Gregorian.GetDate(1, 1)

        throws<NotSupportedException> (fun () -> date.ToBinary())

module GregorianCase =
    let private chr = SimpleCalendar.Gregorian
    let private dataSet = ProlepticGregorianDataSet.Instance

    let dayNumberInfoData = dataSet.DayNumberInfoData
    let dateInfoData = dataSet.DateInfoData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData

    [<Fact>]
    let ``Constructor throws when "year" is out of range`` () =
        let supportedYearsTester = new SupportedYearsTester(chr.YearsValidator.Range)

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
        date.Calendar  ==& chr

    [<Theory>]
    [<InlineData(-1, 1, "001/-0001 (Gregorian)")>]
    [<InlineData(0, 1, "001/0000 (Gregorian)")>]
    [<InlineData(1, 1, "001/0001 (Gregorian)")>]
    [<InlineData(1, 3, "003/0001 (Gregorian)")>]
    [<InlineData(11, 254, "254/0011 (Gregorian)")>]
    [<InlineData(111, 26, "026/0111 (Gregorian)")>]
    [<InlineData(2019, 3, "003/2019 (Gregorian)")>]
    [<InlineData(9999, 365, "365/9999 (Gregorian)")>]
    let ``ToString()`` (y: int) doy str =
        let date = new OrdinalDate(y, doy)

        date.ToString() === str

    //
    // Adjustments
    //

    [<Fact>]
    let ``WithYear() invalid result`` () =
        // End of a leap year mapped to a common year.
        let date = new OrdinalDate(4, 366)

        outOfRangeExn "newYear" (fun () -> date.WithYear(3))

    [<Fact>]
    let ``WithYear() valid result`` () =
        // End of a leap year mapped to another leap year.
        let date = new OrdinalDate(4, 366)
        let exp = new OrdinalDate(8, 366)

        date.WithYear(8) === exp

module JulianCase =
    let private chr = SimpleCalendar.Julian
    let private dataSet = ProlepticJulianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Roundtrip serialization`` (info: DateInfo) =
        let y, doy = info.Yedoy.Deconstruct()
        let date = chr.GetDate(y, doy)

        OrdinalDate.FromBinary(date.ToBinary()) === date

module Conversions =
    let data = CalCalDataSet.GregorianToJulianData

    let ``WithCalendar() throws when the result is out of range`` () =
        let chr = SimpleCalendar.Julian
        // Julian.MinDayNumber < Gregorian.MinDayNumber.
        let date = chr.GetDate(chr.Domain.Min).ToOrdinalDate()

        outOfRangeExn "dayNumber" (fun () -> date.WithCalendar(SimpleCalendar.Gregorian))

    [<Theory; MemberData(nameof(data))>]
    let ``WithCalendar() Gregorian <-> Julian`` (pair: YemodaPair) =
        let (g, j) = pair.Deconstruct()
        let gdate = SimpleCalendar.Gregorian.GetDate(g.Year, g.Month, g.Day).ToCalendarDay()
        let jdate = SimpleCalendar.Julian.GetDate(j.Year, j.Month, j.Day).ToCalendarDay()

        gdate.WithCalendar(SimpleCalendar.Julian)    === jdate
        jdate.WithCalendar(SimpleCalendar.Gregorian) === gdate
