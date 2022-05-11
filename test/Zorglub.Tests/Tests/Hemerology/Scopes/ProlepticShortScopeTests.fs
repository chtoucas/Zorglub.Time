// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.ProlepticShortScopeTests

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
    let badLunarProfile = FauxCalendricalSchema.NotLunar
    let badLunisolarProfile = FauxCalendricalSchema.NotLunisolar
    let badSolar12Profile = FauxCalendricalSchema.NotSolar12
    let badSolar13Profile = FauxCalendricalSchema.NotSolar13

    // We repeat some tests found in ShortScopeTests using a faux.

    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new FauxProlepticShortScope(null, epoch))

    [<Fact>]
    let ``Constructor throws when schema.MinYear > minYear`` () =
        let range = Range.Create(ProlepticShortScope.MinYear + 1, ProlepticShortScope.MaxYear)
        let sch = new FauxCalendricalSchema(range)

        argExn "schema" (fun () -> new FauxProlepticShortScope(sch, epoch))

    [<Fact>]
    let ``Constructor throws when schema.MaxYear < 9999`` () =
        let range = Range.Create(1, ProlepticShortScope.MaxYear - 1)
        let sch = new FauxCalendricalSchema(range)

        argExn "schema" (fun () -> new FauxProlepticShortScope(sch, epoch))

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new FauxProlepticShortScope(FauxCalendricalSchema.Default, epoch)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let scope = new FauxProlepticShortScope(sch, epoch)
        let minDayNumber = epoch + sch.GetStartOfYear(ProlepticShortScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(ProlepticShortScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property SupportedYears`` () =
        let scope = new FauxProlepticShortScope(FauxCalendricalSchema.Default, epoch)
        let range = Range.Create(ProlepticShortScope.MinYear, ProlepticShortScope.MaxYear)

        scope.SupportedYears === range

    //
    // Factories
    //
    // We fully test ProlepticShortScope.Create() in CalendarScopeTests.

    [<Fact>]
    let ``Create() throws when "schema" is null`` () =
        nullExn "schema" (fun () -> ProlepticShortScope.Create(null, epoch))

    //
    // Actual scopes
    //

    [<Fact>]
    let ``Constructors for actual scopes throw when "schema" is null`` () =
        nullExn "schema" (fun () -> new DefaultProlepticShortScope(null, epoch))
        nullExn "schema" (fun () -> new GregorianProlepticShortScope(null, epoch))
        nullExn "schema" (fun () -> new Solar12ProlepticShortScope(null, epoch))

    [<Theory; MemberData(nameof(badSolar12Profile))>]
    let ``Solar12ProlepticShortScope constructor throws when "schema" is not solar12``(sch) =
        argExn "schema" (fun () -> new Solar12ProlepticShortScope(sch, epoch))

module YearOverflowChecker =
    let validYearData = ProlepticShortScopeFacts.ValidYearData
    let invalidYearData = ProlepticShortScopeFacts.InvalidYearData

    let checker = ProlepticShortScope.YearOverflowChecker

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``Check() overflows when "year" is out of range`` y =
        (fun () -> checker.Check(y)) |> overflows

    [<Theory; MemberData(nameof(validYearData))>]
    let ``Check() does not overflow for valid years`` y =
        checker.Check(y)

    [<Fact>]
    let ``CheckLowerBound() overflows when "year" is out of range`` () =
        (fun () -> checker.CheckLowerBound(Int32.MinValue)) |> overflows
        (fun () -> checker.CheckLowerBound(ProlepticShortScope.MinYear - 1)) |> overflows

    [<Fact>]
    let ``CheckLowerBound() does not overflow for valid years`` () =
        checker.CheckLowerBound(ProlepticShortScope.MinYear)
        checker.CheckLowerBound(ProlepticShortScope.MaxYear)
        checker.CheckLowerBound(ProlepticShortScope.MaxYear + 1)
        checker.CheckLowerBound(Int32.MaxValue)

    [<Fact>]
    let ``CheckUpperBound() overflows when "year" is out of range`` () =
        (fun () -> checker.CheckUpperBound(ShortScope.MaxYear + 1)) |> overflows
        (fun () -> checker.CheckUpperBound(Int32.MaxValue)) |> overflows

    [<Fact>]
    let ``CheckUpperBound() does not overflow for valid years`` () =
        checker.CheckUpperBound(Int32.MinValue)
        checker.CheckUpperBound(ProlepticShortScope.MinYear - 1)
        checker.CheckUpperBound(ProlepticShortScope.MinYear)
        checker.CheckUpperBound(ProlepticShortScope.MaxYear)

module Gregorian =
    let private dataSet = GregorianDataSet.Instance

    let validYearData = ProlepticShortScopeFacts.ValidYearData
    let invalidYearData = ProlepticShortScopeFacts.InvalidYearData

    let dateInfoData = dataSet.DateInfoData
    let monthInfoData = dataSet.MonthInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    [<Fact>]
    let ``Static property DefaultDomain`` () =
        let epoch = DayZero.NewStyle
        let sch = new GregorianSchema()
        let minDayNumber = epoch + sch.GetStartOfYear(ProlepticShortScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(ProlepticShortScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        GregorianProlepticShortScope.DefaultDomain === range

    [<Fact>]
    let ``CheckOverflow()`` () =
        let epoch = DayZero.NewStyle
        let domain = GregorianProlepticShortScope.DefaultDomain
        let minDaysSinceEpoch = domain.Min - epoch
        let maxDaysSinceEpoch = domain.Max - epoch

        (fun () -> GregorianProlepticShortScope.CheckOverflow(minDaysSinceEpoch - 1)) |> overflows
        GregorianProlepticShortScope.CheckOverflow(minDaysSinceEpoch)
        GregorianProlepticShortScope.CheckOverflow(maxDaysSinceEpoch)
        (fun () -> GregorianProlepticShortScope.CheckOverflow(maxDaysSinceEpoch + 1)) |> overflows

    // ValidateYearImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearImpl() throws when "dayOfYear" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticShortScope.ValidateYearImpl(y))
        outOfRangeExn "y" (fun () -> GregorianProlepticShortScope.ValidateYearImpl(y, nameof(y)))

    [<Theory; MemberData(nameof(validYearData))>]
    let ``ValidateYearImpl() does not throw when the input is valid`` y =
        GregorianProlepticShortScope.ValidateYearImpl(y)
        GregorianProlepticShortScope.ValidateYearImpl(y, nameof(y))

    // ValidateYearMonthImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonthImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticShortScope.ValidateYearMonthImpl(y, 1))
        outOfRangeExn "y" (fun () -> GregorianProlepticShortScope.ValidateYearMonthImpl(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonthImpl() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianProlepticShortScope.ValidateYearMonthImpl(y, m))
        outOfRangeExn "m" (fun () -> GregorianProlepticShortScope.ValidateYearMonthImpl(y, m, nameof(m)))

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ValidateYearMonthImpl() does not throw when the input is valid`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        GregorianProlepticShortScope.ValidateYearMonthImpl(y, m)

    // ValidateYearMonthDayImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonthDayImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, 1, 1))
        outOfRangeExn "y" (fun () -> GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, 1, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonthDayImpl() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, m, 1))
        outOfRangeExn "m" (fun () -> GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, m, 1, nameof(m)))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``ValidateYearMonthDayImpl() when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, m, d))
        outOfRangeExn "d" (fun () -> GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, m, d, nameof(d)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateYearMonthDayImpl() does not throw when the input is valid`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        GregorianProlepticShortScope.ValidateYearMonthDayImpl(y, m, d)

    // ValidateOrdinalImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateOrdinalImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianProlepticShortScope.ValidateOrdinalImpl(y,  1))
        outOfRangeExn "y" (fun () -> GregorianProlepticShortScope.ValidateOrdinalImpl(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``ValidateOrdinalImpl() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> GregorianProlepticShortScope.ValidateOrdinalImpl(y, doy))
        outOfRangeExn "doy" (fun () -> GregorianProlepticShortScope.ValidateOrdinalImpl(y, doy, nameof(doy)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateOrdinalImpl() does not throw when the input is valid`` (x: DateInfo) =
        let y, doy = x.Yedoy.Deconstruct()
        GregorianProlepticShortScope.ValidateOrdinalImpl(y, doy)
