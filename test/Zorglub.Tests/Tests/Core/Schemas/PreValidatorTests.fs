// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Core.Schemas.PreValidatorTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data
open Zorglub.Testing.Data.Schemas
open Zorglub.Testing.Facts

open Zorglub.Time.Core
open Zorglub.Time.Core.Schemas
open Zorglub.Time.Core.Validation

open Xunit

module Prelude =
    let badLunarProfile = FauxCalendricalSchema.NotLunar
    let badLunisolarProfile = FauxCalendricalSchema.NotLunisolar
    let badSolar12Profile = FauxCalendricalSchema.NotSolar12
    let badSolar13Profile = FauxCalendricalSchema.NotSolar13

    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new CalendricalPreValidator(null))
        nullExn "schema" (fun () -> new DefaultPreValidator(null))
        nullExn "schema" (fun () -> new LunarPreValidator(null))
        nullExn "schema" (fun () -> new LunisolarPreValidator(null))
        nullExn "schema" (fun () -> new Solar12PreValidator(null))
        nullExn "schema" (fun () -> new Solar13PreValidator(null))

    [<Theory; MemberData(nameof(badLunarProfile))>]
    let ``LunarPreValidator constructor throws for non-lunar schema`` (sch) =
        argExn "schema" (fun () -> new LunarPreValidator(sch))

    [<Theory; MemberData(nameof(badLunisolarProfile))>]
    let ``LunisolarPreValidator constructor throws for non-lunisolar schema`` (sch) =
        argExn "schema" (fun () -> new LunisolarPreValidator(sch))

    [<Theory; MemberData(nameof(badSolar12Profile))>]
    let ``Solar12PreValidator constructor throws for non-solar12 schema`` (sch) =
        argExn "schema" (fun () -> new Solar12PreValidator(sch))

    [<Theory; MemberData(nameof(badSolar13Profile))>]
    let ``Solar13PreValidator constructor throws for non-solar13 schema`` (sch) =
        argExn "schema" (fun () -> new Solar13PreValidator(sch))

