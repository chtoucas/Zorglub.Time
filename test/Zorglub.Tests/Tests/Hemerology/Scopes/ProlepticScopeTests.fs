﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.ProlepticScopeTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Core
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Hemerology.Scopes

open Xunit

let private epoch = DayZero.OldStyle

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new ProlepticScope(null, epoch))

    [<Fact>]
    let ``Constructor throws when schema.MinYear > minYear`` () =
        let range = Range.Create(ProlepticScope.MinYear + 1, ProlepticScope.MaxYear)
        let sch = new FauxCalendricalSchema(range)

        outOfRangeExn "year" (fun () -> new ProlepticScope(sch, epoch))

    [<Fact>]
    let ``Constructor throws when schema.MaxYear < 9999`` () =
        let range = Range.Create(1, ProlepticScope.MaxYear - 1)
        let sch = new FauxCalendricalSchema(range)

        outOfRangeExn "year" (fun () -> new ProlepticScope(sch, epoch))

    [<Fact>]
    let ``Constructor throws for PaxSchema`` () =
        let sch = SchemaActivator.CreateInstance<PaxSchema>()

        outOfRangeExn "year" (fun () -> new ProlepticScope(sch, epoch)) // PaxSchema.MinYear = 1 > -9999.

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new ProlepticScope(new FauxCalendricalSchema(), epoch)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let scope = new ProlepticScope(sch, epoch)
        let minDayNumber = epoch + sch.GetStartOfYear(ProlepticScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(ProlepticScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property SupportedYears`` () =
        let scope = new ProlepticScope(new FauxCalendricalSchema(), epoch)
        let range = Range.Create(ProlepticScope.MinYear, ProlepticScope.MaxYear)

        scope.Segment.SupportedYears === range

module YearDomain =
    let validYearData = ProlepticScopeFacts.ValidYearData
    let invalidYearData = ProlepticScopeFacts.InvalidYearData

    let checker = ProlepticScope.YearDomain

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``Check() overflows when "year" is out of range`` y =
        (fun () -> checker.Check(y)) |> overflows

    [<Theory; MemberData(nameof(validYearData))>]
    let ``Check() does not overflow for valid years`` y =
        checker.Check(y)

    [<Fact>]
    let ``CheckLowerBound() overflows when "year" is out of range`` () =
        (fun () -> checker.CheckLowerBound(Int32.MinValue)) |> overflows
        (fun () -> checker.CheckLowerBound(ProlepticScope.MinYear - 1)) |> overflows

    [<Fact>]
    let ``CheckLowerBound() does not overflow for valid years`` () =
        checker.CheckLowerBound(ProlepticScope.MinYear)
        checker.CheckLowerBound(ProlepticScope.MaxYear)
        checker.CheckLowerBound(ProlepticScope.MaxYear + 1)
        checker.CheckLowerBound(Int32.MaxValue)

    [<Fact>]
    let ``CheckUpperBound() overflows when "year" is out of range`` () =
        (fun () -> checker.CheckUpperBound(ProlepticScope.MaxYear + 1)) |> overflows
        (fun () -> checker.CheckUpperBound(Int32.MaxValue)) |> overflows

    [<Fact>]
    let ``CheckUpperBound() does not overflow for valid years`` () =
        checker.CheckUpperBound(Int32.MinValue)
        checker.CheckUpperBound(ProlepticScope.MinYear - 1)
        checker.CheckUpperBound(ProlepticScope.MinYear)
        checker.CheckUpperBound(ProlepticScope.MaxYear)

module GregorianCase =
    let private dataSet = GregorianDataSet.Instance

    let validYearData = ProlepticScopeFacts.ValidYearData
    let invalidYearData = ProlepticScopeFacts.InvalidYearData

    let dateInfoData = dataSet.DateInfoData
    let monthInfoData = dataSet.MonthInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    [<Fact>]
    let ``Static property DefaultDomain`` () =
        let epoch = DayZero.NewStyle
        let sch = new GregorianSchema()
        let minDayNumber = epoch + sch.GetStartOfYear(ProlepticScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(ProlepticScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        GregorianProlepticScope.DefaultDomain === range

    [<Fact>]
    let ``CheckOverflow()`` () =
        let epoch = DayZero.NewStyle
        let domain = GregorianProlepticScope.DefaultDomain
        let minDaysSinceEpoch = domain.Min - epoch
        let maxDaysSinceEpoch = domain.Max - epoch

        (fun () -> GregorianProlepticScope.CheckOverflow(minDaysSinceEpoch - 1)) |> overflows
        GregorianProlepticScope.CheckOverflow(minDaysSinceEpoch)
        GregorianProlepticScope.CheckOverflow(maxDaysSinceEpoch)
        (fun () -> GregorianProlepticScope.CheckOverflow(maxDaysSinceEpoch + 1)) |> overflows

    // ValidateYear()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYear() throws when "dayOfYear" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticScope.ValidateYear(y))
        outOfRangeExn "y" (fun () -> GregorianProlepticScope.ValidateYear(y, nameof(y)))

    [<Theory; MemberData(nameof(validYearData))>]
    let ``ValidateYear() does not throw when the input is valid`` y =
        GregorianProlepticScope.ValidateYear(y)
        GregorianProlepticScope.ValidateYear(y, nameof(y))

    // ValidateYearMonth()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonth() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticScope.ValidateYearMonth(y, 1))
        outOfRangeExn "y" (fun () -> GregorianProlepticScope.ValidateYearMonth(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonth() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianProlepticScope.ValidateYearMonth(y, m))
        outOfRangeExn "m" (fun () -> GregorianProlepticScope.ValidateYearMonth(y, m, nameof(m)))

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ValidateYearMonth() does not throw when the input is valid`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        GregorianProlepticScope.ValidateYearMonth(y, m)

    // ValidateYearMonthDay()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonthDay() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticScope.ValidateYearMonthDay(y, 1, 1))
        outOfRangeExn "y" (fun () -> GregorianProlepticScope.ValidateYearMonthDay(y, 1, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonthDay() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianProlepticScope.ValidateYearMonthDay(y, m, 1))
        outOfRangeExn "m" (fun () -> GregorianProlepticScope.ValidateYearMonthDay(y, m, 1, nameof(m)))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``ValidateYearMonthDay() when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> GregorianProlepticScope.ValidateYearMonthDay(y, m, d))
        outOfRangeExn "d" (fun () -> GregorianProlepticScope.ValidateYearMonthDay(y, m, d, nameof(d)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateYearMonthDay() does not throw when the input is valid`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        GregorianProlepticScope.ValidateYearMonthDay(y, m, d)

    // ValidateOrdinal()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateOrdinal() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticScope.ValidateOrdinal(y,  1))
        outOfRangeExn "y" (fun () -> GregorianProlepticScope.ValidateOrdinal(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``ValidateOrdinal() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> GregorianProlepticScope.ValidateOrdinal(y, doy))
        outOfRangeExn "doy" (fun () -> GregorianProlepticScope.ValidateOrdinal(y, doy, nameof(doy)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateOrdinal() does not throw when the input is valid`` (x: DateInfo) =
        let y, doy = x.Yedoy.Deconstruct()
        GregorianProlepticScope.ValidateOrdinal(y, doy)
