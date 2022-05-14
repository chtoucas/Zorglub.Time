// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

module Zorglub.Tests.Hemerology.CivilDateTests

open System

open Zorglub.Testing

open Zorglub.Time
open Zorglub.Time.Hemerology

open Xunit

/// Obtain the day of the week following the specified one.
let private nextDayOfWeek (dayOfWeek: DayOfWeek) =
    let r = (1 + int dayOfWeek) % 7
    (if r >= 0 then r else r + 7) |> enum<DayOfWeek>

module Postlude =
    let private maxDayNumber = CivilDate.Domain.Max

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
