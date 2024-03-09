// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

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

module Prelude =
    [<Fact>]
    let ``Create() throws when "schema" is null`` () =
        nullExn "schema" (fun () -> ProlepticScope.Create(null, DayZero.OldStyle))

    [<Fact>]
    let ``Create() throws when schema.MinYear > -9997`` () =
        let range = Range.Create(ProlepticScope.MinYear + 1, ProlepticScope.MaxYear)
        let sch = new FauxCalendricalSchema(range)

        argExn "supportedYears" (fun () -> ProlepticScope.Create(sch, DayZero.OldStyle))

    [<Fact>]
    let ``Create() throws when schema.MaxYear < 9999`` () =
        let range = Range.Create(1, ProlepticScope.MaxYear - 1)
        let sch = new FauxCalendricalSchema(range)

        argExn "supportedYears" (fun () -> ProlepticScope.Create(sch, DayZero.OldStyle))

    [<Fact>]
    let ``Create() throws for PaxSchema`` () =
        let sch = SchemaActivator.CreateInstance<PaxSchema>()

        argExn "supportedYears" (fun () -> ProlepticScope.Create(sch, DayZero.OldStyle)) // PaxSchema.MinYear = 1 > -9999.

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = ProlepticScope.Create(new FauxCalendricalSchema(), epoch)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let scope = ProlepticScope.Create(sch, epoch)
        let minDayNumber = epoch + sch.GetStartOfYear(ProlepticScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(ProlepticScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property SupportedYears`` () =
        let scope = ProlepticScope.Create(new FauxCalendricalSchema(), DayZero.OldStyle)
        let range = Range.Create(ProlepticScope.MinYear, ProlepticScope.MaxYear)

        scope.Segment.SupportedYears === range

module YearsValidatorImpl =
    let validYearData = ProlepticScopeFacts.ValidYearData
    let invalidYearData = ProlepticScopeFacts.InvalidYearData

    let private validator = ProlepticScope.YearsValidatorImpl

    [<Fact>]
    let ``Property Range`` () =
        validator.Range === Range.Create(ProlepticScope.MinYear, ProlepticScope.MaxYear)

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
        (fun () -> validator.CheckLowerBound(ProlepticScope.MinYear - 1)) |> overflows

    [<Fact>]
    let ``CheckLowerBound() does not overflow for valid years`` () =
        validator.CheckLowerBound(ProlepticScope.MinYear)
        validator.CheckLowerBound(ProlepticScope.MaxYear)
        validator.CheckLowerBound(ProlepticScope.MaxYear + 1)
        validator.CheckLowerBound(Int32.MaxValue)

    [<Fact>]
    let ``CheckUpperBound() overflows when "year" is out of range`` () =
        (fun () -> validator.CheckUpperBound(ProlepticScope.MaxYear + 1)) |> overflows
        (fun () -> validator.CheckUpperBound(Int32.MaxValue)) |> overflows

    [<Fact>]
    let ``CheckUpperBound() does not overflow for valid years`` () =
        validator.CheckUpperBound(Int32.MinValue)
        validator.CheckUpperBound(ProlepticScope.MinYear - 1)
        validator.CheckUpperBound(ProlepticScope.MinYear)
        validator.CheckUpperBound(ProlepticScope.MaxYear)

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
    let ``Static properties`` () =
        let scope = ProlepticScope.Create(new GregorianSchema(), DayZero.NewStyle)

        GregorianProlepticScope.DefaultDomain === scope.Domain
        GregorianProlepticScope.YearsValidator ==& ProlepticScope.YearsValidatorImpl
        // It's enough to check the property Range.
        GregorianProlepticScope.DaysValidator.Range === scope.DaysValidator.Range

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
