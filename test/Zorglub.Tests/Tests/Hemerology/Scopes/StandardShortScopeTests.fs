// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.StandardShortScopeTests

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
        nullExn "schema" (fun () -> new FauxStandardShortScope(null, epoch))

    [<Fact>]
    let ``Constructor throws when schema.MinYear > minYear`` () =
        let range = Range.Create(StandardShortScope.MinYear + 1, StandardShortScope.MaxYear)
        let sch = new FauxCalendricalSchema(range)

        argExn "schema" (fun () -> new FauxStandardShortScope(sch, epoch))

    [<Fact>]
    let ``Constructor throws when schema.MaxYear < 9999`` () =
        let range = Range.Create(1, StandardShortScope.MaxYear - 1)
        let sch = new FauxCalendricalSchema(range)

        argExn "schema" (fun () -> new FauxStandardShortScope(sch, epoch))

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new FauxStandardShortScope(new FauxCalendricalSchema(), epoch)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let scope = new FauxStandardShortScope(sch, epoch)
        let minDayNumber = epoch + sch.GetStartOfYear(StandardShortScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(StandardShortScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property SupportedYears`` () =
        let scope = new FauxStandardShortScope(new FauxCalendricalSchema(), epoch)
        let range = Range.Create(StandardShortScope.MinYear, StandardShortScope.MaxYear)

        scope.SupportedYears === range

    //
    // Factories
    //
    // We fully test StandardShortScope.Create() in CalendarScopeTests.

    [<Fact>]
    let ``Create() throws when "schema" is null`` () =
        nullExn "schema" (fun () -> StandardShortScope.Create(null, epoch))

    //
    // Actual scopes
    //

    [<Fact>]
    let ``Constructors for actual scopes throw when "schema" is null`` () =
        nullExn "schema" (fun () -> new PlainStandardShortScope(null, epoch))
        nullExn "schema" (fun () -> new GregorianStandardShortScope(null, epoch))
        nullExn "schema" (fun () -> new LunarStandardShortScope(null, epoch))
        nullExn "schema" (fun () -> new LunisolarStandardShortScope(null, epoch))
        nullExn "schema" (fun () -> new Solar12StandardShortScope(null, epoch))
        nullExn "schema" (fun () -> new Solar13StandardShortScope(null, epoch))

    [<Theory; MemberData(nameof(badLunarProfile))>]
    let ``LunarStandardShortScope constructor throws when "schema" is not lunar`` (sch) =
        argExn "schema" (fun () -> new LunarStandardShortScope(sch, epoch))

    [<Theory; MemberData(nameof(badLunisolarProfile))>]
    let ``LunisolarStandardShortScope constructor throws when "schema" is not lunisolar`` (sch) =
        argExn "schema" (fun () -> new LunisolarStandardShortScope(sch, epoch))

    [<Theory; MemberData(nameof(badSolar12Profile))>]
    let ``Solar12ProlepticShortScope constructor throws when "schema" is not solar12`` (sch) =
        argExn "schema" (fun () -> new Solar12StandardShortScope(sch, epoch))

    [<Theory; MemberData(nameof(badSolar13Profile))>]
    let ``Solar13StandardShortScope constructor throws when "schema" is not solar13`` (sch) =
        argExn "schema" (fun () -> new Solar13StandardShortScope(sch, epoch))

module YearOverflowChecker =
    let validYearData = StandardShortScopeFacts.ValidYearData
    let invalidYearData = StandardShortScopeFacts.InvalidYearData

    let checker = StandardShortScope.YearOverflowChecker

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``Check() overflows when "year" is out of range`` y =
        (fun () -> checker.Check(y)) |> overflows

    [<Theory; MemberData(nameof(validYearData))>]
    let ``Check() does not overflow for valid years`` y =
        checker.Check(y)

    [<Fact>]
    let ``CheckLowerBound() overflows when "year" is out of range`` () =
        (fun () -> checker.CheckLowerBound(Int32.MinValue)) |> overflows
        (fun () -> checker.CheckLowerBound(StandardShortScope.MinYear - 1)) |> overflows

    [<Fact>]
    let ``CheckLowerBound() does not overflow for valid years`` () =
        checker.CheckLowerBound(StandardShortScope.MinYear)
        checker.CheckLowerBound(StandardShortScope.MaxYear)
        checker.CheckLowerBound(StandardShortScope.MaxYear + 1)
        checker.CheckLowerBound(Int32.MaxValue)

    [<Fact>]
    let ``CheckUpperBound() overflows when "year" is out of range`` () =
        (fun () -> checker.CheckUpperBound(ShortScope.MaxYear + 1)) |> overflows
        (fun () -> checker.CheckUpperBound(Int32.MaxValue)) |> overflows

    [<Fact>]
    let ``CheckUpperBound() does not overflow for valid years`` () =
        checker.CheckUpperBound(Int32.MinValue)
        checker.CheckUpperBound(StandardShortScope.MinYear - 1)
        checker.CheckUpperBound(StandardShortScope.MinYear)
        checker.CheckUpperBound(StandardShortScope.MaxYear)

module GregorianCase =
    let private dataSet = GregorianDataSet.Instance

    let validYearData = StandardShortScopeFacts.ValidYearData
    let invalidYearData = StandardShortScopeFacts.InvalidYearData

    let dateInfoData = dataSet.DateInfoData
    let monthInfoData = dataSet.MonthInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    [<Fact>]
    let ``Static property DefaultDomain`` () =
        let epoch = DayZero.NewStyle
        let sch = new GregorianSchema()
        let minDayNumber = epoch + sch.GetStartOfYear(StandardShortScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(StandardShortScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        GregorianStandardShortScope.DefaultDomain === range

    [<Fact>]
    let CheckOverflow () =
        let epoch = DayZero.NewStyle
        let domain = GregorianStandardShortScope.DefaultDomain
        let minDaysSinceEpoch = domain.Min - epoch
        let maxDaysSinceEpoch = domain.Max - epoch

        (fun () -> GregorianStandardShortScope.CheckOverflow(minDaysSinceEpoch - 1)) |> overflows
        GregorianStandardShortScope.CheckOverflow(minDaysSinceEpoch)
        GregorianStandardShortScope.CheckOverflow(maxDaysSinceEpoch)
        (fun () -> GregorianStandardShortScope.CheckOverflow(maxDaysSinceEpoch + 1)) |> overflows

    // ValidateYearImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianStandardShortScope.ValidateYearImpl(y))
        outOfRangeExn "y" (fun () -> GregorianStandardShortScope.ValidateYearImpl(y, nameof(y)))

    [<Theory; MemberData(nameof(validYearData))>]
    let ``ValidateYearImpl() does not throw when the input is valid`` y =
        GregorianStandardShortScope.ValidateYearImpl(y)

    // ValidateYearMonthImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonthImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianStandardShortScope.ValidateYearMonthImpl(y, 1))
        outOfRangeExn "y" (fun () -> GregorianStandardShortScope.ValidateYearMonthImpl(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonthImpl() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianStandardShortScope.ValidateYearMonthImpl(y, m))
        outOfRangeExn "m" (fun () -> GregorianStandardShortScope.ValidateYearMonthImpl(y, m, nameof(m)))

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ValidateYearMonthImpl() does not throw when the input is valid`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        GregorianStandardShortScope.ValidateYearMonthImpl(y, m)
        GregorianStandardShortScope.ValidateYearImpl(y, nameof(y))

    // ValidateYearMonthDayImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonthDayImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianStandardShortScope.ValidateYearMonthDayImpl(y, 1, 1))
        outOfRangeExn "y" (fun () -> GregorianStandardShortScope.ValidateYearMonthDayImpl(y, 1, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonthDayImpl() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianStandardShortScope.ValidateYearMonthDayImpl(y, m, 1))
        outOfRangeExn "m" (fun () -> GregorianStandardShortScope.ValidateYearMonthDayImpl(y, m, 1, nameof(m)))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``ValidateYearMonthDayImpl() throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> GregorianStandardShortScope.ValidateYearMonthDayImpl(y, m, d))
        outOfRangeExn "d" (fun () -> GregorianStandardShortScope.ValidateYearMonthDayImpl(y, m, d, nameof(d)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateYearMonthDayImpl() does not throw when the input is valid`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        GregorianStandardShortScope.ValidateYearMonthDayImpl(y, m, d)

    // ValidateOrdinalImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateOrdinalImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianStandardShortScope.ValidateOrdinalImpl(y,  1))
        outOfRangeExn "y" (fun () -> GregorianStandardShortScope.ValidateOrdinalImpl(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``ValidateOrdinalImpl() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> GregorianStandardShortScope.ValidateOrdinalImpl(y, doy))
        outOfRangeExn "doy" (fun () -> GregorianStandardShortScope.ValidateOrdinalImpl(y, doy, nameof(doy)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateOrdinalImpl() does not throw when the input is valid`` (x: DateInfo) =
        let y, doy = x.Yedoy.Deconstruct()
        GregorianStandardShortScope.ValidateOrdinalImpl(y, doy)