module PlainOrDefault =
    // We use the Copic13 schema because it may overflow when calling
    // CountDaysInYear() or CountDaysInMonth(). Both rely on IsLeapYear() which
    // overflows at Int32.MaxYear.
    let private schema = schemaOf<Coptic13Schema>()
    let private supportedYears = schema.SupportedYears

    [<Sealed>]
    type CalendricalPreValidatorTests() =
        inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(
            new CalendricalPreValidator(schema),
            supportedYears.Min,
            supportedYears.Max)

        override x.ValidateMonthDay_AtAbsoluteMaxYear() =
            let validator = x.ValidatorUT
            (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

        override x.ValidateDayOfYear_AtAbsoluteMaxYear() =
            let validator = x.ValidatorUT
            (fun () -> validator.ValidateDayOfYear(Int32.MaxValue, 1)) |> overflows

    [<Sealed>]
    type DefaultPreValidatorTests() =
        inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(
            new DefaultPreValidator(schema),
            supportedYears.Min,
            supportedYears.Max)

        override x.ValidateMonthDay_AtAbsoluteMaxYear() =
            let validator = x.ValidatorUT
            (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

        override x.ValidateDayOfYear_AtAbsoluteMaxYear() =
            // The base method works fine but we want to show that the method may
            // overflow with other parameters.
            let validator = x.ValidatorUT
            // At the start of the year, the next method does not overflow because
            // we are below the limit Coptic13Schema.MinDaysInYear.
            validator.ValidateDayOfYear(Int32.MaxValue, 1)
            // We use 366 to be sure that dayOfYear > Coptic13Schema.MinDaysInYear = 365.
            (fun () -> validator.ValidateDayOfYear(Int32.MaxValue, 366)) |> overflows

module Gregorian =
    let private dataSet = GregorianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    // ValidateMonthDay()

    [<Fact>]
    let ``ValidateMonthDay() ignores year and does not throw`` () =
        GregorianPreValidator.ValidateMonthDay(Int64.MinValue, 1, 1)
        GregorianPreValidator.ValidateMonthDay(Int64.MaxValue, 1, 1)
        // End of december: day > MinDaysInMonth.
        GregorianPreValidator.ValidateMonthDay(Int64.MinValue, 12, 31)
        GregorianPreValidator.ValidateMonthDay(Int64.MaxValue, 12, 31)

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateMonthDay() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> GregorianPreValidator.ValidateMonthDay(y, m, 1))
        outOfRangeExn "m" (fun () -> GregorianPreValidator.ValidateMonthDay(y, m, 1, nameof(m)))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``ValidateMonthDay() throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> GregorianPreValidator.ValidateMonthDay(y, m, d))
        outOfRangeExn "d" (fun () -> GregorianPreValidator.ValidateMonthDay(y, m, d, nameof(d)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateMonthDay() does not throw when the input is valid`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        GregorianPreValidator.ValidateMonthDay(y, m, d)

    // ValidateDayOfYear()

    [<Fact>]
    let ``ValidateDayOfYear() ignores year and does not throw`` () =
        GregorianPreValidator.ValidateDayOfYear(Int64.MinValue, 1)
        GregorianPreValidator.ValidateDayOfYear(Int64.MaxValue, 1)
        // Leap year: dayOfYear > MinDaysInYear.
        // NB: the next tests are a bit redundant. Indeed, if IsLeapYear() does
        // not overflow, then ValidateDayOfYear() won't either.
        GregorianFormulae.IsLeapYear(Int64.MinValue) |> ok
        GregorianFormulae.IsLeapYear(Int64.MaxValue - 3L) |> ok
        GregorianPreValidator.ValidateDayOfYear(Int64.MinValue, 366)
        GregorianPreValidator.ValidateDayOfYear(Int64.MaxValue - 3L, 366)

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``ValidateDayOfYear() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> GregorianPreValidator.ValidateDayOfYear(y, doy))
        outOfRangeExn "doy" (fun () -> GregorianPreValidator.ValidateDayOfYear(y, doy, nameof(doy)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateDayOfYear() does not throw when the input is valid`` (x: DateInfo) =
        let y, doy = x.Yedoy.Deconstruct()
        GregorianPreValidator.ValidateDayOfYear(y, doy)

module Julian =
    let private dataSet = JulianDataSet.Instance

    let dateInfoData = dataSet.DateInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    // ValidateMonthDay()

    [<Fact>]
    let ``ValidateMonthDay() ignores year and does not throw`` () =
        JulianPreValidator.ValidateMonthDay(Int64.MinValue, 1, 1)
        JulianPreValidator.ValidateMonthDay(Int64.MaxValue, 1, 1)
        // End of december: day > MinDaysInMonth.
        JulianPreValidator.ValidateMonthDay(Int64.MinValue, 12, 31)
        JulianPreValidator.ValidateMonthDay(Int64.MaxValue, 12, 31)

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateMonthDay() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> JulianPreValidator.ValidateMonthDay(y, m, 1))
        outOfRangeExn "m" (fun () -> JulianPreValidator.ValidateMonthDay(y, m, 1, nameof(m)))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``ValidateMonthDay() throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> JulianPreValidator.ValidateMonthDay(y, m, d))
        outOfRangeExn "d" (fun () -> JulianPreValidator.ValidateMonthDay(y, m, d, nameof(d)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateMonthDay() does not throw when the input is valid`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        JulianPreValidator.ValidateMonthDay(y, m, d)

    // ValidateDayOfYear()

    [<Fact>]
    let ``ValidateDayOfYear() ignores year and does not throw`` () =
        JulianPreValidator.ValidateDayOfYear(Int64.MinValue, 1)
        JulianPreValidator.ValidateDayOfYear(Int64.MaxValue, 1)
        // Leap year: dayOfYear > MinDaysInYear.
        // NB: the next tests are a bit redundant. Indeed, if IsLeapYear() does
        // not overflow, then ValidateDayOfYear() won't either.
        JulianFormulae.IsLeapYear(Int64.MinValue) |> ok
        JulianFormulae.IsLeapYear(Int64.MaxValue - 3L) |> ok
        JulianPreValidator.ValidateDayOfYear(Int64.MinValue, 366)
        JulianPreValidator.ValidateDayOfYear(Int64.MaxValue - 3L, 366)

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``ValidateDayOfYear() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> JulianPreValidator.ValidateDayOfYear(y, doy))
        outOfRangeExn "doy" (fun () -> JulianPreValidator.ValidateDayOfYear(y, doy, nameof(doy)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateDayOfYear() does not throw when the input is valid`` (x: DateInfo) =
        let y, doy = x.Yedoy.Deconstruct()
        JulianPreValidator.ValidateDayOfYear(y, doy)
