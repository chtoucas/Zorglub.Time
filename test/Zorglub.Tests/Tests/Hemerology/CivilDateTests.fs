// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.CivilDateTests

open System

open Zorglub.Testing
open Zorglub.Testing.Data.Bounded
open Zorglub.Testing.Facts
open Zorglub.Testing.Facts.Hemerology

open Zorglub.Time
open Zorglub.Time.Core.Intervals
open Zorglub.Time.Specialized

open Xunit

module Bundles =
    // NB: notice the use of StandardGregorianDataSet.

    let private supportedYears = Range.Create(CivilDate.MinYear, CivilDate.MaxYear)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<CivilDate, StandardGregorianDataSet>(supportedYears, CivilDate.Domain)

        override __.MinDate = CivilDate.MinValue
        override __.MaxDate = CivilDate.MaxValue

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type AdjustableDateTests() =
        inherit IAdjustableDateFacts<CivilDate, StandardGregorianDataSet>(supportedYears)

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

module Adjustments =
    [<Fact>]
    let ``WithYear() invalid result`` () =
        // Intercalary day mapped to a common year.
        let date = new CivilDate(4, 2, 29)

        outOfRangeExn "newYear" (fun () -> date.WithYear(3))

    [<Fact>]
    let ``WithYear() valid result`` () =
        // Intercalary day mapped to another leap year.
        let date = new CivilDate(4, 2, 29)
        let exp = new CivilDate(8, 2, 29)

        date.WithYear(8) === exp

module Postlude =
    let private maxDayNumber = CivilDate.Domain.Max

    /// Obtain the day of the week following the specified one.
    let private nextDayOfWeek (dayOfWeek: DayOfWeek) =
        let r = (1 + int dayOfWeek) % 7
        (if r >= 0 then r else r + 7) |> enum<DayOfWeek>

    /// Compare the core properties.
    let rec private compareToBcl (date: CivilDate) (time: DateTime) =
        let passed =
            date.Year = time.Year
            && date.Month = time.Month
            && date.Day = time.Day
            && date.DayOfWeek = time.DayOfWeek
            && date.DayOfYear = time.DayOfYear

        if passed then
            if date.ToDayNumber() = maxDayNumber then
                (true, "OK")
            else
                compareToBcl (date.NextDay()) (time.AddDays(1.0))
        else
            (false, sprintf "First failure: %O." date)

    [<Fact>]
    [<TestPerformance(TestPerformance.SlowUnit)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    let ``Deep comparison between CivilDate and DateTime from the BCL`` () =
        // NB: both start on Monday January 1, 1 (CE).
        compareToBcl CivilDate.MinValue DateTime.MinValue |> Assert.True

    /// Check the methods FromDayNumber(), ToDayNumber(), FromOrdinalDate() and Next(),
    /// and the property DayOfWeek.
    let rec private selfCheck dayNumber dayOfWeek (date: CivilDate)  =
        let fromDaysSinceEpoch = CivilDate.FromDayNumber(dayNumber)
        let fromOrdinalDate = CivilDate.FromOrdinalDate(date.Year, date.DayOfYear)

        let passed =
            date.ToDayNumber() = dayNumber
            && date.DayOfWeek = dayOfWeek
            && fromDaysSinceEpoch = date
            && fromOrdinalDate = date

        if passed then
            if dayNumber = maxDayNumber then
                (true, "OK")
            else
                selfCheck (dayNumber.NextDay()) (dayOfWeek |> nextDayOfWeek) (date.NextDay())
        else
            (false, sprintf "First failure: %O." date)

    [<Fact>]
    [<TestPerformance(TestPerformance.SlowUnit)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    let ``Check self-consistency of the CivilDate type`` () =
        selfCheck DayZero.NewStyle DayOfWeek.Monday CivilDate.MinValue |> Assert.True
