// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.Scopes.StandardScopeTests

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

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new StandardScope(null, DayZero.OldStyle))

    [<Fact>]
    let ``Constructor throws when schema.MinYear > minYear`` () =
        let range = Range.Create(StandardScope.MinYear + 1, StandardScope.MaxYear)
        let sch = new FauxCalendricalSchema(range)

        outOfRangeExn "year" (fun () -> new StandardScope(sch, DayZero.OldStyle))

    [<Fact>]
    let ``Constructor throws when schema.MaxYear < 9999`` () =
        let range = Range.Create(1, StandardScope.MaxYear - 1)
        let sch = new FauxCalendricalSchema(range)

        outOfRangeExn "year" (fun () -> new StandardScope(sch, DayZero.OldStyle))

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new StandardScope(new FauxCalendricalSchema(), epoch)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let scope = new StandardScope(sch, epoch)
        let minDayNumber = epoch + sch.GetStartOfYear(StandardScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(StandardScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property SupportedYears`` () =
        let scope = new StandardScope(new FauxCalendricalSchema(), DayZero.OldStyle)
        let range = Range.Create(StandardScope.MinYear, StandardScope.MaxYear)

        scope.Segment.SupportedYears === range

module YearsValidatorImpl =
    let validYearData = StandardScopeFacts.ValidYearData
    let invalidYearData = StandardScopeFacts.InvalidYearData

    let private validator = StandardScope.YearsValidatorImpl

    [<Fact>]
    let ``Property Range`` () =
        validator.Range === Range.Create(StandardScope.MinYear, StandardScope.MaxYear)

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``Validate() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> validator.Validate(y))
        outOfRangeExn "y" (fun () -> validator.Validate(y, nameof(y)))

    [<Theory; MemberData(nameof(validYearData))>]
    let ``Validate() does not throw when the input is valid`` y =
        validator.Validate(y)
        validator.Validate(y, nameof(y))

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``CheckOverflow() overflows when "year" is out of range`` y =
        (fun () -> validator.CheckOverflow(y)) |> overflows

    [<Theory; MemberData(nameof(validYearData))>]
    let ``CheckOverflow() does not overflow for valid years`` y =
        validator.CheckOverflow(y)

    [<Fact>]
    let ``CheckLowerBound() overflows when "year" is out of range`` () =
        (fun () -> validator.CheckLowerBound(Int32.MinValue)) |> overflows
        (fun () -> validator.CheckLowerBound(StandardScope.MinYear - 1)) |> overflows

    [<Fact>]
    let ``CheckLowerBound() does not overflow for valid years`` () =
        validator.CheckLowerBound(StandardScope.MinYear)
        validator.CheckLowerBound(StandardScope.MaxYear)
        validator.CheckLowerBound(StandardScope.MaxYear + 1)
        validator.CheckLowerBound(Int32.MaxValue)

    [<Fact>]
    let ``CheckUpperBound() overflows when "year" is out of range`` () =
        (fun () -> validator.CheckUpperBound(StandardScope.MaxYear + 1)) |> overflows
        (fun () -> validator.CheckUpperBound(Int32.MaxValue)) |> overflows

    [<Fact>]
    let ``CheckUpperBound() does not overflow for valid years`` () =
        validator.CheckUpperBound(Int32.MinValue)
        validator.CheckUpperBound(StandardScope.MinYear - 1)
        validator.CheckUpperBound(StandardScope.MinYear)
        validator.CheckUpperBound(StandardScope.MaxYear)

module GregorianCase =
    let private dataSet = GregorianDataSet.Instance

    let validYearData = StandardScopeFacts.ValidYearData
    let invalidYearData = StandardScopeFacts.InvalidYearData

    let dateInfoData = dataSet.DateInfoData
    let monthInfoData = dataSet.MonthInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    [<Fact>]
    let ``Static property DefaultDomain`` () =
        let epoch = DayZero.NewStyle
        let sch = new GregorianSchema()
        let minDayNumber = epoch + sch.GetStartOfYear(StandardScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(StandardScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        GregorianStandardScope.DefaultDomain === range

    [<Fact>]
    let ``Static property YearsValidator`` () =
        GregorianStandardScope.YearsValidator ==& StandardScope.YearsValidatorImpl

    [<Fact>]
    let ``Static property DaysValidator`` () =
        let epoch = DayZero.NewStyle
        let domain = GregorianStandardScope.DefaultDomain
        let minDaysSinceEpoch = domain.Min - epoch
        let maxDaysSinceEpoch = domain.Max - epoch
        let range = Range.Create(minDaysSinceEpoch, maxDaysSinceEpoch)
        let validator = GregorianStandardScope.DaysValidator

        validator.Range === range

    // ValidateYearMonth()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonth() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianStandardScope.ValidateYearMonth(y, 1))
        outOfRangeExn "y" (fun () -> GregorianStandardScope.ValidateYearMonth(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonth() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianStandardScope.ValidateYearMonth(y, m))
        outOfRangeExn "m" (fun () -> GregorianStandardScope.ValidateYearMonth(y, m, nameof(m)))

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ValidateYearMonth() does not throw when the input is valid`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        GregorianStandardScope.ValidateYearMonth(y, m)

    // ValidateYearMonthDay()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonthDay() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianStandardScope.ValidateYearMonthDay(y, 1, 1))
        outOfRangeExn "y" (fun () -> GregorianStandardScope.ValidateYearMonthDay(y, 1, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonthDay() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianStandardScope.ValidateYearMonthDay(y, m, 1))
        outOfRangeExn "m" (fun () -> GregorianStandardScope.ValidateYearMonthDay(y, m, 1, nameof(m)))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``ValidateYearMonthDay() throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> GregorianStandardScope.ValidateYearMonthDay(y, m, d))
        outOfRangeExn "d" (fun () -> GregorianStandardScope.ValidateYearMonthDay(y, m, d, nameof(d)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateYearMonthDay() does not throw when the input is valid`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        GregorianStandardScope.ValidateYearMonthDay(y, m, d)

    // ValidateOrdinal()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateOrdinal() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> GregorianStandardScope.ValidateOrdinal(y,  1))
        outOfRangeExn "y" (fun () -> GregorianStandardScope.ValidateOrdinal(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``ValidateOrdinal() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> GregorianStandardScope.ValidateOrdinal(y, doy))
        outOfRangeExn "doy" (fun () -> GregorianStandardScope.ValidateOrdinal(y, doy, nameof(doy)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateOrdinal() does not throw when the input is valid`` (x: DateInfo) =
        let y, doy = x.Yedoy.Deconstruct()
        GregorianStandardScope.ValidateOrdinal(y, doy)
